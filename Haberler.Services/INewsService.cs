using Haberler.Services.Entities;
using Haberler.Services.Enums;
using ReadSharp;
using System;
using System.Threading.Tasks;

namespace Haberler.Services
{
    public interface INewsService
    {
        void SetSource(Source source);

        void SetCategory(Category category);

        void SetNews(News news);

        Task LoadNewsList(OrderType orderType);

        Task<Article> LoadNews(News item);

        string GenerateNewsHtml(Article article, int fontSize, Theme theme);

        string GenerateHtml(string title, string uri, string uriShort, string date, Uri image, string text, int fontSize, Theme theme);
    }
}