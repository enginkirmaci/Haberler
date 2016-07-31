using Prism.Events;

namespace Haberler.Events
{
    public class BackForwardParameter
    {
        public string PageToken { get; set; }

        public object Parameter { get; set; }
    }

    public class BackForwardEvent : PubSubEvent<BackForwardParameter>
    {
    }
}