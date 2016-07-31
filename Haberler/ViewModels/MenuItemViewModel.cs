using Prism.Windows.Mvvm;
using System.Windows.Input;

namespace Haberler.ViewModels
{
    public class MenuItemViewModel : ViewModelBase
    {
        public int ID { get; set; }

        public string PageToken { get; set; }

        public string DisplayName { get; set; }

        public string FontIcon { get; set; }

        public string Logo { get; set; }

        public string Color { get; set; }

        public bool IsHeader { get; set; }

        public ICommand Command { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}