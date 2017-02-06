using IDeliverable.Bits.Summarizer.Generators;

namespace IDeliverable.Bits.Summarizer.Services
{
    public class SummarizeContext
    {
        public string Text { get; set; }
        public string Flavor { get; set; }
        public string Html { get; set; }
        public int BoundaryCount { get; set; }
        public string Ellipses { get; set; }
        public string[] HtmlWhiteList { get; set; }
        public string GeneratorType { get; set; }

        public SummarizeContext()
        {
            BoundaryCount = 20;
            Ellipses = "...";
            GeneratorType = SummaryGenerators.Characters;
            HtmlWhiteList = new[] {"b", "strong", "p", "em", "i", "a"};
        }
    }
}