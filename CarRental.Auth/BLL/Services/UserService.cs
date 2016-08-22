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
                // Add role
                Database.UserManager.AddToRole(user.Id, userDto.Role);
                // Create user profile
                var clientProfile = new ClientProfile { Id = user.Id, Name = userDto.Name };
                Database.ClientManager.Create(clientProfile);
                Database.Save();
                return new OperationDetails(true, "Registration succeed", "");
            }
            return new OperationDetails(false, "User with such login already exists", "Email");
        }

        public IEnumerable<UserDTO> GetAll()
        {
            List<UserDTO> usersDto = new List<UserDTO>();
            IEnumerable<ApplicationUser> users = Database.UserManager.Users.ToList();
            foreach (var user in users)
            {
                string role = Database.RoleManager.FindById(user.Roles.Last().RoleId).Name;
                usersDto.Add(new UserDTO
                {
                    UserName = user.Email,
                    Name = user.ClientProfile.Name,
                    Email = user.Email,
                    Id = user.Id,
                    Role = role
                });
            }
            return usersDto;
        }

        public UserDTO Get(string id)
        {
            ApplicationUser user = Database.UserManager.FindById(id);
            if (user != null)
            {
                string role = Database.RoleManager.FindById(user.Roles.Last().RoleId).Name;
                return new UserDTO
                {
                    UserName = user.Email,
                    Name = user.ClientProfile.Name,
                    Email = user.Email,
                    Id = user.Id,
                    Role = role
                };
            }
            return null;
        }

        public OperationDetails SetRole(UserDTO userDto, string roleName)
        {
            // Search for user
            ApplicationUser user = Database.UserManager.FindByEmail(userDto.Email);
            if (user != null)
            {
                // Removing old roles
                Database.UserManager.RemoveFromRoles(user.Id, Database.UserManager.GetRoles(user.Id).ToArray());
                var role = Database.RoleManager.FindByName(roleName);
                // If the role has already existed
                if (role != null)
                {                   
                    Database.UserManager.AddToRole(user.Id, roleName);
                    Database.Save();
                    return new OperationDetails(true, "Role successfuly set", "");
                }
                // If the role has NOT existed
                Database.RoleManager.Create(new ApplicationRole {Name = roleName});
                Database.UserManager.AddToRole(user.Id, roleName);
                Database.Save();
                return new OperationDetails(true, "Role successfuly created and set", "");
            }            
            return new OperationDetails(false, "User wasn't found", "");
        }

        public ClaimsIdentity Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            // Search for user
            ApplicationUser user = Database.UserManager.Find(userDto.Email, userDto.Password);
            // Authorize user and return ClaimsIdentity object
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