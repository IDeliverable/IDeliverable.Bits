using System;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace IDeliverable.Bits.Summarizer.Helpers
{
	public static class HtmlSanitizer
	{
		/// <summary>
		/// Takes raw HTML input and cleans against a whitelist.
		/// </summary>
		/// <param name="source">HTML source.</param>
		/// <param name="whitelist">A list of HTML tags that should not be stripped from the source.</param>
		/// <returns>Clean output.</returns>
		public static string SanitizeHtml(this string source, string[] whitelist = null)
		{
			var html = GetHtml(source);
			if (html == null)
				return String.Empty;

			// All the nodes.
			var allNodes = html.DocumentNode;

			// Scrub tags not in whitelist.
			CleanNodes(allNodes, whitelist);

			return allNodes.InnerHtml;
		}

		/// <summary>
		/// Takes a raw source and removes all HTML tags.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string StripHtml(this string source)
		{
			source = SanitizeHtml(source);

			// No need to continue if we have no clean HTML.
			if (String.IsNullOrEmpty(source))
				return String.Empty;

			var html = GetHtml(source);
			var result = new StringBuilder();

			// For each node, extract only the innerText.
			foreach (var node in html.DocumentNode.ChildNodes)
				result.Append(node.InnerText);

			return result.ToString();
		}

		/// <summary>
		/// Recursively delete nodes not in the whitelist.
		/// </summary>
		private static void CleanNodes(HtmlNode node, string[] whitelist)
		{
			if (node.NodeType == HtmlNodeType.Element)
			{
				if (whitelist == null || !whitelist.Contains(node.Name))
				{
					node.ParentNode.RemoveChild(node);
					return; // We're done.
				}
			}

			if (node.HasChildNodes)
				CleanChildren(node, whitelist);
		}

		/// <summary>
		/// Apply CleanNodes to each of the child nodes.
		/// </summary>
		private static void CleanChildren(HtmlNode parent, string[] whitelist)
		{
			for (var i = parent.ChildNodes.Count - 1; i >= 0; i--)
				CleanNodes(parent.ChildNodes[i], whitelist);
		}

		/// <summary>
		/// Helper function that returns an HTML document from text.
		/// </summary>
		private static HtmlDocument GetHtml(string source)
		{
			var html = new HtmlDocument
			{
				OptionFixNestedTags = true,
				OptionAutoCloseOnEnd = true,
				OptionDefaultStreamEncoding = Encoding.UTF8
			};

			html.LoadHtml(source);

			return html;
		}
	}
}