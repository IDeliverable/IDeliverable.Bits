using System;
using IDeliverable.Bits.Summarizer.Helpers;
using IDeliverable.Bits.Summarizer.Services;

namespace IDeliverable.Bits.Summarizer.Generators
{
    public class WordsSummaryGenerator : ISummaryGenerator
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
            var isAttribute = false;

            while (index < html.Length && (count < max || isWord))
            {
                var character = html[index];

                if (!isTag && !isEntity)
                {
                    if (character == '<')
                    {
                        isTag = true;
                        isEndTag = false;
                        isWord = false;
                        isAttribute = false;
                    }
                    else if (character == '&')
                    {
                        isEntity = true;
                        isWord = false;
                    }
                    else if ((character >= 'a' && character <= 'z') || (character >= 'A' && character <= 'Z') || (character >= '0' && character <= '9'))
                    {
                        if (!isWord)
                        {
                            isWord = true;
                            count++;
                        }
                    }
                    else
                    {
                        isWord = false;
                    }
                }
                else if (isEntity)
                {
                    switch (character)
                    {
                        case ';':
                            isEntity = false;
                            break;
                        default:
                            tagName += character;
                            break;
                    }
                }
                else
                {
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

            var htmlExcerpt = html.Substring(0, index);

            if (!isEndTag && !String.IsNullOrWhiteSpace(lastTagName))
            {
                var endTag = "</" + lastTagName + ">";
                htmlExcerpt += endTag;
            }

            return htmlExcerpt;
        }
    }
}