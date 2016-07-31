using Haberler.Services.Entities;
using Haberler.Services.Repository;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Haberler.SourceTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            listboxSource.ItemsSource = SourceRepository.Sources.Values.Where(i => i.ID != 9999);
        }

        private void listboxSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = e.AddedItems[0];
            if (selectedItem != null && selectedItem is Source)
            {
                listboxCategory.ItemsSource = null;
                listboxNews.ItemsSource = null;

                App.Service.CurrentSource = (Source)selectedItem;
                listboxCategory.ItemsSource = App.Service.CurrentSource.Categories;
                listboxCategory.SelectedIndex = 0;
            }
        }

        async private void listboxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selectedItem = e.AddedItems[0];
                if (selectedItem != null && selectedItem is Category)
                {
                    App.Service.CurrentCategory = (Category)selectedItem;
                    try
                    {
                        await App.Service.LoadNewsList(Services.Enums.OrderType.DateDesc);

                        listboxNews.ItemsSource = App.Service.CurrentCategory.News;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("RSS ERROR: " + Environment.NewLine + ex.ToString());
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (listboxNews.SelectedItem != null)
            {
                var webview = new WebViewPage();
                App.Service.CurrentNews = (News)listboxNews.SelectedItem;

                NavigationWindow navWIN = new NavigationWindow();
                navWIN.Content = webview;
                navWIN.ShowInTaskbar = false;
                navWIN.ShowsNavigationUI = false;
                navWIN.Width = 640;
                navWIN.Show();
            }
        }
    }
}