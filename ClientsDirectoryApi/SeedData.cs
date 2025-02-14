using Microsoft.EntityFrameworkCore;

namespace ClientsDirectoryApi;

public class SeedData
{
    public static void MigrateAndSeed(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
        
        if (!context.Clients.Any())
        {
            var clients = new List<Client>();
            var random = new Random();
            var firstNames = new[] { "John", "Jane", "Mike", "Anna", "Chris", "Laura", "David", "Emma", "Tom", "Sophia" };
            var lastNames = new[] { "Smith", "Johnson", "Brown", "Taylor", "Anderson", "Thomas", "Harris", "Martin", "Lee", "Walker" };
            var countries = new[] { "USA", "UK", "Germany", "France", "Italy", "Spain", "Canada", "Australia", "Netherlands", "Sweden" };
            
            for (int i = 1; i <= 50; i++)
            {
                var firstName = firstNames[random.Next(firstNames.Length)];
                var lastName = lastNames[random.Next(lastNames.Length)];
                var country = countries[random.Next(countries.Length)];
                var city = "City" + random.Next(1, 20);
                var documentId = random.Next(100000000, 999999999).ToString();
                while (documentId.Length < 11)  
                {
                    documentId = "0" + documentId; 
                }
                var phoneNumber = "5" + random.Next(10000000, 99999999);                
                var client = new Client
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Gender = (Gender)random.Next(1, 3),
                    DocumentId = documentId,
                    PhoneNumber = phoneNumber,
                    LegalAddressCountry = country,
                    LegalAddressCity = city,
                    LegalAddressLine = "Street " + random.Next(1, 100),
                    ActualAddressCountry = country,
                    ActualAddressCity = city,
                    ActualAddressLine = "Street " + random.Next(1, 100),
                    CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 365)),
                    Accounts = new List<Account>
                    {
                        new Account
                        {
                            Type = (AccountType)random.Next(1, 4),
                            Currency = (Currency)random.Next(1, 4),
                            Status = AccountStatus.Active
                        }
                    }
                };
                clients.Add(client);
            }
            context.Clients.AddRange(clients);
            context.SaveChanges();
        }
    }
}