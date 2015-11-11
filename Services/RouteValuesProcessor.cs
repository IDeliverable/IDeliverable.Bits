using System;
using System.Web.Routing;
using Orchard.Tokens;

namespace IDeliverable.Bits.Services
{
    public class RouteValuesProcessor : IRouteValuesProcessor
    {
        public RouteValuesProcessor(ITokenizer tokenizer)
        {
            mTokenizer = tokenizer;
        }

        private readonly ITokenizer mTokenizer;

        public RouteValueDictionary Parse(string routeValuesText)
        {
            var routeValues = new RouteValueDictionary();

            if (!String.IsNullOrWhiteSpace(routeValuesText))
            {
                var items = routeValuesText.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in items)
                {
                    var pair = item.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                    if (pair.Length == 2)
                    {
                        var key = pair[0].Trim();
                        var value = pair[1].Trim();

                        value = mTokenizer.Replace(value, null, new ReplaceOptions
                        {
                            Encoding = ReplaceOptions.NoEncode
                        });

                        routeValues[key] = value;
                    }
                }
            }

            return routeValues;
        }
    }
}