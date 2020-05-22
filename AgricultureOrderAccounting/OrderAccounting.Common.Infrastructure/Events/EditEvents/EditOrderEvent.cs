using OrderAccounting.Common.Infrastructure.Interfaces;
using Prism.Events;

namespace OrderAccounting.Common.Infrastructure.Events.EditEvents
{
    public class EditOrderEvent : PubSubEvent
    {
        public IOrderViewModel CurrentOrder { get; set; }

        public EditOrderEvent(IOrderViewModel currentOrder)
        {
            CurrentOrder = currentOrder;
        }
    }
}
