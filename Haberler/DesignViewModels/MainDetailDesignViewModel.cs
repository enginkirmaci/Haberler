using Haberler.Services.Entities;
using Haberler.Services.Repository;

namespace Haberler.DesignViewModels
{
    public class MainDetailDesignViewModel
    {
        public Source SelectedSource { get; set; }

        public MainDetailDesignViewModel()
        {
            SelectedSource = SourceRepository.Sources[0];
        }
    }
}