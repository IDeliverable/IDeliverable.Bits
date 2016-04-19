using System;
using System.Collections.Generic;
using System.Linq;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Projections.Descriptors.Filter;
using Orchard.Projections.Services;
using Orchard.Taxonomies.Models;
using Orchard.Taxonomies.Services;

namespace IDeliverable.Bits.Projections
{
    [OrchardFeature("IDeliverable.Bits.Projections")]
    public class TaxonomyTermsFilter : Component, IFilterProvider
    {
        private readonly ITaxonomyService mTaxonomyService;
        private int mTermsFilterId;

        public TaxonomyTermsFilter(ITaxonomyService taxonomyService)
        {
            mTaxonomyService = taxonomyService;
        }

        public void Describe(DescribeFilterContext describe)
        {
            describe
                .For("Taxonomy", T("Taxonomy"), T("Taxonomy"))
                .Element("HasTermIds", T("Has Term IDs"), T("Categorized content items containing a list of specified term IDs"), ApplyFilter, DisplayFilter, "EnterTerms");
        }

        public void ApplyFilter(FilterContext context)
        {
            var termIds = (string)context.State.TermIds;

            if (String.IsNullOrEmpty(termIds))
                return;

            var ids = termIds.Split(',').Select(Int32.Parse).ToArray();

            if (ids.Length == 0)
                return;

            int op = Convert.ToInt32(context.State.Operator);
            var terms = ids.Select(mTaxonomyService.GetTerm).ToList();
            var allChildren = new List<TermPart>();

            foreach (var term in terms)
            {
                allChildren.AddRange(mTaxonomyService.GetChildren(term));
                allChildren.Add(term);
            }

            allChildren = allChildren.Distinct().ToList();

            var allIds = allChildren.Select(x => x.Id).ToList();

            switch (op)
            {
                case 0: // is one of
                    // Unique alias so we always get a unique join everytime so can have > 1 HasTerms filter on a query.
                    Action<IAliasFactory> s = alias => alias.ContentPartRecord<TermsPartRecord>().Property("Terms", "terms" + mTermsFilterId++);
                    Action<IHqlExpressionFactory> f = x => x.InG("TermRecord.Id", allIds);
                    context.Query.Where(s, f);
                    break;
                case 1: // is all of
                    foreach (var id in allIds)
                    {
                        var termId = id;
                        Action<IAliasFactory> selector =
                            alias => alias.ContentPartRecord<TermsPartRecord>().Property("Terms", "terms" + termId);
                        Action<IHqlExpressionFactory> filter = x => x.Eq("TermRecord.Id", termId);
                        context.Query.Where(selector, filter);
                    }
                    break;
            }
        }

        public LocalizedString DisplayFilter(FilterContext context)
        {
            var terms = (string)context.State.TermIds;

            if (String.IsNullOrEmpty(terms))
                return T("Any term");

            int op = Convert.ToInt32(context.State.Operator);
            switch (op)
            {
                case 0:
                    return T("Categorized with one of {0}", terms);

                case 1:
                    return T("Categorized with all of {0}", terms);
            }

            return T("Categorized with {0}", terms);
        }
    }
}