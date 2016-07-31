using Haberler.Common;
using Haberler.Common.Constants;
using Haberler.Events;
using Haberler.Services;
using Library10.Core.IO;
using Library10.Core.UI;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Unity.Windows;
using Prism.Windows.AppModel;
using Prism.Windows.Navigation;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Haberler
{
    public sealed partial class App : PrismUnityApplication
    {
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session |
                Microsoft.ApplicationInsights.WindowsCollectors.UnhandledException |
                Microsoft.ApplicationInsights.WindowsCollectors.PageView
                );

            this.InitializeComponent();
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            this.Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));

            RegisterTypeIfMissing(typeof(IStoreService), typeof(StoreService), true);
            RegisterTypeIfMissing(typeof(IStorageService), typeof(StorageService), true);
            RegisterTypeIfMissing(typeof(INewsService), typeof(NewsService), true);
            RegisterTypeIfMissing(typeof(IDialogService), typeof(DialogService), false);

            return base.OnInitializeAsync(args);
        }

        protected override UIElement CreateShell(Frame rootFrame)
        {
            var shell = this.Container.Resolve<AppShell>();
            shell.AppFrame = rootFrame;

            shell.AppFrame.Navigated += shell.AppFrame_Navigated;
            shell.AppFrame.Navigating += shell.AppFrame_Navigating;
            shell.AppFrame.Navigated += AppFrame_Navigated;

            return shell;
        }

        private void AppFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back || e.NavigationMode == NavigationMode.Forward)
            {
                var eventAggregator = this.Container.Resolve<IEventAggregator>();
                eventAggregator.GetEvent<BackForwardEvent>().Publish(
                    new BackForwardParameter()
                    {
                        PageToken = e.SourcePageType.Name,
                        Parameter = e.Parameter
                    });
            }
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            Settings.Initialize();

            //var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            //titleBar.BackgroundColor = ((SolidColorBrush)Application.Current.Resources["SystemControlBackgroundChromeMediumBrush"]).Color;
            //titleBar.InactiveBackgroundColor = new Color() { A = 255, R = 20, G = 20, B = 20 };
            //titleBar.ForegroundColor = ((SolidColorBrush)Application.Current.Resources["SystemControlForegroundBaseHighBrush"]).Color;
            //titleBar.InactiveForegroundColor = new Color() { A = 255, R = 255, G = 255, B = 255 };
            //titleBar.ButtonBackgroundColor = ((SolidColorBrush)Application.Current.Resources["SystemControlBackgroundChromeMediumBrush"]).Color;

            //titleBar.ButtonHoverBackgroundColor = new Color() { A = 255, R = 40, G = 40, B = 40 };
            //titleBar.ButtonInactiveBackgroundColor = new Color() { A = 255, R = 20, G = 20, B = 20 };

            //titleBar.ButtonForegroundColor = new Color() { A = 255, R = 255, G = 255, B = 255 };
            //titleBar.ButtonHoverForegroundColor = new Color() { A = 255, R = 255, G = 255, B = 255 };
            //titleBar.ButtonInactiveForegroundColor = new Color() { A = 255, R = 255, G = 255, B = 255 };

            //if (DevTools.IsMobile)
            //{
            //    var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            //    statusbar.BackgroundColor = ((SolidColorBrush)Application.Current.Resources["SystemControlBackgroundChromeMediumBrush"]).Color;
            //    statusbar.BackgroundOpacity = 1;
            //    statusbar.ForegroundColor = ((SolidColorBrush)Application.Current.Resources["SystemControlForegroundBaseHighBrush"]).Color;
            //    statusbar.ShowAsync();
            //}

            if (string.IsNullOrWhiteSpace(args.Arguments))
                this.NavigationService.Navigate(PageTokens.Main, null);
            else
            {
                var eventAggregator = this.Container.Resolve<IEventAggregator>();
                eventAggregator.GetEvent<BackForwardEvent>().Publish(
                    new BackForwardParameter()
                    {
                        PageToken = PageTokens.Reading,
                        Parameter = int.Parse(args.Arguments)
                    });
                this.NavigationService.Navigate(PageTokens.Reading, int.Parse(args.Arguments));
            }

            DeviceGestureService.GoBackRequested += DeviceGestureService_GoBackRequested;

            if (!Settings.News.IsBackgroundActive)
                BackgroundTaskHelper.UnRegisterBackgroundTask();
            else
                BackgroundTaskHelper.RegisterBackgroundTask();

            return Task.FromResult<object>(null);
        }

        private void DeviceGestureService_GoBackRequested(object sender, Prism.Windows.AppModel.DeviceGestureEventArgs e)
        {
            var navigationService = this.Container.Resolve<INavigationService>();

            // Check to see if this is the top-most page on the app back stack.
            if (navigationService.CanGoBack())
            {
                navigationService.GoBack();

                e.Handled = true;
                e.Cancel = true;
                return;
            }

            e.Cancel = true;
            e.Handled = false;
        }

        /*
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = new Color() { A = 255, R = 20, G = 20, B = 20 };
            titleBar.InactiveBackgroundColor = new Color() { A = 255, R = 20, G = 20, B = 20 };
            titleBar.ForegroundColor = new Color() { A = 255, R = 255, G = 255, B = 255 };
            titleBar.InactiveForegroundColor = new Color() { A = 255, R = 255, G = 255, B = 255 };

            titleBar.ButtonBackgroundColor = new Color() { A = 255, R = 10, G = 10, B = 10 };
            titleBar.ButtonHoverBackgroundColor = new Color() { A = 255, R = 40, G = 40, B = 40 };
            titleBar.ButtonInactiveBackgroundColor = new Color() { A = 255, R = 20, G = 20, B = 20 };

            titleBar.ButtonForegroundColor = new Color() { A = 255, R = 255, G = 255, B = 255 };
            titleBar.ButtonHoverForegroundColor = new Color() { A = 255, R = 255, G = 255, B = 255 };
            titleBar.ButtonInactiveForegroundColor = new Color() { A = 255, R = 255, G = 255, B = 255 };
        }
        */
    }
}