using FMS.DataManagers.Interfaces;
using OrderAccounting.Common.Infrastructure.Interfaces;
using OrderAccounting.Common.Infrastructure.ViewModels.OrderViewModels;

namespace OrderAccounting.Common.Infrastructure.Factories
{
    public class OrderFactory : IOrderFactory
    {
        #region Variables

        private IOrderItemFactory _orderItemFactory;

        private IDirectoryManager _directoryManager;

        #endregion


        #region Initialization

        public OrderFactory(IDirectoryManager directoryManager, IOrderItemFactory orderItemFactory)
        {
            _orderItemFactory = orderItemFactory;

            _directoryManager = directoryManager;
        }

        #endregion

        #region Methods

        public IOrderViewModel CreateOrderByType(int orderTypeId, int index)
        {
            switch (orderTypeId)
            {
                case 0:
                    return CreateBasicWorkOrder(index);

                case 2:
                    return CreateAdditionalWorkOrder(index);

                case 1:
                    return CreateManualWorkOrder(index);
            }

            return null;
        }

        private IOrderViewModel CreateBasicWorkOrder(int index)
        {
            return new BasicWorkViewModel(_directoryManager, _orderItemFactory)
            {
                Index = index
            };
        }

        private IOrderViewModel CreateAdditionalWorkOrder(int index)
        {
            return new AdditionalWorkViewModel(_directoryManager, _orderItemFactory)
            {
                Index = index
            };
        }

        private IOrderViewModel CreateManualWorkOrder(int index)
        {
            return new ManualWorkViewModel(_directoryManager, _orderItemFactory)
            {
                Index = index
            };
        }

        #endregion

    }
}
