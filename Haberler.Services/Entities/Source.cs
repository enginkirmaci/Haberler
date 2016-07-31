using Library10.Common.Entities;
using Library10.Net.Entities;
using System.Collections.ObjectModel;

namespace Haberler.Services.Entities
{
    public class Source : ObservableObjectEx
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public string Color { get; set; }

        public bool IsFrontImageDisabled { get; set; }

        //private bool _isSelected;
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

        private int _order = int.MaxValue;
        public int Order
        {
            get { return _order; }
            set { SetProperty(ref _order, value); }
        }

        public ObservableCollectionEx<Category> Categories { get; set; }

        //public ObservableCollection<Category> FavoritedCategories
        //{
        //    get
        //    {
        //        return new ObservableCollection<Category>(Categories.Where(i => i.IsFavorite));
        //    }
        //}

        private ObservableCollection<Category> _favoritedCategories;
        public ObservableCollection<Category> FavoritedCategories
        {
            get { return _favoritedCategories; }
            set { SetProperty(ref _favoritedCategories, value); }
        }

        public ParserSetting ParserSetting { get; set; }
    }
}