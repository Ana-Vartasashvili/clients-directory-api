using System.Text.Json.Serialization;

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
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    public List<Account>? Accounts { get; set; } = new List<Account>();
}

public enum Gender
{
    Female = 1,
    Male = 2
}

public class Account
{
    public  int Id { get; set; }
    public  int ClientId { get; set; }
    public  Client Client { get; set; } = null!;
    public required AccountType Type { get; set; }
    public required Currency Currency { get; set; }
    public required AccountStatus Status { get; set; } = AccountStatus.Active;
}

public enum AccountType
{
    Current=1,
    Savings=2,
    Accrual=3
}

public enum Currency
{
    GEL=1,
    USD=2,
    EUR=3
}

public enum AccountStatus
{
    Active=1,
    Closed=2
}