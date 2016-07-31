using System.Collections.Generic;

namespace Haberler.Common.Entities
{
    public class ChangeLogItem
    {
        public string Header { get; set; }
        public List<string> Lines { get; set; }
    }
}