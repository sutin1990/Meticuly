using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Helper
{
    public static class UserMenuExtensions
    {
        public static bool IsMenuActive(this IHtmlHelper htmlHelper, string menuItemUrl)
        {
            var viewContext = htmlHelper.ViewContext;
            var currentPageUrl = viewContext.ViewData["ActiveMenu"] as string ?? viewContext.HttpContext.Request.Path;
            return currentPageUrl.StartsWith(menuItemUrl, StringComparison.OrdinalIgnoreCase);
        }
    }
}
