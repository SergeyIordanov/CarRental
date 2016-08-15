using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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

        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };
                IdentityResult result = await Database.UserManager.CreateAsync(user, userDto.Password);
                if (result.Errors.Any())
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                // add role
                await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                // create user profile
                var clientProfile = new ClientProfile { Id = user.Id, Name = userDto.Name };
                Database.ClientManager.Create(clientProfile);
                await Database.SaveAsync();
                return new OperationDetails(true, "Registration succeed", "");
            }
            return new OperationDetails(false, "User with such login already exists", "Email");
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            // search for user
            ApplicationUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
            // authorize user and return ClaimsIdentity object
            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        public async Task SetInitialData(UserDTO adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                ApplicationRole role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }
            await Create(adminDto);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}