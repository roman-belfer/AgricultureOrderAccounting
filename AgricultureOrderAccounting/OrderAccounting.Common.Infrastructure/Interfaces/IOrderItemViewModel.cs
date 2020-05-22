using System;

namespace OrderAccounting.Common.Infrastructure.Interfaces
{
    public interface IOrderItemViewModel
    {
        #region Properties

        bool IsChecked { get; set; }

        string Year { get; set; }

        #endregion

        #region Events

        event EventHandler DataChanged;

        #endregion

        #region Methods

        bool IsDataValid();

        int? GetOperationId();

        int? GetBaseOperationId();

        int? GetPhaseId();

        int? GetOperationTypeId();

        void SetPreviousData(IOrderItemViewModel previousData);

        void SetNextData(IOrderItemViewModel nextData);

        void SetObjectData(bool isTransportation);

        void ConvertDataToDTO<T>(T item) where T : class;

        void ConvertDataFromDTO<T>(T item) where T : class;

        #endregion

    }
}
