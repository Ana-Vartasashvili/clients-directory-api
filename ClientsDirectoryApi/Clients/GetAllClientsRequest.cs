namespace ClientsDirectoryApi.Clients;

public class GetAllClientsRequest
{
    public int? Id {get; set;}
    public string? Name { get; set; }
    public Gender? Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string? DocumentId { get; set; }
    public string? LegalAddressCountry { get; set; }
    public string? LegalAddressCity { get; set; }
    public string? ActualAddressCountry { get; set; }
    public string? ActualAddressCity { get; set; }
    public string? SortBy { get; set; } 
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}