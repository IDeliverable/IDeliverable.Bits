using System;
using System.Linq;
using Orchard;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Projections.Descriptors.Filter;
using Orchard.Projections.Services;

namespace IDeliverable.Bits.Projections
{
    [OrchardFeature("IDeliverable.Bits.Projections")]
    public class ExclusionFilter : Component, IFilterProvider
    {
        public void Describe(DescribeFilterContext describe)
        {
            describe
                .For("Content", T("Content"), T("Content"))
                .Element("Exclude", T("Exclude"), T("Exclude specific content items"), ApplyFilter, DisplayFilter, "ExcludeContentItems");
        }

        public void ApplyFilter(FilterContext context)
        {
            var contentItemIds = (string)context.State.ContentItemIds;

            if (String.IsNullOrEmpty(contentItemIds))
                return;
            
            var ids = contentItemIds.Split(',').Select(Int32.Parse).ToArray();
            
            foreach (var id in ids) {
                context.Query.Where(alias => alias.ContentItem(), filter => filter.Not(n => n.Eq("Id", id)));
            }
        }

        public LocalizedString DisplayFilter(FilterContext context)
        {
            var contentItemIds = (string)context.State.ContentItemIds;

            if (String.IsNullOrEmpty(contentItemIds))
                return T("Exclude none");
            
            return T("Exclude {0}", contentItemIds);
        }
    }
}