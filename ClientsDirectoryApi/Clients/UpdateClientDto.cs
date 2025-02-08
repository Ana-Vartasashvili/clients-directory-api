using System.Linq.Expressions;
using FluentValidation;

namespace ClientsDirectoryApi.Clients;

public class UpdateClientDto
{
    public string?  FirstName { get; set; }
    public string?  LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? LegalAddressCountry { get; set; }
    public string?  LegalAddressCity { get; set; }
    public string?  LegalAddressLine { get; set; }
    public string?  ActualAddressCountry { get; set; }
    public string?  ActualAddressCity { get; set; }
    public string?  ActualAddressLine { get; set; }
    public IFormFile? ProfileImage { get; set; }
}

public class UpdateClientDtoValidator : AbstractValidator<UpdateClientDto>
{
    public UpdateClientDtoValidator()
    {
        ValidateNotEmptyIfProvided(x => x.FirstName, "First name")
            .Length(2, 50)
            .Matches(@"^(?!.*[\u10A0-\u10FF].*[a-zA-Z])(?!.*[a-zA-Z].*[\u10A0-\u10FF])[\u10A0-\u10FFa-zA-Z]+$")
            .WithMessage("First name must contain only Georgian or only Latin characters.");

        ValidateNotEmptyIfProvided(x => x.LastName, "Last name")
            .Length(2, 50)
            .Matches(@"^(?!.*[\u10A0-\u10FF].*[a-zA-Z])(?!.*[a-zA-Z].*[\u10A0-\u10FF])[\u10A0-\u10FFa-zA-Z]+$")
            .WithMessage("Last name must contain only Georgian or only Latin characters.");

        ValidateNotEmptyIfProvided(x => x.PhoneNumber, "Phone number")
            .Length(9)
            .Matches(@"^5").WithMessage("Phone number should start with '5'.");

        ValidateNotEmptyIfProvided(x => x.LegalAddressCountry, "Legal address country");
        ValidateNotEmptyIfProvided(x => x.LegalAddressCity, "Legal address city");
        ValidateNotEmptyIfProvided(x => x.LegalAddressLine, "Legal address line");
        ValidateNotEmptyIfProvided(x => x.ActualAddressCountry, "Actual address country");
        ValidateNotEmptyIfProvided(x => x.ActualAddressCity, "Actual address city");
        ValidateNotEmptyIfProvided(x => x.ActualAddressLine, "Actual address line");
    }
    
    private IRuleBuilderOptions<UpdateClientDto, string?> ValidateNotEmptyIfProvided(
        Expression<Func<UpdateClientDto, string?>> property, string fieldName)
    {
        return RuleFor(property)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .When(x => property.Compile()(x) != null)
            .WithMessage($"{fieldName} cannot be empty or whitespace.");
    }
}