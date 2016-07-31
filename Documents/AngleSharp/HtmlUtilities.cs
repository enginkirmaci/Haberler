using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using System;
using System.IO;

namespace ReadSharp
{
    public class HtmlUtilities
    {
        /// <summary>
        /// Converts HTML to plain text / strips tags.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        public static string ConvertToPlainText(string html)
        {
            var parser = new HtmlParser();
            var document = parser.Parse(html);

            StringWriter sw = new StringWriter();
            ConvertTo(document.DocumentElement, sw);
            sw.Flush();
            return sw.ToString();
        }

        private static void ConvertTo(INode node, TextWriter outText)
        {
            string html;
            switch (node.NodeType)
            {
                case NodeType.Comment:
                    // don't output comments
                    break;

                case NodeType.Document:
                    ConvertContentTo(node, outText);
                    break;

                case NodeType.Text:
                    // script and style must not be output
                    string parentName = node.Parent.NodeName;
                    if ((parentName == "script") || (parentName == "style"))
                        break;

                    // get text
                    html = node.TextContent;

                    //TODO Convert to AngleSharp
                    //// is it in fact a special closing node output as text?
                    //if (HtmlNode.IsOverlappedClosingElement(html))
                    //    break;
                    //// check the text is meaningful and not a bunch of whitespaces
                    //if (html.Trim().Length > 0)
                    //{
                    //    outText.Write(HtmlEntity.DeEntitize(html));
                    //}
                    break;

                case NodeType.Element:
                    switch (node.NodeName)
                    {
                        case "p":
                            // treat paragraphs as crlf
                            outText.Write("\r\n");
                            break;

                        case "br":
                            outText.Write("\r\n");
                            break;
                    }

                    if (node.HasChildNodes)
                    {
                        ConvertContentTo(node, outText);
                    }
                    break;
            }
        }

        private static void ConvertContentTo(INode node, TextWriter outText)
        {
            foreach (INode subnode in node.ChildNodes)
            {
                ConvertTo(subnode, outText);
            }
        }
    }
}