using Argo.DataAccess.LaborDetail.Model;
using FMS.DataManagers.Interfaces;
using OrderAccounting.Common.Infrastructure.Enums;
using OrderAccounting.Common.Infrastructure.Factories;
using OrderAccounting.Common.Infrastructure.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OrderAccounting.Common.Infrastructure.ViewModels.OrderViewModels
{
    public abstract class BaseWorkViewModel : BindableBase, IOrderViewModel, IProcessable
    {
        #region Variables

        protected IDirectoryManager _directoryManager;

        protected IOrderItemFactory _orderItemFactory;

        protected List<OrderItemTypes> _dataItemTypes = new List<OrderItemTypes>();

        protected bool _isEmpty;

        #endregion

        #region Properties

        public abstract int Type { get; set; }

        public int Index { get; set; }

        public int LaborDetailId { get; set; }

        public int? StatusId { get; set; }

        private ObservableCollection<IOrderItemViewModel> _dataCollection;
        public ObservableCollection<IOrderItemViewModel> DataCollection
        {
            get { return _dataCollection; }
            set
            {
                if (value != null)
                {
                    if (_dataCollection != value)
                    {
                        _dataCollection = value;
                        RaisePropertyChanged("DataCollection");
                    }
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

                    RaisePropertyChanged("OperationTypeId");
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

                    RaisePropertyChanged("ActualPhaseId");
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

                    RaisePropertyChanged("BaseOperationId");
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

                    RaisePropertyChanged("OperationId");
                }
            }
        }

        #endregion

        #region Events

        public event EventHandler DataChanged;

        #endregion

        #region Event Executors

        private void DataItem_DataChanged(object sender, EventArgs e)
        {
            OnDataChanged();

            UpdatePhase();

            UpdateOperationType();

            UpdateBaseOperation();

            UpdateOperation();
        }

        #endregion

        #region Initialization

        protected BaseWorkViewModel(IDirectoryManager directoryManager, IOrderItemFactory orderItemFactory)
        {
            _directoryManager = directoryManager;

            _orderItemFactory = orderItemFactory;

            InitializeDataItems();

            InitializeDataCollection();

            if (_directoryManager != null && _directoryManager.LaborStatus != null)
            {
                var status = _directoryManager.LaborStatus.FirstOrDefault(x => x.Name == "active");
                if (status != null)
                {
                    StatusId = status != null ? (int?)status.Identity : null;
                }
            }
        }

        /// <summary>
        /// Initialize data items collection and adding items depending on current view model
        /// </summary>
        private void InitializeDataCollection()
        {
            if (_dataItemTypes != null && _dataItemTypes.Count() > 0)
            {
                foreach (var item in _dataItemTypes)
                {
                    AddDataItem(_orderItemFactory.CreateOrderItem(item));
                }

                OnDataCollectionChanged();
            }
        }

        /// <summary>
        /// Initialize data items types
        /// </summary>
        protected abstract void InitializeDataItems();

        #endregion

        #region Methods

        protected void OnDataChanged()
        {
            if (DataChanged != null)
            {
                DataChanged.Invoke(this, new EventArgs());
            }
        }

        protected void OnDataCollectionChanged()
        {
            foreach (var item in DataCollection)
            {
                item.DataChanged += DataItem_DataChanged;
            }
        }

        /// <summary>
        /// Returns integer value as percent of filled data
        /// </summary>
        public int GetProcessed()
        {
            int process = 0;

            if (DataCollection != null && DataCollection.Count > 0)
            {
                var validItemCount = DataCollection.Where(x => x.IsDataValid()).Count();

                process = (int)(((double)validItemCount / (double)DataCollection.Count) * 100);
            }

            _isEmpty = process != 100;

            return process;
        }

        /// <summary>
        /// Add new IDataViewModel to DataCollection
        /// Set Previous and Next items
        /// Set IsChecked = true if element is first in the collection
        /// </summary>
        private void AddDataItem(IOrderItemViewModel dataItem)
        {
            if (DataCollection == null)
            {
                DataCollection = new ObservableCollection<IOrderItemViewModel>();
            }

            var previousItem = DataCollection.LastOrDefault();

            if (previousItem != null)
            {
                previousItem.SetNextData(dataItem);
                dataItem.SetPreviousData(previousItem);
            }
            else
            {
                dataItem.IsChecked = true;
            }

            DataCollection.Add(dataItem);
        }

        protected virtual void UpdateOperation()
        {
            _operationId = null;

            var item = _dataCollection.FirstOrDefault(x => x.GetOperationId().HasValue);
            if (item != null)
            {
                OperationId = item.GetOperationId();
            }

            RaisePropertyChanged("OperationId");
        }

        protected virtual void UpdateBaseOperation()
        {
            _baseOperationId = null;

            var item = _dataCollection.FirstOrDefault(x => x.GetBaseOperationId().HasValue);
            if (item != null)
            {
                BaseOperationId = item.GetBaseOperationId();
            }

            var operation = _directoryManager.BaseOperations.FirstOrDefault(x => x.Identity == BaseOperationId);
            if (operation != null)
            {
                foreach (var i in _dataCollection)
                {
                    i.SetObjectData(operation.Name == "transportation");
                }
            }

            RaisePropertyChanged("BaseOperationId");
        }

        protected virtual void UpdateOperationType()
        {
            _operationTypeId = null;

            var item = _dataCollection.FirstOrDefault(x => x.GetOperationTypeId().HasValue);
            if (item != null)
            {
                OperationTypeId = item.GetOperationTypeId();
            }

            RaisePropertyChanged("OperationTypeId");
        }

        protected virtual void UpdatePhase()
        {
            _actualPhaseId = null;

            var item = _dataCollection.FirstOrDefault(x => x.GetPhaseId().HasValue);
            if (item != null)
            {
                ActualPhaseId = item.GetPhaseId();
            }

            RaisePropertyChanged("ActualPhaseId");
        }

        public virtual void ConvertToModel<TItem>(TItem result) where TItem : class
        {
            if (!_isEmpty)
            {
                if (typeof(TItem) == typeof(LaborDetail.Item))
                {
                    var model = (result as LaborDetail.Item);
                    model.Identity = LaborDetailId;
                    model.Status = new Argo.DataAccess.All.Models.LookupValue.ListItem() { Identity = StatusId.Value };

                    foreach (var i in DataCollection)
                    {
                        i.ConvertDataToDTO(result);
                    }
                }
            }
        }

        public virtual void ConvertToViewModel<TItem>(TItem item) where TItem : class
        {
            if (typeof(TItem) == typeof(LaborDetail.Item))
            {
                var model = (item as LaborDetail.Item);
                LaborDetailId = model.Identity;
                StatusId = model.Status.Identity;

                foreach (var i in DataCollection)
                {
                    i.ConvertDataFromDTO(item);
                }
            }
        }

        #endregion

    }
}
