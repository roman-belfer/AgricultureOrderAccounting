using System;
using System.Collections.ObjectModel;

namespace OrderAccounting.Common.Infrastructure.Interfaces
{
    public interface IOrderViewModel
    {
        #region Properties

        ObservableCollection<IOrderItemViewModel> DataCollection { get; set; }

        int Type { get; set; }
        int Index { get; set; }
        int LaborDetailId { get; set; }
        int? StatusId { get; set; }

        #endregion

        #region Events

        event EventHandler DataChanged;

        #endregion

        #region Methods

        void ConvertToModel<T>(T result) where T : class;
        void ConvertToViewModel<T>(T result) where T : class;

        #endregion

    }
}
