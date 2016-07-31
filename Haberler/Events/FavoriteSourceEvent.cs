using Haberler.Services.Entities;
using Prism.Events;

namespace Haberler.Events
{
    public class FavoriteSourceEvent : PubSubEvent<Source>
    {
    }
}