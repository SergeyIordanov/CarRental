using System;
using System.Collections.Generic;
using System.Security.Claims;
using CarRental.Auth.BLL.DTO;
using CarRental.Auth.BLL.Infrastructure;

namespace CarRental.Auth.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        OperationDetails Create(UserDTO userDto);

        IEnumerable<UserDTO> GetAll();

        UserDTO Get(string id);

        OperationDetails SetRole(UserDTO userDto, string role);

        OperationDetails RemoveRole(UserDTO userDto, string role);

        ClaimsIdentity Authenticate(UserDTO userDto);

        void SetInitialData(UserDTO adminDto, List<string> roles);
    }
}