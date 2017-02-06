using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Orchard;
using Orchard.DisplayManagement.Implementation;
using Orchard.DisplayManagement.Shapes;
using Orchard.Environment.Extensions;

namespace IDeliverable.Bits.Navigation.Shapes
{
    [OrchardFeature("IDeliverable.Bits.Navigation")]
    public class RouteValuesAlternatesProvider : ShapeDisplayEvents
    {
        public RouteValuesAlternatesProvider(IWorkContextAccessor workContextAccessor)
        {
            mWorkContextAccessor = workContextAccessor;
        }

        private readonly IWorkContextAccessor mWorkContextAccessor;

        public override void Displaying(ShapeDisplayingContext context)
        {
            var routeValues = new RouteValueDictionary(mWorkContextAccessor.GetContext().HttpContext.Request.RequestContext.RouteData.Values);
            var list = new List<string> { TakeValue(routeValues, "area"), TakeValue(routeValues, "controller"), TakeValue(routeValues, "action") };

            list.AddRange(routeValues.OrderBy(x => x.Key).Select(x => (string)x.Value));
            AddAlternates(context.ShapeMetadata, list);
        }

        private static void AddAlternates(ShapeMetadata metadata, ICollection<string> values)
        {
            for (var i = 0; i < values.Count; i++)
            {
                var alternate = EncodeAlternate(String.Format("{0}__{1}", metadata.Type, String.Join("_", values.Take(i + 1))));
                metadata.Alternates.Add(alternate);
            }
        }

        private static string TakeValue(RouteValueDictionary routeValues, string key)
        {
            if (routeValues.ContainsKey(key))
            {
                var value = routeValues[key];
                routeValues.Remove(key);
                return (string)value;
            }
            return null;
        }

        private static string EncodeAlternate(string alternate)
        {
            return alternate.Replace("/", "__").Replace(".", "__");
        }
    }
}