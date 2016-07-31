using Library10.Common.Entities;

namespace Haberler.Services.Entities
{
    public class Category : ObservableObjectEx
    {
        public int ID { get; set; }

        public int SourceID { get; set; }

        public string CombinedID { get { return string.Format("{0}|{1}", SourceID, ID); } }

        public string Name { get; set; }

        public string Url { get; set; }

        private bool _isSynchronized;
        public bool IsSynchronized
        {
            get { return _isSynchronized; }
            set { SetProperty(ref _isSynchronized, value); }
        }

        //private bool _isSelected = true;
        //public bool IsSelected
        //{
        //    get { return _isSelected; }
        //    set { SetProperty(ref _isSelected, value); }
        //}

        private bool _isFavorite;
        public bool IsFavorite
        {
            get { return _isFavorite; }
            set { SetProperty(ref _isFavorite, value); }
        }

        private ObservableCollectionEx<News> _news = null;
        public ObservableCollectionEx<News> News
        {
            get { return _news; }
            set { SetProperty(ref _news, value); }
        }
    }
}