using Haberler.Services.Entities;
using Library10.Common.Entities;
using Library10.Net.Entities;
using System.Collections.Generic;

namespace Haberler.Services.Repository
{
    public class SourceRepository
    {
        private static Dictionary<int, Source> _sources = new Dictionary<int, Source>()
        {
            {9999, new Source()
                {
                    ID = 9999,
                    Name = "Reading List",
                    Logo = string.Empty,
                    Color = string.Empty,
                    Categories = new ObservableCollectionEx<Category>()
                        {
                            new Category() { ID=0, SourceID=9999  },
                        }
                }
            },
            { 0, new Source()
                {
                    ID = 0,
                    Name = "Hürriyet",
                    Logo = "ms-appx:///Assets/Icons/Sources/hurriyet.png",
                    Color = "#FFD01714",
                    Categories = new ObservableCollectionEx<Category>()
                        {
                            new Category() { ID=0, SourceID=0, Name="anasayfa", Url="http://rss.hurriyet.com.tr/rss.aspx?sectionId=1"},
                            new Category() { ID=1, SourceID=0, Name="gündem", Url="http://rss.hurriyet.com.tr/rss.aspx?sectionId=2"},
                            new Category() { ID=2, SourceID=0, Name="ekonomi", Url="http://rss.hurriyet.com.tr/rss.aspx?sectionId=4"},
                            new Category() { ID=3, SourceID=0, Name="spor", Url="http://rss.hurriyet.com.tr/rss.aspx?sectionId=14"},
                            new Category() { ID=4, SourceID=0, Name="magazin", Url="http://rss.hurriyet.com.tr/rss.aspx?sectionId=2035"},
                            new Category() { ID=5, SourceID=0, Name="teknoloji", Url="http://rss.hurriyet.com.tr/rss.aspx?sectionId=2158"},
                            new Category() { ID=6, SourceID=0, Name="sağlık", Url="http://rss.hurriyet.com.tr/rss.aspx?sectionId=2446"},
                            new Category() { ID=7, SourceID=0, Name="kültür sanat", Url="http://rss.hurriyet.com.tr/rss.aspx?sectionId=2451"},
                            new Category() { ID=8, SourceID=0, Name="sinema", Url="http://rss.hurriyet.com.tr/rss.aspx?sectionId=2437"}
                        },
                    ParserSetting = new ParserSetting()
                        {
                            Allowed = ".newsDetail",
                            Disallowed = new string[]
                            {
                                ".h-topic-advice",
                                ".newsInfoTop",
                                ".newsDate",
                                ".toolBox",
                                ".mavi",
                                ".kirmizi",
                                ".detailTextAd",
                                ".reklam620x110",
                                ".hsw-topic-wrapper",
                                ".hswt-description",
                                ".hsw-topic",
                                ".detailKeywords",
                                ".evamRelatedNews",
                                ".hurBox",
                                ".comments",
                                ".commentsBand",
                                ".hurRight",
                                ".date"
                            }
                        }
                }
            },
            {1, new Source()
                {
                    ID = 1,
                    Name = "ntvmsnbc",
                    Logo = "ms-appx:///Assets/Icons/Sources/ntvmsnbc.png",
                    Color = "#FF0277BA",
                    IsFrontImageDisabled = true,
                    Categories = new ObservableCollectionEx<Category>()
                        {
                            new Category() { ID=0, SourceID=1, Name="gündem", Url="http://www.ntv.com.tr/gundem.rss"},
                            new Category() { ID=1, SourceID=1, Name="türkiye", Url="http://www.ntv.com.tr/turkiye.rss"},
                            new Category() { ID=2, SourceID=1, Name="dünya", Url="http://www.ntv.com.tr/dunya.rss"},
                            new Category() { ID=3, SourceID=1, Name="ekonomi", Url="http://www.ntv.com.tr/ekonomi.rss"},
                            new Category() { ID=4, SourceID=1, Name="spor", Url="http://www.ntv.com.tr/spor.rss"},
                            new Category() { ID=5, SourceID=1, Name="sanat", Url="http://www.ntv.com.tr/sanat.rss"},
                            new Category() { ID=6, SourceID=1, Name="teknoloji", Url="http://www.ntv.com.tr/teknoloji.rss"},
                            new Category() { ID=7, SourceID=1, Name="yaşam", Url="http://www.ntv.com.tr/yasam.rss"},
                            new Category() { ID=8, SourceID=1, Name="sağlık", Url="http://www.ntv.com.tr/saglik.rss"}
                        },
                    ParserSetting = new ParserSetting()
                        {
                            Allowed = "story",
                            Disallowed = new string[]
                            {
                                "ShareBar",
                                ".textMedBlack",
                                ".textMedBlackBold",
                                ".textTimestamp",
                                ".aC",
                                "oylamaGenel",
                                ".fb-activity-title",
                                ".fb-activity",
                                "commentArea",
                                "comments",
                                "commentBottom",
                                "SaveAndShare",
                                ".cbx",
                                "bbb",
                                "substory",
                                "Dcolumn",
                                "viewRelatedPhotosLink",
                                ".related-story",
                                ".source",
                                ".tags",
                                ".follow-us",
                                ".category",
                                ".story-nav",
                                ".ros",
                                ".more"
                            }
                        }
                }
            },
            {2, new Source()
                {
                    ID = 2,
                    Name = "CNN TÜRK",
                    Logo = "ms-appx:///Assets/Icons/Sources/cnnturk.png",
                    Color = "#FFA00002",
                    IsFrontImageDisabled = true,
                    Categories = new ObservableCollectionEx<Category>()
                        {
                            new Category() { ID=0, SourceID=2, Name="son Haberler", Url="http://www.cnnturk.com/feed/rss/news"},
                            new Category() { ID=1, SourceID=2, Name="türkiye", Url="http://www.cnnturk.com/feed/rss/news/turkiye"},
                            new Category() { ID=2, SourceID=2, Name="dünya", Url="http://www.cnnturk.com/feed/rss/news/dunya"},
                            new Category() { ID=3, SourceID=2, Name="kültür sanat", Url="http://www.cnnturk.com/feed/rss/news/kultur-sanat"},
                            new Category() { ID=4, SourceID=2, Name="bilim teknoloji", Url="http://www.cnnturk.com/feed/rss/news/bilim-teknoloji"},
                            new Category() { ID=5, SourceID=2, Name="yaşam", Url="http://www.cnnturk.com/feed/rss/news/yasam"},
                            new Category() { ID=6, SourceID=2, Name="magazin", Url="http://www.cnnturk.com/feed/rss/news/magazin"},
                            new Category() { ID=7, SourceID=2, Name="ekonomi", Url="http://www.cnnturk.com/feed/rss/news/ekonomi"},
                            new Category() { ID=8, SourceID=2, Name="spor", Url="http://www.cnnturk.com/feed/rss/news/spor"},
                            new Category() { ID=9, SourceID=2, Name="sağlık", Url="http://www.cnnturk.com/feed/rss/news/saglik"}
                        },
                    ParserSetting = new ParserSetting()
                        {
                            DontStripUnlikelys = true,
                            Allowed = ".news-body",
                            Disallowed = new string[]
                            {
                                ".date",
                                ".icon",
                                ".social-wrapper",
                                ".btn-breaking-news",
                                ".custom-category-footer",
                                ".tech-footer",
                                ".news-nav",
                                ".share-wrapper",
                                ".ads-wrapper",
                                ".disqus",
                                ".releated-news",
                                ".col-md-4",
                                ".col-xs-4",
                                ".fb-post",
                                ".keywords",
                                ".news-source",
                                ".content-gallery"
                            }
                        }
                }
            },
            {3, new Source()
                {
                    ID = 3,
                    Name = "Sözcü",
                    Logo = "ms-appx:///Assets/Icons/Sources/sozcu.png",
                    Color = "#FFEE1B23",
                    IsFrontImageDisabled = true,
                    Categories = new ObservableCollectionEx<Category>()
                        {
                            new Category() { ID=0, SourceID=3, Name="anasayfa", Url="http://sozcu.com.tr/feed/"},
                            new Category() { ID=1, SourceID=3, Name="gündem", Url="http://sozcu.com.tr/kategori/gundem/feed/"},
                            new Category() { ID=2, SourceID=3, Name="dünya", Url="http://sozcu.com.tr/kategori/dunya/feed/"},
                            new Category() { ID=3, SourceID=3, Name="ekonomi", Url="http://sozcu.com.tr/kategori/ekonomi/feed/"},
                            new Category() { ID=4, SourceID=3, Name="magazin", Url="http://sozcu.com.tr/kategori/magazin/feed/"},
                            new Category() { ID=5, SourceID=3, Name="teknoloji", Url="http://www.sozcu.com.tr/kategori/teknoloji/feed/"},
                            new Category() { ID=6, SourceID=3, Name="sağlık", Url="http://www.sozcu.com.tr/kategori/saglik/feed/"},
                            new Category() { ID=7, SourceID=3, Name="kültür sanat", Url="http://www.sozcu.com.tr/kultur-sanat/feed/"},
                        },
                    ParserSetting = new ParserSetting()
                        {
                            Allowed = ".single"
                            ,Disallowed = new string[]
                            {
                                ".in_fallow",
                                ".babil-breadcrumb-wrap",
                                ".date"
                            }
                        }
                }
            },
            {4, new Source()
                {
                    ID = 4,
                    Name = "Vatan",
                    Logo = "ms-appx:///Assets/Icons/Sources/vatan.png",
                    Color = "#FFDA1C23",
                    IsFrontImageDisabled = true,
                    Categories = new ObservableCollectionEx<Category>()
                        {
                            new Category() { ID=0, SourceID=4, Name="gündem", Url="http://www.gazetevatan.com/rss/gundem.xml"},
                            new Category() { ID=1, SourceID=4, Name="dünya", Url="http://www.gazetevatan.com/rss/dunya.xml"},
                            new Category() { ID=2, SourceID=4, Name="ekonomi", Url="http://www.gazetevatan.com/rss/ekonomi.xml"},
                            new Category() { ID=3, SourceID=4, Name="yaşam", Url="http://www.gazetevatan.com/rss/yasam.xml"},
                            new Category() { ID=4, SourceID=4, Name="magazin", Url="http://www.gazetevatan.com/rss/magazin.xml"},
                            new Category() { ID=5, SourceID=4, Name="futbol", Url="http://www.gazetevatan.com/rss/futbol.xml"},
                            new Category() { ID=6, SourceID=4, Name="teknoloji", Url="http://www.gazetevatan.com/rss/teknoloji.xml"},
                            new Category() { ID=7, SourceID=4, Name="sağlık", Url="http://www.gazetevatan.com/rss/saglik.xml"}
                        },
                    ParserSetting = new ParserSetting()
                        {
                            Allowed = ".dtyL",
                            Disallowed = new string[]
                            {
                                ".bcumb",
                                ".tools",
                                ".datesrc",
                                ".bhorz",
                                ".dtyad",
                                ".comments",
                                ".metas",
                                "iframeHolder610x250"
                            }
                        }
                }
            },
            {5, new Source()
                {
                    ID = 5,
                    Name = "fotoMaç",
                    Logo = "ms-appx:///Assets/Icons/Sources/fotomac.png",
                    Color = "#FF0B2945",
                    Categories = new ObservableCollectionEx<Category>()
                        {
                            new Category() { ID=0, SourceID=5, Name="ana Sayfa", Url="http://www.fotomac.com.tr/rss/Anasayfa.xml"},
                            new Category() { ID=1, SourceID=5, Name="futbol", Url="http://www.fotomac.com.tr/rss/Futbol.xml"},
                            new Category() { ID=2, SourceID=5, Name="son 24 saat", Url="http://www.fotomac.com.tr/rss/Son24Saat.xml"},
                            new Category() { ID=3, SourceID=5, Name="spor toto süper lig", Url="http://www.fotomac.com.tr/rss/SuperLig.xml"},
                            new Category() { ID=4, SourceID=5, Name="PTT 1. Lig", Url="http://www.fotomac.com.tr/rss/PTTBirinciLig.xml"},
                            new Category() { ID=5, SourceID=5, Name="beşiktaş", Url="http://www.fotomac.com.tr/rss/Besiktas.xml"},
                            new Category() { ID=6, SourceID=5, Name="fenerbahçe", Url="http://www.fotomac.com.tr/rss/Fenerbahce.xml"},
                            new Category() { ID=7, SourceID=5, Name="galatasaray", Url="http://www.fotomac.com.tr/rss/Galatasaray.xml"},
                            new Category() { ID=8, SourceID=5, Name="trabzonspor", Url="http://www.fotomac.com.tr/rss/Trabzonspor.xml"},
                            new Category() { ID=9, SourceID=5, Name="basketbol", Url="http://www.fotomac.com.tr/rss/Basketbol.xml"},
                            new Category() { ID=10, SourceID=5, Name="voleybol", Url="http://www.fotomac.com.tr/rss/Voleybol.xml"},
                            new Category() { ID=11, SourceID=5, Name="tenis", Url="http://www.fotomac.com.tr/rss/Tenis.xml"}
                        },
                    ParserSetting = new ParserSetting()
                        {
                            DontStripUnlikelys = true,
                            Allowed = ".news",
                            Disallowed = new string[]
                            {
                                ".breadcrumbs",
                                ".row",
                                ".labels",
                                ".faceLink"
                            }
                        }
                }
            },
            {6, new Source()
                {
                    ID = 6,
                    Name = "Amk",
                    Logo = "ms-appx:///Assets/Icons/Sources/amk.png",
                    Color = "#FFFFD500",
                    Categories = new ObservableCollectionEx<Category>()
                        {
                            new Category() { ID=0, SourceID=6, Name="anasayfa", Url="http://amkspor.com/feed/"},
                            new Category() { ID=1, SourceID=6, Name="futbol", Url="http://amkspor.sozcu.com.tr/kategori/futbol-2/feed/"},
                            new Category() { ID=2, SourceID=6, Name="basketbol", Url="http://amkspor.sozcu.com.tr/kategori/basketbol-2/feed/"},
                            new Category() { ID=3, SourceID=6, Name="at yarışı", Url="http://amkspor.sozcu.com.tr/kategori/at-yarisi-2/feed/"},
                            new Category() { ID=4, SourceID=6, Name="voleybol", Url="http://amkspor.sozcu.com.tr/kategori/voleybol-2/feed/"},
                            new Category() { ID=5, SourceID=6, Name="tenis", Url="http://amkspor.sozcu.com.tr/kategori/diger-sporlar-2/tenis-diger-sporlar-2/feed/"},
                            new Category() { ID=6, SourceID=6, Name="motor sporları", Url="http://amkspor.sozcu.com.tr/kategori/diger-sporlar-2/motor-sporlari-diger-sporlar-2/feed/"}
                        },
                    ParserSetting = new ParserSetting()
                        {
                            Allowed = ".index-sol",
                            Disallowed = new string[]
                            {
                                ".in_breadcrumb",
                                ".in_time",
                                ".in_social",
                                ".in_sidebar",
                                ".in_fallow",
                                ".in_comini",
                                ".in_disqus",
                                ".in_comline",
                                ".discussion"
                            }
                        }
                }
            },
            {7, new Source()
                {
                    ID = 7,
                    Name = "Cumhuriyet",
                    Logo = "ms-appx:///Assets/Icons/Sources/cumhuriyet.png",
                    Color = "#FFED1C24",
                    IsFrontImageDisabled = true,
                    Categories = new ObservableCollectionEx<Category>()
                        {
                            new Category() { ID=0, SourceID=7, Name="son dakika", Url="http://www.cumhuriyet.com.tr/rss/son_dakika"},
                            new Category() { ID=1, SourceID=7, Name="anasayfa", Url="http://www.cumhuriyet.com.tr/rss/1"},
                            new Category() { ID=2, SourceID=7, Name="siyaset", Url="http://www.cumhuriyet.com.tr/rss/3"},
                            new Category() { ID=3, SourceID=7, Name="ekonomi", Url="http://www.cumhuriyet.com.tr/rss/6"},
                            new Category() { ID=4, SourceID=7, Name="dünya", Url="http://www.cumhuriyet.com.tr/rss/5"},
                            new Category() { ID=5, SourceID=7, Name="spor", Url="http://www.cumhuriyet.com.tr/rss/9"},
                            new Category() { ID=6, SourceID=7, Name="sağlık", Url="http://www.cumhuriyet.com.tr/rss/12"},
                            new Category() { ID=7, SourceID=7, Name="kültür sanat", Url="http://www.cumhuriyet.com.tr/rss/7"},
                            new Category() { ID=8, SourceID=7, Name="bilim teknik", Url="http://www.cumhuriyet.com.tr/rss/11"}
                        },
                    ParserSetting = new ParserSetting()
                        {
                            DontStripUnlikelys = true,
                            Allowed = "content",
                            Disallowed = new string[]
                            {
                                "share-bar",
                                ".publish-date",
                                "font-adjust",
                                "comment-area",
                                "onerilen-haberler"
                            }
                        }
                }
            },
            {8, new Source()
                {
                    ID = 8,
                    Name = "Radikal",
                    Logo = "ms-appx:///Assets/Icons/Sources/radikal.png",
                    Color = "#FF0095D9",
                    IsFrontImageDisabled = true,
                    Categories = new ObservableCollectionEx<Category>()
                        {
                            new Category() { ID=0, SourceID=8, Name="son dakika", Url="http://www.radikal.com.tr/d/rss/RssSD.xml"},
                            new Category() { ID=1, SourceID=8, Name="türkiye", Url="http://www.radikal.com.tr/d/rss/Rss_77.xml"},
                            new Category() { ID=2, SourceID=8, Name="dünya", Url="http://www.radikal.com.tr/d/rss/Rss_81.xml"},
                            new Category() { ID=3, SourceID=8, Name="ekonomi", Url="http://www.radikal.com.tr/d/rss/Rss_80.xml"},
                            new Category() { ID=4, SourceID=8, Name="kültür", Url="http://www.radikal.com.tr/d/rss/Rss_82.xml"},
                            new Category() { ID=5, SourceID=8, Name="politika", Url="http://www.radikal.com.tr/d/rss/Rss_78.xml"},
                            new Category() { ID=6, SourceID=8, Name="teknoloji", Url="http://www.radikal.com.tr/d/rss/Rss_117.xml"},
                            new Category() { ID=7, SourceID=8, Name="radikal 2", Url="http://www.radikal.com.tr/d/rss/Rss_42.xml"},
                            new Category() { ID=8, SourceID=8, Name="sağlık", Url="http://www.radikal.com.tr/d/rss/Rss_118.xml"},
                            new Category() { ID=9, SourceID=8, Name="sinema", Url="http://www.radikal.com.tr/d/rss/Rss_120.xml"},
                            new Category() { ID=10, SourceID=8, Name="spor", Url="http://www.radikal.com.tr/d/rss/Rss_84.xml"}
                        },
                    ParserSetting = new ParserSetting()
                        {
                            DontStripUnlikelys = true,
                            Allowed = ".news-content-area",
                            Disallowed = new string[]
                            {
                                ".breadcrumb",
                                "adver",
                                "lightboxgemius",
                                ".id-control",
                                ".text-info",
                                ".date",
                                ".options",
                                ".social-area",
                                ".index_keywords",
                                ".id-control",
                                "extra-image",
                                ".suggest-news",
                                ".twit-news"
                            }
                        }
                }
            },
            {9, new Source()
                {
                    ID = 9,
                    Name = "Milliyet",
                    Logo = "ms-appx:///Assets/Icons/Sources/milliyet.png",
                    Color = "#FFED1C24",
                    IsFrontImageDisabled = true,
                    Categories = new ObservableCollectionEx<Category>()
                        {
                            new Category() { ID=0, SourceID=9, Name="son dakika", Url="http://www.milliyet.com.tr/D/rss/rss/RssSD.xml"},
                            new Category() { ID=1, SourceID=9, Name="siyaset", Url="http://www.milliyet.com.tr/D/rss/rss/Rss_4.xml"},
                            new Category() { ID=2, SourceID=9, Name="ekonomi", Url="http://www.milliyet.com.tr/D/rss/rss/Rss_3.xml"},
                            new Category() { ID=3, SourceID=9, Name="yaşam", Url="http://www.milliyet.com.tr/D/rss/rss/Rss_5.xml"},
                            new Category() { ID=4, SourceID=9, Name="magazin", Url="http://www.milliyet.com.tr/D/rss/rss/Rss_23.xml"},
                            new Category() { ID=5, SourceID=9, Name="kültür sanat", Url="http://www.milliyet.com.tr/D/rss/rss/Rss_30.xml"},
                            new Category() { ID=6, SourceID=9, Name="teknoloji", Url="http://www.milliyet.com.tr/D/rss/rss/Rss_36.xml"}
                        },
                    ParserSetting = new ParserSetting()
                        {
                            Allowed = "attribute|itemtype|http://schema.org/Article",
                            Disallowed = new string[]
                            {
                                ".cls",
                                ".date",
                                ".breadC",
                                ".controls2",
                                ".share2",
                                ".imza",
                                ".etiketler"
                            }
                        }
                }
            },
            {10, new Source()
                {
                    ID = 10,
                    Name = "Habertürk",
                    Logo = "ms-appx:///Assets/Icons/Sources/haberturk.png",
                    Color = "#FF9B0000",
                    IsFrontImageDisabled = true,
                    Categories = new ObservableCollectionEx<Category>()
                        {
                            new Category() { ID=0, SourceID=10, Name="anasayfa", Url="http://www.haberturk.com/rss"}
                        },
                    ParserSetting = new ParserSetting()
                        {
                            //Charset =  "utf-8",
                            DontStripUnlikelys = true,
                            Allowed = ".span3",
                            Disallowed = new string[]
                            {
                                "saveMessage",
                                ".favourite-btn",
                                "socialBox",
                                ".categorys",
                                ".news-date-create",
                                "TavsiyeHaberContainer",
                                ".NewsCommentMain",
                                ".tag-listed",
                                ".box2",
                                ".category-other",
                                ".span8",
                                ".news-wd",
                                ".comments"
                            }
                        }
                }
            },
            {11, new Source()
                {
                    ID = 11,
                    Name = "Zaman",
                    Logo = "ms-appx:///Assets/Icons/Sources/zaman.png",
                    Color = "#FF0051A2",
                    IsFrontImageDisabled = true,
                    Categories = new ObservableCollectionEx<Category>()
                        {
                            new Category() { ID=0, SourceID=11, Name="manşet", Url="http://www.zaman.com.tr/manset.rss"},
                            new Category() { ID=0, SourceID=11, Name="gündem", Url="http://www.zaman.com.tr/gundem.rss"},
                            new Category() { ID=0, SourceID=11, Name="politika", Url="http://www.zaman.com.tr/politika.rss"},
                            new Category() { ID=0, SourceID=11, Name="ekonomi", Url="http://www.zaman.com.tr/ekonomi.rss"},
                            new Category() { ID=0, SourceID=11, Name="dünya", Url="http://www.zaman.com.tr/dishaberler.rss"},
                            new Category() { ID=0, SourceID=11, Name="spor", Url="http://www.zaman.com.tr/spor.rss"},
                            new Category() { ID=0, SourceID=11, Name="kültür", Url="http://www.zaman.com.tr/kultursanat.rss"},
                            new Category() { ID=0, SourceID=11, Name="teknoloji", Url="http://www.zaman.com.tr/bilisim.rss"},
                            new Category() { ID=0, SourceID=11, Name="magazin", Url="http://www.zaman.com.tr/magazin.rss"}
                        },
                    ParserSetting = new ParserSetting()
                        {
                            //Charset =  "utf-8",
                            DontStripUnlikelys = true,
                            Allowed = ".detayLeft",
                            Disallowed = new string[]
                            {
                                ".detayMuhabir",
                                ".detayTarihNew",
                                "socialNetworkBeta",
                                "socialNetworkBetaNew",
                                ".newsDetailSocialMediaBox",
                                ".detayUyari",
                                ".dmaBaslik",
                                ".detailRelated",
                                ".relatedBox",
                                ".bread-crump-wrap",
                                ".share"
                            }
                        }
                }
            },
            {12, new Source()
                {
                    ID = 12,
                    Name = "Posta",
                    Logo = "ms-appx:///Assets/Icons/Sources/posta.png",
                    Color = "#FFED1B24",
                    IsFrontImageDisabled = true,
                    Categories = new ObservableCollectionEx<Category>()
                        {
                            new Category() { ID=0, SourceID=12, Name="haber hattı", Url="http://www.posta.com.tr/xml/rss/rss_1_0.xml"},
                            new Category() { ID=1, SourceID=12, Name="3. sayfa", Url="http://www.posta.com.tr/xml/rss/rss_352_0.xml"},
                            new Category() { ID=2, SourceID=12, Name="siyaset", Url="http://www.posta.com.tr/xml/rss/rss_4_0.xml"},
                            new Category() { ID=3, SourceID=12, Name="ekonomi", Url="http://www.posta.com.tr/xml/rss/rss_8_0.xml"},
                            new Category() { ID=4, SourceID=12, Name="dünya", Url="http://www.posta.com.tr/xml/rss/rss_5_0.xml"},
                            new Category() { ID=5, SourceID=12, Name="spor", Url="http://www.posta.com.tr/xml/rss/rss_7_0.xml"},
                            new Category() { ID=6, SourceID=12, Name="yaşam", Url="http://www.posta.com.tr/xml/rss/rss_10_0.xml"},
                            new Category() { ID=7, SourceID=12, Name="bilim & teknoloji", Url="http://www.posta.com.tr/xml/rss/rss_269_0.xml"},
                            new Category() { ID=8, SourceID=12, Name="magazin", Url="http://www.posta.com.tr/xml/rss/rss_6_0.xml"}
                        },
                    ParserSetting = new ParserSetting()
                        {
                            //Charset =  "utf-8",
                            DontStripUnlikelys = true,
                            Allowed = ".detayLeft",
                            Disallowed = new string[]
                            {
                                ".social_box",
                                ".dateAndFontsize_box",
                                ".sticker_box",
                                ".other_news_box"
                            }
                        }
                }
            },
            {13, new Source()
                {
                    ID = 13,
                    Name = "Akşam",
                    Logo = "ms-appx:///Assets/Icons/Sources/aksam.png",
                    Color = "#FFED1B24",
                    IsFrontImageDisabled = true,
                    Categories = new ObservableCollectionEx<Category>()
                        {
                            new Category() { ID=0, SourceID=13, Name="anasayfa", Url="http://www.aksam.com.tr/cache/rss.xml"}
                        },
                    ParserSetting = new ParserSetting()
                        {
                            Charset =  "iso-8859-9",
                            DontStripUnlikelys = true,
                            Allowed = ".double-wide",
                            Disallowed = new string[]
                            {
                                "._pubserver_div"
                            }
                        }
                }
            },
            {14, new Source()
                {
                    ID = 14,
                    Name = "Sabah",
                    Logo = "ms-appx:///Assets/Icons/Sources/aksam.png",
                    Color = "#FFED1B24",
                    IsFrontImageDisabled = true,
                    Categories = new ObservableCollectionEx<Category>()
                        {
                            new Category() { ID=0, SourceID=14, Name="Ekonomi", Url="http://www.sabah.com.tr/rss/ekonomi.xml"},
                            new Category() { ID=1, SourceID=14, Name="Spor", Url="http://www.sabah.com.tr/rss/spor.xml"},
                            new Category() { ID=2, SourceID=14, Name="Gündem", Url="http://www.sabah.com.tr/rss/gundem.xml"},
                            new Category() { ID=3, SourceID=14, Name="Yaşam", Url="http://www.sabah.com.tr/rss/yasam.xml"},
                            new Category() { ID=4, SourceID=14, Name="Dünya", Url="http://www.sabah.com.tr/rss/dunya.xml"},
                            new Category() { ID=5, SourceID=14, Name="Teknoloji", Url="http://www.sabah.com.tr/rss/teknoloji.xml"},
                            new Category() { ID=6, SourceID=14, Name="Turizm", Url="http://www.sabah.com.tr/rss/turizm.xml"},
                            new Category() { ID=7, SourceID=14, Name="Otomobil", Url="http://www.sabah.com.tr/rss/otomobil.xml"},
                            new Category() { ID=8, SourceID=14, Name="Ana Sayfa", Url="http://www.sabah.com.tr/rss/anasayfa.xml"},
                            new Category() { ID=9, SourceID=14, Name="Yazarlar", Url="http://www.sabah.com.tr/rss/yazarlar.xml"},
                            new Category() { ID=10, SourceID=14, Name="Sağlık", Url="http://www.sabah.com.tr/rss/saglik.xml"},
                            new Category() { ID=11, SourceID=14, Name="Günün İçinden", Url="http://www.sabah.com.tr/rss/gununicinden.xml"},
                            new Category() { ID=12, SourceID=14, Name="Haftanın Filmleri", Url="http://www.sabah.com.tr/rss/vizyondakiler.xml"},
                            new Category() { ID=13, SourceID=14, Name="Son Dakika", Url="http://www.sabah.com.tr/rss/sondakika.xml"},
                            new Category() { ID=14, SourceID=14, Name="Galatasaray", Url="http://www.sabah.com.tr/rss/galatasaray.xml"},
                            new Category() { ID=15, SourceID=14, Name="Fenerbahçe", Url="http://www.sabah.com.tr/rss/fenerbahce.xml"},
                            new Category() { ID=16, SourceID=14, Name="Beşiktaş", Url="http://www.sabah.com.tr/rss/besiktas.xml"},
                            new Category() { ID=17, SourceID=14, Name="Trabzonspor", Url="http://www.sabah.com.tr/rss/trabzonspor.xml"},
                            new Category() { ID=18, SourceID=14, Name="Bursaspor", Url="http://www.sabah.com.tr/rss/bursaspor.xml"},
                            new Category() { ID=19, SourceID=14, Name="Kültür Sanat", Url="http://www.sabah.com.tr/rss/kultur_sanat.xml"},
                            new Category() { ID=20, SourceID=14, Name="Oyun", Url="http://www.sabah.com.tr/rss/oyun.xml"}
                        },
                    ParserSetting = new ParserSetting()
                        {
                            Charset =  "utf-8",
                            //DontStripUnlikelys = true,
                            Allowed = ".detay",
                            Disallowed = new string[]
                            {
                                "._pubserver_div"
                            }
                        }
                }
            },
            //NEW SOURCES
            //{14, new Source()
            //    {
            //        ID = 14,
            //        Name = "Takvim",
            //        Logo = "ms-appx:///Assets/Icons/Sources/aksam.png",
            //        Color = "#FFED1B24",
            //        IsFrontImageDisabled = true,
            //        Categories = new ObservableCollectionEx<Category>()
            //            {
            //                new Category() { ID=0, SourceID=14, Name="anasayfa", Url="http://www.takvim.com.tr/rss/AnaSayfa.xml"},
            //                new Category() { ID=1, SourceID=14, Name="güncel", Url="http://www.takvim.com.tr/rss/Guncel.xml"},
            //                new Category() { ID=2, SourceID=14, Name="ekonomi", Url="http://www.takvim.com.tr/rss/Ekonomi.xml"},
            //                new Category() { ID=3, SourceID=14, Name="otomobil", Url="http://www.takvim.com.tr/rss/Otomobil.xml"},
            //                new Category() { ID=4, SourceID=14, Name="saklambaç", Url="http://www.takvim.com.tr/rss/Saklambac.xml"},
            //                new Category() { ID=5, SourceID=14, Name="spor", Url="http://www.takvim.com.tr/rss/Spor.xml"},
            //                new Category() { ID=6, SourceID=14, Name="yaşam", Url="http://www.takvim.com.tr/rss/Yasam.xml"},
            //                new Category() { ID=7, SourceID=14, Name="televizyon", Url="http://www.takvim.com.tr/rss/Televizyon.xml"},
            //                new Category() { ID=8, SourceID=14, Name="astroloji", Url="http://www.takvim.com.tr/rss/Astroloji.xml"}
            //            },
            //        ParserSetting = new ParserSetting()
            //            {
            //                //Charset =  "iso-8859-9",
            //                DontStripUnlikelys = true,
            //                Allowed = ".content",
            //                Disallowed = new string[]
            //                {
            //                    "._pubserver_div"
            //                }
            //            }
            //    }
            //}
        };

        public static Dictionary<int, Source> Sources
        {
            get
            {
                return _sources;
            }
        }
    }
}