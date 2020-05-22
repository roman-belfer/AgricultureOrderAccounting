using Argo.DataAccess.LaborDetail.Model;
using FMS.DataManagers.Interfaces;
using OrderAccounting.Common.Infrastructure.Interfaces;
using System.Linq;

namespace OrderAccounting.Common.Infrastructure.ViewModels.OrderItemViewModels
{
    public class OperationItemViewModel : BaseDataItemViewModel, IOrderItemViewModel
    {
        #region Properties

        protected bool _hasOperation;
        public bool HasOperation
        {
            get { return _hasOperation; }
            set
            {
                if (_hasOperation != value)
                {
                    _hasOperation = value;
                    RaisePropertyChanged("HasOperation");
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
                    OnDataChanged();
                    RaisePropertyChanged("OperationId");
                }
            }
        }

        protected bool _hasBaseOperation;
        public bool HasBaseOperation
        {
            get { return _hasBaseOperation; }
            set
            {
                if (_hasBaseOperation != value)
                {
                    _hasBaseOperation = value;
                    RaisePropertyChanged("HasBaseOperation");
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
                    OnDataChanged();

                    RaisePropertyChanged("BaseOperationId");
                }
            }
        }

        #endregion

        #region Initialization

        public OperationItemViewModel(IDirectoryManager directoryManager) : base(directoryManager)
        {
            Title = "Технологічна операція";
        }

        #endregion

        #region Methods

        protected void OnOperationIdChanged(int? _operationId)
        {
            Operation = string.Empty;

            if (_directoryManager != null)
            {
                var operation = _directoryManager.OperationsInformation.FirstOrDefault(x => x.OperationId == _operationId);
                if (operation != null)
                {
                    Operation = operation.OperationName;
                }
            }

            HasOperation = _operationId.HasValue;
        }

        public override void ConvertDataToDTO<T>(T item)
        {
            if (typeof(T) == typeof(LaborDetail.Item))
            {
                var model = (item as LaborDetail.Item);

                model.Operation = OperationId.HasValue ? new Argo.DataAccess.All.Models.Operation.ListItem()
                {
                    Identity = OperationId.Value
                } : null;

                model.BaseOperation = BaseOperationId.HasValue ? new Argo.DataAccess.All.Models.LookupValue.ListItem()
                {
                    Identity = BaseOperationId.Value
                } : null;
            }
        }

        public override void ConvertDataFromDTO<T>(T item)
        {
            if (typeof(T) == typeof(LaborDetail.Item))
            {
                var model = (item as LaborDetail.Item);

                OperationId = model.Operation != null ? (int?)model.Operation.Identity : null;

                BaseOperationId = model.BaseOperation != null ? (int?)model.BaseOperation.Identity : null;
            }
        }

        public override int? GetOperationId()
        {
            return _operationId;
        }

        public override int? GetBaseOperationId()
        {
            return _baseOperationId;
        }

        public override int? GetPhaseId()
        {
            return null;
        }

        public override int? GetOperationTypeId()
        {
            return null;
        }

        public override void SetObjectData(bool isTransportation)
        { }

        protected virtual void OnBaseOperationIdChanged(int? _baseOperationId)
        {
            BaseOperation = string.Empty;

            if (_directoryManager != null && _directoryManager.BaseOperations != null)
            {
                var operation = _directoryManager.BaseOperations.FirstOrDefault(x => x.Identity == _baseOperationId);
                if (operation != null)
                {
                    BaseOperation = operation.DisplayName;
                }
            }

            HasBaseOperation = _baseOperationId.HasValue;
        }

        /// <summary>
        /// Returns validation bool value if base operation data is not empty
        /// </summary>
        public override bool IsDataValid()
        {
            IsValid = BaseOperationId.HasValue;

            return IsValid;
        }

        #endregion

        #region Command Executors

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
