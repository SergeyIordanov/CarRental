﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using CarRental.Auth.BLL.DTO;
using CarRental.Auth.BLL.Infrastructure;
using CarRental.Auth.BLL.Interfaces;
using CarRental.Auth.DAL.Interfaces;
using CarRental.Entities.Identity;
using Microsoft.AspNet.Identity;
using NLog;

namespace CarRental.Auth.BLL.Services
{
    public class UserService : IUserService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        IUnitOfWork Database { get; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public OperationDetails Create(UserDTO userDto)
        {
            Logger.Debug("Auth: Create() called");
            OperationDetails validationResult = Validator.ValidateUser(userDto);
            if (validationResult.Succedeed == false)
                return validationResult;
            ApplicationUser user = Database.UserManager.FindByEmail(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };
                IdentityResult result = Database.UserManager.Create(user, userDto.Password);
                if (result.Errors.Any())
                {
                    Logger.Debug("Auth: User creating failed: {0}", result.Errors.FirstOrDefault());
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                }                 
                // Add role
                Database.UserManager.AddToRole(user.Id, userDto.Role);
                // Create user profile
                var clientProfile = new ClientProfile { Id = user.Id, Name = userDto.Name };
                Database.ClientManager.Create(clientProfile);
                Database.Save();
                Logger.Debug("Auth: User creating successful");
                return new OperationDetails(true, "Registration succeed", "");
            }
            Logger.Debug("Auth: User creating failed: user with such login already exists");
            return new OperationDetails(false, "User with such login already exists", "Email");
        }

        public IEnumerable<UserDTO> GetAll()
        {
            Logger.Debug("Auth: GetAll() called");
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
            Logger.Debug("Auth: Get({0}) called", id);
            ApplicationUser user = Database.UserManager.FindById(id);
            if (user != null)
            {
                var role = Database.RoleManager.FindById(user.Roles.Last().RoleId);
                string roleName = null;
                if (role != null)
                    roleName = role.Name;
                return new UserDTO
                {
                    UserName = user.Email,
                    Name = user.ClientProfile.Name,
                    Email = user.Email,
                    Id = user.Id,
                    Role = roleName
                };
            }
            return null;
        }

        public OperationDetails SetRole(UserDTO userDto, string roleName)
        {
            Logger.Debug("Auth: SetRole(user, {0}) called", roleName);
            if (string.IsNullOrEmpty(userDto?.Email))
                return new OperationDetails(false, "Email cannot be empty", "Email");
            if (string.IsNullOrEmpty(roleName))
                return new OperationDetails(false, "Role cannot be empty", "");
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
                    Logger.Debug("Auth: Role '{0}' successfully setted", roleName);
                    return new OperationDetails(true, "Role successfuly set", "");
                }
                // If the role has NOT existed
                Database.RoleManager.Create(new ApplicationRole {Name = roleName});
                Database.UserManager.AddToRole(user.Id, roleName);
                Database.Save();
                Logger.Debug("Auth: Role '{0}' successfully created and setted", roleName);
                return new OperationDetails(true, "Role successfuly created and setted", "");
            }
            Logger.Debug("Auth: Role '{0}' was not created: user wasn't found", roleName);
            return new OperationDetails(false, "User wasn't found", "");
        }

        public ClaimsIdentity Authenticate(UserDTO userDto)
        {
            Logger.Debug("Auth: Authentificate(user) called");
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
            Logger.Debug("Auth: SetInitialData(user, roles) called");
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