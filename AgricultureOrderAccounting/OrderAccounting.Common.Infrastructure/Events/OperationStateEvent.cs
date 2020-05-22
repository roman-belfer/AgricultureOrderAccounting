using Prism.Events;

namespace OrderAccounting.Common.Infrastructure.Events
{
    public class OperationStateEvent : PubSubEvent
    {
        public string StateName { get; set; }

        public OperationStateEvent(string stateId)
        {
            StateName = stateId;
        }
    }
}
