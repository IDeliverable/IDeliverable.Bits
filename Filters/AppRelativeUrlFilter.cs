using System;
using System.Linq;
using System.Web.Mvc;
using Orchard.Environment.Extensions;
using Orchard.Services;

namespace IDeliverable.Bits.Filters
{
    [OrchardFeature("IDeliverable.Bits.AppRelativeUrlFilter")]
    public class AppRelativeUrlFilter : IHtmlFilter
    {
        private readonly string mAppRoot;

        public AppRelativeUrlFilter(UrlHelper urlHelper)
        {
            mAppRoot = urlHelper.Content("~/");
        }

        public string ProcessContent(string text, string flavor)
        {
            if (!StringEqualsAny(flavor, "html", "markdown"))
                return text;
            
            return text.Replace("~/", mAppRoot);
        }

        private bool StringEqualsAny(string subject, params string[] strings)
        {
            return strings.Any(x => String.Equals(subject, x, StringComparison.OrdinalIgnoreCase));
        }
    }
}