using Haberler.Services.Entities;
using Haberler.Services.Repository;
using Library10.Common.Converters;
using Library10.Core.Development;
using Library10.Net;
using Library10.Net.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace Haberler.BackgroundTasks
{
    public sealed class NewsTileBackgroundTask : IBackgroundTask
    {
        private Source ActiveSource = null;
        private Category CurrentCategory = null;

        //private List<Source> Sources { get; set; }
        private static string textElementName = "text";

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // Get a deferral, to prevent the task from closing prematurely
            // while asynchronous code is still running.
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            try
            {
                var secondaryTiles = await SecondaryTile.FindAllForPackageAsync();

                foreach (var item in secondaryTiles)
                {
                    int sourceId = 0;
                    int.TryParse(item.Arguments, out sourceId);

                    await UpdateTile(sourceId, item.TileId);
                }
            }
            catch (Exception ex)
            {
                DevTools.WriteLine(ex.ToString());
            }

            // Inform the system that the task is finished.
            deferral.Complete();
        }

        async private Task UpdateTile(int? sourceID, string tileID = null)
        {
            if (!sourceID.HasValue)
                return; //   sourceID = Sources.FirstOrDefault().ID;

            var items = await LoadNews(sourceID.Value);

            var updater = tileID != null ? TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileID) : TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();

            // Keep track of the number feed items that get tile notifications.
            int itemCount = 0;

            // Create a tile notification for each feed item.
            foreach (var item in items)
            {
                XmlDocument tileWideXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Text09);
                XmlDocument tileSquare150x150Xml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Text02);
                XmlDocument tileSquare310x310Xml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare310x310TextList02);

                string titleText = getTitle(item); //string.Format("{0}\n{1}", item.Title, DateTimeConverter.ToTurkishRelativeTime(item.Date));
                string otherText = tileID != null ? CurrentCategory.Name.ToLower() : string.Format("{0} {1}", ActiveSource.Name, CurrentCategory.Name.ToLower());

                tileWideXml.GetElementsByTagName(textElementName)[0].InnerText = otherText;
                tileWideXml.GetElementsByTagName(textElementName)[1].InnerText = titleText;
                updater.Update(new TileNotification(tileWideXml));

                tileSquare150x150Xml.GetElementsByTagName(textElementName)[0].InnerText = otherText;
                tileSquare150x150Xml.GetElementsByTagName(textElementName)[1].InnerText = titleText;
                updater.Update(new TileNotification(tileSquare150x150Xml));

                var empty1 = false;
                if (items.Count > (itemCount * 3))
                {
                    var text1 = getTitle(items[(itemCount * 3) + 0]);
                    if (!string.IsNullOrEmpty(text1))
                    {
                        empty1 = false;
                        tileSquare310x310Xml.GetElementsByTagName(textElementName)[0].InnerText = text1;
                    }
                    else
                        empty1 = true;
                }
                else
                    empty1 = true;
                if (items.Count > (itemCount * 3) + 1)
                {
                    tileSquare310x310Xml.GetElementsByTagName(textElementName)[1].InnerText = getTitle(items[(itemCount * 3) + 1]);
                }
                if (items.Count > (itemCount * 3) + 2)
                {
                    tileSquare310x310Xml.GetElementsByTagName(textElementName)[2].InnerText = getTitle(items[(itemCount * 3) + 2]);
                }

                if (!empty1)
                    updater.Update(new TileNotification(tileSquare310x310Xml));

                // Don't create more than 5 notifications.
                if (itemCount++ > 5) break;
            }
        }

        private string getTitle(RSSItem item)
        {
            if (item == null)
                return string.Empty;
            return string.Format("{0}\n{1}", item.Title, DateTimeConverter.ToTurkishRelativeTime(item.Date));
        }

        async private Task<List<RSSItem>> LoadNews(int sourceID)
        {
            var result = new List<RSSItem>();

            try
            {
                ActiveSource = SourceRepository.Sources[sourceID];
                CurrentCategory = ActiveSource.Categories.FirstOrDefault(i => i.IsFavorite);
                if (CurrentCategory == null)
                    CurrentCategory = ActiveSource.Categories.FirstOrDefault();

                var rssAggregator = new RSSAggregator();
                var stream = await rssAggregator.GetFeedAsyncs(CurrentCategory.Url, ActiveSource.ParserSetting.Charset);
                var feedNews = rssAggregator.ParseFeeds(stream, ActiveSource.ParserSetting);

                var count = 0;
                foreach (var item in feedNews)
                {
                    if (count++ > 20)
                        break;

                    if (string.IsNullOrWhiteSpace(item.Title) ||
                        DateTime.Now < item.Date ||
                        string.IsNullOrWhiteSpace(item.Link))
                        continue;

                    result.Add(item);
                }
            }
            catch
            {
            }

            return result;
        }
    }
}