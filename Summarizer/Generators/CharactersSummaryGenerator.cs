using System.Text.RegularExpressions;
using IDeliverable.Bits.Summarizer.Helpers;
using IDeliverable.Bits.Summarizer.Services;

namespace IDeliverable.Bits.Summarizer.Generators
{
    public class CharactersSummaryGenerator : ISummaryGenerator
    {
        public string Generate(SummarizeContext context)
        {
            var html = context.Html.SanitizeHtml(context.HtmlWhiteList);
            var max = context.BoundaryCount;
            var index = 0;
            var count = 0;
            var isTag = false;
            var isEndTag = false;
            var isEntity = false;
            var lastTagName = "";
            var tagName = "";
            var isWord = false;
            var lastWordIndex = 0;
            var isAttribute = false;

            while (index < html.Length && count < max)
            {
                var character = html[index];

                if (!isTag && !isEntity)
                {
                    switch (character)
                    {
                        case '<':
                            isTag = true;
                            isEndTag = false;
                            isWord = false;
                            isAttribute = false;
                            break;
                        case '&':
                            isEntity = true;
                            isWord = false;
                            break;
                        default:
                            count++;
                            if ((character >= 'a' && character <= 'z') || (character >= 'A' && character <= 'Z'))
                            {
                                if (!isWord)
                                {
                                    isWord = true;
                                    lastWordIndex = index;
                                }
                            }
                            else if (isWord)
                            {
                                isWord = false;
                            }
                            break;
                    }
                }
                else if (isEntity)
                {
                    switch (character)
                    {
                        case ';':
                            isEntity = false;
                            break;
                    }
                }
                else {
                    switch (character)
                    {
                        case '/':
                            if (!isAttribute)
                            {
                                isEndTag = true;
                                lastTagName = tagName;
                                tagName = "";
                            }
                            break;
                        case '>':
                            isTag = false;
                            isAttribute = false;
                            lastTagName = tagName;
                            tagName = "";
                            break;
                        case ' ':
                            isAttribute = true;
                            break;
                        default:
                            if (!isAttribute)
                                tagName += character;
                            break;
                    }
                }
                index++;
            }

            string htmlExcerpt;

            if (isWord && lastWordIndex > 0 && index < html.Length && ((html[index] >= 'a' && html[index] <= 'z') || (html[index] >= 'A' && html[index] <= 'Z')))
            {
                htmlExcerpt = html.Substring(0, lastWordIndex);
            }
            else
            {
                htmlExcerpt = html.Substring(0, index);
            }

            if (index < html.Length)
            {
                htmlExcerpt = Regex.Replace(htmlExcerpt, @"(&nbsp;|\s|\t)(&nbsp;|\s|\t)*$", string.Empty);
            }

            if (!isEndTag)
            {
                var endTag = "</" + lastTagName + ">";
                htmlExcerpt += endTag;
            }

            return htmlExcerpt;
        }
    }
}