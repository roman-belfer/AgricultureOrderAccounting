using Argo.DataAccess.LaborDetail.Model;
using OrderAccounting.Common.Infrastructure.Interfaces;
using Prism.Mvvm;
using System;

namespace OrderAccounting.Common.Infrastructure.Models
{
    public class FilterModel : BindableBase, IFilterModel
    {
        #region Properties

        private DateTime? _dateFrom;
        public DateTime? DateFrom
        {
            get { return _dateFrom; }
            set
            {
                if (_dateFrom != value)
                {
                    _dateFrom = value;
                    RaisePropertyChanged("DateFrom");
                }
            }
        }

        private DateTime? _dateTo;
        public DateTime? DateTo
        {
            get { return _dateTo; }
            set
            {
                if (_dateTo != value)
                {
                    _dateTo = value;
                    RaisePropertyChanged("DateTo");
                }
            }
        }

        private int? _vehicleId;
        public int? VehicleId
        {
            get { return _vehicleId; }
            set
            {
                if (_vehicleId != value)
                {
                    _vehicleId = value;
                    RaisePropertyChanged("VehicleId");
                }
            }
        }

        private int? _unitId;
        public int? UnitId
        {
            get { return _unitId; }
            set
            {
                if (_unitId != value)
                {
                    _unitId = value;
                    RaisePropertyChanged("UnitId");
                }
            }
        }

        private int? _operationId;
        public int? OperationId
        {
            get { return _operationId; }
            set
            {
                if (_operationId != value)
                {
                    _operationId = value;
                    RaisePropertyChanged("OperationId");
                }
            }
        }

        private int? _baseOperationId;
        public int? BaseOperationId
        {
            get { return _baseOperationId; }
            set
            {
                if (_baseOperationId != value)
                {
                    _baseOperationId = value;
                    RaisePropertyChanged("BaseOperationId");
                }
            }
        }

        private int? _actualPhaseId;
        public int? ActualPhaseId
        {
            get { return _actualPhaseId; }
            set
            {
                if (_actualPhaseId != value)
                {
                    _actualPhaseId = value;
                    RaisePropertyChanged("ActualPhaseId");
                }
            }
        }

        private int? _operationTypeId;
        public int? OperationTypeId
        {
            get { return _operationTypeId; }
            set
            {
                if (_operationTypeId != value)
                {
                    _operationTypeId = value;
                    RaisePropertyChanged("OperationTypeId");
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Return bool value if order model contains base data of dates, operations and phase according to the filter
        /// </summary>
        private bool CheckBaseValues(bool isValid, OrderModel model)
        {
            isValid &= DateFrom.HasValue ? model.DateFrom >= DateFrom : true;

            isValid &= DateTo.HasValue ? model.DateTo <= DateTo : true;

            isValid &= ActualPhaseId.HasValue ? model.ActualPhaseId == ActualPhaseId : true;

            isValid &= OperationTypeId.HasValue ? model.OperationTypeId == OperationTypeId : true;

            isValid &= BaseOperationId.HasValue ? model.BaseOperationId == BaseOperationId : true;

            return isValid;
        }

        /// <summary>
        /// Return bool value if order model contains base data about vehicles, units and objects according to the filter
        /// </summary>
        private bool CheckTransportValues(bool isValid, OrderModel model)
        {
            isValid &= CheckBaseValues(isValid, model);

            isValid &= OperationId.HasValue ? model.OperationId == OperationId : true;

            isValid &= VehicleId.HasValue ? model.VehicleId == VehicleId : true;

            isValid &= UnitId.HasValue ? model.UnitId == UnitId : true;

            return isValid;
        }

        /// <summary>
        /// Return bool value if order model contains data according to the filter
        /// </summary>
        public bool CheckFilter<T>(T orderModel)
        {
            bool isValid = true;

            if (typeof(T) == typeof(OrderModel))
            {
                var model = orderModel as OrderModel;

                isValid = CheckTransportValues(isValid, model);
            }
            else
            {
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Return bool value if filter data is not empty
        /// </summary>
        public bool IsFilterNotEmpty()
        {
            bool isValid = true;

            isValid &= (DateFrom.HasValue && DateTo.HasValue) ? DateFrom.Value < DateTo.Value : true;

            return isValid && (DateFrom.HasValue || DateTo.HasValue || VehicleId.HasValue || UnitId.HasValue || OperationId.HasValue || BaseOperationId.HasValue);
        }

        /// <summary>
        /// Turn all properties to NULL
        /// </summary>
        public void Reset()
        {
            DateFrom = null;
            DateTo = null;
            VehicleId = null;
            UnitId = null;
            OperationId = null;
            BaseOperationId = null;
            ActualPhaseId = null;
            OperationTypeId = null;
        }

        public T ConvertFromModel<T>() where T : class
        {
            if (typeof(T) == typeof(LaborDetail.Filter))
            {
                var result = new LaborDetail.Filter();

                result.DateFrom = DateFrom;
                result.DateTo = DateTo;
                result.UnitID = UnitId;
                result.VehicleID = VehicleId;
                result.BaseOperationID = BaseOperationId;
                result.OperationID = OperationId;
                result.ActualPhaseID = ActualPhaseId;
                result.OperationTypeID = OperationTypeId;

                return result as T;
            }

            return default(T);
        }

        #endregion

    }
}
