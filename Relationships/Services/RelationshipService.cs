using System;
using System.Collections.Generic;
using System.Linq;
using IDeliverable.Bits.Relationships.Models;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Layouts.Helpers;

namespace IDeliverable.Bits.Relationships.Services
{
    [OrchardFeature("IDeliverable.Bits.Relationships")]
    public class RelationshipService : IRelationshipService
    {
        private readonly IRepository<RelationshipRecord> mRepository;
        private readonly IContentManager mContentManager;

        public RelationshipService(IRepository<RelationshipRecord> repository, IContentManager contentManager)
        {
            mRepository = repository;
            mContentManager = contentManager;
        }

        public IEnumerable<T> GetRelatedContentItems<T>(int contentId, VersionOptions options, QueryHints hints, string discriminator = null) where T: class, IContent
        {
            var query = from record in mRepository.Table where record.ContentItemId == contentId select record;

            if (!String.IsNullOrEmpty(discriminator))
                query = from record in query where record.Discriminator == discriminator select record;

            var relatedContentItemIds = query.Select(x => x.RelatedContentItemId).ToList();
            return mContentManager.GetMany<T>(relatedContentItemIds, options, hints);
        }

        public IDictionary<string, IEnumerable<T>> GetRelatedByContentItems<T>(int contentId, VersionOptions options, QueryHints hints) where T : class, IContent
        {
            var query = from record in mRepository.Table where record.RelatedContentItemId == contentId select record;
            var records = query.ToList();
            var relatedContentItemIds = records.Select(x => x.ContentItemId).ToList();
            var relatedContentItems = mContentManager.GetMany<T>(relatedContentItemIds, options, hints).ToLookup(x => x.Id);
            var dictionary = new Dictionary<string, IEnumerable<T>>();

            foreach (var record in records)
            {
                if(!dictionary.ContainsKey(record.Discriminator))
                    dictionary.Add(record.Discriminator, new List<T>());

                var list = (IList<T>)dictionary[record.Discriminator];
                
                list.AddRange(relatedContentItems[record.ContentItemId]);
            }

            return dictionary;
        }

        public void SetRelationships(int contentItemId, string discriminator, IEnumerable<int> relatedContentItemIds)
        {
            BreakRelationships(contentItemId, discriminator);

            foreach (var relatedContentItemId in relatedContentItemIds)
            {
                mRepository.Create(new RelationshipRecord
                {
                    ContentItemId = contentItemId,
                    Discriminator = discriminator,
                    RelatedContentItemId = relatedContentItemId
                });
            }
        }

        public void BreakRelationships(int contentItemId, string discriminator = null)
        {
            var query = from record in mRepository.Table where record.ContentItemId == contentItemId select record;

            if(!String.IsNullOrEmpty(discriminator))
                query = from record in query where record.Discriminator == discriminator select record;

            foreach (var record in query)
            {
                mRepository.Delete(record);
            }
        }
    }
}