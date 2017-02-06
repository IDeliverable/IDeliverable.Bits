using System.Linq;
using HtmlAgilityPack;
using IDeliverable.Bits.Summarizer.Helpers;
using IDeliverable.Bits.Summarizer.Services;

namespace IDeliverable.Bits.Summarizer.Generators
{
    public class ParagraphsSummaryGenerator : ISummaryGenerator
    {
        public string Generate(SummarizeContext context)
        {
            var document = new HtmlDocument();
            var html = context.Html.SanitizeHtml(context.HtmlWhiteList);
            document.LoadHtml(html);
            var paragraphs = document.DocumentNode.Elements("p").Take(context.BoundaryCount);
            var htmlExcerpt = string.Join("\r\n", paragraphs.Select(p => p.OuterHtml));
            return htmlExcerpt;
        }
    }
}