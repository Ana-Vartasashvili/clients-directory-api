namespace ClientsDirectoryApi.Clients;

public class GetAccountDto
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public AccountType Type { get; set; }
    public Currency Currency { get; set; }
    public AccountStatus status { get; set; }
}