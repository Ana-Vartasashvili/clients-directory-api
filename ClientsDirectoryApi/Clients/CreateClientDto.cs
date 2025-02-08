using System.Text.RegularExpressions;
using FluentValidation;

namespace ClientsDirectoryApi.Clients;

public class CreateClientDto
{
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
    public IFormFile? ProfileImage { get; set; }
}

public class CreateClientDtoValidator : AbstractValidator<CreateClientDto>
{
    public CreateClientDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50)
            .Matches(@"^(?!.*[\u10A0-\u10FF].*[a-zA-Z])(?!.*[a-zA-Z].*[\u10A0-\u10FF])[\u10A0-\u10FFa-zA-Z]+$")
            .WithMessage("First name must contain only Georgian or only Latin characters.");
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50)
            .Matches(@"^(?!.*[\u10A0-\u10FF].*[a-zA-Z])(?!.*[a-zA-Z].*[\u10A0-\u10FF])[\u10A0-\u10FFa-zA-Z]+$")
            .WithMessage("First name must contain only Georgian or only Latin characters.");

        RuleFor(x => x.DocumentId)
            .NotEmpty()
            .Length(11);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Length(9)
            .Matches(@"^5").WithMessage("Phone number should start with '5'.");

        RuleFor(x => x.LegalAddressCountry).NotEmpty();
        RuleFor(x=>x.LegalAddressCity).NotEmpty();
        RuleFor(x=>x.LegalAddressLine).NotEmpty();
        RuleFor(x=>x.ActualAddressCountry).NotEmpty();
        RuleFor(x=>x.ActualAddressCity).NotEmpty();
        RuleFor(x=>x.ActualAddressLine).NotEmpty();
    }
}