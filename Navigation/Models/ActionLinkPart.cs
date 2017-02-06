using System.Web.Routing;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace IDeliverable.Bits.Navigation.Models
{
    public class ActionLinkPart : ContentPart
    {
        internal readonly LazyField<RouteValueDictionary> RouteValueDictionaryField = new LazyField<RouteValueDictionary>();

        public string ActionName
        {
            get { return this.Retrieve(x => x.ActionName); }
            set { this.Store(x => x.ActionName, value); }
        }

        public string ControllerName
        {
            get { return this.Retrieve(x => x.ControllerName); }
            set { this.Store(x => x.ControllerName, value); }
        }

        public string AreaName
        {
            get { return this.Retrieve(x => x.AreaName); }
            set { this.Store(x => x.AreaName, value); }
        }

        public string RouteValues
        {
            get { return this.Retrieve(x => x.RouteValues); }
            set { this.Store(x => x.RouteValues, value); }
        }

        public RouteValueDictionary RouteValueDictionary
        {
            get { return RouteValueDictionaryField.Value; }
        }
    }
}