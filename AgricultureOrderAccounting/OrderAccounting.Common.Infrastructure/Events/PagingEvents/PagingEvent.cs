using OrderAccounting.Common.Infrastructure.Enums;
using Prism.Events;

namespace OrderAccounting.Common.Infrastructure.Events
{
    public class PagingEvent : PubSubEvent
    {
        public ViewState ViewState { get; set; }

        public PagingEvent(ViewState viewState)
        {
            ViewState = viewState;
        }
    }
}

