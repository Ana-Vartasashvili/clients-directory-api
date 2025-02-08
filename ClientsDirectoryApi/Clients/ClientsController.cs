using AutoMapper;
using ClientsDirectoryApi.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientsDirectoryApi.Clients;

public class ClientsController : BaseController
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public ClientsController(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Gets all of the clients in the system.
    /// </summary>
    /// <returns>Returns the clients in a JSON array.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetClientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllClientsRequest request)
    {
        var query = _dbContext.Clients.AsQueryable();
        
        query = ApplyFilters(query, request);
        query = ApplySorting(query, request);
        query = ApplyPagination(query, request);

        var clients = await query.ToArrayAsync();

        var result = new PaginatedResponse<GetClientDto>
        {
            Items = clients.Select(client => _mapper.Map<GetClientDto>(client)).ToList(),
            TotalCount = clients.Length,
            Page = request.Page,
            PageSize = request.PageSize
        };

        return Ok(result);
    }
    
    private IQueryable<Client> ApplyFilters(IQueryable<Client> query, GetAllClientsRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.FirstName))
            query = query.Where(c => c.FirstName.Contains(request.FirstName, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(request.LastName))
            query = query.Where(c => c.LastName.Contains(request.LastName, StringComparison.OrdinalIgnoreCase));
        if (request.Gender.HasValue)
            query = query.Where(c => c.Gender == request.Gender.Value);
        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            query = query.Where(c => c.PhoneNumber.Contains(request.PhoneNumber));
        if (!string.IsNullOrWhiteSpace(request.LegalAddressCountry))
            query = query.Where(c => c.LegalAddressCountry.Contains(request.LegalAddressCountry, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(request.LegalAddressCity))
            query = query.Where(c => c.LegalAddressCity.Contains(request.LegalAddressCity, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(request.ActualAddressCountry))
            query = query.Where(c => c.ActualAddressCountry.Contains(request.ActualAddressCountry, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(request.ActualAddressCity))
            query = query.Where(c => c.ActualAddressCity.Contains(request.ActualAddressCity, StringComparison.OrdinalIgnoreCase));

        return query;
    }
    
    private IQueryable<Client> ApplySorting(IQueryable<Client> query, GetAllClientsRequest filter)
    {
        if (string.IsNullOrEmpty(filter.SortBy)) return query;
        
        var sortOrder = filter.SortOrder?.ToLower() == "desc" ? "desc" : "asc";
            
        switch (filter.SortBy.ToLower())
        { 
            case "id": 
                query = sortOrder == "desc" ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id);
                break;
            case "firstname":
                query = sortOrder == "desc" ? query.OrderByDescending(c => c.FirstName) : query.OrderBy(c => c.FirstName);
                break;
            case "lastname":
                query = sortOrder == "desc" ? query.OrderByDescending(c => c.LastName) : query.OrderBy(c => c.LastName);
                break;
            default:
                query = query.OrderBy(c => c.Id);
                break;
        }
            
        return query;
    }

    private IQueryable<Client> ApplyPagination(IQueryable<Client> query, GetAllClientsRequest filter)
    {
        return query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
    }

    /// <summary>
    /// Creates a new client.
    /// </summary>
    /// <param name="clientDto">The client to be created.</param>
    /// <returns>A link to the client that was created.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(GetClientDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateClient([FromForm] CreateClientDto clientDto)
    {
        var newClient = _mapper.Map<Client>(clientDto);

        if (clientDto.ProfileImage != null && clientDto.ProfileImage.Length > 0)
        {
            var allowedTypes = new[] { "image/webp", "image/jpeg", "image/png", "image/svg+xml" };
            
            if (!allowedTypes.Contains(clientDto.ProfileImage.ContentType))
            {
                return BadRequest("Unsupported file type");
            }
            
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/profile-images");
            Directory.CreateDirectory(uploadsFolder); 
            
            var fileName = $"{Guid.NewGuid()}_{clientDto.ProfileImage.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);
            
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await clientDto.ProfileImage.CopyToAsync(stream);
            }

            newClient.ProfileImageUrl = $"/profile-images/{fileName}";
        }
        
        _dbContext.Add(newClient);
        await _dbContext.SaveChangesAsync();
        
        // return CreatedAtAction(nameof(GetClientById), new { id = newClient.Id }, newClient);
        return Ok(newClient);
    }

}