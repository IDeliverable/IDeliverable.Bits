using System;
using System.Collections.Generic;
using System.Linq;
using IDeliverable.Bits.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Projections.Descriptors.Filter;
using Orchard.Projections.Services;
using Orchard.Taxonomies.Fields;
using Orchard.Taxonomies.Models;
using Orchard.Taxonomies.Services;

namespace IDeliverable.Bits.Projections
{
    [OrchardFeature("IDeliverable.Bits.Projections")]
    public class RelatedContentFilter : Component, IFilterProvider
    {
        private readonly ITaxonomyService mTaxonomyService;
        private readonly ICurrentContentAccessor mCurrentContentAccessor;

        public RelatedContentFilter(ITaxonomyService taxonomyService, ICurrentContentAccessor currentContentAccessor)
        {
            mTaxonomyService = taxonomyService;
            mCurrentContentAccessor = currentContentAccessor;
        }

        public void Describe(DescribeFilterContext describe)
        {
            describe
                .For("Taxonomy", T("Taxonomy"), T("Taxonomy"))
                .Element("RelatedContent", T("Related Content"), T("Related content based on the specified taxonomy field of the current content item"), ApplyFilter, DisplayFilter, "RelatedContent");
        }

        public void ApplyFilter(FilterContext context)
        {
            var taxonomyFieldExpression = (string)context.State.TaxonomyFieldExpression;

            if (String.IsNullOrWhiteSpace(taxonomyFieldExpression))
                return;

            var expressionParts = taxonomyFieldExpression.Split('.').Select(x => x.Trim()).ToArray();

            if (!expressionParts.Any())
                return;

            var currentContentItem = mCurrentContentAccessor.CurrentContentItem;
            var taxonomyFieldQuery = expressionParts.Length == 1
                ? from part in currentContentItem.Parts from field in part.Fields where field.FieldDefinition.Name == typeof(TaxonomyField).Name && field.Name == expressionParts[0] select field
                : from part in currentContentItem.Parts where part.PartDefinition.Name == expressionParts[0] select part.Fields.FirstOrDefault(x => x.Name == expressionParts[1]);

            var taxonomyField = taxonomyFieldQuery.FirstOrDefault() as TaxonomyField;

            if (taxonomyField == null)
                return;

            var terms = taxonomyField.Terms.ToArray();

            if (!terms.Any())
                return;

            // Expand terms.
            var allChildren = ExpandTerms(terms).Distinct().ToList();
            var allIds = allChildren.Select(x => x.Id).ToList();

            // All content items that share any of the terms of the current content item.
            Action<IAliasFactory> s = alias => alias.ContentPartRecord<TermsPartRecord>().Property("Terms", "relatedterms" + currentContentItem.Id);
            Action<IHqlExpressionFactory> f = x => x.InG("TermRecord.Id", allIds);

            // Exclude the current content item from the results.
            context.Query.Where(alias => alias.ContentItem(), filter => filter.Not(n => n.Eq("Id", currentContentItem.Id)));

            context.Query.Where(s, f);
        }

        public LocalizedString DisplayFilter(FilterContext context)
        {
            var taxonomyFieldExpression = (string)context.State.TaxonomyFieldExpression;

            return String.IsNullOrEmpty(taxonomyFieldExpression)
                ? T("All content items")
                : T("Related content items whose {0} field share one or more terms with the current content item", taxonomyFieldExpression);
        }

        private IEnumerable<TermPart> ExpandTerms(IEnumerable<TermPart> terms)
        {
            foreach (var term in terms)
            {
                var children = mTaxonomyService.GetChildren(term);
                foreach (var childTerm in ExpandTerms(children))
                {
                    yield return childTerm;
                }
                yield return term;
            }
        }
    }
}