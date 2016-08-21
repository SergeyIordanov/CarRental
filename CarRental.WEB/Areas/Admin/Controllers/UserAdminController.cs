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

        [HttpGet]
        public ActionResult Index()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserViewModel>());
            var mapper = config.CreateMapper();
            return View(mapper.Map<IEnumerable<UserViewModel>>(UserService.GetAll()));
        }

        [HttpPost]
        public ActionResult Block(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }
            var user = UserService.Get(id);
            UserService.SetRole(user, "blocked");
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserViewModel>());
            var mapper = config.CreateMapper();
            return PartialView("Partials/_UsersList", mapper.Map<IEnumerable<UserViewModel>>(UserService.GetAll()));
        }

        [HttpPost]
        public ActionResult SetManager(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }
            var user = UserService.Get(id);
            UserService.SetRole(user, "manager");
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserViewModel>());
            var mapper = config.CreateMapper();
            return PartialView("Partials/_UsersList", mapper.Map<IEnumerable<UserViewModel>>(UserService.GetAll()));
        }

        [HttpPost]
        public ActionResult Unblock(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }
            var user = UserService.Get(id);
            UserService.SetRole(user, "user");
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserViewModel>());
            var mapper = config.CreateMapper();
            return PartialView("Partials/_UsersList", mapper.Map<IEnumerable<UserViewModel>>(UserService.GetAll()));
        }

        [HttpPost]
        public ActionResult SetUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }
            var user = UserService.Get(id);
            UserService.SetRole(user, "user");
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserViewModel>());
            var mapper = config.CreateMapper();
            return PartialView("Partials/_UsersList", mapper.Map<IEnumerable<UserViewModel>>(UserService.GetAll()));
        }
    }
}