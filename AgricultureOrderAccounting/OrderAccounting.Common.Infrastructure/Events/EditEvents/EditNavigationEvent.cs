using Prism.Events;

namespace OrderAccounting.Common.Infrastructure.Events.EditEvents
{
    public class EditNavigationEvent : PubSubEvent
    {
        public int WorkTypeId { get; set; }
        public int? Index { get; set; }

        public EditNavigationEvent(int workTypeId, int? index = null)
        {
            WorkTypeId = workTypeId;
            Index = index;
        }
    }
}
