﻿using System.Collections.Generic;
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

namespace CarRental.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class ReviewAdminController : Controller
    {
        private IUserService UserService =>
            HttpContext.GetOwinContext().GetUserManager<IUserService>();

        readonly IRentService _rentService;
        public ReviewAdminController(IRentService serv)
        {
            _rentService = serv;
        }

        [HttpGet]
        public ActionResult Index()
        {
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
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            try
            {
                _rentService.DeleteReview(id);
            }
            catch (ValidationException) { }
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReviewDTO, ReviewViewModel>().AfterMap((src, dest) =>
                    dest.UserName =
                        GetUserViewModel(src.UserId) == null ? null : GetUserViewModel(src.UserId).Name);
            });
            var mapper = config.CreateMapper();
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