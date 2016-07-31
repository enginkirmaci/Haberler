using Haberler.Services.Entities;
using Haberler.Services.Repository;
using Haberler.ViewModels;
using System.Collections.ObjectModel;

namespace Haberler.DesignViewModels
{
    public class MenuViewDesignViewModel
    {
        internal dummyViewModel ConcreteDataContext { get; set; }

        public MenuViewDesignViewModel()
        {
            ConcreteDataContext = new dummyViewModel()
            {
                HeaderCommands = new ObservableCollection<MenuItemViewModel>
                {
                    new MenuItemViewModel { DisplayName = "Haber Kaynakları", FontIcon = "\ue12a" },
                    new MenuItemViewModel { DisplayName = "Okuma Listesi", FontIcon = "\ue7bc" }
                },
                FavoriteCommands = new ObservableCollection<MenuItemViewModel>
                {
                    //new MenuItemViewModel { DisplayName = "Favoriler", FontIcon = "\ue734", IsHeader=true },
                },
                FooterCommands = new ObservableCollection<MenuItemViewModel>
                {
                    new MenuItemViewModel { DisplayName = "Geri Bildirim", FontIcon = "\ue170" },
                    new MenuItemViewModel { DisplayName = "Ayarlar", FontIcon = "\ue713" },
                }
            };

            SourceRepository.Sources[0].IsFavorite = true;
            SourceRepository.Sources[2].IsFavorite = true;
            SourceRepository.Sources[6].IsFavorite = true;
            SourceRepository.Sources[3].IsFavorite = true;
            SourceRepository.Sources[8].IsFavorite = true;

            var sources = new ObservableCollection<Source>()
            {
                SourceRepository.Sources[0],
                SourceRepository.Sources[1],
                SourceRepository.Sources[2],
                SourceRepository.Sources[4],
                SourceRepository.Sources[5],
                SourceRepository.Sources[6],
                SourceRepository.Sources[7],
                SourceRepository.Sources[8],
                SourceRepository.Sources[3]
            };

            foreach (var source in sources)
                ConcreteDataContext.FavoriteCommands.Add(new MenuItemViewModel
                {
                    DisplayName = source.Name,
                    Logo = source.Logo,
                    Color = source.Color
                });
        }
    }

    internal class dummyViewModel
    {
        public ObservableCollection<MenuItemViewModel> HeaderCommands { get; set; }

        public ObservableCollection<MenuItemViewModel> FavoriteCommands { get; set; }

        public ObservableCollection<MenuItemViewModel> FooterCommands { get; set; }
    }
}