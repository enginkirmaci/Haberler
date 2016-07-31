using Library10.Common.Entities;
using System;

namespace Haberler.Services.Entities
{
    public class News : ObservableObjectEx
    {
        public int ID { get; set; }

        public int SourceID { get; set; }

        public int CategoryID { get; set; }

        public string CombinedID { get { return string.Format("{0}|{1}|{2}", SourceID, CategoryID, ID); } }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public DateTime Date { get; set; }

        public string Image { get; set; }
        private bool _isNewsLoaded = false;
        public bool IsLoaded
        {
            get
            {
                return _isNewsLoaded;
            }
            set
            {
                SetProperty(ref _isNewsLoaded, value);
            }
        }
    }
}