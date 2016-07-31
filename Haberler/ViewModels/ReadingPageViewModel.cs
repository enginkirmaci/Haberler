using Haberler.Common;
using Haberler.Common.Constants;
using Haberler.Events;
using Haberler.Services;
using Haberler.Services.Entities;
using Haberler.Services.Enums;
using Haberler.Services.Repository;
using Library10.Common.Entities;
using Library10.Common.Enums;
using Library10.Common.Extensions;
using Library10.Core.IO;
using Library10.Core.Serialization;
using Library10.Core.UI;
using Prism.Commands;
using Prism.Events;
using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using ReadSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Haberler.ViewModels
{
    public class ReadingPageViewModel : ViewModelBase
    {
        protected bool HasNetwork { get { return NetworkInterface.GetIsNetworkAvailable(); } }

        private readonly INavigationService _navigationService;
        private readonly IResourceLoader _resourceLoader;
        private readonly IDialogService _dialogService;
        private readonly IStorageService _storageService;
        private readonly INewsService _newsService;
        private readonly IEventAggregator _eventAggregator;

        private bool _isNarrow;
        private bool _isLoadAllCategoryNews;

        public Category CurrentCategory { get; set; }

        public bool HasNoConnection { get; set; }

        public bool IsPinned { get; set; }

        public bool IsBought
        {
            get { return !InAppHelper.IsTrial; }
        }

        private bool _isFavorited;
        public bool IsFavorited
        {
            get { return _isFavorited; }
            set { SetProperty(ref _isFavorited, value); }
        }

        private Source _selectedSource;
        public Source SelectedSource
        {
            get { return _selectedSource; }
            set { SetProperty(ref _selectedSource, value); }
        }

        private ReadingDetailPageViewModel _detailViewModel;
        public ReadingDetailPageViewModel DetailViewModel
        {
            get { return _detailViewModel; }
            set { SetProperty(ref _detailViewModel, value); }
        }

        private bool _isFilterActive;
        public bool IsFilterActive
        {
            get { return _isFilterActive; }
            set { SetProperty(ref _isFilterActive, value); }
        }

        private string _keyword;
        public string Keyword
        {
            get { return _keyword; }
            set { SetProperty(ref _keyword, value); }
        }

        private int _selectedCategoryIndex;
        public int SelectedCategoryIndex
        {
            get { return _selectedCategoryIndex; }
            set
            {
                SetProperty(ref _selectedCategoryIndex, value);

                LoadCategoryNews(_selectedCategoryIndex, isSelectionChanged: true);
            }
        }

        public Thickness Margin { get; set; }

        public OrderType ActiveOrderType { get; set; }

        public DelegateCommand<ItemClickEventArgs> SelectNewsCommand { get; private set; }

        public DelegateCommand PinToStartCommand { get; private set; }

        public DelegateCommand RefreshCategoryCommand { get; private set; }

        public DelegateCommand FavoriteSourceCommand { get; private set; }

        public DelegateCommand RemoveAdsCommand { get; private set; }

        public ReadingPageViewModel(
            INavigationService navigationService,
            IResourceLoader resourceLoader,
            IDialogService dialogService,
            IStorageService storageService,
            INewsService newsService,
            IEventAggregator eventAggregator)
        {
            _navigationService = navigationService;
            _resourceLoader = resourceLoader;
            _dialogService = dialogService;
            _storageService = storageService;
            _newsService = newsService;
            _eventAggregator = eventAggregator;

            SelectNewsCommand = new DelegateCommand<ItemClickEventArgs>(SelectNews);
            PinToStartCommand = new DelegateCommand(PinToStart);
            RefreshCategoryCommand = new DelegateCommand(RefreshCategory);
            FavoriteSourceCommand = new DelegateCommand(FavoriteSource);
            RemoveAdsCommand = new DelegateCommand(BuyApplication);

            SourceRepository.Sources[9999].Name = _resourceLoader.GetString("ReadingListHeader/Text");
        }

        //public void NavigateNews(News item)
        //{
        //    CheckConnection();

        //    //if (!HasNetwork)
        //    //{
        //    //    _dialogService.ShowAsync(_resourceLoader.GetString(STR.OfflineMessage), _resourceLoader.GetString(STR.ErrorTitle), _resourceLoader.GetString(STR.OK));
        //    //    return;
        //    //}

        //    //NavigationService.NavigateToViewModel<ReadingViewModel>(item);
        //}

        async public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);

            _detailViewModel = new ReadingDetailPageViewModel(_navigationService, _dialogService, _storageService, _newsService);

            Margin = new Thickness(-12, 0, -12, 0);

            if (IsFavorited)
                IsFavorited = false;

            _isNarrow = Window.Current.Bounds.Width < 720;
            _isLoadAllCategoryNews = false;

            if (e.Parameter != null && e.Parameter is int)
            {
                if (SelectedSource == null || SelectedSource.ID != (int)e.Parameter)
                {
                    SelectedSource = SourceRepository.Sources[(int)e.Parameter];

                    ActiveOrderType = OrderType.DateDesc;
                    CheckConnection();
                    CheckIsPinned();

                    //TODO optimize here
                    _newsService.SetSource(SelectedSource);
                    LoadCategoryNews(0);

                    DetailViewModel.SelectedNews = null;
                    OnPropertyChanged(() => DetailViewModel);
                    OnPropertyChanged(() => SelectedSource);
                }
                else if (CurrentCategory.News == null)
                {
                    RefreshCategory();

                    DetailViewModel.SelectedNews = null;
                    OnPropertyChanged(() => DetailViewModel);
                }
                else if (SelectedSource != null && SelectedSource.ID == (int)e.Parameter)
                {
                    OnPropertyChanged(() => DetailViewModel);
                    OnPropertyChanged(() => DetailViewModel.SelectedNews);
                    OnPropertyChanged(() => SelectedSource);
                }
            }
            else if (e.Parameter is string && !string.IsNullOrWhiteSpace(e.Parameter as string))
            {
                var IDs = (e.Parameter as string).Split('|');
                var sourceID = int.Parse(IDs[0]);
                var categoryID = IDs.Count() > 1 ? int.Parse(IDs[1]) : 0;

                SelectedSource = SourceRepository.Sources[sourceID];

                ActiveOrderType = OrderType.DateDesc;
                CheckConnection();
                CheckIsPinned();

                //TODO optimize here
                _isLoadAllCategoryNews = true;
                _newsService.SetSource(SelectedSource);
                LoadCategoryNews(categoryID);

                DetailViewModel.SelectedNews = null;
                OnPropertyChanged(() => DetailViewModel);
                OnPropertyChanged(() => SelectedSource);
            }
            else
            {
                Margin = new Thickness(-12, -32, -12, 0);

                ActiveOrderType = OrderType.DateDesc;
                IsFavorited = true;
                var loadFavorited = await LoadFavorited();

                if (loadFavorited)
                {
                    DetailViewModel.SelectedNews = null;
                    OnPropertyChanged(() => DetailViewModel);
                    OnPropertyChanged(() => SelectedSource);
                }
            }

            Window.Current.SizeChanged += Reading_Window_SizeChanged;
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);

            Window.Current.SizeChanged -= Reading_Window_SizeChanged;
        }

        private void Reading_Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            if (!_isNarrow && Window.Current.Bounds.Width < 720 && DetailViewModel.SelectedNews != null)
                _navigationService.Navigate(PageTokens.ReadingDetail, DetailViewModel.SelectedNews.CombinedID);

            _isNarrow = Window.Current.Bounds.Width < 720;
        }

        private void FavoriteSource()
        {
            _eventAggregator.GetEvent<FavoriteSourceEvent>().Publish(SelectedSource);

            if (SelectedSource.IsFavorite)
            {
                SelectedSource.IsFavorite = false;

                SelectedSource.Categories.ForEach(i => i.IsFavorite = false);
            }
            else
            {
                SelectedSource.IsFavorite = true;

                if (SelectedSource.Categories.All(i => !i.IsFavorite))
                    SelectedSource.Categories.ForEach(i => i.IsFavorite = true);
            }
        }

        async public void BuyApplication()
        {
            var status = await InAppHelper.BuyApplication();

            switch (status)
            {
                case BuyStatus.AlreadyBought:
                case BuyStatus.Bought:
                    OnPropertyChanged(() => IsBought);

                    break;

                case BuyStatus.NotPurchased:

                    await _dialogService.ShowAsync(string.Format(_resourceLoader.GetString("NotBuyErrorMessage"), Settings.General.AppNameCapitalized), _resourceLoader.GetString("WarningTitle"), _resourceLoader.GetString("OK"));
                    break;

                case BuyStatus.Error:
                case BuyStatus.NotFulfilled:
                default:

                    await _dialogService.ShowAsync(_resourceLoader.GetString("BuyErrorMessage"), _resourceLoader.GetString("ErrorTitle"), _resourceLoader.GetString("OK"));
                    break;
            }
        }

        async private Task<bool> LoadFavorited()
        {
            Keyword = string.Empty;

            SelectedSource = SourceRepository.Sources[9999];

            CurrentCategory = SelectedSource.Categories[0];
            CurrentCategory.IsSynchronized = false;

            if (SelectedSource.FavoritedCategories == null)
                SelectedSource.FavoritedCategories = new ObservableCollection<Category>()
                {
                    CurrentCategory
                };

            var newsList = new List<News>();

            var folder = await _storageService.CreateDirectories(Settings.Application.FavoriteFolder, StorageStrategy.Roaming);
            var files = await folder.GetFilesAsync();

            var id = 0;
            foreach (var item in files)
            {
                var hasError = false;
                try
                {
                    var stream = await _storageService.OpenFile(string.Format(Settings.Application.FavoriteFolder, item.Name), StorageStrategy.Roaming);
                    var article = Serializer.Deserialize<Article>(stream);

                    if (article == null)
                        hasError = true;
                    else
                    {
                        var image = article.Images.FirstOrDefault();

                        var newsitem = new News()
                        {
                            ID = id++,
                            SourceID = SelectedSource.ID,
                            CategoryID = CurrentCategory.ID,
                            Link = article.Link.OriginalString,
                            Title = article.Title,
                            Date = article.Date,
                            Image = image != null ? image.Uri.OriginalString : string.Empty
                        };

                        newsList.Add(newsitem);
                    }
                }
                catch
                {
                    hasError = true;
                }

                if (hasError)
                {
                    try
                    {
                        var stream = await _storageService.OpenFile(string.Format(Settings.Application.FavoriteFolder, item), StorageStrategy.Roaming);
                        var newsitem = Serializer.Deserialize<News>(stream);

                        newsList.Add(newsitem);
                    }
                    catch
                    {
                    }
                }
            }

            CurrentCategory.IsSynchronized = true;

            if (CurrentCategory != null && CurrentCategory.News != null && CurrentCategory.News.Count == newsList.Count)
                return false;

            switch (ActiveOrderType)
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

            return true;
        }

        async private void LoadCategoryNews(int categoryIndex, bool isSelectionChanged = false)
        {
            if (IsFavorited)
                return;

            Keyword = string.Empty;

            if (!_isLoadAllCategoryNews)
            {
                var categories = SelectedSource.Categories.Where(i => i.IsFavorite);
                if (SelectedSource.FavoritedCategories == null || SelectedSource.FavoritedCategories.Count != categories.Count())
                    SelectedSource.FavoritedCategories = new ObservableCollection<Category>(SelectedSource.Categories.Where(i => i.IsFavorite));

                if (SelectedSource.FavoritedCategories.Count == 0)
                    SelectedSource.FavoritedCategories = new ObservableCollection<Category>(SelectedSource.Categories);
            }
            else
            {
                if (SelectedSource.FavoritedCategories == null || SelectedSource.FavoritedCategories.Count != SelectedSource.Categories.Count)
                {
                    SelectedSource.FavoritedCategories = new ObservableCollection<Category>(SelectedSource.Categories);
                }

                if (!isSelectionChanged)
                    SelectedCategoryIndex = categoryIndex;
            }
            if (SelectedSource.FavoritedCategories.Count == 0)
                return;

            CurrentCategory = SelectedSource.FavoritedCategories[categoryIndex];
            _newsService.SetCategory(CurrentCategory);

            if (!HasNetwork)
            {
                CurrentCategory.IsSynchronized = true;
                return;
            }

            if (CurrentCategory.IsSynchronized)
                return;

            try
            {
                await _newsService.LoadNewsList(ActiveOrderType);

                CurrentCategory.IsSynchronized = true;
            }
            catch
            {
                await _dialogService.ShowAsync(_resourceLoader.GetString("ParseErrorMessage"), _resourceLoader.GetString("WarningTitle"), _resourceLoader.GetString("OK"));

                CurrentCategory.IsSynchronized = true;
            }
        }

        private void SelectNews(ItemClickEventArgs obj)
        {
            if (obj.ClickedItem == null)
                return;

            DetailViewModel.SelectedNews = (News)obj.ClickedItem;

            if (_isNarrow)
                _navigationService.Navigate(PageTokens.ReadingDetail, DetailViewModel.SelectedNews.CombinedID);
            else
                DetailViewModel.Initialize();
        }

        public void CheckConnection()
        {
            if (!HasNetwork)
                HasNoConnection = true;
            else
                HasNoConnection = false;

            OnPropertyChanged(() => HasNoConnection);
        }

        public void ShowHideFilters()
        {
            IsFilterActive = !IsFilterActive;
        }

        public void ApplyFilters()
        {
            if (IsFilterActive)
            {
                RefreshCategory();
                ShowHideFilters();
            }
        }

        public void Filters()
        {
            ShowHideFilters();
        }

        public void FilterTextbox(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                ApplyFilters();
        }

        public void TitleOrderASC()
        {
            if (ActiveOrderType == OrderType.TitleAsc)
                return;

            ActiveOrderType = OrderType.TitleAsc;

            ApplyFilters();
        }

        public void TitleOrderDESC()
        {
            if (ActiveOrderType == OrderType.TitleDesc)
                return;

            ActiveOrderType = OrderType.TitleDesc;

            ApplyFilters();
        }

        public void DateOrderASC()
        {
            if (ActiveOrderType == OrderType.DateAsc)
                return;

            ActiveOrderType = OrderType.DateAsc;

            ApplyFilters();
        }

        public void DateOrderDESC()
        {
            if (ActiveOrderType == OrderType.DateDesc)
                return;

            ActiveOrderType = OrderType.DateDesc;

            ApplyFilters();
        }

        public void RefreshCategory()
        {
            if (CurrentCategory != null)
            {
                CheckConnection();

                if (CurrentCategory.News != null)
                    CurrentCategory.News.Clear();
                CurrentCategory.IsSynchronized = false;
                LoadCategoryNews(SelectedSource.Categories.IndexOf(CurrentCategory));
            }
        }

        private bool CheckIsPinned(bool notifyPropertyChange = true)
        {
            if (SelectedSource == null)
            {
                IsPinned = false;

                if (notifyPropertyChange)
                    OnPropertyChanged(() => IsPinned);

                return IsPinned;
            }

            var tileId = string.Format(Settings.Application.TileID, SelectedSource.ID);

            IsPinned = SecondaryTile.Exists(tileId);

            if (notifyPropertyChange)
                OnPropertyChanged(() => IsPinned);

            return IsPinned;
        }

        async public void PinToStart()
        {
            try
            {
                var tileId = string.Format(Settings.Application.TileID, SelectedSource.ID);

                if (CheckIsPinned(false))
                {
                    // Unpin
                    SecondaryTile secondaryTile = new SecondaryTile(tileId);

                    await secondaryTile.RequestDeleteAsync();
                }
                else
                {
                    // Pin
                    var squareLogo = new Uri(SelectedSource.Logo, UriKind.Absolute);
                    var wide310x150Logo = new Uri(SelectedSource.Logo.Replace(".png", "_wide.png"), UriKind.Absolute);
                    var tileActivationArguments = SelectedSource.ID.ToString();

                    var secondaryTile = new SecondaryTile(tileId, SelectedSource.Name, tileActivationArguments, squareLogo, TileSize.Square150x150);
                    secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = true;
                    secondaryTile.VisualElements.Wide310x150Logo = wide310x150Logo;
                    secondaryTile.VisualElements.ShowNameOnWide310x150Logo = true;
                    secondaryTile.VisualElements.Square310x310Logo = squareLogo;
                    secondaryTile.VisualElements.ShowNameOnSquare310x310Logo = true;

                    if (!Settings.News.IsTransparentTile)
                        secondaryTile.VisualElements.BackgroundColor = Color.FromArgb(
                                Convert.ToByte(SelectedSource.Color.Substring(1, 2), 16),
                                Convert.ToByte(SelectedSource.Color.Substring(3, 2), 16),
                                Convert.ToByte(SelectedSource.Color.Substring(5, 2), 16),
                                Convert.ToByte(SelectedSource.Color.Substring(7, 2), 16)
                            );

                    await secondaryTile.RequestCreateAsync();
                }

                CheckIsPinned();
            }
            catch { }
        }
    }
}