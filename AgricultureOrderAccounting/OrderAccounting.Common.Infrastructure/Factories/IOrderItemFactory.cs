using OrderAccounting.Common.Infrastructure.Enums;
using OrderAccounting.Common.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace OrderAccounting.Common.Infrastructure.Factories
{
    public interface IOrderItemFactory
    {
        /// <summary>
        /// Creates IDataItemViewModel realization depending on DataItemTypes enumeration value
        /// </summary>
        IOrderItemViewModel CreateOrderItem(OrderItemTypes orderItemType);

        /// <summary>
        /// Creates list of IDataItemViewModel depending on array of DataItemTypes enumeration values
        /// </summary>
        List<IOrderItemViewModel> InitializeOrderItemCollection(params OrderItemTypes[] orderItemTypes);
    }
}
