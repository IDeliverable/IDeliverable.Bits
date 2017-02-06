using System.Web;
using System.Web.Routing;
using Orchard.Alias.Implementation;
using Orchard.Alias.Implementation.Holder;
using Orchard.Alias.Implementation.Map;

namespace IDeliverable.Bits.Routing
{
	public class PartialAliasRoute : AliasRoute
	{
		public PartialAliasRoute(IAliasHolder aliasHolder, string areaName, IRouteHandler routeHandler)
			: base(aliasHolder, areaName, routeHandler)
		{
			mAliasMap = aliasHolder.GetMap(areaName);
			mRouteHandler = routeHandler;
		}

		private readonly AliasMap mAliasMap;
		private readonly IRouteHandler mRouteHandler;

		public override RouteData GetRouteData(HttpContextBase httpContext)
		{
			// Don't compute unnecessary virtual path if the map is empty.
			if (!mAliasMap.Any())
				return null;

			var requestPath = GetRequestPath(httpContext);

			var aliasInfo = PartialAliasHelper.GetClosestParentAlias(requestPath, mAliasMap, false);

			if (aliasInfo != null)
			{
				httpContext.Items.Add("AutoroutePath", aliasInfo.Path);
				return CreateRouteData(aliasInfo);
			}
				
			return null;
		}

		private string GetRequestPath(HttpContextBase httpContext)
		{
			return httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo;
		}

		private RouteData CreateRouteData(AliasInfo aliasInfo)
		{
			var data = new RouteData(this, mRouteHandler);
			foreach (var routeValue in aliasInfo.RouteValues)
			{
				var key = routeValue.Key;
				if (key.EndsWith("-"))
					data.Values.Add(key.Substring(0, key.Length - 1), routeValue.Value);
				else
					data.Values.Add(key, routeValue.Value);
			}

			data.Values["area"] = Area;
			data.DataTokens["area"] = Area;

			return data;
		}
	}
}