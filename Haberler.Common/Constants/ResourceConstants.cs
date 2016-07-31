namespace Haberler.Common.Constants
{
    public class ResourceConstants
    {
        private static Constants _resourceConstants = new Constants();

        public Constants ResourceConstant { get { return _resourceConstants; } }
    }

    public class Constants
    {
        public static string AppLogo { get { return IconConstants.AppIcon; } }

        public static string AppName { get { return Settings.General.AppName; } }

        public static string AppNameCapitalized { get { return Settings.General.AppNameCapitalized; } }

        public static string AppVersion { get { return string.Format("versiyon {0}", Settings.General.AppVersion); } }

        public static string AppUrl { get { return Settings.General.AppUrl; } }

        public static string CompanyName { get { return Settings.General.CompanyName; } }

        public static string CompanyUrl { get { return Settings.General.CompanyUrl; } }

        public static string TwitterLogo { get { return IconConstants.Twitter; } }

        public static string FacebookLogo { get { return IconConstants.Facebook; } }

        public static string LinkedinLogo { get { return IconConstants.Linkedin; } }
    }
}