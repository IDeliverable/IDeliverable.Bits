using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Fields.Fields;
using Orchard.Localization;
using Orchard.Taxonomies.Helpers;
using Orchard.Taxonomies.Models;
using Orchard.Taxonomies.Services;
using Orchard.UI.Navigation;

namespace IDeliverable.Bits.Navigation.Filters
{
    /// <summary>
    /// Dynamically injects taxonomy items as menu items on TaxonomyNavigationMenuItem elements. In addition, if the "IncludeRelatedContent" boolean field is set, the associated content items are injected as menu items as well.
    /// The code is largely a copy of the original <see cref="Orchard.Taxonomies.Navigation.TaxonomyNavigationProvider"/> class.
    /// A Github issue was opened to request an option so we can get rid of this custom implementation: https://github.com/OrchardCMS/Orchard/issues/6846.
    /// </summary>
    [OrchardFeature("IDeliverable.Bits.Navigation")]
    [OrchardSuppressDependency("Orchard.Taxonomies.Navigation")]
    public class TaxonomyNavigationProvider : INavigationFilter
    {
        public TaxonomyNavigationProvider(IContentManager contentManager, ITaxonomyService taxonomyService)
        {
            mContentManager = contentManager;
            mTaxonomyService = taxonomyService;
        }

        private readonly IContentManager mContentManager;
        private readonly ITaxonomyService mTaxonomyService;

        public IEnumerable<MenuItem> Filter(IEnumerable<MenuItem> items)
        {
            foreach (var item in items)
            {
                if (item.Content != null && item.Content.ContentItem.ContentType == "TaxonomyNavigationMenuItem")
                {
                    var taxonomyNavigationPart = item.Content.As<TaxonomyNavigationPart>();
                    var includeAssociatedContent = ((BooleanField)taxonomyNavigationPart.Get(typeof(BooleanField), "IncludeAssociatedContent"))?.Value == true;
                    var rootTerm = mTaxonomyService.GetTerm(taxonomyNavigationPart.TermId);

                    TermPart[] allTerms;

                    if (rootTerm != null)
                    {
                        // if DisplayRootTerm is specified add it to the menu items to render
                        allTerms = mTaxonomyService.GetChildren(rootTerm, taxonomyNavigationPart.DisplayRootTerm).ToArray();
                    }
                    else
                    {
                        allTerms = mTaxonomyService.GetTerms(taxonomyNavigationPart.TaxonomyId).ToArray();
                    }

                    var rootLevel = rootTerm != null
                        ? rootTerm.GetLevels()
                        : 0;

                    var menuPosition = item.Position;
                    var rootPath = rootTerm == null || taxonomyNavigationPart.DisplayRootTerm ? "" : rootTerm.FullPath;

                    var startLevel = rootLevel + 1;
                    if (rootTerm == null || taxonomyNavigationPart.DisplayRootTerm)
                    {
                        startLevel = rootLevel;
                    }

                    var endLevel = Int32.MaxValue;
                    if (taxonomyNavigationPart.LevelsToDisplay > 0)
                    {
                        endLevel = startLevel + taxonomyNavigationPart.LevelsToDisplay - 1;
                    }

                    foreach (var term in allTerms)
                    {
                        if (term != null)
                        {
                            var part = term;
                            var level = part.GetLevels();

                            // Filter levels?
                            if (level < startLevel || level > endLevel)
                            {
                                continue;
                            }

                            // Ignore menu item if there are no content items associated to the term.
                            if (taxonomyNavigationPart.HideEmptyTerms && part.Count == 0)
                            {
                                continue;
                            }

                            var termMetadata = mContentManager.GetItemMetadata(part);
                            var termMenuText = termMetadata.DisplayText;
                            var termRoutes = termMetadata.DisplayRouteValues;

                            if (taxonomyNavigationPart.DisplayContentCount)
                            {
                                termMenuText += " (" + part.Count + ")";
                            }

                            // Create.
                            var positions = term.FullPath.Substring(rootPath.Length)
                                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(p => Array.FindIndex(allTerms, t => t.Id == Int32.Parse(p)))
                                .ToArray();

                            var termMenuPosition = menuPosition + ":" + String.Join(".", positions.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray());

                            var termMenuItem = new MenuItem
                            {
                                Text = new LocalizedString(termMenuText),
                                IdHint = item.IdHint,
                                Classes = item.Classes,
                                Url = item.Url,
                                Href = item.Href,
                                LinkToFirstChild = false,
                                RouteValues = termRoutes,
                                LocalNav = item.LocalNav,
                                Items = new MenuItem[0],
                                Position = termMenuPosition,
                                Permissions = item.Permissions,
                                Content = part
                            };

                            // Include associated content?
                            if(includeAssociatedContent)
                            {
                                var contentItems = mTaxonomyService.GetContentItems(term).ToList();
                                var contentMenuItems = new List<MenuItem>(contentItems.Count);
                                var contentMenuPosition = 0;

                                foreach(var contentItem in contentItems)
                                {
                                    var metadata = mContentManager.GetItemMetadata(contentItem);
                                    var menuText = metadata.DisplayText;
                                    var routes = metadata.DisplayRouteValues;

                                    var menuItem = new MenuItem
                                    {
                                        Text = new LocalizedString(menuText),
                                        IdHint = item.IdHint,
                                        Classes = item.Classes,
                                        Url = item.Url,
                                        Href = item.Href,
                                        LinkToFirstChild = false,
                                        RouteValues = routes,
                                        LocalNav = item.LocalNav,
                                        Items = new MenuItem[0],
                                        Position = termMenuPosition + "." + contentMenuPosition++,
                                        Permissions = item.Permissions,
                                        Content = contentItem
                                    };

                                    contentMenuItems.Add(menuItem);
                                }

                                termMenuItem.Items = contentMenuItems;
                            }

                            yield return termMenuItem;
                        }
                    }
                }
                else
                {
                    yield return item;
                }
            }
        }
    }
}