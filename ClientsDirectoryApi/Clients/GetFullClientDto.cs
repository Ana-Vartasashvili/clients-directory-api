namespace ClientsDirectoryApi.Clients;

public class GetFullClientDto
{
    public int Id { get; set; }
    public  string  FirstName { get; set; }
    public  string  LastName { get; set; }
    public Gender Gender { get; set; }
    public  string  DocumentId { get; set; }
    public  string PhoneNumber { get; set; }
    public  string LegalAddressCountry { get; set; }
    public  string  LegalAddressCity { get; set; }
    public  string  LegalAddressLine { get; set; }
    public  string  ActualAddressCountry { get; set; }
    public  string  ActualAddressCity { get; set; }
    public  string  ActualAddressLine { get; set; }
    public string? ProfileImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    public List<GetAccountDto> Accounts { get; set; }
}