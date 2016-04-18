using System.Web.Mvc;
using Autofac;
using IDeliverable.Bits.Filters;
using NUnit.Framework;

namespace IDeliverable.Bits.Tests.Filters
{
    [TestFixture]
    public class AppRelativeUrlFilterTests
    {
        private IContainer m_container;
        private AppRelativeUrlFilter m_filter;

        [SetUp]
        public virtual void Init()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterType<AppRelativeUrlFilter>().AsSelf();
            builder.RegisterType<FakeUrlHelper>().As<UrlHelper>();

            m_container = builder.Build();
            m_filter = m_container.Resolve<AppRelativeUrlFilter>();
        }

        [TearDown]
        public virtual void Cleanup()
        {
            m_container?.Dispose();
        }

        [Test]
        public void ProcessContentWithHtmlFlavorShouldProcessAppRelativeUrls()
        {
            var result = m_filter.ProcessContent(@"<a href=""~/relative-path"">Hello World</a>", "html");

            StringAssert.AreEqualIgnoringCase(@"<a href=""/OrchardLocal/relative-path"">Hello World</a>", result);
        }

        [Test]
        public void ProcessContentWithMarkdownFlavorShouldProcessAppRelativeUrls()
        {
            var result = m_filter.ProcessContent(@"[inline-style link](~/relative-path)", "markdown");

            StringAssert.AreEqualIgnoringCase(@"[inline-style link](/OrchardLocal/relative-path)", result);
        }

        [Test]
        public void ProcessContentWithUnsupportedFlavorShouldNotProcessAppRelativeUrls()
        {
            var result = m_filter.ProcessContent(@"<a href=""~/relative-path"">Hello World</a>", "text");

            StringAssert.AreEqualIgnoringCase(@"<a href=""~/relative-path"">Hello World</a>", result);
        }
    }

    public class FakeUrlHelper : UrlHelper {
        public override string Content(string contentPath) {
            return contentPath.Replace("~/", "/OrchardLocal/");
        }
    }
}
