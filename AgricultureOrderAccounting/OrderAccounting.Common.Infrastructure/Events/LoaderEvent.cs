using OrderAccounting.Common.Infrastructure.Enums;
using Prism.Events;

namespace OrderAccounting.Common.Infrastructure.Events
{
    public class LoaderEvent : PubSubEvent
    {
        public ViewState ViewState { get; set; }

        public LoaderEvent(ViewState viewState)
        {
            ViewState = viewState;
        }
    }
}
