namespace ClientsDirectoryApi.Clients;

public class CreateAccountDto
{
    public required AccountType Type { get; set; }
    public required Currency Currency { get; set; }
}