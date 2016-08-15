﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CarRental.Auth.BLL.DTO;
using CarRental.Auth.BLL.Interfaces;
using CarRental.BLL.DTO;
using CarRental.BLL.Interfaces;
using CarRental.WEB.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CarRental.WEB.Controllers
{
    public class ReviewController : Controller
    {
        private IUserService UserService =>
            HttpContext.GetOwinContext().GetUserManager<IUserService>();

        readonly IRentService _rentService;
        public ReviewController(IRentService serv)
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
                        GetUserViewModel(src.UserId).Result == null ? null : GetUserViewModel(src.UserId).Result.Name);
            });
            var mapper = config.CreateMapper();
            return View(mapper.Map<IEnumerable<ReviewViewModel>>(_rentService.GetReviews()));
        }

        [HttpPost]
        public ActionResult Index(ReviewViewModel reviewViewModel)
        {
            reviewViewModel.PublishDate = DateTime.Now;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReviewViewModel, ReviewDTO>().AfterMap((src, dest) => dest.UserId = User.Identity.GetUserId());
            });
            var mapper = config.CreateMapper();
            _rentService.CreateReview(mapper.Map<ReviewDTO>(reviewViewModel));
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReviewDTO, ReviewViewModel>();
            });
            mapper = config.CreateMapper();
            return PartialView("Partials/_ReviewsList", mapper.Map<IEnumerable<ReviewViewModel>>(_rentService.GetReviews()));
        }

        private async Task<UserViewModel> GetUserViewModel(string id)
        {
            UserDTO userDto = await UserService.Get(id);
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