using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CarRental.Auth.BLL.DTO;
using CarRental.Auth.BLL.Interfaces;
using CarRental.BLL.Interfaces;
using CarRental.WEB.ViewModels;
using Microsoft.AspNet.Identity.Owin;

namespace CarRental.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserAdminController : Controller
    {
        private IUserService UserService =>
            HttpContext.GetOwinContext().GetUserManager<IUserService>();

        readonly IRentService _rentService;
        public UserAdminController(IRentService serv)
        {
            _rentService = serv;
        }

        /// <summary>
        /// Showing a table with all users of the application
        /// </summary>
        /// <returns>View with a table with all users of the application</returns>
        [HttpGet]
        public ActionResult Index()
        {
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
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }
            var user = UserService.Get(id);
            UserService.SetRole(user, role);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserViewModel>());
            var mapper = config.CreateMapper();
            return PartialView("Partials/_UsersList", mapper.Map<IEnumerable<UserViewModel>>(UserService.GetAll()));
        }
    }
}