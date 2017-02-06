using System;
using System.Collections.Generic;
using System.Web.Mvc;
using IDeliverable.Bits.Navigation.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Logging;
using Orchard.UI.Navigation;

namespace IDeliverable.Bits.Navigation.Filters
{
    /// <summary>
    /// Removes all action link menu items whose routes are not reachable, to prevent the entire menu from failing to render.
    /// </summary>
    [OrchardFeature("IDeliverable.Bits.Navigation")]
    public class ActionLinkNavigationFilter : Component, INavigationFilter
    {
        public ActionLinkNavigationFilter(UrlHelper urlHelper)
        {
            mUrlHelper = urlHelper;
        }

        private readonly UrlHelper mUrlHelper;

        public IEnumerable<MenuItem> Filter(IEnumerable<MenuItem> menuItems)
        {
            foreach (var menuItem in menuItems)
            {
                var actionLinkPart = menuItem.Content.As<ActionLinkPart>();
                if (actionLinkPart != null)
                {
                    var actionLinkUrl = mUrlHelper.RouteUrl(actionLinkPart.RouteValueDictionary);
                    if (String.IsNullOrEmpty(actionLinkUrl))
                    {
                        Logger.Warning("Action link '{0}' was suppressed because its configured route values could not be resolved into a URL.", menuItem.Text);
                        continue;
                    }
                }

                yield return menuItem;
            }
        }
    }
}