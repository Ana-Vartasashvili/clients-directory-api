namespace ClientsDirectoryApi;

public class Client
{
    public int Id { get; set; }
    public required string  FirstName { get; set; }
    public required string  LastName { get; set; }
    public Gender Gender { get; set; }
    public required string  DocumentId { get; set; }
    public required string PhoneNumber { get; set; }
    public required Address  LegalAddress { get; set; }
    public required Address ActualAddress { get; set; }
    public byte[]? ProfileImage { get; set; }
}

public enum Gender
{
    Male,
    Female
}

public abstract class Address
{
    public required string Country { get; set; }
    public required string City { get; set; }
    public required string AddressLine { get; set; }
} 