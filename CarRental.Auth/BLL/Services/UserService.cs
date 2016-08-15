using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using CarRental.Auth.BLL.DTO;
using CarRental.Auth.BLL.Infrastructure;
using CarRental.Auth.BLL.Interfaces;
using CarRental.Auth.DAL.Interfaces;
using CarRental.Entities.Identity;
using Microsoft.AspNet.Identity;

namespace CarRental.Auth.BLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public OperationDetails Create(UserDTO userDto)
        {
            ApplicationUser user = Database.UserManager.FindByEmail(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };
                IdentityResult result = Database.UserManager.Create(user, userDto.Password);
                if (result.Errors.Any())
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                // add role
                Database.UserManager.AddToRole(user.Id, userDto.Role);
                // create user profile
                var clientProfile = new ClientProfile { Id = user.Id, Name = userDto.Name };
                Database.ClientManager.Create(clientProfile);
                Database.SaveAsync();
                return new OperationDetails(true, "Registration succeed", "");
            }
            return new OperationDetails(false, "User with such login already exists", "Email");
        }

        public UserDTO Get(string id)
        {
            ApplicationUser user = Database.UserManager.FindById(id);
            if (user != null)
            {
                return new UserDTO
                {
                    UserName = user.Email,
                    Name = user.ClientProfile.Name,
                    Email = user.Email,
                    Id = user.Id
                };
            }
            return null;
        }

        public ClaimsIdentity Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            // search for user
            ApplicationUser user = Database.UserManager.Find(userDto.Email, userDto.Password);
            // authorize user and return ClaimsIdentity object
            if (user != null)
                claim = Database.UserManager.CreateIdentity(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        public void SetInitialData(UserDTO adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                ApplicationRole role = Database.RoleManager.FindByName(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    Database.RoleManager.Create(role);
                }
            }
            Create(adminDto);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}