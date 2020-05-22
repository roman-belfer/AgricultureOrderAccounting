using OrderAccounting.Common.Infrastructure.Enums;
using OrderAccounting.Common.Infrastructure.Interfaces;
using Prism.Events;
using System.Collections.Generic;
using System.Linq;

namespace OrderAccounting.Common.Infrastructure.Events
{
    public class DetailEvent : PubSubEvent
    {
        public int? OrderId { get; set; }
        public string Status { get; set; }
        public DetailState DetailState { get; set; }
        public List<IOrderViewModel> OrderCollection { get; set; }

        public DetailEvent(DetailState detailState, int? orderId = null, List<IOrderViewModel> orders = null, string status = null)
        {
            DetailState = detailState;
            OrderId = orderId;
            Status = status;

            if (orders != null)
            {
                OrderCollection = orders.ToList();
            }
        }
    }
}
