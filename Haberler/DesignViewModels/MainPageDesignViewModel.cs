using Haberler.Services.Entities;
using Haberler.Services.Repository;
using System;
using System.Collections.ObjectModel;

namespace Haberler.DesignViewModels
{
    public class MainPageDesignViewModel
    {
        public ObservableCollection<Source> Sources { get; set; }

        public MainDetailDesignViewModel DetailViewModel { get; set; }

        public MainPageDesignViewModel()
        {
            DetailViewModel = new MainDetailDesignViewModel();
            DetailViewModel.SelectedSource = SourceRepository.Sources[0];

            SourceRepository.Sources[0].IsFavorite = true;
            SourceRepository.Sources[2].IsFavorite = true;
            SourceRepository.Sources[6].IsFavorite = true;
            SourceRepository.Sources[3].IsFavorite = true;
            SourceRepository.Sources[8].IsFavorite = true;

            Sources = new ObservableCollection<Source>();

            var random = new Random();
            foreach (var item in SourceRepository.Sources)
            {
                foreach (var cat in item.Value.Categories)
                {
                    cat.IsFavorite = random.Next() % 2 == 1;
                }
                Sources.Add(item.Value);
            }
        }
    }
}