using OrderAccounting.Common.Infrastructure.Interfaces;
using Prism.Events;

namespace OrderAccounting.Common.Infrastructure.Events.EditEvents
{
    public class NavigationProcessEvent : PubSubEvent
    {
        public IProcessable Processable { get; set; }

        public NavigationProcessEvent(IProcessable processable)
        {
            Processable = processable;
        }
    }
}
