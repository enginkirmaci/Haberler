using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Haberler.SourceTool
{
    /// <summary>
    /// Interaction logic for WebViewPage.xaml
    /// </summary>
    public partial class WebViewPage : Page
    {
        public WebViewPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadNews();
        }

        async private void loadNews()
        {
            if (App.Service.CurrentNews != null)
            {
                var article = await App.Service.LoadNews(App.Service.CurrentNews);

                try
                {
                    webbrowser.NavigateToString(App.Service.GenerateNewsHtml(article, Settings.News.FontSize, Settings.News.Theme));
                    richtextbox.Document.Blocks.Clear();
                    richtextbox.Document.Blocks.Add(new Paragraph(new Run(article.Content)));
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}