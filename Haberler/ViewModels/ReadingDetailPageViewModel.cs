using Haberler.Common;
using Haberler.Services;
using Haberler.Services.Entities;
using Haberler.Services.Repository;
using Library10.Common.Extensions;
using Library10.Core.IO;
using Library10.Core.Serialization;
using Library10.Core.UI;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using ReadSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Haberler.ViewModels
{
    public class ReadingDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly IStorageService _storageService;
        private readonly INewsService _newsService;

        private WebView _webBrowser = null;
        private string _filename = null;
        private Article _article = null;
        private bool IsNarrow;

        private string _headerText;
        public string HeaderText
        {
            get { return _headerText; }
            set { SetProperty(ref _headerText, value); }
        }

        private News _selectedNews;
        public News SelectedNews
        {
            get { return _selectedNews; }
            set { SetProperty(ref _selectedNews, value); }
        }

        private bool _prevButton;
        public bool PrevButton
        {
            get { return _prevButton; }
            set { SetProperty(ref _prevButton, value); }
        }

        private bool _nextButton;
        public bool NextButton
        {
            get { return _nextButton; }
            set { SetProperty(ref _nextButton, value); }
        }

        private bool _favoriteButton;
        public bool FavoriteButton
        {
            get { return _favoriteButton; }
            set { SetProperty(ref _favoriteButton, value); }
        }

        public DelegateCommand PrevButtonCommand { get; private set; }

        public DelegateCommand NextButtonCommand { get; private set; }

        public DelegateCommand FavoriteButtonCommand { get; private set; }

        public DelegateCommand ShareButtonCommand { get; private set; }

        public DelegateCommand ShowInBrowserCommand { get; private set; }

        public ReadingDetailPageViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IStorageService storageService,
            INewsService newsService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _storageService = storageService;
            _newsService = newsService;

            FavoriteButton = true;

            PrevButtonCommand = new DelegateCommand(PrevArticle);
            NextButtonCommand = new DelegateCommand(NextArticle);
            FavoriteButtonCommand = new DelegateCommand(FavoriteUnFavorite);
            ShareButtonCommand = new DelegateCommand(Share);
            ShowInBrowserCommand = new DelegateCommand(ShowInBrowser);
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);

            IsNarrow = Window.Current.Bounds.Width < 720;

            if (!string.IsNullOrWhiteSpace((string)e.Parameter))
            {
                var IDs = new List<int>();
                foreach (var id in ((string)e.Parameter).Split('|'))
                    IDs.Add(int.Parse(id));

                if (IDs.Count() == 1)
                {
                    IDs.Add(0);
                    IDs.Add(0);
                }
                else if (IDs.Count() == 2)
                {
                    IDs.Add(0);
                }

                Initialize(IDs[0], IDs[1], IDs[2]);
            }
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);

            if (_webBrowser != null)
                _webBrowser.NavigationStarting -= _webBrowser_NavigationStarting;

            Window.Current.SizeChanged -= Window_SizeChanged;
            DataTransferManager.GetForCurrentView().DataRequested -= OnDataRequested;
        }

        public void Initialize()
        {
            Initialize(SelectedNews.SourceID, SelectedNews.CategoryID, SelectedNews.ID);
        }

        public void Initialize(int sourceID, int categoryID, int newsID)
        {
            var category = SourceRepository.Sources[sourceID].Categories.First(i => i.ID == categoryID);
            if (category != null && category.News != null && category.News.Count > 0)
                SelectedNews = SourceRepository.Sources[sourceID].Categories.First(i => i.ID == categoryID).News.First(i => i.ID == newsID);

            LoadNews();

            Window.Current.SizeChanged += Window_SizeChanged;
            DataTransferManager.GetForCurrentView().DataRequested += OnDataRequested;
        }

        private void Window_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (IsNarrow && Window.Current.Bounds.Width >= 720)
            {
                Window.Current.SizeChanged -= Window_SizeChanged;

                IsNarrow = Window.Current.Bounds.Width < 720;

                if (_navigationService.CanGoBack())
                    _navigationService.GoBack();
            }

            IsNarrow = Window.Current.Bounds.Width < 720;
        }

        protected void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            try
            {
                args.Request.Data.Properties.Title = _article.Title;

                //Limit content
                args.Request.Data.Properties.Description = _article.PlainContent;
                args.Request.Data.Properties.ApplicationName = Settings.General.AppNameCapitalized;

                //args.Request.Data.Properties.Thumbnail =

                var html = _newsService.GenerateNewsHtml(_article, Settings.News.FontSize, Settings.News.Theme);

                args.Request.Data.SetWebLink(_article.Link);
                args.Request.Data.SetHtmlFormat(html);
                args.Request.Data.SetRtf(html);
                args.Request.Data.SetText(_article.PlainContent);
            }
            catch
            {
            }
        }

        private async void _webBrowser_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (args.Uri != null && sender.Source == null)
                if (!args.Uri.Query.Contains("browser=1"))
                {
                    args.Cancel = true;

                    await Launcher.LaunchUriAsync(args.Uri);
                }
                else
                {
                    if (SelectedNews != null)
                        SelectedNews.IsLoaded = true;
                }
        }

        private void SetControls()
        {
            try
            {
                if (_webBrowser == null)
                {
                    var browser = VisualHelper.FindVisualChildInsideFrame<WebView>(Window.Current.Content, controlName: "browser");

                    _webBrowser = browser;
                    _webBrowser.NavigationStarting += _webBrowser_NavigationStarting;
                }

                FavoriteButton = true;

                var category = SourceRepository.Sources[SelectedNews.SourceID].Categories.First(i => i.ID == SelectedNews.CategoryID);
                var index = category.News.IndexOf(SelectedNews);
                var count = category.News.Count;

                if (index == -1)
                {
                    PrevButton = false;
                    NextButton = false;
                }
                else
                {
                    if (index > 0)
                        PrevButton = true;
                    else
                        PrevButton = false;

                    if (index + 1 == count)
                        NextButton = false;
                    else
                        NextButton = true;
                }
            }
            catch
            { }
        }

        public void PrevArticle()
        {
            try
            {
                //if (!HasNetwork)
                //{
                //    _dialogService.ShowAsync(Resource.Get(STR.OfflineMessage), Resource.Get(STR.ErrorTitle), Resource.Get(STR.OK));
                //    return;
                //}

                var category = SourceRepository.Sources[SelectedNews.SourceID].Categories.First(i => i.ID == SelectedNews.CategoryID);
                var index = category.News.IndexOf(SelectedNews);
                SelectedNews = category.News.ElementAt(index - 1);
                LoadNews();

                //NavigationService.NavigateToViewModel<ReadingViewModel>(newsItem);
            }
            catch { }
        }

        public void NextArticle()
        {
            try
            {
                //if (!HasNetwork)
                //{
                //    _dialogService.ShowAsync(Resource.Get(STR.OfflineMessage), Resource.Get(STR.ErrorTitle), Resource.Get(STR.OK));
                //    return;
                //}

                var category = SourceRepository.Sources[SelectedNews.SourceID].Categories.First(i => i.ID == SelectedNews.CategoryID);
                var index = category.News.IndexOf(SelectedNews);
                SelectedNews = category.News.ElementAt(index + 1);
                LoadNews();

                //NavigationService.NavigateToViewModel<ReadingViewModel>(newsItem);
            }
            catch { }
        }

        public async void ShowInBrowser()
        {
            if (SelectedNews == null || !SelectedNews.IsLoaded)
                return;

            await Launcher.LaunchUriAsync(new Uri(SelectedNews.Link, UriKind.Absolute));
        }

        public void Share()
        {
            try
            {
                DataTransferManager.ShowShareUI();
            }
            catch
            {
            }
        }

        async public void FavoriteUnFavorite()
        {
            try
            {
                if (FavoriteButton)
                {
                    byte[] byteArray;
                    if (_article != null)
                        byteArray = Serializer.Serialize(_article);
                    else
                        byteArray = Serializer.Serialize(SelectedNews);

                    await _storageService.CreateFile(string.Format(Settings.Application.FavoriteFolder, _filename), byteArray, StorageStrategy.Roaming, CreationCollisionOption.ReplaceExisting);

                    FavoriteButton = false;
                }
                else
                {
                    _storageService.DeleteFile(string.Format(Settings.Application.FavoriteFolder, _filename), StorageStrategy.Roaming);

                    FavoriteButton = true;
                }
            }
            catch
            {
            }
        }

        async private void LoadNews()
        {
            var uri = new Uri(SelectedNews.Link);
            _filename = SelectedNews.Link.GenerateSlug() + Settings.Application.FavoriteFileExt;

            SetControls();

            var hasError = false;
            try
            {
                var stream = await _storageService.OpenFile(string.Format(Settings.Application.FavoriteFolder, _filename), StorageStrategy.Roaming);
                FavoriteButton = false;

                _article = Serializer.Deserialize<Article>(stream);

                if (_article == null)
                    hasError = true;
            }
            catch
            {
                hasError = true;
            }

            if (hasError && SelectedNews == null)
            {
                try
                {
                    var stream = await _storageService.OpenFile(string.Format(Settings.Application.FavoriteFolder, _filename), StorageStrategy.Roaming);
                    SelectedNews = Serializer.Deserialize<News>(stream);

                    FavoriteButton = false;
                }
                catch
                {
                }
            }

            try
            {
                if (_article == null || _article.Link.ToString() != SelectedNews.Link)
                    _article = await _newsService.LoadNews(SelectedNews);

                if (SelectedNews != null)
                    SelectedNews.IsLoaded = true;

                HeaderText = _article.Title;

                _webBrowser.NavigateToString(_newsService.GenerateNewsHtml(_article, Settings.News.FontSize, Settings.News.Theme));
            }
            catch
            {
                var errorUri = SelectedNews.Link.ToUri();
                if (string.IsNullOrEmpty(errorUri.Query))
                    errorUri = (errorUri.OriginalString + "?browser=1").ToUri();
                else
                    errorUri = (errorUri.OriginalString + "&browser=1").ToUri();

                _webBrowser.Navigate(errorUri);

                if (SelectedNews != null)
                    SelectedNews.IsLoaded = true;
            }
        }
    }
}