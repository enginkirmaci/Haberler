using Haberler.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Haberler
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppShell : Page
    {
        public AppShell()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        public void AppFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.SourcePageType == typeof(ReadingDetailPage))
            {
                var IsNarrow = Window.Current.Bounds.Width < 720;

                if (IsNarrow)
                {
                    togglePaneButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        public void AppFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            togglePaneButton.Visibility = Visibility.Visible;
        }

        public Frame AppFrame
        {
            get { return rootSplitView.Content as Frame; }
            set { rootSplitView.Content = value; }
        }

        public void SetMenuPaneContent(UIElement content)
        {
            rootSplitView.Pane = content;
        }
    }
}