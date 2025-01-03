using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Application.Dtos
{
    public class RegisterDto : AccountBase
    {
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string? Fullname { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Required]
        public string? ConfirmPassword { get; set; }
    }
}
