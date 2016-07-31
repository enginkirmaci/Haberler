using System.Collections.Generic;

namespace Haberler.Common.Constants
{
    public class RefreshInterval
    {
        public uint Value { get; set; }
        public string Resource { get; set; }
    }

    public class RefreshIntervalRepository
    {
        private static List<RefreshInterval> _intervals = new List<RefreshInterval>() {
                new RefreshInterval() { Value = 15, Resource = "15 dakika"},
                new RefreshInterval() { Value = 30, Resource = "30 dakika"},
                new RefreshInterval() { Value = 60, Resource = "1 saat"},
                new RefreshInterval() { Value = 120, Resource = "2 saat"},
                new RefreshInterval() { Value = 240, Resource = "4 saat"},
                new RefreshInterval() { Value = 480, Resource = "8 saat"},
            };

        public static List<RefreshInterval> Intervals { get { return _intervals; } }
    }
}