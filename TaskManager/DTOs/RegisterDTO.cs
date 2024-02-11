using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.DTOs
{
    public class RegisterDTO
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string ConfirmPassword { get; set; }
    }

    public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidator()
        {
           

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Full Name is required.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password is required.")
                .Must((user, confirmPassword) => confirmPassword == user.Password).WithMessage("Confirm Password must match Password.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is not a valid email address.");

            RuleFor(x => x.Password)
                 .NotEmpty().WithMessage("Password is required.")
                 .MinimumLength(6).WithMessage("Passwords must be at least 6 characters long.")
                 .Matches(@"\d").WithMessage("Passwords must have at least one digit ('0'-'9').")
                 .Matches(@"[a-z]").WithMessage("Passwords must have at least one lowercase letter ('a'-'z').")
                 .Matches(@"[A-Z]").WithMessage("Passwords must have at least one uppercase letter ('A'-'Z').");
        }
    }
}