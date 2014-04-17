using ImageOrganizer.Web.CustomLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ImageOrganizer.Web.Controllers
{
    public class ImageController : Controller
    {
        public ActionResult Get()
        {
            var imagePathFromRequestBase64 = RouteData.Values["pathInfo"] as string;
            var bytesFromImagePath = Convert.FromBase64String(imagePathFromRequestBase64);
            var imagePath = Encoding.UTF8.GetString(bytesFromImagePath);

            var imgData = ImageHelper.GetImageData(FileHelper.GetRootPath() + imagePath, Request["mode"]);

            return File(imgData.ImageBytes, imgData.MimeType);
        }
	}
}