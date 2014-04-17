using ImageOrganizer.Web.CustomLogic;
using ImageOrganizer.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageOrganizer.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var viewModel = new HomeIndexModel();

            viewModel.FolderInformation = FileHelper.GetFolderInformation(RouteData.Values["pathInfo"] as string);

            return View("~/Views/Home/Index.cshtml", viewModel);
        }
    }
}