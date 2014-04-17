using ImageMagick;
using ImageOrganizer.Web.CustomLogic.Definitions;
using ImageOrganizer.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageOrganizer.Web.CustomLogic
{
    public class ImageHelper
    {
        public static ImageData GetImage(string path = null, string mode = null)
        {
            var absolutePath = String.Concat(FileHelper.GetRootPath(), path);

            if (!String.IsNullOrWhiteSpace(mode))
            {
                absolutePath = GetImageInMode(absolutePath, mode);
            }

            return new ImageData()
            {
                ImageBytes = File.ReadAllBytes(absolutePath),
                MimeType = "image/jpeg"
            };
        }

        private static string GetImageInMode(string absolutePath, string mode)
        {
            if (!String.IsNullOrWhiteSpace(absolutePath))
            {
                var splittedPath = absolutePath.Split(new[] { '\\' }).ToList();

                if (splittedPath.Count > 0)
                {
                    splittedPath.Insert(splittedPath.Count - 1, mode);
                    var absolutePathExtended = String.Join("\\", splittedPath);

                    var fileInformation = new FileInfo(absolutePathExtended);

                    if (!Directory.Exists(fileInformation.Directory.FullName))
                        Directory.CreateDirectory(fileInformation.Directory.FullName);

                    if (!File.Exists(fileInformation.FullName))
                    {
                        using (MagickImage image = new MagickImage(absolutePath))
                        {
                            image.BackgroundColor = new MagickColor(255, 255, 255);
                            image.Resize(new MagickGeometry(200, 200) { FillArea = false });
                            image.Quality = 50;
                            
                            image.Write(absolutePathExtended);
                        }
                    }

                    absolutePath = absolutePathExtended;
                }
            }

            return absolutePath;
        }
    }
}