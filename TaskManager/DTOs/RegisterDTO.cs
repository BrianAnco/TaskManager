using System.ComponentModel.DataAnnotations;

namespace TaskManager.DTOs
{
    public class RegisterDTO : LoginDTO
    {
        [Required]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = "";
    }
}