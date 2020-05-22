using FMS.DataManagers.Interfaces;
using OrderAccounting.Common.Infrastructure.Interfaces;
using OrderAccounting.Common.Infrastructure.Enums;
using OrderAccounting.Common.Infrastructure.Factories;

namespace OrderAccounting.Common.Infrastructure.ViewModels.OrderViewModels
{
    public class BasicWorkViewModel : TransportWorkViewModel, IProcessable
    {
        #region Properties

        public override int Type { get; set; }

        #endregion

        #region Initialization

        /// <summary>
        /// Initialize view model by work order
        /// </summary>
        public BasicWorkViewModel(IDirectoryManager directoryManager, IOrderItemFactory orderItemFactory)
            : base(directoryManager, orderItemFactory)
        {
            Type = 0;
        }

        /// <summary>
        /// Initialize data items types
        /// </summary>
        protected override void InitializeDataItems()
        {
            _dataItemTypes.Add(OrderItemTypes.Detail);

            base.InitializeDataItems();
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return "Основна";
        }

        #endregion

    }
}
