using FMS.DataManagers.Interfaces;
using OrderAccounting.Common.Infrastructure.Interfaces;
using Prism.Mvvm;
using System;
using System.Linq;

namespace OrderAccounting.Common.Infrastructure.Models
{
    /// <summary>
    /// Abstact class which describes base model of work in order
    /// </summary>
    public class OrderModel : BindableBase, IOrderModel
    {
        #region Variables 

        protected IDirectoryManager _directoryManager;

        #endregion

        #region Properties

        public int Id { get; set; }

        public string StateName { get; set; }

        public int TypeId { get; set; }

        protected string _number;
        public string Number
        {
            get { return _number; }
            set
            {
                if (_number != value)
                {
                    _number = value;

                    RaisePropertyChanged("Number");
                }
            }
        }

        protected int _masterNumber;
        public int MasterNumber
        {
            get { return _masterNumber; }
            set
            {
                if (_masterNumber != value)
                {
                    _masterNumber = value;

                    RaisePropertyChanged("MasterNumber");
                }
            }
        }

        protected string _vehicle;
        public string Vehicle
        {
            get { return _vehicle; }
            protected set
            {
                if (_vehicle != value)
                {
                    _vehicle = value;

                    RaisePropertyChanged("Vehicle");
                }
            }
        }

        protected int? _vehicleId;
        public int? VehicleId
        {
            get { return _vehicleId; }
            set
            {
                if (_vehicleId != value)
                {
                    _vehicleId = value;

                    OnVehicleIdChanged(_vehicleId);
                }
            }
        }

        protected string _unit;
        public string Unit
        {
            get { return _unit; }
            protected set
            {
                if (_unit != value)
                {
                    _unit = value;

                    RaisePropertyChanged("Unit");
                }
            }
        }

        protected int? _unitId;
        public int? UnitId
        {
            get { return _unitId; }
            set
            {
                if (_unitId != value)
                {
                    _unitId = value;

                    OnUnitIdChanged(_unitId);
                }
            }
        }

        protected string _baseOperation;
        public string BaseOperation
        {
            get { return _baseOperation; }
            protected set
            {
                if (_baseOperation != value)
                {
                    _baseOperation = value;

                    RaisePropertyChanged("BaseOperation");
                }
            }
        }

        protected int? _baseOperationId;
        public int? BaseOperationId
        {
            get { return _baseOperationId; }
            set
            {
                if (_baseOperationId != value)
                {
                    _baseOperationId = value;

                    OnBaseOperationIdChanged(_baseOperationId);
                }
            }
        }

        protected string _operation;
        public string Operation
        {
            get { return _operation; }
            protected set
            {
                if (_operation != value)
                {
                    _operation = value;

                    RaisePropertyChanged("Operation");
                }
            }
        }

        protected int? _operationId;
        public int? OperationId
        {
            get { return _operationId; }
            set
            {
                if (_operationId != value)
                {
                    _operationId = value;

                    OnOperationIdChanged(_operationId);
                }
            }
        }

        protected string _operationType;
        public string OperationType
        {
            get { return _operationType; }
            protected set
            {
                if (_operationType != value)
                {
                    _operationType = value;

                    RaisePropertyChanged("OperationType");
                }
            }
        }

        protected int? _operationTypeId;
        public int? OperationTypeId
        {
            get { return _operationTypeId; }
            set
            {
                if (_operationTypeId != value)
                {
                    _operationTypeId = value;

                    OnOperationTypeIdChanged(_operationTypeId);
                }
            }
        }

        protected string _actualPhase;
        public string ActualPhase
        {
            get { return _actualPhase; }
            protected set
            {
                if (_actualPhase != value)
                {
                    _actualPhase = value;

                    RaisePropertyChanged("ActualPhase");
                }
            }
        }

        protected int? _actualPhaseId;
        public int? ActualPhaseId
        {
            get { return _actualPhaseId; }
            set
            {
                if (_actualPhaseId != value)
                {
                    _actualPhaseId = value;

                    OnActualPhaseIdChanged(_actualPhaseId);
                }
            }
        }

        protected string _displayDateFrom;
        public string DisplayDateFrom
        {
            get { return _displayDateFrom; }
            protected set
            {
                if (_displayDateFrom != value)
                {
                    _displayDateFrom = value;

                    RaisePropertyChanged("DisplayDateFrom");
                }
            }
        }

        protected DateTime? _dateFrom;
        public DateTime? DateFrom
        {
            get { return _dateFrom; }
            set
            {
                if (_dateFrom != value)
                {
                    _dateFrom = value;

                    OnDateFromChanged(_dateFrom);
                }
            }

        }

        protected string _displayDateTo;
        public string DisplayDateTo
        {
            get { return _displayDateTo; }
            protected set
            {
                if (_displayDateTo != value)
                {
                    _displayDateTo = value;

                    RaisePropertyChanged("DisplayDateTo");
                }
            }
        }

        protected DateTime? _dateTo;
        public DateTime? DateTo
        {
            get { return _dateTo; }
            set
            {
                if (_dateTo != value)
                {
                    _dateTo = value;

                    OnDateToChanged(_dateTo);
                }
            }

        }

        #endregion

        #region Initialization

        public OrderModel(IDirectoryManager directoryManager)
        {
            _directoryManager = directoryManager;
        }

        #endregion

        #region Methods

        protected void OnBaseOperationIdChanged(int? baseOperationId)
        {
            BaseOperation = string.Empty;

            if (_directoryManager != null)
            {
                var operation = _directoryManager.BaseOperations.FirstOrDefault(x => x.Identity == baseOperationId);
                if (operation != null)
                {
                    BaseOperation = operation.DisplayName;
                }
            }

        }

        protected void OnOperationIdChanged(int? operationId)
        {
            Operation = string.Empty;

            if (_directoryManager != null)
            {
                var operation = _directoryManager.Operations.FirstOrDefault(x => x.Identity == operationId);
                if (operation != null)
                {
                    Operation = operation.OperationName;
                }
            }

        }

        protected void OnOperationTypeIdChanged(int? operationTypeId)
        {
            OperationType = string.Empty;

            if (_directoryManager != null)
            {
                var operation = _directoryManager.OperationTypes.FirstOrDefault(x => x.Identity == operationTypeId);
                if (operation != null)
                {
                    OperationType = operation.DisplayName;
                }
            }

        }

        protected void OnActualPhaseIdChanged(int? actualPhaseId)
        {
            ActualPhase = string.Empty;

            if (_directoryManager != null)
            {
                var operation = _directoryManager.ActualPhases.FirstOrDefault(x => x.Identity == actualPhaseId);
                if (operation != null)
                {
                    ActualPhase = operation.Phase.OCName;
                }
            }

        }

        protected void OnDateFromChanged(DateTime? dateFrom)
        {
            if (dateFrom.HasValue)
            {
                DisplayDateFrom = dateFrom.Value.ToShortDateString();
            }
        }

        protected void OnDateToChanged(DateTime? dateTo)
        {
            if (dateTo.HasValue)
            {
                DisplayDateTo = dateTo.Value.ToShortDateString();
            }
        }

        private void OnVehicleIdChanged(int? vehicleId)
        {
            Vehicle = string.Empty;

            if (_directoryManager != null)
            {
                var vehicle = _directoryManager.VehiclesCollection.FirstOrDefault(x => x.Id == vehicleId);
                if (vehicle != null)
                {
                    Vehicle = vehicle.DisplayName;
                }
            }
        }

        private void OnUnitIdChanged(int? unitId)
        {
            Unit = string.Empty;

            if (_directoryManager != null)
            {
                var unit = _directoryManager.UnitsCollection.FirstOrDefault(x => x.Id == unitId);
                if (unit != null)
                {
                    Unit = unit.DisplayName;
                }
            }
        }

        #endregion

    }

}
