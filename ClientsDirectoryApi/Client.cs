namespace ClientsDirectoryApi;

public class Client
{
    public int Id { get; set; }
    public required string  FirstName { get; set; }
    public required string  LastName { get; set; }
    public Gender Gender { get; set; }
    public required string  DocumentId { get; set; }
    public required string PhoneNumber { get; set; }
    public required string LegalAddressCountry { get; set; }
    public required string  LegalAddressCity { get; set; }
    public required string  LegalAddressLine { get; set; }
    public required string  ActualAddressCountry { get; set; }
    public required string  ActualAddressCity { get; set; }
    public required string  ActualAddressLine { get; set; }
    public string? ProfileImageUrl { get; set; }
}

public enum Gender
{
    Male,
    Female
}

