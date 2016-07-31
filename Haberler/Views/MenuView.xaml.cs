using Haberler.ViewModels;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Haberler.Views
{
    public sealed partial class MenuView : UserControl, INotifyPropertyChanged
    {
        public MenuView()
        {
            this.InitializeComponent();
            DataContextChanged += MenuControl_DataContextChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MenuViewModel ConcreteDataContext
        {
            get
            {
                return DataContext as MenuViewModel;
            }
        }

        private void MenuControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(ConcreteDataContext)));
            }
        }
    }
}