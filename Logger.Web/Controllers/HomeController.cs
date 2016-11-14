using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logger.Service.Interfaces;

namespace Logger.Web.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IApplicationService ApplicationService;

        //public HomeController(IApplicationService applicationService)
        //{
        //    ApplicationService = applicationService;
        //}

        public ActionResult Index()
        {
            ViewBag.Title = "Logger API v1 - Tong Quang Hoang";

            return View();
        }
    }
}
