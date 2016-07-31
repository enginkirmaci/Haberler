using Haberler.Services.Entities;
using Haberler.Services.Repository;
using Library10.Common.Entities;
using System;
using Windows.UI.Xaml;

namespace Haberler.DesignViewModels
{
    public class ReadingPageDesignViewModel
    {
        public Source SelectedSource { get; set; }

        public int SelectedCategoryIndex { get; set; }

        public bool IsBought { get; set; }

        public bool IsFavorited { get; set; }

        public ReadingDetailPageDesignViewModel DetailViewModel { get; set; }

        public Thickness Margin { get; set; }

        public ReadingPageDesignViewModel()
        {
            Margin = new Thickness(-12, -32, -12, 0);
            IsBought = false;
            IsFavorited = true;
            SelectedSource = SourceRepository.Sources[9999];

            SelectedSource.Categories[0].IsFavorite = true;

            SelectedSource.Categories[0].IsSynchronized = false;
            SelectedSource.Categories[0].News = new ObservableCollectionEx<News>()
            {
                new News() { Title="Haber Başlık Haber Başlık Haber Başlık Haber Başlık Haber Başlık Haber Başlık",
                    Description="Türk Hava Yolları’nın (THY) İstanbul-Moskova seferini yapacak olan yolcu uçağında bir yolcunun ‘Klimaları açın patladık’ demesi üzerine tüm yolcular uçaktan indirildi ve ‘Patlama’ kelimesi havacılık kuralı gereği şüpheli kabul edilince uçakta bomba araması yapıldı.",
                    Link="http://gundem.milliyet.com.tr/thy-ucaginda-ilginc-olay/gundem/detay/1866506/default.htm",
                    Image="/Assets/test.jpg",
                    Date=DateTime.Now.AddMinutes(-5)},
                new News() { Title="Haber Başlık",
                    Description="Türk Hava Yolları’nın (THY) İstanbul-Moskova seferini yapacak olan yolcu uçağında bir yolcunun ‘Klimaları açın patladık’ demesi üzerine tüm yolcular uçaktan indirildi ve ‘Patlama’ kelimesi havacılık kuralı gereği şüpheli kabul edilince uçakta bomba araması yapıldı.",
                    Link="http://gundem.milliyet.com.tr/thy-ucaginda-ilginc-olay/gundem/detay/1866506/default.htm",
                    Image="ms-appx:///Assets/test.jpg",
                    Date=DateTime.Now.AddDays(-1)},
                new News() { Title="Haber Başlık Haber Başlık Haber Başlık",
                    Description="Türk Hava Yolları’nın (THY) İstanbul-Moskova seferini yapacak olan yolcu uçağında bir yolcunun ‘Klimaları açın patladık’ demesi üzerine tüm yolcular uçaktan indirildi ve ‘Patlama’ kelimesi havacılık kuralı gereği şüpheli kabul edilince uçakta bomba araması yapıldı.",
                    Link="http://gundem.milliyet.com.tr/thy-ucaginda-ilginc-olay/gundem/detay/1866506/default.htm"},
                new News() { Title="Haber Başlık",
                    Description="Türk Hava Yolları’nın (THY) İstanbul-Moskova seferini yapacak olan yolcu uçağında bir yolcunun ‘Klimaları açın patladık’ demesi üzerine tüm yolcular uçaktan indirildi ve ‘Patlama’ kelimesi havacılık kuralı gereği şüpheli kabul edilince uçakta bomba araması yapıldı.",
                    Link="http://gundem.milliyet.com.tr/thy-ucaginda-ilginc-olay/gundem/detay/1866506/default.htm",
                    Image="ms-appx:///Assets/test.jpg",
                    Date=DateTime.Now.AddDays(-2)},
                new News() { Title="Haber Başlık",
                    Description="Türk Hava Yolları’nın (THY) İstanbul-Moskova seferini yapacak olan yolcu uçağında bir yolcunun ‘Klimaları açın patladık’ demesi üzerine tüm yolcular uçaktan indirildi ve ‘Patlama’ kelimesi havacılık kuralı gereği şüpheli kabul edilince uçakta bomba araması yapıldı.",
                    Link="http://gundem.milliyet.com.tr/thy-ucaginda-ilginc-olay/gundem/detay/1866506/default.htm",
                    Date=DateTime.Now.AddDays(-3)
                   }
            };

            DetailViewModel = new ReadingDetailPageDesignViewModel();
            DetailViewModel.SelectedNews = SelectedSource.Categories[0].News[0];

            //SelectedNews = SelectedSource.Categories[0].News[0];
        }
    }
}