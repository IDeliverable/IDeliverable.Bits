using Orchard;
using IDeliverable.Bits.Summarizer.Services;

namespace IDeliverable.Bits.Summarizer.Generators
{
    public interface ISummaryGenerator : IDependency
    {
        string Generate(SummarizeContext context);
    }
}