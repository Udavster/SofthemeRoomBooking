using System;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Routing;

namespace SofthemeRoomBooking.Helpers
{
    public static class Pagination
    {
        public static MvcHtmlString PageLinks(this HtmlHelper htmlHelper, int currPage, int totalPages,
            Func<int, string> pageUrl, object containerHtmlAttributes = null, object linkHtmlAttributes = null)
        {
            var defaultContainerHtmlAttr = new { @class = "pagination" };
            var defaultLinkHtmlAttr = new { @class = "page-link" };

            TagBuilder containerBuilder = new TagBuilder("div");
            containerBuilder.MergeAttributes(new RouteValueDictionary(containerHtmlAttributes ?? defaultContainerHtmlAttr));

            for (int i = 1; i <= totalPages; i++)
            {
                if (i == 1 || i == totalPages || ((i > currPage - 2) && (i < currPage + 2)))
                {
                    TagBuilder pageBuilder = new TagBuilder("span");

                    TagBuilder linkBuilder = new TagBuilder("a");
                    linkBuilder.MergeAttributes(new RouteValueDictionary(linkHtmlAttributes ?? defaultLinkHtmlAttr));

                    if (i == currPage)
                    {
                        linkBuilder.MergeAttribute("href", "#");
                        linkBuilder.AddCssClass("page-link__active");
                    }
                    else
                    {
                        linkBuilder.MergeAttribute("href", pageUrl.Invoke(i));
                    }

                    linkBuilder.InnerHtml += i.ToString(CultureInfo.InvariantCulture);
                    pageBuilder.InnerHtml += linkBuilder.ToString(TagRenderMode.Normal);
                    containerBuilder.InnerHtml += pageBuilder.ToString(TagRenderMode.Normal);
                }
                else if((i == 2 && currPage > 3) || ((i == totalPages - 1) && (currPage < totalPages - 2)))
                {
                    TagBuilder dotsBuilder = new TagBuilder("span");
                    dotsBuilder.MergeAttributes(new RouteValueDictionary(linkHtmlAttributes ?? defaultLinkHtmlAttr));
                    dotsBuilder.AddCssClass("page-dots");
                    dotsBuilder.InnerHtml += "...";
                    containerBuilder.InnerHtml += dotsBuilder.ToString(TagRenderMode.Normal);
                }
            }

            return MvcHtmlString.Create(containerBuilder.ToString(TagRenderMode.Normal));
        }
    }
}