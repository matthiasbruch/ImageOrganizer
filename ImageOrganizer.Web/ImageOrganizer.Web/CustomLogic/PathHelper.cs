using ImageOrganizer.Web.CustomLogic.Definitions;
using ImageOrganizer.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageOrganizer.Web.CustomLogic
{
    public class PathHelper
    {
        internal static List<BreadCrumbPart> GetBreadCrumb(string path)
        {
            var result = new List<BreadCrumbPart>();
            result.Add(new BreadCrumbPart()
            {
                Label = "Root",
                Path = "/"
            });

            if (!String.IsNullOrWhiteSpace(path))
            {
                path = path.Replace(FileHelper.GetRootPath(), String.Empty);

                var splittedPath = path.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (splittedPath.Any())
                {
                    string previousPath = String.Empty;
                    foreach (var pathPart in splittedPath)
                    {
                        previousPath += pathPart + "/";

                        result.Add(new BreadCrumbPart()
                        {
                            Label = pathPart,
                            Path = String.Format("/{0}/{1}", PathDefinition.LOCAL_PATH_PREFIX, previousPath)
                        });
                    }
                }
            }

            return result;
        }
    }
}