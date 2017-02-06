using System;
using System.Linq;
using System.Web.Routing;

namespace MiP.Cms.Helpers
{
    public static class RouteValueDictionaryExtensions
    {
        public static string ToQueryString(this RouteValueDictionary routeValues)
        {
            return String.Join("&", routeValues.Select(x => String.Format("{0}={1}", x.Key, x.Value)));
        }
    }
}