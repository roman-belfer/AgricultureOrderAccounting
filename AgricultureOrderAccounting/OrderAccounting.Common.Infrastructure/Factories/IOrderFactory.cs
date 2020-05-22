using OrderAccounting.Common.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace OrderAccounting.Common.Infrastructure.Factories
{
    public interface IOrderFactory
    {
        IOrderViewModel CreateOrderByType(int orderTypeId, int index);
    }
}
