using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using CarRental.Auth.BLL.DTO;
using CarRental.Auth.BLL.Infrastructure;
using CarRental.Auth.BLL.Interfaces;
using CarRental.WEB.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using NLog;

namespace CarRental.WEB.Controllers
{
    public class AccountController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private IUserService UserService => 
            HttpContext.GetOwinContext().GetUserManager<IUserService>();

        private IAuthenticationManager AuthenticationManager => 
            HttpContext.GetOwinContext().Authentication;

        public ActionResult Login()
        {
            Logger.Debug("Request to login page. User: {0}", string.IsNullOrEmpty(User.Identity.Name) ? "Anonymous" : User.Identity.Name);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            Logger.Debug("Login attempt");
            SetInitialData();
            if (ModelState.IsValid)
            {
                var userDto = new UserDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = UserService.Authenticate(userDto);
                if (claim == null)
                {
                    Logger.Debug("Login attempt failed: Invalid login or password");
                    ModelState.AddModelError("", "Invalid login or password");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    Logger.Info("Login successful. User: {0}", string.IsNullOrEmpty(User.Identity.Name) ? "Anonymous" : User.Identity.Name);
                    return RedirectToAction("Index", "Home");
                }
            }
            Logger.Debug("Login attempt failed: model is not valid");
            return View(model);
        }

        public ActionResult Logout()
        {
            Logger.Info("Logout. User: {0}", string.IsNullOrEmpty(User.Identity.Name) ? "Anonymous" : User.Identity.Name);
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            Logger.Debug("Request to register page. User: {0}", string.IsNullOrEmpty(User.Identity.Name) ? "Anonymous" : User.Identity.Name);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            SetInitialData();
            Logger.Debug("Registration attempt");
            if (ModelState.IsValid)
            {
                var userDto = new UserDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    Name = model.Name,
                    Role = "user"
                };
                OperationDetails operationDetails = UserService.Create(userDto);

                if (!operationDetails.Succedeed)
                {
                    Logger.Debug("Registration failed. Validation error");
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
                }
                else
                {
                    Logger.Info("Registration successful. Data: email: {0}, name: {1}", userDto.Email, userDto.Name);
                    return View("SuccessRegister");
                }                 
            }
            Logger.Debug("Registration failed. Invalid model");
            return View(model);
        }

        private void SetInitialData()
        {
            UserService.SetInitialData(new UserDTO
            {
                Email = "admin@gmail.com",
                UserName = "admin@gmail.com",
                Password = "admin123",
                Name = "Admin Adminovich",
                Role = "admin",
            }, new List<string> { "user", "admin", "manager", "blocked" });
        }
    }
}