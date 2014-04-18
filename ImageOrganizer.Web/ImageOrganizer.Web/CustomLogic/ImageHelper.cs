using Humanizer;
using Humanizer.Bytes;
using ImageMagick;
using ImageOrganizer.Web.CustomLogic.Definitions;
using ImageOrganizer.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace ImageOrganizer.Web.CustomLogic
{
    public class ImageHelper
    {
        public static ImageInformation GetImageInformation(string path, ImageData imageData = null)
        {
            var imageInfo = new ImageInformation();

            if (imageData == null)
            {
                imageData = GetImageData(path);
            }

            var fileInfo = new FileInfo(path);
            imageInfo.FileHash = GetFileHash(imageData.ImageBytes);
            imageInfo.Size = fileInfo.Length;
            imageInfo.HRSize = ByteSize.FromBytes(fileInfo.Length).Humanize("MB");
            imageInfo.LastChange = fileInfo.LastWriteTime;
            imageInfo.FullName = fileInfo.FullName;
            imageInfo.Name = fileInfo.Name;
            imageInfo.Extension = fileInfo.Extension;


            using (MagickImage image = new MagickImage(imageInfo.FullName))
            {
                // Searching for correct exif tag.
                // [MB]
                var exifProfile = image.GetExifProfile();
                var originalDateTime = exifProfile.Values.FirstOrDefault(exifInfo => {
                    if (exifInfo.Tag != null)
                    {
                        var tagName = exifInfo.Tag.ToString().ToLowerInvariant();

                        return (tagName == "datetimeoriginal");
                    }

                    return false;
                });

                // Trying to get the date from the exif-tag.
                // [MB]
                if (originalDateTime != null)
                {
                    DateTime parsedDate;
                    if (DateTime.TryParseExact(originalDateTime.Value as string, "yyyy:MM:dd hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                    {
                        imageInfo.OriginalDate = parsedDate;
                    }
                }
            }

            return imageInfo;
        }

        public static ImageData GetImageData(string path, string mode = null)
        {
            var absolutePath = path;

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

        public static string GetFileHash(string filePath)
        {
            return GetFileHash(File.ReadAllBytes(filePath));
        }
        
        public static string GetFileHash(byte[] fileBytes)
        {
            using (var sha = SHA256.Create())
            {
                var computedHash = sha.ComputeHash(fileBytes);
                return Convert.ToBase64String(computedHash);
            }
        }
    }
}