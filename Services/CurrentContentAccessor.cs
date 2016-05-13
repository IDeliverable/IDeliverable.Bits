using System.Linq;
using Orchard;
using Orchard.Autoroute.Models;
using Orchard.Autoroute.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace IDeliverable.Bits.Services
{
    public class CurrentContentAccessor : ICurrentContentAccessor
    {
        private readonly LazyField<ContentItem> _currentContentItemField = new LazyField<ContentItem>();
        private readonly IOrchardServices _orchardServices;
        private readonly IHomeAliasService _homeAliasService;

        public CurrentContentAccessor(IOrchardServices orchardServices, IHomeAliasService homeAliasService)
        {
            _orchardServices = orchardServices;
            _currentContentItemField.Loader(GetCurrentContentItem);
            _homeAliasService = homeAliasService;
        }

        public ContentItem CurrentContentItem
        {
            get { return _currentContentItemField.Value; }
        }

        private ContentItem GetCurrentContentItem()
        {
            var alias = _orchardServices.WorkContext.HttpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(1).Trim('/');
            
            if (alias == "")
            {
                var homePage = _homeAliasService.GetHomePage();
                return homePage != null ? homePage.ContentItem : null;
            }

            var autoroutePart = _orchardServices.ContentManager.Query<AutoroutePart, AutoroutePartRecord>().Where(x => x.DisplayAlias == alias).Slice(0, 1).SingleOrDefault();
            return autoroutePart != null ? autoroutePart.ContentItem : null;
        }
    }
}