using Prism.Events;

namespace OrderAccounting.Common.Infrastructure.Events.EditEvents
{
    public class AdditionalEvent : PubSubEvent
    {
        public int Index;
        public int Id;

        public AdditionalEvent(int id, int index)
        {
            Id = id;
            Index = index;
        }
    }
}
