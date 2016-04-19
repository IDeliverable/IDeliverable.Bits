using System.Linq;
using Orchard;
using Orchard.Autoroute.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace IDeliverable.Bits.Services
{
    public class CurrentContentAccessor : ICurrentContentAccessor
    {
        private readonly LazyField<ContentItem> _currentContentItemField = new LazyField<ContentItem>();
        private readonly IOrchardServices _orchardServices;

        public CurrentContentAccessor(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
            _currentContentItemField.Loader(GetCurrentContentItem);
        }

        public ContentItem CurrentContentItem
        {
            get { return _currentContentItemField.Value; }
        }

        private ContentItem GetCurrentContentItem()
        {
            var alias = _orchardServices.WorkContext.HttpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(1).Trim('/');
            var autoroutePart = _orchardServices.ContentManager.Query<AutoroutePart, AutoroutePartRecord>().Where(x => x.DisplayAlias == alias).Slice(0, 1).SingleOrDefault();
            return autoroutePart?.ContentItem;
        }
    }
}