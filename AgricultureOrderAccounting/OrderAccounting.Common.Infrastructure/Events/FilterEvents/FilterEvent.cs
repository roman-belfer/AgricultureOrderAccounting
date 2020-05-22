using OrderAccounting.Common.Infrastructure.Interfaces;
using Prism.Events;

namespace OrderAccounting.Common.Infrastructure.Events
{
    public class FilterEvent : PubSubEvent
    {
        public bool IsFilterEmpty { get; set; }

        public IFilterModel CurrentFilter { get; set; }

        public FilterEvent(bool isFilterEmpty, IFilterModel currentFilter)
        {
            IsFilterEmpty = isFilterEmpty;
            CurrentFilter = currentFilter;
        }
    }
}


