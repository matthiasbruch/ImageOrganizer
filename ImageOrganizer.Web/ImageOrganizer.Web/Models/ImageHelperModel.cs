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

    public class ImageInformation
    {
        public ImageInformation()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string FileHash { get; set; }
        public long Size { get; set; }
        public string HRSize { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
    }
}