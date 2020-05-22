using Prism.Events;

namespace OrderAccounting.Common.Infrastructure.Events.EditEvents
{
    public class NavigationTypeEvent : PubSubEvent
    {
        public int TypeId;

        public NavigationTypeEvent(int id)
        {
            TypeId = id;
        }
    }
}
