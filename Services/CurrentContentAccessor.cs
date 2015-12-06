using System;
using System.Linq;
using System.Web.Routing;
using Orchard;
using Orchard.Alias;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace IDeliverable.Bits.Services
{
    public class CurrentContentAccessor : ICurrentContentAccessor
    {
        private readonly LazyField<ContentItem> _currentContentItemField = new LazyField<ContentItem>();
        private readonly IContentManager _contentManager;
        private readonly IAliasService _aliasService;
        private readonly IWorkContextAccessor _wca;

        public CurrentContentAccessor(IContentManager contentManager, IAliasService aliasService, IWorkContextAccessor wca)
        {
            _contentManager = contentManager;
            _aliasService = aliasService;
            _wca = wca;
            _currentContentItemField.Loader(GetCurrentContentItem);
        }

        public ContentItem CurrentContentItem
        {
            get { return _currentContentItemField.Value; }
        }

        private ContentItem GetCurrentContentItem()
        {
            var contentId = GetCurrentContentItemId();
            return contentId == null ? null : _contentManager.Get(contentId.Value);
        }

        private int? GetCurrentContentItemId()
        {
            var itemRoute = _aliasService.Get(_wca.GetContext().HttpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(1).Trim('/'));
            if (itemRoute == null)
                return null;

            var key = FindKey(itemRoute);
            return key != null ? Convert.ToInt32(itemRoute[key]) : default(int?);
        }

        private static string FindKey(RouteValueDictionary itemRoute)
        {
            var supportedKeys = new[] { "id", "blogid" };
            return supportedKeys.FirstOrDefault(itemRoute.ContainsKey);
        }
    }
}