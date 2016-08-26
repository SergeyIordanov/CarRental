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

namespace CarRental.WEB.Controllers
{
    public class AccountController : Controller
    {
        private IUserService UserService => 
            HttpContext.GetOwinContext().GetUserManager<IUserService>();

        private IAuthenticationManager AuthenticationManager => 
            HttpContext.GetOwinContext().Authentication;

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            SetInitialData();
            if (ModelState.IsValid)
            {
                var userDto = new UserDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = UserService.Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Invalid login or password");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            SetInitialData();
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
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
                else
                    return View("SuccessRegister");                    
            }
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