using System.Web.Routing;
using Orchard;

namespace IDeliverable.Bits.Services
{
    public interface IRouteValuesProcessor : IDependency
    {
        RouteValueDictionary Parse(string routeValuesText);
    }
}