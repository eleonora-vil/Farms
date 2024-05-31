using FluentValidation;
using Mock_Project_Net03.Common.Payloads.Requests;

namespace Mock_Project_Net03.Validation
{
    public class SignUpRequestValidator : AbstractValidator<SignupRequest>
    {
        public SignUpRequestValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Wrong Email Format");
            RuleFor(x => x.fullname)
                .NotEmpty().WithMessage("Fullname can not be empty")
                .Matches("^[a-zA-Z\\s]*$")
                .WithMessage("Fullname can not be empty");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password can not be empty");
            RuleFor(x => x.gender)
                .NotEmpty().WithMessage("Gender must be choice");
            RuleFor(x => x.phone)
                .NotEmpty().WithMessage("PhoneNumber can not be empty")
                .Matches("^0\\d{9,11}$").WithMessage("PhoneNumber must have 10-12 numbers and 0 as first");
        }
    }

}
