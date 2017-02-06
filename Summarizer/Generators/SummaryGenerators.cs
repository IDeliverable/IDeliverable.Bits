namespace IDeliverable.Bits.Summarizer.Generators
{
    public static class SummaryGenerators
    {
        public static string PlainTextWords = typeof(PlainTextWordsSummaryGenerator).Name;
        public static string Characters = typeof (CharactersSummaryGenerator).Name;
        public static string Words = typeof (WordsSummaryGenerator).Name;
        public static string Paragraphs = typeof(ParagraphsSummaryGenerator).Name;
    }
}