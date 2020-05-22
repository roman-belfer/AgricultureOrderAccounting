using FMS.DataManagers.Interfaces;
using OrderAccounting.Common.Infrastructure.Enums;
using OrderAccounting.Common.Infrastructure.Interfaces;
using OrderAccounting.Common.Infrastructure.ViewModels.OrderItemViewModels;
using System.Collections.Generic;

namespace OrderAccounting.Common.Infrastructure.Factories
{
    public class OrderItemFactory : IOrderItemFactory
    {
        #region Variables

        private IDirectoryManager _directoryManager;

        #endregion

        #region Initialization

        public OrderItemFactory(IDirectoryManager directoryManager)
        {
            _directoryManager = directoryManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates IDataItemViewModel realization depending on DataItemTypes enumeration value
        /// </summary>
        public IOrderItemViewModel CreateOrderItem(OrderItemTypes orderItemType)
        {
            switch (orderItemType)
            {
                case OrderItemTypes.ManualOperation:
                    return new ManualOperationItemViewModel(_directoryManager);

                case OrderItemTypes.Operation:
                    return new OperationItemViewModel(_directoryManager);

                case OrderItemTypes.Stuff:
                    return new StuffItemViewModel(_directoryManager);

                case OrderItemTypes.Detail:
                    return new DetailItemViewModel(_directoryManager);

                case OrderItemTypes.Employee:
                    return new EmployeeItemViewModel(_directoryManager);

                case OrderItemTypes.Object:
                    return new ObjectItemViewModel(_directoryManager);

                case OrderItemTypes.Transport:
                    return new TransportItemViewModel(_directoryManager);
            }

            return null;
        }

        /// <summary>
        /// Creates list of IDataItemViewModel depending on array of DataItemTypes enumeration values
        /// </summary>
        public List<IOrderItemViewModel> InitializeOrderItemCollection(params OrderItemTypes[] orderItemTypes)
        {
            var result = new List<IOrderItemViewModel>();

            if (orderItemTypes != null)
            {
                for (int i = 0; i < orderItemTypes.Length; i++)
                {
                    result.Add(CreateOrderItem(orderItemTypes[i]));
                }
            }

            return result;
        }

        #endregion

    }
}
