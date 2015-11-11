using System;
using System.Collections.Generic;
using Orchard;
using Orchard.Core.Navigation.Services;
using Orchard.Environment.Extensions;
using Orchard.Logging;
using Orchard.UI.Navigation;

namespace IDeliverable.Bits.Navigation.Filters
{
    /// <summary>
    /// Dynamically injects all menu items from the system.
    /// </summary>
    [OrchardFeature("IDeliverable.Bits.Navigation")]
    public class OmniNavigationProvider : Component, INavigationFilter
    {
        private readonly IMenuService mMenuService;
        private readonly Lazy<INavigationManager> mNavigationManager; // Using Lazy because NavigationManager has a dependency on IEnumerable<NavigationFilter>.
        private readonly Stack<int> mCallStack = new Stack<int>();

        public OmniNavigationProvider(IMenuService menuService, Lazy<INavigationManager> navigationManager)
        {
            mMenuService = menuService;
            mNavigationManager = navigationManager;
        }

        public IEnumerable<MenuItem> Filter(IEnumerable<MenuItem> items)
        {
            foreach (var item in items)
            {
                if (item.Content != null && item.Content.ContentItem.ContentType == "OmniNavigationMenuItem")
                {
                    if (mCallStack.Contains(item.Content.Id))
                    {
                        Logger.Debug("Skipping omni navigation menu item as it is the root of the menu tree.");
                        continue;
                    }

                    var menus = mMenuService.GetMenus();

                    foreach (var menu in menus)
                    {
                        mCallStack.Push(item.Content.Id);
                        var menuItems = mNavigationManager.Value.BuildMenu(menu);
                        mCallStack.Pop();

                        foreach (var menuItem in menuItems)
                        {
                            yield return menuItem;
                        }
                    }
                }
                else {
                    yield return item;
                }
            }
        }
    }
}