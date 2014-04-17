using ImageOrganizer.Web.CustomLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageOrganizer.Web
{
    public class ImageHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var imgData = ImageHelper.GetImage(HttpUtility.UrlDecode(context.Request["localPath"]), context.Request["mode"]);

            context.Response.ContentType = imgData.MimeType;
            context.Response.BinaryWrite(imgData.ImageBytes);
        }

        #region IsReusable
        public bool IsReusable { get { return false; } }
        #endregion
    }
}