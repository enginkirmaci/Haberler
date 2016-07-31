using Haberler.Services.Entities;
using System;

namespace Haberler.DesignViewModels
{
    public class ReadingDetailPageDesignViewModel
    {
        public string HeaderText { get; set; }

        public News SelectedNews { get; set; }

        public ReadingDetailPageDesignViewModel()
        {
            HeaderText = "Haber Başlık Haber Başlık Haber Başlık Haber Başlık Haber Başlık Haber Başlık";
            SelectedNews = new News()
            {
                Title = "Haber Başlık Haber Başlık Haber Başlık Haber Başlık Haber Başlık Haber Başlık",
                Description = "Türk Hava Yolları’nın (THY) İstanbul-Moskova seferini yapacak olan yolcu uçağında bir yolcunun ‘Klimaları açın patladık’ demesi üzerine tüm yolcular uçaktan indirildi ve ‘Patlama’ kelimesi havacılık kuralı gereği şüpheli kabul edilince uçakta bomba araması yapıldı.",
                Link = "http://gundem.milliyet.com.tr/thy-ucaginda-ilginc-olay/gundem/detay/1866506/default.htm",
                Image = "/Assets/test.jpg",
                Date = DateTime.Now.AddMinutes(-5),
                IsLoaded = true
            };
        }
    }
}