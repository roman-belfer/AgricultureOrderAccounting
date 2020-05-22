using Argo.DataAccess.All.Models;
using Argo.DataAccess.LaborDetail.Model;
using FMS.Core;
using FMS.DataManagers.Interfaces;
using System;
using System.Linq;

namespace OrderAccounting.Common.Infrastructure.ViewModels.OrderItemViewModels
{
    public class DetailItemViewModel : BaseDataItemViewModel
    {
        #region Properties

        protected bool _hasYear;
        public bool HasYear
        {
            get { return _hasYear; }
            set
            {
                if (_hasYear != value)
                {
                    _hasYear = value;
                    RaisePropertyChanged("HasYear");
                }
            }
        }

        protected bool _hasEnterprise;
        public bool HasEnterprise
        {
            get { return _hasEnterprise; }
            set
            {
                if (_hasEnterprise != value)
                {
                    _hasEnterprise = value;
                    RaisePropertyChanged("HasEnterprise");
                }
            }
        }

        protected bool _hasActualPhase;
        public bool HasActualPhase
        {
            get { return _hasActualPhase; }
            set
            {
                if (_hasActualPhase != value)
                {
                    _hasActualPhase = value;
                    RaisePropertyChanged("HasActualPhase");
                }
            }
        }

        protected bool _hasOperationType;
        public bool HasOperationType
        {
            get { return _hasOperationType; }
            set
            {
                if (_hasOperationType != value)
                {
                    _hasOperationType = value;
                    RaisePropertyChanged("HasOperationType");
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

        protected int? _enterpriseId;
        public int? EnterpriseId
        {
            get { return _enterpriseId; }
            set
            {
                if (_enterpriseId != value)
                {
                    _enterpriseId = value;

                    OnEnterpriseIdChanged(_enterpriseId);
                    OnDataChanged();

                    RaisePropertyChanged("EnterpriseId");
                }
            }
        }

        protected string _enterprise;
        public string Enterprise
        {
            get { return _enterprise; }
            set
            {
                if (_enterprise != value)
                {
                    _enterprise = value;

                    RaisePropertyChanged("Enterprise");
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
                    OnDataChanged();

                    RaisePropertyChanged("OperationTypeId");
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
                    OnDataChanged();
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
                    OnDataChanged();
                    RaisePropertyChanged("DateFrom");
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
                    OnDataChanged();
                    RaisePropertyChanged("DateTo");
                }
            }

        }

        #endregion

        #region Commands

        public RellayCommand OperationTypeCommand { get; set; }

        public RellayCommand ActualPhaseCommand { get; set; }

        #endregion

        #region Initialization

        public DetailItemViewModel(IDirectoryManager directoryManager) : base(directoryManager)
        {
            Title = "Початкова інформація";

            DateFrom = DateTime.Now;
            DateTo = DateTime.Now.AddDays(1);
        }

        protected override void InitializeCommands()
        {
            base.InitializeCommands();

            OperationTypeCommand = new RellayCommand(OperationTypeCommandExecute);
            ActualPhaseCommand = new RellayCommand(ActualPhaseCommandExecute);
        }

        #endregion

        #region Methods

        protected void OnOperationTypeIdChanged(int? operationTypeId)
        {
            OperationType = string.Empty;

            if (_directoryManager != null && _directoryManager.OperationTypes != null)
            {
                var operation = _directoryManager.OperationTypes.FirstOrDefault(x => x.Identity == operationTypeId);
                if (operation != null)
                {
                    OperationType = operation.DisplayName;
                    HasEnterprise = operation.Name == "foreigncontractors";
                    HasOperationType = operation.Name != "auxiliaryproduction";
                    if (operation.Name != "mainproduction")
                    {
                        Year = "";
                    }
                }
            }
        }

        private void OnEnterpriseIdChanged(int? enterpriseId)
        {
            Enterprise = string.Empty;

            if (_directoryManager != null && _directoryManager.EnterprisesInformation != null)
            {
                var enterprise = _directoryManager.EnterprisesInformation.FirstOrDefault(x => x.EntId == enterpriseId);
                if (enterprise != null)
                {
                    Enterprise = enterprise.EntName;
                }
            }

            HasActualPhase &= !enterpriseId.HasValue;
        }

        protected void OnActualPhaseIdChanged(int? actualPhaseId)
        {
            ActualPhase = string.Empty;

            if (_directoryManager != null && _directoryManager.ActualPhases != null)
            {
                var phase = _directoryManager.ActualPhases.FirstOrDefault(x => x.Identity == _actualPhaseId);
                if (phase != null)
                {
                    ActualPhase = phase.Phase.OCName;
                    Year = phase.Year.DisplayName;
                }
                else
                {
                    Year = string.Empty;
                }
            }

            HasActualPhase = actualPhaseId.HasValue;
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

        /// <summary>
        /// Returns validation bool value if details of actual phase, 
        /// operation type and dates is not empty
        /// </summary>
        public override bool IsDataValid()
        {
            IsValid = OperationTypeId.HasValue && DateFrom.HasValue && DateTo.HasValue && DateFrom < DateTo;

            if (HasOperationType)
            {
                IsValid &= HasEnterprise ? EnterpriseId.HasValue : ActualPhaseId.HasValue;
            }

            return IsValid;
        }

        public override int? GetPhaseId()
        {
            return ActualPhaseId;
        }

        public override int? GetOperationTypeId()
        {
            return OperationTypeId;
        }

        public override void ConvertDataToDTO<T>(T item)
        {
            if (typeof(T) == typeof(LaborDetail.Item))
            {
                var model = (item as LaborDetail.Item);

                if (ActualPhaseId.HasValue)
                {
                    model.ActualPhase = new OperationCategory.ListItem()
                    {
                        Identity = ActualPhaseId.Value
                    };

                    var phase = _directoryManager.ActualPhases.FirstOrDefault(x => x.Identity == ActualPhaseId);
                    model.ActualPhaseYear = phase != null ? phase.Year : null;
                }

                model.Contragent = EnterpriseId.HasValue ? new Argo.DataAccess.All.Models.Enterprise.ListItem()
                {
                    Identity = EnterpriseId.Value
                } : null;

                model.OperationType = OperationTypeId.HasValue ? new LookupValue.ListItem()
                {
                    Identity = OperationTypeId.Value
                } : null;

                model.DateFrom = DateFrom.HasValue ? DateFrom.Value : DateTime.Now;

                model.DateTo = DateTo.HasValue ? DateTo.Value : DateTime.Now;
            }
        }

        public override void ConvertDataFromDTO<T>(T item)
        {
            if (typeof(T) == typeof(LaborDetail.Item))
            {
                var model = (item as LaborDetail.Item);

                ActualPhaseId = model.ActualPhase != null ? (int?)model.ActualPhase.Identity : null;
                EnterpriseId = model.Contragent != null ? (int?)model.Contragent.Identity : null;
                OperationTypeId = model.OperationType != null ? (int?)model.OperationType.Identity : null;
                DateFrom = model.DateFrom;
                DateTo = model.DateTo;
            }
        }

        public override int? GetOperationId()
        {
            return null;
        }

        public override int? GetBaseOperationId()
        {
            return null;
        }

        public override void SetObjectData(bool isTransportation)
        { }

        #endregion

        #region Command Executors

        private void OperationTypeCommandExecute(object obj)
        {
            int? operationTypeId = obj as int?;

            if (operationTypeId.HasValue)
            {
                OperationTypeId = operationTypeId.Value;
            }
        }

        private void ActualPhaseCommandExecute(object obj)
        {
            int? phaseId = obj as int?;

            if (phaseId.HasValue)
            {
                ActualPhaseId = phaseId.Value;
            }
        }

        protected override void AddCommandExecute(object obj)
        { }
        protected override bool AddCommandCanExecute(object obj)
        {
            return false;
        }

        protected override void RemoveCommandExecute(object obj)
        { }

        #endregion

    }
}
