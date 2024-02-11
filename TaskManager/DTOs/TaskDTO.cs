using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TaskManager.Models;
using FluentValidation;

namespace TaskManager.DTOs
{
    public class TaskDTO
    {
        public string TaskId { get; set; }

        public string UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string StateId { get; set; }

        public ApplicationUser User { get; set; }

        public TaskState TaskState { get; set; }
    }

    public class TaskDTOValidator : AbstractValidator<TaskDTO>
    {
        public TaskDTOValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

            RuleFor(x => x.StateId)
                .NotEmpty().WithMessage("State is required.");
        }
    }
}
