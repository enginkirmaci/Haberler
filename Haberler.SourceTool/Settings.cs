using Haberler.Services.Enums;

namespace Haberler.SourceTool
{
    public class Settings
    {
        public static class News
        {
            private static int _fontSize = 16;
            private static Theme _theme = Theme.Dark;
            public static int FontSize { get { return _fontSize; } set { _fontSize = value; } }
            public static Theme Theme { get { return _theme; } set { _theme = value; } }
        }

        private Settings()
        {
        }
    }
}