using Haberler.Common.Constants;
using Haberler.Common.Entities;
using Haberler.Services.Enums;
using Library10.Core.BaseClasses;
using Library10.Core.Configuration;
using System.Collections.Generic;
using Windows.ApplicationModel;

namespace Haberler.Common
{
    public class Settings : BaseSettings
    {
        public static bool RateReminderShown { get { return Read("RateReminderShown", false); } set { Write("RateReminderShown", value, SettingsStrategy.Temporary); } }

        //public static bool ForceNewsSourceRefresh { get { return Read("ForceNewsSourceRefresh", false); } set { Write("ForceNewsSourceRefresh", value, SettingsStrategy.Temporary); } }

        public static class Advertising
        {
            public const string Mobile_ApplicationId = "6342d8ae-baaa-47e7-8e7f-919e2cd9c26f";
            public const string Mobile_AdUnitId = "247855";
            public const string Desktop_ApplicationId = "238357df-3163-4a7d-80d4-a9f58f454382";
            public const string Desktop_AdUnitId = "247854";
        }

        public static class Application
        {
            public const string TaskName = "NewsTileBackgroundTask";
            public const string TaskEntryPoint = "Haberler.BackgroundTasks.NewsTileBackgroundTask";
            public const string FavoriteFolder = "favorites/{0}";
            public const string FavoriteFileExt = ".article";
            public const string TileID = "haberler_tile_{0}";

            public static int StartCount { get { return Read("StartCount", 0); } set { Write("StartCount", value, SettingsStrategy.Roaming); } }

            public static int RateReminderSkipCount { get { return Read("RateReminderSkipCount", 0); } set { Write("RateReminderSkipCount", value, SettingsStrategy.Roaming); } }
        }

        public static class News
        {
            public static int FontSize { get { return Read("FontSize", 16); } set { Write("FontSize", value, SettingsStrategy.Roaming); } }

            public static Theme Theme { get { return Read("Theme", Theme.Dark); } set { Write("Theme", value, SettingsStrategy.Roaming); } }

            public static List<UserSources> UserSources { get { return Read<List<UserSources>>("UserSources"); } set { Write("UserSources", value, SettingsStrategy.Roaming); } }

            public static bool IsTransparentTile { get { return Read("IsTransparent", false); } set { Write("IsTransparent", value, SettingsStrategy.Roaming); } }

            public static bool IsBackgroundActive { get { return Read("IsBackgroundActive", true); } set { Write("IsBackgroundActive", value, SettingsStrategy.Roaming); } }

            public static uint RefreshInterval { get { return Read("RefreshInterval", RefreshIntervalRepository.Intervals[0].Value); } set { Write("RefreshInterval", value, SettingsStrategy.Roaming); } }
        }

        private Settings()
        {
        }

        public static void Initialize()
        {
            General.AppName = Package.Current.DisplayName.ToLowerInvariant();
            General.AppId = 5;
            General.AppMarketId = "5f33aefb-0222-4428-a388-91ee7a6db0b0";
            General.AppNameCapitalized = Package.Current.DisplayName;
            General.AppVersion = string.Format("{0}.{1}.{2}",
                Package.Current.Id.Version.Major,
                Package.Current.Id.Version.Minor,
                Package.Current.Id.Version.Build);
            General.AppUrl = "enginkirmaci.com/projects/haberler";
            General.PrivacyPolicyUrl = "enginkirmaci.com/projects/haberler/haberler-gizlilik-politikasi";
            General.CompanyName = Package.Current.PublisherDisplayName;
            General.CompanyUrl = "enginkirmaci.com";
            General.FeedbackUrl = "contact@enginkirmaci.com";
            General.ConnectionString = "haberler.db";

            General.ServiceUrl = "http://localhost:63728";
            General.TwitterUrl = "twitter.com/enginkirmaci";
            General.FacebookUrl = "facebook.com/engin.kirmaci";
            General.LinkedinUrl = "linkedin.com/in/enginkirmaci";

            Product.ProductKey = "remove_ads";
        }
    }
}