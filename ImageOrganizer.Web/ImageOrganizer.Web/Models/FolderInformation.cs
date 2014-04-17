using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageOrganizer.Web.Models
{
    public class FolderInformation
    {
        public List<CustomFolderInformation> Directories { get; set; }
        public List<CustomFileInformation> Files { get; set; }

        public string Path { get; set; }
    }

    public class CustomFolderInformation
    {
        public string FullName { get; set; }
        public string Name { get; set; }
        public string LocalPath { get; set; }
    }

    public class CustomFileInformation : ImageInformation
    {
        public CustomFileInformation(ImageInformation imageInfo)
        {
            Extension = imageInfo.Extension;
            FileHash = imageInfo.FileHash;
            FullName = imageInfo.FullName;
            HRSize = imageInfo.HRSize;
            Id = imageInfo.Id;
            Name = imageInfo.Name;
            Size = imageInfo.Size;
        }

        public string ImageHandlerPath { get; set; }
    }
}