using System;
using System.Collections.Generic;
using System.Linq;
using IDeliverable.Bits.ConnectedContent.Models;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Layouts.Helpers;

namespace IDeliverable.Bits.ConnectedContent.Services
{
    [OrchardFeature("IDeliverable.Bits.ConnectedContent")]
    public class ContentConnectionService : IContentConnectionService
    {
        private readonly IRepository<ContentConnectionRecord> mRepository;
        private readonly IContentManager mContentManager;

        public ContentConnectionService(IRepository<ContentConnectionRecord> repository, IContentManager contentManager)
        {
            mRepository = repository;
            mContentManager = contentManager;
        }

        public IEnumerable<T> GetConnectedItems<T>(int contentId, VersionOptions options, QueryHints hints, string groupName = null) where T: class, IContent
        {
            var query = from record in mRepository.Table where record.ContentId == contentId select record;

            if (!String.IsNullOrEmpty(groupName))
                query = from record in query where record.GroupName == groupName select record;

            var connectedItemIds = query.Select(x => x.ConnectedContentId).ToList();
            return mContentManager.GetMany<T>(connectedItemIds, options, hints);
        }

        public IDictionary<string, IEnumerable<T>> GetConnectedByItems<T>(int contentId, VersionOptions options, QueryHints hints) where T : class, IContent
        {
            var query = from record in mRepository.Table where record.ConnectedContentId == contentId select record;
            var records = query.ToList();
            var connectedItemIds = records.Select(x => x.ContentId).ToList();
            var contentItems = mContentManager.GetMany<T>(connectedItemIds, options, hints).ToLookup(x => x.Id);
            var dictionary = new Dictionary<string, IEnumerable<T>>();

            foreach (var record in records)
            {
                if(!dictionary.ContainsKey(record.GroupName))
                    dictionary.Add(record.GroupName, new List<T>());

                var list = (IList<T>)dictionary[record.GroupName];
                
                list.AddRange(contentItems[record.ContentId]);
            }

            return dictionary;
        }

        public void SetConnectedItems(int contentId, string groupName, IEnumerable<int> connectedItemIds)
        {
            DisconnectItems(contentId, groupName);

            foreach (var connectedItemId in connectedItemIds)
            {
                mRepository.Create(new ContentConnectionRecord
                {
                    ContentId = contentId,
                    GroupName = groupName,
                    ConnectedContentId = connectedItemId
                });
            }
        }

        public void DisconnectItems(int contentId, string groupName = null)
        {
            var query = from record in mRepository.Table where record.ContentId == contentId select record;

            if(!String.IsNullOrEmpty(groupName))
                query = from record in query where record.GroupName == groupName select record;

            foreach (var record in query)
            {
                mRepository.Delete(record);
            }
        }
    }
}