using Prism.Events;

namespace OrderAccounting.Common.Infrastructure.Events
{
    public class OperationTypeEvent : PubSubEvent
    {
        public int TypeId { get; set; }

        public OperationTypeEvent(int typeId)
        {
            TypeId = typeId;
        }
    }
}
