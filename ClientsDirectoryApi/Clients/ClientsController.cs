using AutoMapper;
using Microsoft.AspNetCore.Mvc;

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