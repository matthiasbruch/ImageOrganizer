using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageOrganizer.Web.Controllers
{
    public class CommandController : Controller
    {
        public ActionResult Execute(CommandConfig request)
        {
            return View();
        }
	}
}