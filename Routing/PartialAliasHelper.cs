using Orchard.Alias.Implementation.Holder;
using Orchard.Alias.Implementation.Map;
using System;
using System.Linq;

namespace IDeliverable.Bits.Routing
{
	public static class PartialAliasHelper
	{
		public static AliasInfo GetAlias(string requestPath, IAliasHolder aliasHolder)
		{
			var alias = GetClosestParentAlias(requestPath, aliasHolder.GetMap("Contents"), true);
			if (alias != null)
				return alias;

			foreach (var map in aliasHolder.GetMaps())
			{
				alias = GetClosestParentAlias(requestPath, map, true);
				if (alias != null)
					return alias;
			}

			return null;
		}

		public static AliasInfo GetClosestParentAlias(string virtualPath, AliasMap aliasMap, bool considerCurrentPath)
		{
			var aliases = aliasMap.GetAliases();
			var path = considerCurrentPath ? virtualPath : RemoveOneSubdirectoryFromPath(virtualPath);
			AliasInfo aliasInfo = null;

			while (path.Length > 0)
			{
				aliasInfo = aliases.Where(x => x.Path == path).SingleOrDefault();

				if (aliasInfo != null)
					break;

				path = RemoveOneSubdirectoryFromPath(path);
			}

			return aliasInfo;
		}

		private static string RemoveOneSubdirectoryFromPath(string path)
		{
			return String.Join("/", path.Split('/').Reverse().Skip(1).Reverse());
		}
	}
}