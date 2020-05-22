using Prism.Events;

namespace OrderAccounting.Common.Infrastructure.Events.EditEvents
{
    public class ChangeStatusEvent : PubSubEvent
    {
        public int StatusId;

        public ChangeStatusEvent(int id)
        {
            StatusId = id;
        }
    }
}
