using System;
using System.Web.Mvc;
using IDeliverable.Bits.Summarizer.Generators;
using IDeliverable.Bits.Summarizer.Services;
using Orchard.Core.Common.Models;
using Orchard.Core.Common.Settings;
using Orchard.Mvc.Html;

namespace IDeliverable.Bits.Summarizer.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static string Summarize(this HtmlHelper htmlHelper, string text, string flavor, int boundaryCount = 20, string ellipses = "...")
        {
            var summarizer = htmlHelper.GetWorkContext().Resolve<ISummarizer>();
            return summarizer.Summarize(text, flavor, boundaryCount, ellipses);
        }

        public static string Summarize(this HtmlHelper htmlHelper, string text, string flavor, string generatorType, int boundaryCount = 20, string ellipses = "...")
        {
            var summarizer = htmlHelper.GetWorkContext().Resolve<ISummarizer>();
            return summarizer.Summarize(text, flavor, generatorType, boundaryCount, ellipses);
        }

        public static string Summarize(this HtmlHelper htmlHelper, SummarizeContext context)
        {
            var summarizer = htmlHelper.GetWorkContext().Resolve<ISummarizer>();
            return summarizer.Summarize(context);
        }

        public static string Summarize(this HtmlHelper htmlHelper, BodyPart part, int boundary = 20, string ellpises = "...")
        {
            return htmlHelper.Summarize(part.Text, GetFlavor(part), SummaryGenerators.PlainTextWords, boundary, ellpises);
        }

        public static string Summarize(this HtmlHelper htmlHelper, BodyPart part, string generatorType, int boundary = 20, string ellpises = "...")
        {
            return htmlHelper.Summarize(part.Text, GetFlavor(part), generatorType, boundary, ellpises);
        }

        private static string GetFlavor(BodyPart part)
        {
            var typePartSettings = part.Settings.GetModel<BodyTypePartSettings>();
            return typePartSettings != null && !String.IsNullOrWhiteSpace(typePartSettings.Flavor) ? typePartSettings.Flavor : part.PartDefinition.Settings.GetModel<BodyPartSettings>().FlavorDefault;
        }
    }
}