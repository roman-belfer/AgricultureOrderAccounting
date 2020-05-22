using FMS.DataManagers.Interfaces;
using OrderAccounting.Common.Infrastructure.Enums;
using OrderAccounting.Common.Infrastructure.Factories;

namespace OrderAccounting.Common.Infrastructure.ViewModels.OrderViewModels
{
    public abstract class TransportWorkViewModel : BaseWorkViewModel 
    {
        #region Initialization

        /// <summary>
        /// Initialize view model by work order
        /// </summary>
        public TransportWorkViewModel(IDirectoryManager directoryManager, IOrderItemFactory orderItemFactory)
            : base(directoryManager, orderItemFactory)
        { }
        
        /// <summary>
        /// Initialize data items types
        /// </summary>
        protected override void InitializeDataItems()
        {
            _dataItemTypes.Add(OrderItemTypes.Operation);
            //_dataItemTypes.Add(DataItemTypes.Stuff);
            _dataItemTypes.Add(OrderItemTypes.Transport);
            _dataItemTypes.Add(OrderItemTypes.Object);
        }
        
        #endregion        
    }
}
