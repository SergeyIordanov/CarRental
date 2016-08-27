using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CarRental.Auth.BLL.DTO;
using CarRental.Auth.BLL.Interfaces;
using CarRental.WEB.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using NLog;

namespace CarRental.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserAdminController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private IUserService UserService =>
            HttpContext.GetOwinContext().GetUserManager<IUserService>();

        /// <summary>
        /// Showing a table with all users of the application
        /// </summary>
        /// <returns>View with a table with all users of the application</returns>
        [HttpGet]
        public ActionResult Index()
        {
            Logger.Debug("Request to Admin/Users page. User: {0}", User.Identity.Name);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserViewModel>());
            var mapper = config.CreateMapper();
            return View(mapper.Map<IEnumerable<UserViewModel>>(UserService.GetAll()));
        }

        /// <summary>
        /// Sets user role
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="role">Role to set</param>
        /// <returns>List of users</returns>
        [HttpPost]
        public ActionResult SetRole(string id, string role)
        {
            Logger.Debug("Attempt to set role. Data ( user id:{0}, role: {1}", id, role);
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(role))
            {
                Logger.Warn("Attempt to set role failed. User id or role is null or empty.");
                return HttpNotFound();
            }
            var user = UserService.Get(id);
            string oldRole = user.Role;
            UserService.SetRole(user, role);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserViewModel>());
            var mapper = config.CreateMapper();

            Logger.Info("Admin {0} changed user's role. User id: {1}, old role: {2}, new role: {3}", User.Identity.Name, id, oldRole, role);

            return PartialView("Partials/_UsersList", mapper.Map<IEnumerable<UserViewModel>>(UserService.GetAll()));
        }
    }
}