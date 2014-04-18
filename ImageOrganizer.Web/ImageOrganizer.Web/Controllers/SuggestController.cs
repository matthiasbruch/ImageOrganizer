using ImageOrganizer.Web.CustomLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageOrganizer.Web.Controllers
{
    public class SuggestController : Controller
    {
        public ActionResult GetItems(CommandConfig request)
        {
            var suggestList = new List<SuggestItemConfig>();

            if (request != null)
            {
                switch (request.CommandName)
                {
                    case "move-file":
                    case "move-folder":
                        var directoryList = FileHelper.GetDirectoriesFromFolder(FileHelper.GetAbsolutePathFromLocal(request.Parameter));

                        if (!String.IsNullOrWhiteSpace(request.Parameter))
                        {
                            suggestList.Add(new SuggestItemConfig()
                            {
                                Label = "Execute command",
                                Value = "Execute",
                                ListItemType = "execute"
                            });
                        }

                        foreach (var directory in directoryList)
                        {
                            suggestList.Add(new SuggestItemConfig()
                                {
                                    Label = directory.Name,
                                    Value = directory.LocalPath,
                                    ListItemType = "folder"
                                });
                        }

                        break;
                    default:
                        break;
                }
            }

            return Json(suggestList, JsonRequestBehavior.AllowGet);
        }
	}

    public class SuggestItemConfig
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public string ListItemType { get; set; }
    }

    public class CommandConfig
    {
        public string CommandName { get; set; }
        public string Parameter { get; set; }
        public string CurrentPath { get; set; }
        public string[] Ids { get; set; }
    }
}