
using Prism.Events;

namespace OrderAccounting.Common.Infrastructure.Events
{
    public class PageChangedEvent : PubSubEvent
    {
        public int PageNumber { get; set; }

        public PageChangedEvent(int pageNumber)
        {
            PageNumber = pageNumber;
        }
    }
}
