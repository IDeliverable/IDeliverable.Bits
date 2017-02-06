using System;
using System.Collections.Generic;
using System.Linq;
using IDeliverable.Bits.Summarizer.Generators;
using Orchard.Services;

namespace IDeliverable.Bits.Summarizer.Services
{
    public class Summarizer : ISummarizer
    {
        public Summarizer(IEnumerable<ISummaryGenerator> generators, IEnumerable<IHtmlFilter> filters)
        {
            mGenerators = generators;
            mFilters = filters;
        }

        private readonly IEnumerable<ISummaryGenerator> mGenerators;
        private readonly IEnumerable<IHtmlFilter> mFilters;

        public string Summarize(string text, string flavor, int boundaryCount = 20, string ellipses = "...")
        {
            return Summarize(text, flavor, SummaryGenerators.Characters, boundaryCount, ellipses);
        }

        public string Summarize(string text, string flavor, string generatorType, int boundaryCount = 20, string ellipses = "...")
        {
            return Summarize(new SummarizeContext
            {
                Text = text,
                Flavor = flavor,
                BoundaryCount = boundaryCount,
                Ellipses = ellipses,
                GeneratorType = generatorType
            });
        }

        public string Summarize(SummarizeContext context)
        {
            var flavor = context.Flavor;
            var text = context.Text;
            var generatorType = !String.IsNullOrWhiteSpace(context.GeneratorType) ? context.GeneratorType : SummaryGenerators.Characters;
            var html = mFilters.Where(x => x.GetType().Name.Equals(flavor + "filter", StringComparison.OrdinalIgnoreCase)).Aggregate(text, (t, filter) => filter.ProcessContent(t, flavor));

            context.Html = html;
            var generator = mGenerators.FirstOrDefault(p => p.GetType().Name == generatorType);

            if(generator == null)
                throw new InvalidOperationException(String.Format("No summary generator of type {0} was found.", generatorType));

            return generator.Generate(context);
        }
    }
}