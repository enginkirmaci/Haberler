﻿/*
 * NReadability
 * http://code.google.com/p/nreadability/
 *
 * Copyright 2010 Marek Stój
 * http://immortal.pl/
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using AngleSharp.Parser.Xml;
using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ReadSharp.Ports.NReadability
{
    /// <summary>
    /// A class for constructing a DOM from HTML markup.
    /// </summary>
    public class SgmlDomBuilder
    {
        #region Public methods

        /// <summary>
        /// Constructs a DOM (System.Xml.Linq.XDocument) from HTML markup.
        /// </summary>
        /// <param name="htmlContent">HTML markup from which the DOM is to be constructed.</param>
        /// <returns>System.Linq.Xml.XDocument instance which is a DOM of the provided HTML markup.</returns>
        public XDocument BuildDocument(string htmlContent)
        {
            if (htmlContent == null)
            {
                throw new ArgumentNullException("htmlContent");
            }

            //if (htmlContent.Trim().Length == 0)
            //{
            //    return new XDocument();
            //}

            //// "trim end" htmlContent to ...</html>$ (codinghorror.com puts some scripts after the </html> - sic!)
            //const string htmlEnd = "</html";
            //int indexOfHtmlEnd = htmlContent.LastIndexOf(htmlEnd);

            //if (indexOfHtmlEnd != -1)
            //{
            //    int indexOfHtmlEndBracket = htmlContent.IndexOf('>', indexOfHtmlEnd);

            //    if (indexOfHtmlEndBracket != -1)
            //    {
            //        htmlContent = htmlContent.Substring(0, indexOfHtmlEndBracket + 1);
            //    }
            //}

            //htmlContent = htmlContent.Replace("--", string.Empty);

            htmlContent = CleanXmlComments(htmlContent);

            XDocument document;

            try
            {
                document = LoadDocument(htmlContent);
            }
            catch (InvalidOperationException exc)
            {
                // sometimes SgmlReader doesn't handle <script> tags well and XDocument.Load() throws,
                // so we can retry with the html content with <script> tags stripped off

                if (!exc.Message.Contains("EndOfFile"))
                {
                    throw;
                }

                htmlContent = HtmlUtils.RemoveScriptTags(htmlContent);

                document = LoadDocument(htmlContent);
            }

            return document;
        }

        public static string CleanXmlComments(string text)
        {
            string re = @"(<!--.*?--\>)";
            return Regex.Replace(text, re, string.Empty);
        }

        public static XDocument GetXDocument(string htmlContent)
        {
            var parser = new XmlParser();
            var document = parser.Parse(htmlContent);

            //TODO Convert to AngleSharp
            //HtmlDocument document = new HtmlDocument();
            //document.OptionOutputAsXml = true;
            //document.OptionAutoCloseOnEnd = true;
            //document.OptionDefaultStreamEncoding = System.Text.Encoding.UTF8;
            //document.LoadHtml(htmlContent);

            //XDocument xdoc = XDocument.Parse(document.DocumentNode.OuterHtml);
            XDocument xdoc = XDocument.Parse(document.DocumentElement.OuterHtml);
            return xdoc;
        }

        private static XDocument LoadDocument(string htmlContent)
        {
            try
            {
                return GetXDocument(htmlContent);
            }
            catch
            {
                return GetXDocument(string.Format("<div>{0}</div>", htmlContent));
            }
        }

        #endregion Public methods
    }
}