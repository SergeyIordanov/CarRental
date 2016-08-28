using System.ComponentModel.DataAnnotations;

namespace CarRental.WEB.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^.+@.+\..+$", ErrorMessage = "Wrong e-mail format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[A-z])(?=.*\d)(?!.*\s)(?=^.{8,16}$).*$", ErrorMessage = "Password have to contain at least 1 digit character & letter. Length: 8 - 16")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}