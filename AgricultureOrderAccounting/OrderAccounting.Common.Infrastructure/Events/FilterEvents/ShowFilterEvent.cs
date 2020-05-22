using Prism.Events;

namespace OrderAccounting.Common.Infrastructure.Events
{
    public class FilterVisibilityEvent : PubSubEvent
    {
        public bool IsVisible { get; set; }

        public FilterVisibilityEvent(bool isVisible)
        {
            IsVisible = isVisible;
        }
    }
}
