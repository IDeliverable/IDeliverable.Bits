using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using IDeliverable.Bits.RegularExpressions;
using Orchard.Environment.Extensions;
using Orchard.Services;

namespace IDeliverable.Bits.Filters
{
    [OrchardFeature("IDeliverable.Bits.AppRelativeUrlFilter")]
    public class AppRelativeUrlFilter : IHtmlFilter
    {
        /// <summary>
        /// A precompiled regular expression of ("|')(?<path>~/.*?)("|')
        /// </summary>
        private static readonly Regex AppRelativeUrlRegex = new AppRelativeUrl();
        private readonly UrlHelper m_urlHelper;

        public AppRelativeUrlFilter(UrlHelper urlHelper)
        {
            m_urlHelper = urlHelper;
        }

        public string ProcessContent(string text, string flavor)
        {
            if (!StringEqualsAny(flavor, "html", "markdown"))
                return text;

            text = AppRelativeUrlRegex.Replace(text, match =>
            {
                var appRelativeUrl = match.Groups["path"].Value;
                var relativeUrl = m_urlHelper.Content(appRelativeUrl);

                return match.Value.Replace(appRelativeUrl, relativeUrl);
            });

            return text;
        }

        private bool StringEqualsAny(string subject, params string[] strings)
        {
            return strings.Any(x => String.Equals(subject, x, StringComparison.OrdinalIgnoreCase));
        }
    }
}