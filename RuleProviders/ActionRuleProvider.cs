using System;
using System.Linq;
using System.Web.Routing;
using IDeliverable.Bits.Services;
using Orchard;
using Orchard.Logging;
using Orchard.UI.Navigation;
using Orchard.Widgets.Services;

namespace IDeliverable.Bits.RuleProviders
{
    public class ActionRuleProvider : Component, IRuleProvider
    {
        public ActionRuleProvider(IWorkContextAccessor workContextAccessor, IRouteValuesProcessor routeValuesProcessor)
            : base()
        {
            mWorkContextAccessor = workContextAccessor;
            mRouteValuesProcessor = routeValuesProcessor;
        }

        private const string mRuleFunctionName = "action";
        private readonly IWorkContextAccessor mWorkContextAccessor;
        private readonly IRouteValuesProcessor mRouteValuesProcessor;

        public void Process(RuleContext context)
        {
            // Example: action("action", "controller", "area", "value1=a, value2=b")
            if (!String.Equals(context.FunctionName, mRuleFunctionName, StringComparison.OrdinalIgnoreCase))
                return;

            var actionName = GetArgument(context, 0);
            var controllerName = GetArgument(context, 1);
            var areaName = GetArgument(context, 2);
            var routeValueString = GetArgument(context, 3);
            var routeValues = ParseRouteValues(routeValueString);

            SetRouteValueIfNotEmpty(routeValues, mRuleFunctionName, actionName);
            SetRouteValueIfNotEmpty(routeValues, "controller", controllerName);
            SetRouteValueIfNotEmpty(routeValues, "area", areaName);

            var routeData = mWorkContextAccessor.GetContext().HttpContext.Request.RequestContext.RouteData;
            var result = NavigationHelper.RouteMatches(routeValues, routeData.Values);

            Logger.Debug("Layer rule {0}({1}, {2}, {3}, {4}) evaluated to {5}.", mRuleFunctionName, actionName, controllerName, areaName, routeValueString, result);

            context.Result = result;
        }

        private static void SetRouteValueIfNotEmpty(RouteValueDictionary routeValues, string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                routeValues[key] = value;
        }

        private RouteValueDictionary ParseRouteValues(string routeValuesText)
        {
            return mRouteValuesProcessor.Parse(routeValuesText);
        }

        private static string GetArgument(RuleContext context, int index)
        {
            return context.Arguments.Count() - 1 >= index ? (string)context.Arguments[index] : default(string);
        }
    }
}