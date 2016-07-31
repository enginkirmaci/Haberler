using Haberler.Common.Constants;
using Haberler.Events;
using Haberler.Services.Entities;
using Haberler.Services.Repository;
using Prism.Commands;
using Prism.Events;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Haberler.ViewModels
{
    public class MainDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;

        private double _currentSize;

        private Source _selectedSource;
        public Source SelectedSource
        {
            get { return _selectedSource; }
            set { SetProperty(ref _selectedSource, value); }
        }

        public DelegateCommand<ItemClickEventArgs> SelectCategoryCommand { get; private set; }

        public DelegateCommand<object> FavoriteCategoryCommand { get; private set; }

        public MainDetailPageViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator
            )
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;

            SelectCategoryCommand = new DelegateCommand<ItemClickEventArgs>(SelectCategory);
            FavoriteCategoryCommand = new DelegateCommand<object>(FavoriteCategory);
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);

            _currentSize = Window.Current.Bounds.Width;

            SelectedSource = SourceRepository.Sources[(int)e.Parameter];

            Window.Current.SizeChanged += Window_SizeChanged;
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);

            Window.Current.SizeChanged -= Window_SizeChanged;
        }

        private void Window_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (Window.Current.Bounds.Width >= 720 && _currentSize < 720)
            {
                Window.Current.SizeChanged -= Window_SizeChanged;

                if (_navigationService.CanGoBack())
                    _navigationService.GoBack();
            }
            else
                _currentSize = Window.Current.Bounds.Width;
        }

        private void SelectCategory(ItemClickEventArgs obj)
        {
            if (obj.ClickedItem == null)
                return;

            var param = ((Category)obj.ClickedItem).CombinedID;
            _navigationService.Navigate(PageTokens.Reading, param);
            _eventAggregator.GetEvent<BackForwardEvent>().Publish(
                    new BackForwardParameter()
                    {
                        PageToken = PageTokens.Reading,
                        Parameter = param
                    });
        }

        private void FavoriteCategory(object obj)
        {
            var bindedItem = SelectedSource.Categories.FirstOrDefault(i => i.ID == (int)obj);
            if (bindedItem != null)
            {
                if (bindedItem.IsFavorite)
                {
                    bindedItem.IsFavorite = false;

                    if (SelectedSource.Categories.All(i => !i.IsFavorite))
                    {
                        SelectedSource.IsFavorite = false;
                        _eventAggregator.GetEvent<FavoriteSourceEvent>().Publish(SelectedSource);
                    }
                }
                else {
                    bindedItem.IsFavorite = true;

                    if (!SelectedSource.IsFavorite)
                    {
                        SelectedSource.IsFavorite = true;
                        _eventAggregator.GetEvent<FavoriteSourceEvent>().Publish(SelectedSource);
                    }
                }
            }
        }
    }
}