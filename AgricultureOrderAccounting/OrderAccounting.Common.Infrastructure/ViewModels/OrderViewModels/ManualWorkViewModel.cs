using FMS.DataManagers.Interfaces;
using OrderAccounting.Common.Infrastructure.Enums;
using OrderAccounting.Common.Infrastructure.Factories;
using OrderAccounting.Common.Infrastructure.Interfaces;

namespace OrderAccounting.Common.Infrastructure.ViewModels.OrderViewModels
{
    public class ManualWorkViewModel : BaseWorkViewModel, IProcessable
    {
        #region Properties

        public override int Type { get; set; }

        #endregion

        #region Initialization

        /// <summary>
        /// Initialize view model by work order
        /// </summary>
        public ManualWorkViewModel(IDirectoryManager directoryManager, IOrderItemFactory orderItemFactory)
            : base(directoryManager, orderItemFactory)
        {
            Type = 1;
        }

        /// <summary>
        /// Initialize data items types
        /// </summary>
        protected override void InitializeDataItems()
        {
            _dataItemTypes.Add(OrderItemTypes.ManualOperation);
            _dataItemTypes.Add(OrderItemTypes.Employee);
        }

        #endregion

        #region Methods

        protected override void UpdateOperationType()
        { }

        protected override void UpdatePhase()
        { }

        protected override void UpdateBaseOperation()
        { }

        protected override void UpdateOperation()
        { }

        public override string ToString()
        {
            return "Ручна";
        }

        #endregion

    }
}
