using System;
using System.Linq;
using IDeliverable.Bits.Services;
using Orchard;
using Orchard.Conditions.Services;
using Orchard.Logging;

namespace IDeliverable.Bits.ConditionProviders
{
    public class ContentItemTypeConditionProvider : Component, IConditionProvider
    {
        private const string RuleFunctionName = "contentitemtype";
        private readonly IWorkContextAccessor mWorkContextAccessor;
        private readonly IRouteValuesProcessor mRouteValuesProcessor;
        private readonly ICurrentContentAccessor _currentContentAccessor;

        public ContentItemTypeConditionProvider(ICurrentContentAccessor currentContentAccessor) {
            _currentContentAccessor = currentContentAccessor;
        }

        public void Evaluate(ConditionEvaluationContext context)
        {
            // Example: contentitemtype("Page").
            if (!String.Equals(context.FunctionName, RuleFunctionName, StringComparison.OrdinalIgnoreCase))
                return;

            var contentTypeName = GetArgument(context, 0);
            var result = _currentContentAccessor.CurrentContentItem?.ContentType == contentTypeName;

            Logger.Debug("Layer rule {0}({1}) evaluated to {2}.", RuleFunctionName, contentTypeName, result);

            context.Result = result;
        }
        
        private static string GetArgument(ConditionEvaluationContext context, int index)
        {
            return context.Arguments.Count() - 1 >= index ? (string)context.Arguments[index] : default(string);
        }
    }
}