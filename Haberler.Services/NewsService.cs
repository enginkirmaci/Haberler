using Haberler.Services.Constants;
using Haberler.Services.Entities;
using Haberler.Services.Enums;
using Haberler.Services.Repository;
using Library10.Common.Converters;
using Library10.Common.Entities;
using Library10.Common.Extensions;
using Library10.Net;
using ReadSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Haberler.Services
{
    public class NewsService : INewsService
    {
        public Source CurrentSource { get; set; }

        public Category CurrentCategory { get; set; }

        public News CurrentNews { get; set; }

        private Reader _reader = null;
        private ReadOptions _options = null;

        public NewsService()
        {
            _reader = new Reader();

            _options = new ReadOptions()
            {
                PrettyPrint = true,
                UseDeepLinks = false,
                HasHeadline = false,
                HasHeaderTags = false,
                MultipageDownload = false
            };
        }

        public void SetSource(Source source)
        {
            CurrentSource = source;

            CurrentCategory = null;
            CurrentNews = null;
        }

        public void SetCategory(Category category)
        {
            CurrentCategory = category;

            CurrentNews = null;
        }

        public void SetNews(News news)
        {
            CurrentNews = news;
        }

        async public Task LoadNewsList(OrderType orderType)
        {
            var newsList = new List<News>();

            var rssAggregator = new RSSAggregator();

            var stream = await rssAggregator.GetFeedAsyncs(CurrentCategory.Url, CurrentSource.ParserSetting.Charset);
            var feedNews = rssAggregator.ParseFeeds(stream, CurrentSource.ParserSetting);

            var id = 0;
            foreach (var item in feedNews)
            {
                if (string.IsNullOrWhiteSpace(item.Title) ||
                    DateTime.Now < item.Date ||
                    string.IsNullOrWhiteSpace(item.Link))
                    continue;

                newsList.Add(new News()
                {
                    ID = id++,
                    SourceID = CurrentSource.ID,
                    CategoryID = CurrentCategory.ID,
                    Title = item.Title,
                    Link = item.Link,
                    Date = item.Date,
                    Image = item.Image
                });
            }

            switch (orderType)
            {
                case OrderType.DateAsc:
                    CurrentCategory.News = new ObservableCollectionEx<News>(newsList.OrderBy(i => i.Date));
                    break;

                case OrderType.DateDesc:
                    CurrentCategory.News = new ObservableCollectionEx<News>(newsList.OrderByDescending(i => i.Date));
                    break;

                case OrderType.TitleAsc:
                    CurrentCategory.News = new ObservableCollectionEx<News>(newsList.OrderBy(i => i.Title));
                    break;

                case OrderType.TitleDesc:
                    CurrentCategory.News = new ObservableCollectionEx<News>(newsList.OrderByDescending(i => i.Title));
                    break;
            }

            CurrentCategory.IsSynchronized = true;
        }

        async public Task<Article> LoadNews(News item)
        {
            var uri = item.Link.ToUri();
            var currentSource = SourceRepository.Sources[item.SourceID];

            try
            {
                var article = await _reader.Read(uri, currentSource.ParserSetting, _options);

                article.Title = string.IsNullOrEmpty(item.Title) ? UnicodeConverter.ToTurkish(article.Title) : item.Title;
                article.Content = UnicodeConverter.ToTurkish(article.Content);
                article.PlainContent = UnicodeConverter.ToTurkish(article.PlainContent);
                article.Date = item.Date;
                article.FrontImage = article.FrontImage == null ? string.IsNullOrEmpty(item.Image) ? null : item.Image.ToUri() : article.FrontImage;

                if (currentSource.IsFrontImageDisabled)
                    article.FrontImage = null;

                return article;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GenerateNewsHtml(Article article, int fontSize, Theme theme)
        {
            return GenerateHtml(
                    article.Title,
                    article.Link.OriginalString,
                    article.Link.Host.Replace("www.", string.Empty),
                    DateTimeConverter.ToTurkishRelativeTime(article.Date),
                    article.FrontImage,
                    article.Content,
                    fontSize,
                    theme);
        }

        public string GenerateHtml(string title, string uri, string uriShort, string date, Uri image, string text, int fontSize, Theme theme)
        {
            var frontImage = string.Empty;
            if (image != null && text.Contains(image.OriginalString))
                frontImage = string.Empty;
            else if (image != null)
                frontImage = image.OriginalString;

            var hideImage = string.IsNullOrWhiteSpace(frontImage) ? HtmlConstants.HideStyle : string.Empty;

            switch (theme)
            {
                case Theme.Dark:
                default:
                    return string.Format(HtmlConstants.HTML, HtmlConstants.BaseTheme + HtmlConstants.DarkTheme, title, uri, uriShort, date, frontImage, hideImage, text, fontSize, HtmlConstants.JS);

                case Theme.Sepia:
                    return string.Format(HtmlConstants.HTML, HtmlConstants.BaseTheme + HtmlConstants.SepiaTheme, title, uri, uriShort, date, frontImage, hideImage, text, fontSize, HtmlConstants.JS);

                case Theme.Light:
                    return string.Format(HtmlConstants.HTML, HtmlConstants.BaseTheme + HtmlConstants.LightTheme, title, uri, uriShort, date, frontImage, hideImage, text, fontSize, HtmlConstants.JS);
            }
        }
    }
}