using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using IDeliverable.Bits.Summarizer.Services;

namespace IDeliverable.Bits.Summarizer.Generators
{
    public class PlainTextWordsSummaryGenerator : ISummaryGenerator
    {
        public string Generate(SummarizeContext context)
        {
            var ingress = Regex.Replace(context.Html, "<(.|\n)+?>", String.Empty, RegexOptions.IgnoreCase);
            if (!String.IsNullOrEmpty(ingress))
            {
                var original = ingress;
                var wordColl = Regex.Matches(original, @"(\S+\s+)");
                if (wordColl.Count > context.BoundaryCount)
                {
                    var word = new StringBuilder();
                    foreach (var subWord in wordColl.Cast<Match>().Select(r => r.Value).Take(context.BoundaryCount))
                        word.Append(subWord);
                    ingress = word.ToString().Trim() + context.Ellipses;
                }
            }

            return ingress;
        }
    }
}