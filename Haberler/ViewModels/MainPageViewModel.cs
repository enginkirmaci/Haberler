using Haberler.Common;
using Haberler.Common.Constants;
using Haberler.Events;
using Haberler.Services.Entities;
using Haberler.Services.Repository;
using Library10.Common.Entities;
using Library10.Common.Enums;
using Library10.Common.Extensions;
using Library10.Core.UI;
using Prism.Commands;
using Prism.Events;
using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Haberler.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly IResourceLoader _resourceLoader;
        private readonly IEventAggregator _eventAggregator;

        private bool IsNarrow;

        public bool IsBought
        {
            get
            {
                return !InAppHelper.IsTrial;
            }
        }

        public ObservableCollectionEx<Source> Sources { get; set; }

        private MainDetailPageViewModel _detailViewModel;
        public MainDetailPageViewModel DetailViewModel
        {
            get { return _detailViewModel; }
            set { SetProperty(ref _detailViewModel, value); }
        }

        public DelegateCommand<ItemClickEventArgs> SelectSourceCommand { get; private set; }

        public DelegateCommand<object> FavoriteSourceCommand { get; private set; }

        public DelegateCommand RemoveAdsCommand { get; private set; }

        public MainPageViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IResourceLoader resourceLoader,
            IEventAggregator eventAggregator)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _resourceLoader = resourceLoader;
            _eventAggregator = eventAggregator;

            SelectSourceCommand = new DelegateCommand<ItemClickEventArgs>(SelectSource);
            FavoriteSourceCommand = new DelegateCommand<object>(FavoriteSource);
            RemoveAdsCommand = new DelegateCommand(BuyApplication);
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);

            if (Sources == null)
                Sources = new ObservableCollectionEx<Source>(SourceRepository.Sources.Values.Where(i => i.ID != 9999).OrderBy(i => i.Name));

            _detailViewModel = new MainDetailPageViewModel(_navigationService, _eventAggregator);

            IsNarrow = Window.Current.Bounds.Width < 720;

            Window.Current.SizeChanged += Window_SizeChanged;
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);

            Window.Current.SizeChanged -= Window_SizeChanged;
        }

        private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            if (!IsNarrow && Window.Current.Bounds.Width < 720 && DetailViewModel.SelectedSource != null)
            {
                _navigationService.Navigate(PageTokens.MainDetail, DetailViewModel.SelectedSource.ID);
            }

            IsNarrow = Window.Current.Bounds.Width < 720;
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

        private void FavoriteSource(object obj)
        {
            var bindedItem = Sources.FirstOrDefault(i => i.ID == (int)obj);
            if (bindedItem != null)
            {
                _eventAggregator.GetEvent<FavoriteSourceEvent>().Publish(bindedItem);

                if (bindedItem.IsFavorite)
                {
                    bindedItem.IsFavorite = false;

                    bindedItem.Categories.ForEach(i => i.IsFavorite = false);
                }
                else {
                    bindedItem.IsFavorite = true;

                    if (bindedItem.Categories.All(i => !i.IsFavorite))
                        bindedItem.Categories.ForEach(i => i.IsFavorite = true);
                }
            }
        }

        private void SelectSource(ItemClickEventArgs obj)
        {
            if (obj.ClickedItem == null)
                return;

            DetailViewModel.SelectedSource = (Source)obj.ClickedItem;

            if (IsNarrow && Window.Current.Bounds.Width < 720)
                _navigationService.Navigate(PageTokens.MainDetail, DetailViewModel.SelectedSource.ID);
        }

        //TODO do this
        //private void RateReminder()
        //{
        //    if (Settings.RateReminderShown || !HasNetwork)
        //        return;

        //    var startCount = Settings.Application.StartCount;
        //    var skipCount = Settings.Application.RateReminderSkipCount;

        //    if (skipCount <= 2 && startCount % (5 + (skipCount * 10)) == 4)
        //    {
        //        _dialogService.ShowAsync(
        //            Resource.Get(STR.RateMessage),
        //            string.Format(Resource.Get(STR.RateMessageTitle),
        //            Settings.General.AppNameCapitalized),
        //            new UICommand() { Id = 0, Label = Resource.Get(STR.YES), Invoked = new UICommandInvokedHandler(async cmd => await _storeService.RateReview()) },
        //            new UICommand() { Id = 1, Label = Resource.Get(STR.NO) }
        //            );

        //        Settings.Application.RateReminderSkipCount = ++skipCount;
        //        Settings.RateReminderShown = true;
        //    }

        //    Settings.Application.StartCount = ++startCount;
        //}
    }
}