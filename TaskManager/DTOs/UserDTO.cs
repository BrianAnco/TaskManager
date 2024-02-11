using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.DTOs
{
    public class UserDTO
    {
        public string ID { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is not a valid email address.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");


            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).When(x => !string.IsNullOrEmpty(x.CurrentPassword)).WithMessage("Current Password must be at least 6 characters long.")
                .Matches(@"\d").When(x => !string.IsNullOrEmpty(x.CurrentPassword)).WithMessage("Current Password must have at least one digit ('0'-'9').")
                .Matches(@"[a-z]").When(x => !string.IsNullOrEmpty(x.CurrentPassword)).WithMessage("Current Password must have at least one lowercase ('a'-'z').")
                .Matches(@"[A-Z]").When(x => !string.IsNullOrEmpty(x.CurrentPassword)).WithMessage("Current Password must have at least one uppercase ('A'-'Z').");


            RuleFor(x => x.NewPassword)
                .MinimumLength(6).When(x => !string.IsNullOrEmpty(x.NewPassword)).WithMessage("New Password must be at least 6 characters long.")
                .Matches(@"\d").When(x => !string.IsNullOrEmpty(x.NewPassword)).WithMessage("New Password must have at least one digit ('0'-'9').")
                .Matches(@"[a-z]").When(x => !string.IsNullOrEmpty(x.NewPassword)).WithMessage("New Password must have at least one lowercase ('a'-'z').")
                .Matches(@"[A-Z]").When(x => !string.IsNullOrEmpty(x.NewPassword)).WithMessage("New Password must have at least one uppercase ('A'-'Z').");

            RuleFor(x => x.UserName)
                .Equal(x => x.Email).WithMessage("UserName must always be equal to Email.");
        }
    }
}
