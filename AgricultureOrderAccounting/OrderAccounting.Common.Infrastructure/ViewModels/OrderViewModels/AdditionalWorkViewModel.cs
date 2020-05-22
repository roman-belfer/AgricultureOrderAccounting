using Argo.DataAccess.LaborDetail.Model;
using FMS.DataManagers.Interfaces;
using OrderAccounting.Common.Infrastructure.Factories;
using OrderAccounting.Common.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace OrderAccounting.Common.Infrastructure.ViewModels.OrderViewModels
{
    public class AdditionalWorkViewModel : TransportWorkViewModel, IProcessable
    {
        #region Properties

        public override int Type { get; set; }

        #endregion

        #region Initialization

        public AdditionalWorkViewModel(IDirectoryManager directoryManager, IOrderItemFactory orderItemFactory)
            : base(directoryManager, orderItemFactory)
        {
            Type = 2;
        }

        #endregion

        #region Methods

        protected override void UpdateOperationType()
        { }

        protected override void UpdatePhase()
        { }

        public override string ToString()
        {
            return "Допоміжна";
        }

        public override void ConvertToModel<TItem>(TItem result)
        {
            if (!_isEmpty)
            {
                if (typeof(TItem) == typeof(LaborDetail.Item))
                {
                    var model = (result as LaborDetail.Item);
                    if (model.SubLaborDetails == null)
                    {
                        model.SubLaborDetails = new List<LaborDetail.Item>();
                    }

                    var subLabor = new LaborDetail.Item();
                    model.SubLaborDetails.Add(subLabor);
                    subLabor.DateFrom = model.DateFrom;
                    subLabor.DateTo = model.DateTo;
                    subLabor.OperationType = model.OperationType;
                    subLabor.ActualPhase = model.ActualPhase;
                    subLabor.ActualPhaseYear = model.ActualPhaseYear;
                    base.ConvertToModel(subLabor);
                }
            }
        }

        #endregion

    }
}
