using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageOrganizer.Web.Models
{
    public class ImageData
    {
        public string MimeType { get; set; }
        public byte[] ImageBytes { get; set; }
    }
}