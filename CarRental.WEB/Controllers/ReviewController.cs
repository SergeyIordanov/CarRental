using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using CarRental.BLL.DTO;
using CarRental.BLL.Interfaces;
using CarRental.WEB.ViewModels;

namespace CarRental.WEB.Controllers
{
    public class ReviewController : Controller
    {
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
                cfg.CreateMap<ReviewDTO, ReviewViewModel>().AfterMap((src, dest) => dest.UserName = "YO");
            });
            var mapper = config.CreateMapper();
            return View(mapper.Map<IEnumerable<ReviewViewModel>>(_rentService.GetReviews()));
        }
    }
}