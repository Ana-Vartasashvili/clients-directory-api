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
        var totalCount = await query.CountAsync(); 
        query = ApplySorting(query, request);
        query = ApplyPagination(query, request);

        var clients = await query.ToArrayAsync();

        var result = new PaginatedResponse<GetClientDto>
        {
            Items = clients.Select(client => _mapper.Map<GetClientDto>(client)).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };

        return Ok(result);
    }
    
    private IQueryable<Client> ApplyFilters(IQueryable<Client> query, GetAllClientsRequest request)
    {
        if (request.Id != null)
        {
            query = query.Where(c=>c.Id == request.Id);
        }
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            string nameSearch = request.Name.Trim().ToLower();
            query = query.Where(c => (c.FirstName + " " + c.LastName).ToLower().Contains(nameSearch));
        }
        if (request.Gender.HasValue)
            query = query.Where(c => c.Gender == request.Gender.Value);
        if (!string.IsNullOrWhiteSpace(request.DocumentId))
        {
            query = query.Where(c => c.DocumentId.ToLower().Contains(request.DocumentId.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            query = query.Where(c => c.PhoneNumber.ToLower().Contains(request.PhoneNumber.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(request.LegalAddressCountry))
        {
            query = query.Where(c => c.LegalAddressCountry.ToLower().Contains(request.LegalAddressCountry.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(request.LegalAddressCity))
        {
            query = query.Where(c => c.LegalAddressCity.ToLower().Contains(request.LegalAddressCity.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(request.ActualAddressCountry))
        {
            query = query.Where(c => c.ActualAddressCountry.ToLower().Contains(request.ActualAddressCountry.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(request.ActualAddressCity))
        {
            query = query.Where(c => c.ActualAddressCity.ToLower().Contains(request.ActualAddressCity.ToLower()));
        }

        return query;
    }
    
    private IQueryable<Client> ApplySorting(IQueryable<Client> query, GetAllClientsRequest filter)
    {
        if (string.IsNullOrEmpty(filter.SortBy))
        {
            return query.OrderByDescending(c => c.CreatedAt);
        }
        
        var sortByParts = filter.SortBy.Split('_');
        var sortField = sortByParts[0].ToLower();
        var isDescending = sortByParts.Length > 1 && sortByParts[1].ToLower() == "desc";

        switch (sortField)
        {
            case "lastname":
                query = isDescending ? query.OrderByDescending(c => c.LastName.ToLower()) : query.OrderBy(c => c.LastName.ToLower());
                break;
            case "createdat":
                query = isDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt); 
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
    /// Gets a client by ID.
    /// </summary>
    /// <param name="id">The ID of the client.</param>
    /// <returns>The single client record.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id)
    {
        var client = await _dbContext.Clients
            .Include(c=>c.Accounts)
            .SingleOrDefaultAsync(x => x.Id == id); 
        
        if (client == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<GetFullClientDto>(client));
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

        var profileImageUrl = await ProcessProfileImage(clientDto.ProfileImage);
        if (profileImageUrl == null && clientDto.ProfileImage != null)
        {
            return BadRequest("Unsupported file type");
        }
        
        newClient.ProfileImageUrl = profileImageUrl;
        
        _dbContext.Add(newClient);
        await _dbContext.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetById), new { id = newClient.Id }, _mapper.Map<GetClientDto>(newClient));
    }
    
    private async Task<string?> ProcessProfileImage(IFormFile? profileImage)
    {
        var allowedTypes = new[] { "image/webp", "image/jpeg", "image/png", "image/svg+xml" };
        if (profileImage == null || profileImage.Length == 0 || !allowedTypes.Contains(profileImage.ContentType))
        {
            return null;
        }

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/profile-images");
        Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{Guid.NewGuid()}_{profileImage.FileName}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await profileImage.CopyToAsync(stream);

        return $"/profile-images/{fileName}";
    }

    /// <summary>
    /// Updates a client.
    /// </summary>
    /// <param name="id">The ID of the client to update.</param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(GetFullClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateClient(int id, [FromForm]UpdateClientDto updateClientDto)
    {
        var client = await  _dbContext.Clients
            .SingleOrDefaultAsync(e=>e.Id==id);
        
        _mapper.Map(updateClientDto, client);
        
        if (client == null)
        {
            return NotFound();
        }
        
        var profileImageUrl = await ProcessProfileImage(updateClientDto.ProfileImage);
        if (profileImageUrl == null && updateClientDto.ProfileImage != null)
        {
            return BadRequest("Unsupported file type");
        }
        
        if (profileImageUrl != null)
        {
            client.ProfileImageUrl = profileImageUrl;
        }

        try
        {
            await _dbContext.SaveChangesAsync();
            return Ok(_mapper.Map<GetClientDto>(client));
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while updating the employee");
        }
    }
    
    /// <summary>
    /// Deletes a client.
    /// </summary>
    /// <param name="id">The ID of the client to delete.</param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var client = await _dbContext.Clients.FindAsync(id);

        if (client == null)
        {
            return NotFound();
        }

        _dbContext.Clients.Remove(client);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpGet("{id:int}/accounts")]
    [ProducesResponseType(typeof(IEnumerable<GetAccountDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccountsByClientId(int id)
    {
        var accounts = await _dbContext.Accounts.Where(a => a.ClientId == id).ToListAsync();
    
        if (accounts == null || !accounts.Any())
        {
            return NotFound("No accounts found for this client.");
        }
    
        return Ok(_mapper.Map<IEnumerable<GetAccountDto>>(accounts));
    }

    [HttpPost("{id:int}/accounts")]
    [ProducesResponseType(typeof(GetAccountDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAccount(int id, [FromBody] CreateAccountDto accountDto)
    {
        var client = await _dbContext.Clients.FindAsync(id);
        if (client == null)
        {
            return NotFound("Client not found.");
        }

        var newAccount = _mapper.Map<Account>(accountDto);
        newAccount.ClientId = id;

        _dbContext.Accounts.Add(newAccount);
        await _dbContext.SaveChangesAsync();

        return Ok(_mapper.Map<GetAccountDto>(newAccount));
    }
    
    [HttpPut("accounts/{accountId:int}/close")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CloseAccount(int accountId)
    {
        var account = await _dbContext.Accounts.FindAsync(accountId);

        if (account == null)
        {
            return NotFound("Account not found.");
        }

        if (account.Status == AccountStatus.Closed)
        {
            return BadRequest("Account is already closed.");
        }

        account.Status = AccountStatus.Closed;
        await _dbContext.SaveChangesAsync();

        return Ok($"Account {accountId} has been closed.");
    }
}