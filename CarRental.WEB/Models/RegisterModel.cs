using System.ComponentModel.DataAnnotations;

namespace CarRental.WEB.Models
{
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-z])(?=.*\d)(?!.*\s)(?=^.{8,16}$).*$", ErrorMessage = "Password have to contain at least 1 digit character & letter. Length: 8 - 16")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Name { get; set; }
    }
}