using IDeliverable.Bits.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Tokens;

namespace IDeliverable.Bits.Tokens
{
    public class RequestTokens : Component, ITokenProvider
    {
        private readonly IOrchardServices mOrchardServices;
        private readonly ICurrentContentAccessor mCurrentContentAccessor;

        public RequestTokens(IOrchardServices orchardServices, ICurrentContentAccessor currentContentAccessor)
        {
            mOrchardServices = orchardServices;
            mCurrentContentAccessor = currentContentAccessor;
        }

        public void Describe(DescribeContext context)
        {
            context.For("Request", T("Http Request"), T("Current Http Request tokens."))
                .Token("CurrentContent", T("Current Content"), T("The request routed Content Item."), "Content");
        }

        public void Evaluate(EvaluateContext context)
        {
            var httpContext = mOrchardServices.WorkContext.HttpContext;

            if (httpContext == null)
                return;

            context.For("Request", httpContext.Request)
                .Token("CurrentContent", (request) => DisplayText(GetRoutedContentItem()))
                .Chain("CurrentContent", "Content", (request) => GetRoutedContentItem());
        }

        private ContentItem GetRoutedContentItem()
        {
            return mCurrentContentAccessor.CurrentContentItem;
        }

        private string DisplayText(IContent content)
        {
            return content == null ? "" : mOrchardServices.ContentManager.GetItemMetadata(content).DisplayText;
        }
    }
}