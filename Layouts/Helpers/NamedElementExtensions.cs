using Orchard.Layouts.Framework.Elements;
using Orchard.Layouts.Helpers;

namespace IDeliverable.Bits.Layouts.Helpers {
    public static class NamedElementExtensions {
        public static string GetName(this Element element) {
            return element.Retrieve<string>("Name");
        }

        public static void SetName(this Element element, string value) {
            element.Store("Name", value);
        }

        public static string GetAlternate(this Element element) {
            return element.Retrieve<string>("Alternate");
        }

        public static void SetAlternate(this Element element, string value) {
            element.Store("Alternate", value);
        }
    }
}