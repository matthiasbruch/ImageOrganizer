using ImageOrganizer.Web.CustomLogic.Definitions;
using ImageOrganizer.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageOrganizer.Web.CustomLogic
{
    public class FileHelper
    {
        public static string GetRootPath()
        {
            return ConfigurationManager.AppSettings["ImageOrganizer.RootPath"];
        }

        public static FolderInformation GetFolderInformation(string localPath = null)
        {
            var folderInformation = new FolderInformation();

            var rootPath = GetRootPath();
            folderInformation.Path = String.Concat(rootPath, @"\", localPath);

            if (Directory.Exists(folderInformation.Path))
            {
                // Gathering information about local folders.
                // [MB]
                var directoryList = new DirectoryInfo(folderInformation.Path).GetDirectories();
                folderInformation.Directories = new List<CustomFolderInformation>();
                foreach (var directory in directoryList)
	            {
                    folderInformation.Directories.Add(new CustomFolderInformation()
                    {
                        FullName = directory.FullName,
                        Name = directory.Name,
                        LocalPath = String.Format("/{0}/{1}", PathDefinition.LOCAL_PATH_PREFIX, directory.FullName.Replace(rootPath, String.Empty))
                    });
	            }

                // Gathering information about local files.
                // [MB]
                var fileList = new DirectoryInfo(folderInformation.Path).GetFiles();
                folderInformation.Files = new List<CustomFileInformation>();
                foreach (var file in fileList)
                {
                    folderInformation.Files.Add(new CustomFileInformation()
                    {
                        Name = file.Name,
                        FullName = file.FullName,
                        ImageHandlerPath = String.Format("/ImageHandler.ashx?localPath={0}", HttpUtility.UrlEncode(file.FullName.Replace(rootPath, String.Empty)))
                    });
                }
            }

            return folderInformation;
        }
    }
}