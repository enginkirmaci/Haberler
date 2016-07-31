using Haberler.Common;
using Haberler.Common.Constants;
using Haberler.Common.Entities;
using Haberler.Events;
using Haberler.Services.Entities;
using Haberler.Services.Repository;
using Library10.Common.Entities;
using Library10.Common.Extensions;
using Library10.Core.UI;
using Prism.Commands;
using Prism.Events;
using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Haberler.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;

        private int? _selectedFavoriteSource = null;
        private string _selectedPageToken = null;

        public ObservableCollectionEx<MenuItemViewModel> HeaderCommands { get; set; }

        public ObservableCollectionEx<MenuItemViewModel> FavoriteCommands { get; set; }

        public ObservableCollectionEx<MenuItemViewModel> FooterCommands { get; set; }

        public MenuViewModel(
            INavigationService navigationService,
            IResourceLoader resourceLoader,
            IEventAggregator eventAggregator
            )
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;

            HeaderCommands = new ObservableCollectionEx<MenuItemViewModel>
            {
                new MenuItemViewModel { PageToken = PageTokens.Main, DisplayName = resourceLoader.GetString("MainHeader/Text"), FontIcon = "\ue12a", Command = new DelegateCommand<object>(NavigateToPage, CanNavigateToPage) },
                new MenuItemViewModel { PageToken = PageTokens.Reading, DisplayName = resourceLoader.GetString("ReadingListHeader/Text"), FontIcon = "\ue7bc", Command = new DelegateCommand<object>(NavigateToPage, CanNavigateToPage) }
            };

            FavoriteCommands = new ObservableCollectionEx<MenuItemViewModel>();

            FooterCommands = new ObservableCollectionEx<MenuItemViewModel>
            {
                new MenuItemViewModel { PageToken = PageTokens.Feedback, DisplayName = resourceLoader.GetString("FeedbackHeader/Text"), FontIcon = "\ue170", Command = new DelegateCommand<object>(NavigateToPage, CanNavigateToPage) },
                new MenuItemViewModel { PageToken = PageTokens.Settings,  DisplayName = resourceLoader.GetString("SettingsHeader/Text"), FontIcon = "\ue713", Command = new DelegateCommand<object>(NavigateToPage, CanNavigateToPage) }
            };

            _eventAggregator.GetEvent<FavoriteSourceEvent>().Subscribe(source =>
            {
                var found = FavoriteCommands.FirstOrDefault(i => i.ID == source.ID);

                if (found != null)
                    FavoriteCommands.Remove(found);
                else
                    AddFavoriteMenuItem(source);

                var favoritedSources = Settings.News.UserSources != null ? new List<UserSources>(Settings.News.UserSources) : new List<UserSources>();
                var favItem = favoritedSources.FirstOrDefault(i => i.ID == source.ID);

                if (favItem != null)
                    favoritedSources.Remove(favItem);
                else
                    favoritedSources.Add(new UserSources()
                    {
                        ID = source.ID,
                        CategoryIDs = source.Categories.Where(j => j.IsFavorite).Select(j => j.ID).ToArray()
                    });

                Settings.News.UserSources = favoritedSources;
            }, ThreadOption.UIThread);

            _eventAggregator.GetEvent<BackForwardEvent>().Subscribe(args =>
            {
                if (args.Parameter == null)
                {
                    _selectedPageToken = args.PageToken.Replace("Page", string.Empty);
                    _selectedFavoriteSource = null;
                }
                else
                {
                    _selectedPageToken = null;

                    if (args.Parameter is string && !string.IsNullOrWhiteSpace(args.Parameter as string))
                    {
                        var IDs = (args.Parameter as string).Split('|');
                        _selectedFavoriteSource = int.Parse(IDs[0]);
                    }
                    else
                        _selectedFavoriteSource = (int)args.Parameter;
                }

                RaiseCanExecuteChanged();
            });

            LoadFavoritedSources();
        }

        private void LoadFavoritedSources()
        {
            int orderNo = 1;

            if (Settings.News.UserSources != null && Settings.News.UserSources.Count > 0)
            {
                Settings.News.UserSources.ForEach(i =>
                {
                    var source = SourceRepository.Sources[i.ID];
                    source.Order = orderNo++;
                    source.IsFavorite = true;

                    source.Categories.ForEach(j =>
                    {
                        if (i.CategoryIDs.Contains(j.ID))
                            j.IsFavorite = true;
                    });

                    AddFavoriteMenuItem(source);
                });
            }
        }

        private void AddFavoriteMenuItem(Source source)
        {
            FavoriteCommands.Add(new MenuItemViewModel
            {
                ID = source.ID,
                DisplayName = source.Name,
                Logo = source.Logo,
                Color = source.Color,
                Command = new DelegateCommand<object>(NavigateToReadingPage, CanNavigateToReadingPage)
            });
        }

        private bool CanNavigateToPage(object arg)
        {
            return string.IsNullOrWhiteSpace(_selectedPageToken) || _selectedPageToken != (string)arg;
        }

        private void NavigateToPage(object obj)
        {
            if (_selectedPageToken == (string)obj)
                return;

            _selectedPageToken = (string)obj;
            if (_navigationService.Navigate(_selectedPageToken, null))
            {
                SetPaneOpen();
                _selectedFavoriteSource = null;
                RaiseCanExecuteChanged();
            }
        }

        private bool CanNavigateToReadingPage(object arg)
        {
            if (arg == null) return false;

            return !_selectedFavoriteSource.HasValue || (_selectedFavoriteSource.HasValue && _selectedFavoriteSource.Value != (int)arg);
        }

        private void NavigateToReadingPage(object obj)
        {
            if (_selectedFavoriteSource.HasValue && _selectedFavoriteSource.Value == (int)obj)
                return;

            _selectedFavoriteSource = (int)obj;
            if (_navigationService.Navigate(PageTokens.Reading, _selectedFavoriteSource))
            {
                SetPaneOpen();
                _selectedPageToken = null;
                RaiseCanExecuteChanged();
            }
        }

        private void SetPaneOpen()
        {
            var splitView = VisualHelper.FindVisualChild<SplitView>(Window.Current.Content);
            if (splitView != null && splitView is SplitView)
            {
                if ((splitView as SplitView).DisplayMode == SplitViewDisplayMode.Overlay || (splitView as SplitView).DisplayMode == SplitViewDisplayMode.CompactOverlay)
                    (splitView as SplitView).IsPaneOpen = false;
            }
        }

        private void RaiseCanExecuteChanged()
        {
            foreach (var item in HeaderCommands)
            {
                (item.Command as DelegateCommand<object>).RaiseCanExecuteChanged();
            }
            foreach (var item in FavoriteCommands)
            {
                (item.Command as DelegateCommand<object>).RaiseCanExecuteChanged();
            }
            foreach (var item in FooterCommands)
            {
                (item.Command as DelegateCommand<object>).RaiseCanExecuteChanged();
            }
        }
    }
}