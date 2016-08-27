using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CarRental.Auth.BLL.DTO;
using CarRental.Auth.BLL.Interfaces;
using CarRental.BLL.DTO;
using CarRental.BLL.Infrastructure;
using CarRental.BLL.Interfaces;
using CarRental.WEB.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using NLog;

namespace CarRental.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class ReviewAdminController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private IUserService UserService =>
            HttpContext.GetOwinContext().GetUserManager<IUserService>();

        readonly IRentService _rentService;

        public ReviewAdminController(IRentService serv)
        {
            _rentService = serv;
        }

        /// <summary>
        /// Shows the list of reviews
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            Logger.Debug("Request to Admin/Reviews page. User: {0}", User.Identity.Name);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReviewDTO, ReviewViewModel>().AfterMap((src, dest) =>
                    dest.UserName =
                        GetUserViewModel(src.UserId) == null ? null : GetUserViewModel(src.UserId).Name);
            });
            var mapper = config.CreateMapper();
            return View(mapper.Map<IEnumerable<ReviewViewModel>>(_rentService.GetReviews()));
        }

        /// <summary>
        /// Deleting a review
        /// </summary>
        /// <param name="id">Review id to delete</param>
        /// <returns>List of reviews</returns>
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            Logger.Debug("Attempt to delete a review. Review id: {0}", id);
            try
            {
                _rentService.DeleteReview(id);
            }
            catch (ValidationException ex)
            {
                Logger.Debug("Review deleting failed. Validation error (Property: {0}, Message: {1})", ex.Property, ex.Message);
            }
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReviewDTO, ReviewViewModel>().AfterMap((src, dest) =>
                    dest.UserName =
                        GetUserViewModel(src.UserId) == null ? null : GetUserViewModel(src.UserId).Name);
            });
            var mapper = config.CreateMapper();

            Logger.Info("Review #{0} was deleted by {1}.", id, User.Identity.Name);

            return PartialView("Partials/_ReviewsList", mapper.Map<IEnumerable<ReviewViewModel>>(_rentService.GetReviews()));
        }

        /// <summary>
        /// Uses UserService to get info about the user by its id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>UserViewModel mapped from got user</returns>
        private UserViewModel GetUserViewModel(string id)
        {
            UserDTO userDto = UserService.Get(id);
            if (userDto != null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserDTO, UserViewModel>();
                });
                var mapper = config.CreateMapper();
                return mapper.Map<UserViewModel>(userDto);
            }
            return null;
        }
    }
}