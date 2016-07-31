using Haberler.Common;
using Haberler.Common.Constants;
using Haberler.Common.Entities;
using Haberler.Services;
using Haberler.Services.Enums;
using Haberler.Services.Repository;
using Library10.Common.Converters;
using Library10.Common.Extensions;
using Library10.Core.UI;
using Prism.Commands;
using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.System;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Haberler.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IStoreService _storeService;
        private readonly INewsService _newsService;
        private readonly IResourceLoader _resourceLoader;

        private WebView _webBrowser = null;
        private bool IsLoaded = false;

        private double _fontSize;
        public double FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                _fontSize = value;
                Settings.News.FontSize = (int)_fontSize;
                OnPropertyChanged(() => FontSize);
                LoadPreview();
            }
        }

        public List<RefreshInterval> Intervals { get; set; }

        private RefreshInterval _selectedInterval;
        public RefreshInterval SelectedInterval
        {
            get
            {
                return _selectedInterval;
            }
            set
            {
                _selectedInterval = value;
                Settings.News.RefreshInterval = value.Value;
                OnPropertyChanged(() => SelectedInterval);
                BackgroundTaskUpdater();
            }
        }

        public bool IsBackgroundActive
        {
            get
            {
                return Settings.News.IsBackgroundActive;
            }
            set
            {
                Settings.News.IsBackgroundActive = value;
                OnPropertyChanged(() => IsBackgroundActive);
                BackgroundTaskUpdater();
            }
        }

        public bool IsTransparentTile
        {
            get
            {
                return Settings.News.IsTransparentTile;
            }
            set
            {
                Settings.News.IsTransparentTile = value;
                OnPropertyChanged(() => IsTransparentTile);
                TileUpdater();
            }
        }

        public List<KeyValuePair<string, Theme>> ThemeList { get; set; }

        private KeyValuePair<string, Theme> _selectedTheme;
        public KeyValuePair<string, Theme> SelectedTheme
        {
            get
            {
                return _selectedTheme;
            }
            set
            {
                _selectedTheme = value;
                Settings.News.Theme = value.Value;
                OnPropertyChanged(() => SelectedTheme);
                LoadPreview();
            }
        }

        public DelegateCommand AppWebsiteCommand { get; private set; }

        public DelegateCommand TwitterCommand { get; private set; }

        public DelegateCommand FacebookCommand { get; private set; }

        public DelegateCommand LinkedInCommand { get; private set; }

        public DelegateCommand OtherAppsCommand { get; private set; }

        public DelegateCommand CompanyWebsiteCommand { get; private set; }

        public DelegateCommand PrivacyCommand { get; private set; }

        public SettingsPageViewModel(
            INavigationService navigationService,
            IStoreService storeService,
            INewsService newsService,
            IResourceLoader resourceLoader

            )
        {
            _navigationService = navigationService;
            _storeService = storeService;
            _newsService = newsService;
            _resourceLoader = resourceLoader;

            AppWebsiteCommand = new DelegateCommand(AppWebsite);
            TwitterCommand = new DelegateCommand(Twitter);
            FacebookCommand = new DelegateCommand(Facebook);
            LinkedInCommand = new DelegateCommand(LinkedIn);
            OtherAppsCommand = new DelegateCommand(OtherApps);
            CompanyWebsiteCommand = new DelegateCommand(CompanyWebsite);
            PrivacyCommand = new DelegateCommand(PrivacyPolicy);
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);

            FontSize = Settings.News.FontSize;

            Intervals = new List<RefreshInterval>(RefreshIntervalRepository.Intervals);
            SelectedInterval = RefreshIntervalRepository.Intervals.FirstOrDefault(i => i.Value == Settings.News.RefreshInterval);

            ThemeList = new List<KeyValuePair<string, Theme>>();
            ThemeList.Add(new KeyValuePair<string, Theme>(_resourceLoader.GetString("ThemeDark"), Theme.Dark));
            ThemeList.Add(new KeyValuePair<string, Theme>(_resourceLoader.GetString("ThemeSepia"), Theme.Sepia));
            ThemeList.Add(new KeyValuePair<string, Theme>(_resourceLoader.GetString("ThemeLight"), Theme.Light));
            SelectedTheme = ThemeList.FirstOrDefault(i => i.Value == Settings.News.Theme);

            if (_webBrowser == null)
            {
                var frame = VisualHelper.FindVisualChild<Frame>(Window.Current.Content);

                if (frame != null && frame.Content is Page)
                {
                    var pivot = VisualHelper.FindVisualChild<Pivot>(frame.Content as Page);

                    if (pivot != null)
                    {
                        var browser = VisualHelper.FindVisualChild<WebView>((((pivot.Items[0] as PivotItem).Content as ScrollViewer).Content as StackPanel));

                        _webBrowser = browser;
                    }
                }
            }

            LoadPreview();

            IsLoaded = true;
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
        }

        private void LoadPreview()
        {
            try
            {
                _webBrowser.NavigateToString(
                    _newsService.GenerateHtml(
                        _resourceLoader.GetString("SampleTitle"),
                        "#",
                        _resourceLoader.GetString("SampleUrl"),
                        DateTimeConverter.ToTurkishRelativeTime(DateTime.Now),
                        null,
                        _resourceLoader.GetString("SampleText"),
                        Settings.News.FontSize,
                        Settings.News.Theme
                    ));
            }
            catch { }
        }

        private async void TileUpdater()
        {
            var secondaryTiles = await SecondaryTile.FindAllForPackageAsync();
            var isTransparent = IsTransparentTile;

            foreach (var item in secondaryTiles)
            {
                if (isTransparent)
                    item.VisualElements.BackgroundColor = Colors.Transparent;
                else
                {
                    int sourceId = 0;
                    bool res = int.TryParse(item.Arguments, out sourceId);

                    if (res)
                    {
                        var source = SourceRepository.Sources[sourceId];

                        item.VisualElements.BackgroundColor = Color.FromArgb(
                                    Convert.ToByte(source.Color.Substring(1, 2), 16),
                                    Convert.ToByte(source.Color.Substring(3, 2), 16),
                                    Convert.ToByte(source.Color.Substring(5, 2), 16),
                                    Convert.ToByte(source.Color.Substring(7, 2), 16)
                                );
                    }
                }
                await item.UpdateAsync();
            }
        }

        private async void BackgroundTaskUpdater()
        {
            if (!IsLoaded)
                return;

            var isBackgroundActive = IsBackgroundActive;

            if (!Settings.News.IsBackgroundActive)
                UnRegisterBackgroundTask();
            else
                RegisterBackgroundTask();
        }

        private async void UnRegisterBackgroundTask()
        {
            foreach (var task in BackgroundTaskRegistration.AllTasks)
                if (task.Value.Name == Settings.Application.TaskName)
                    task.Value.Unregister(true);

            var tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            tileUpdater.Clear();

            var secondaryTiles = await SecondaryTile.FindAllForPackageAsync();
            foreach (var item in secondaryTiles)
            {
                var secondaryTileUpdater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(item.TileId);

                secondaryTileUpdater.Clear();
            }
        }

        private async void RegisterBackgroundTask()
        {
            //ActiveSource

            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            if (backgroundAccessStatus == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                backgroundAccessStatus == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                    if (task.Value.Name == Settings.Application.TaskName)
                        task.Value.Unregister(true);

                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
                taskBuilder.Name = Settings.Application.TaskName;
                taskBuilder.TaskEntryPoint = Settings.Application.TaskEntryPoint;
                taskBuilder.SetTrigger(new TimeTrigger(Settings.News.RefreshInterval, false));
                var registration = taskBuilder.Register();
            }
        }

        public async void AppWebsite()
        {
            await Launcher.LaunchUriAsync(Settings.General.AppUrl.ToUri(true));
        }

        public async void Twitter()
        {
            await Launcher.LaunchUriAsync(Settings.General.TwitterUrl.ToUri(true));
        }

        public async void Facebook()
        {
            await Launcher.LaunchUriAsync(Settings.General.FacebookUrl.ToUri(true));
        }

        public async void LinkedIn()
        {
            await Launcher.LaunchUriAsync(Settings.General.LinkedinUrl.ToUri(true));
        }

        public async void CompanyWebsite()
        {
            await Launcher.LaunchUriAsync(Settings.General.CompanyUrl.ToUri(true));
        }

        public async void PrivacyPolicy()
        {
            await Launcher.LaunchUriAsync(Settings.General.PrivacyPolicyUrl.ToUri(true));
        }

        public void OtherApps()
        {
            _storeService.OtherApps();
        }

        public ObservableCollection<ChangeLogItem> ChangeLogs
        {
            get
            {
                return new ObservableCollection<ChangeLogItem>(GetChangeLogs());
            }
        }

        private IEnumerable<ChangeLogItem> GetChangeLogs()
        {
            var result = new List<ChangeLogItem>();

            result.Add(new ChangeLogItem()
            {
                Header = "v3.0.10.0",
                Lines = new List<string>() {
                    "- Windows 10 masaüstü ve telefonlar için sıfırdan geliştiridi.",
                    "- Haber kaynaklarında haber listesinin alınmasında iyileştirmeler ve düzeltmeler yapıldı. Artık birçok kaynakta listede haberin resmini görebileceksiniz.",
                    "- Haberin ayrıştırılmasında iyileştirmeler yapıldı."
                }
            });

            result.Add(new ChangeLogItem()
            {
                Header = "v2.0.2.3",
                Lines = new List<string>() {
                    "- Haber başlıklarında filtreleme eklendi.",
                    "- Haberleri Başlığa ve Tarihe göre sıralama eklendi."
                }
            });

            result.Add(new ChangeLogItem()
            {
                Header = "v2.0.2.1",
                Lines = new List<string>() {
                    "- Haberleri saklama ve sonradan okuma eklendi.",
                    "- Haber kaynakları için geniş kutu eklendi.",
                    "- Ayarlar sayfasına arka plan ve kutu ayarları eklendi.",
                    "- Cumhuriyet sayfa gösterimi düzeltildi."
                }
            });

            result.Add(new ChangeLogItem()
            {
                Header = "v2.0.1.10",
                Lines = new List<string>() {
                    "- Haberleri saklama ve saklanan haberleri okuyabilme eklendi.",
                    "- Hakkında ekranına Gizlilik sözleşmesi linki eklendi.",
                    "- İnternet koptuğunda oluşan hatalar giderildi."
                }
            });

            result.Add(new ChangeLogItem()
            {
                Header = "v2.0.1.8",
                Lines = new List<string>() {
                    "- Haberler kaynakları için canlı kutucuklar eklendi.",
                    "- Ayarların telefonun backup özelliği kullanılarak Microsoft hesabınıza saklanması sağlandı.",
                    "- Yazı boyutu ve tema değiştirme ayarlar sayfasına taşındı.",
                    "- Ana ekrana sabitlenen kaynakların açılırken yanlış kaynağı göstermesi düzeltildi.",
                    "- Geri bildirim formu eklendi.",
                    "- Diğer iyileştirme ve geliştirmeler yapıldı."
                }
            });

            result.Add(new ChangeLogItem()
            {
                Header = "v2.0.1.5",
                Lines = new List<string>() {
                    "- Posta ve Akşam eklendi.",
                    "- Haber okuma sayfası için light, sepia, dark temaları oluştur."
                }
            });

            result.Add(new ChangeLogItem()
            {
                Header = "v2.0.1.4",
                Lines = new List<string>() {
                    "- Milliyet, Habertürk, Zaman eklendi.",
                    "- Hürriyet yeni kategoriler eklendi.",
                    "- Ntvmsnbc kategorileri düzenlendi ve sayfa gösterimde iyileştirmeler yapıldı.",
                    "- Sözcü yeni kategoriler eklendi ve sayfa gösteriminde iyileştirmeler yapıldı."
                }
            });

            result.Add(new ChangeLogItem()
            {
                Header = "v2.0.1.2",
                Lines = new List<string>() {
                    "- CNN Türk listeleme ve haber gösteriminde iyileştirmeler yapıldı.",
                    "- Radikal haber gösteriminde iyileştirmeler yapıldı.",
                    "- Cumhuriyet haber gösteriminde iyileştirmeler yapıldı.",
                    "- Diğer iyileştirmeler"
                }
            });

            result.Add(new ChangeLogItem()
            {
                Header = "v2.0.0.1",
                Lines = new List<string>() {
                    "- Uygulama sıfırdan universal application olarak yazıldı. Yakında Windows 8 ve üzeri versiyonlar için gelecek.",
                    "- Haber kaynaklarının okunması hızlandırıldı ve iyileştirme çalışmaları yapıldı. ",
                    "- Bazı özellikler (sonradan okuma ve kategori seçimi) yeni versiyona eklenmedi. En kısa sürede eklenecek.",
                    "- Kaynakların gösterim sırası değiştirilebilir yapıldı.",
                    "- Haberin yazı boyutu değiştirilebilir yapıldı.",
                    "- Arayüz tamamiyle değiştirildi, geniş ekranlı telefonlara daha uyumlu ve koyu tonlara hakim tasarım yapıldı."
                }
            });

            return result;
        }
    }
}