using System.Text.RegularExpressions;
using CarRental.Auth.BLL.DTO;
using CarRental.Auth.BLL.Infrastructure;

namespace CarRental.Auth.BLL.Services
{
    public static class Validator
    {
        public static OperationDetails ValidateUser(UserDTO userDto)
        {
            if (userDto == null)
                return new OperationDetails(false, "User is null", "");
            if (string.IsNullOrEmpty(userDto.Email))
                return new OperationDetails(false, "Email cannot by empty", "Email");
            if (string.IsNullOrEmpty(userDto.Name))
                return new OperationDetails(false, "Name cannot by empty", "Name");
            if (string.IsNullOrEmpty(userDto.Password) || !Regex.IsMatch(userDto.Password, @"^(?=.*[A-z])(?=.*\d)(?!.*\s)(?=^.{8,16}$).*$"))
                return new OperationDetails(false, "Password have to contain at least 1 digit character & letter. Length: 8 - 16", "Password");
            if (string.IsNullOrEmpty(userDto.Role))
                return new OperationDetails(false, "Role connot be empty", "Role");
            return new OperationDetails(true, "Validation success", "");
        }
    }
}
