using System.Collections.Generic;
using Orchard;
using Orchard.ContentManagement;

namespace IDeliverable.Bits.ConnectedContent.Services
{
    public interface IContentConnectionService : IDependency
    {
        IEnumerable<T> GetConnectedItems<T>(int contentId, VersionOptions options, QueryHints hints, string groupName = null) where T : class, IContent;
        IDictionary<string, IEnumerable<T>> GetConnectedByItems<T>(int contentId, VersionOptions options, QueryHints hints) where T : class, IContent;
        void SetConnectedItems(int contentId, string groupName, IEnumerable<int> connectedItemIds);
        void DisconnectItems(int contentId, string groupName = null);
    }
}