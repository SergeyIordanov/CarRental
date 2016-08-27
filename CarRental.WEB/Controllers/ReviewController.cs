using System;
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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NLog;

namespace CarRental.WEB.Controllers
{
    public class ReviewController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private IUserService UserService => HttpContext.GetOwinContext().GetUserManager<IUserService>();

        readonly IRentService _rentService;

        public ReviewController(IRentService serv)
        {
            _rentService = serv;            
        }

        [HttpGet]
        public ActionResult Index()
        {
            Logger.Debug("Request to Reviews page");
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReviewDTO, ReviewViewModel>().AfterMap((src, dest) =>
                    dest.UserName =
                        GetUserViewModel(src.UserId) == null ? null : GetUserViewModel(src.UserId).Name);
            });
            var mapper = config.CreateMapper();
            return View(mapper.Map<IEnumerable<ReviewViewModel>>(_rentService.GetReviews()));
        }

        [HttpPost]
        public ActionResult Index(ReviewViewModel reviewViewModel)
        {
            Logger.Debug("Attempt to leave a review. User: {0}", string.IsNullOrEmpty(User.Identity.Name) ? "Anonymous" : User.Identity.Name);
            MapperConfiguration config;
            IMapper mapper;
            try
            {
                reviewViewModel.PublishDate = DateTime.Now;
                config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ReviewViewModel, ReviewDTO>().AfterMap((src, dest) => dest.UserId = User.Identity.GetUserId());
                });
                mapper = config.CreateMapper();
                _rentService.CreateReview(mapper.Map<ReviewDTO>(reviewViewModel));
            }
            catch (ValidationException ex)
            {
                Logger.Debug("Attempt to leave a review faild. Validation error. (Property: {0}, Message: {1})", ex.Property, ex.Message);
                ModelState.AddModelError(ex.Property, ex.Message);
            }
                      
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReviewDTO, ReviewViewModel>().AfterMap((src, dest) =>
                    dest.UserName =
                        GetUserViewModel(src.UserId) == null || GetUserViewModel(src.UserId).Name == null ? null : GetUserViewModel(src.UserId).Name);
            });
            mapper = config.CreateMapper();

            Logger.Info("New review is added. User: {0}, Review: {1}", string.IsNullOrEmpty(User.Identity.Name) ? "Anonymous" : User.Identity.Name, reviewViewModel.Text);

            return PartialView("Partials/_ReviewsList", mapper.Map<IEnumerable<ReviewViewModel>>(_rentService.GetReviews()));
        }

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