using Argo.DataAccess.LaborDetail.Model;
using FMS.Core;
using FMS.DataManagers.Interfaces;
using FMS.Services;
using OrderAccounting.Common.Infrastructure.Enums;
using OrderAccounting.Common.Infrastructure.Events;
using OrderAccounting.Common.Infrastructure.Interfaces;
using OrderAccounting.Common.Repository.Interfaces;
using OrderAccounting.Common.SignalRWorker.Interfaces;
using OrderAccounting.Modules.Index.OrderList.Views;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;

namespace OrderAccounting.Modules.Index.OrderList.ViewModels
{
    /// <summary>
    /// Order List
    /// </summary>
    public class OrderListViewModel : BindableBase, IOrderListViewModel
    {
        #region Variables

        private int? _currentOperationTypeId;

        private string _currentOperationStateName;

        private string _errorMessage;

        private IFilterModel _currentFilter;

        private IEventAggregator _eventAggregator;

        private IDirectoryManager _directoryManager;

        private IOrderListView _parentView;

        private IOrderRepository _orderRepository;

        private ISignalRWorker _signalRWorker;

        #endregion

        #region Properties

        /// <summary>
        /// Collection for orders to display after filtering, paging, state and type check changing
        /// </summary>
        private ObservableCollection<IOrderModel> _displayDataCollection = new ObservableCollection<IOrderModel>();
        public ObservableCollection<IOrderModel> DisplayDataCollection
        {
            get { return _displayDataCollection; }
            set
            {
                if (_displayDataCollection != value)
                {
                    _displayDataCollection = value;
                    RaisePropertyChanged("DisplayDataCollection");
                }
            }
        }

        /// <summary>
        /// Collection for canceled and finished orders
        /// </summary>
        private List<IOrderModel> _historyDataCollection = new List<IOrderModel>();
        public List<IOrderModel> HistoryDataCollection
        {
            get { return _historyDataCollection; }
            set
            {
                _historyDataCollection = value;
                UpdateDisplayDataCollection();
            }
        }

        /// <summary>
        /// Collection for active orders
        /// </summary>
        private List<IOrderModel> _activeDataCollection = new List<IOrderModel>();
        public List<IOrderModel> ActiveDataCollection
        {
            get { return _activeDataCollection; }
            set
            {
                _activeDataCollection = value;
                UpdateDisplayDataCollection();
            }
        }

        private int _currentPage = 0;
        private int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;

                LoadData();
            }
        }

        #endregion

        #region Commands

        public RellayCommand EditCommand { get; set; }

        public RellayCommand DetailCommand { get; set; }

        #endregion

        #region Initialziation

        public OrderListViewModel(IDirectoryManager directoryManager, IOrderRepository orderRepository,
            IEventAggregator eventAggregator, IFilterModel filterModel, ISignalRWorker signalRWorker)
        {
            _eventAggregator = eventAggregator;

            _directoryManager = directoryManager;

            _orderRepository = orderRepository;

            _currentFilter = filterModel;

            _signalRWorker = signalRWorker;

            InitializeEvents(_eventAggregator);

            InitializeCommands();

            InitializeCollections();

            InitializeHub();
        }

        private void InitializeHub()
        {
            if (_signalRWorker != null)
            {
                _signalRWorker.OrderAdded += SignalRWorker_OrderAdded;
                _signalRWorker.OrderUpdated += SignalRWorker_OrderUpdated;
                _signalRWorker.OrderRemoved += SignalRWorker_OrderRemoved;
            }
        }

        private void InitializeCollections()
        {
            LoadActiveData();
        }

        private void InitializeEvents(IEventAggregator _eventAggregator)
        {
            _eventAggregator.GetEvent<PubSubEvent<OperationTypeEvent>>().Subscribe(OperationTypeTriggered);
            _eventAggregator.GetEvent<PubSubEvent<OperationStateEvent>>().Subscribe(OperationStateTriggered);

            _eventAggregator.GetEvent<PubSubEvent<PageChangedEvent>>().Subscribe(PageChangedTriggered);

            _eventAggregator.GetEvent<PubSubEvent<FilterEvent>>().Subscribe(FilterTriggered);
            _eventAggregator.GetEvent<PubSubEvent<FilterVisibilityEvent>>().Subscribe(FilterVisibilityTriggered);
        }

        private void InitializeCommands()
        {
            EditCommand = new RellayCommand(EditCommandExecute, EditCommandCanExecute);
            DetailCommand = new RellayCommand(DetailCommandExecute, DetailCommandCanExecute);
        }

        #endregion

        #region Event Triggers

        /// <summary>
        /// Handle event after menu operation state changing
        /// </summary>
        private void OperationStateTriggered(OperationStateEvent obj)
        {
            _currentOperationStateName = obj.StateName;

            OnHideStateAnimation();

            UpdateDisplayDataCollection();
        }

        /// <summary>
        /// Handle event after menu operation type changing
        /// </summary>
        private void OperationTypeTriggered(OperationTypeEvent obj)
        {
            _currentOperationTypeId = obj.TypeId;

            OnHideTypeAnimation();

            UpdateDisplayDataCollection();
        }

        /// <summary>
        /// Handle event after paging state changing
        /// </summary>
        private void PageChangedTriggered(PageChangedEvent obj)
        {
            CurrentPage = obj.PageNumber;
        }

        /// <summary>
        /// Handle event after filter state changing
        /// </summary>
        private void FilterTriggered(FilterEvent obj)
        {
            SetFilter(obj.IsFilterEmpty, obj.CurrentFilter);

            LoadData();

            OnHideFilter();
        }

        /// <summary>
        /// Handle event after filter visibility state
        /// </summary>
        private void FilterVisibilityTriggered(FilterVisibilityEvent obj)
        {
            if (obj.IsVisible)
            {
                OnShowFilter();
            }
            else
            {
                OnHideFilter();
            }
        }

        #endregion

        #region Event Executors

        private void SignalRWorker_OrderAdded(object sender, FMS.Core.OrderEventArgs args)
        {
            if (args.Orders != null && args.Orders.Count > 0)
            {
                var thread = new Thread(() =>
                {
                    foreach (var i in args.Orders)
                    {
                        AddActiveOrder(i);
                    }
                });
                thread.IsBackground = true;
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
        }

        private void SignalRWorker_OrderUpdated(object sender, FMS.Core.OrderEventArgs args)
        {
            if (args.Orders != null && args.Orders.Count > 0)
            {
                var thread = new Thread(() =>
                {
                    foreach (var i in args.Orders)
                    {
                        RemoveActiveOrder(i.Identity);
                    }
                    foreach (var i in args.Orders)
                    {
                        AddActiveOrder(i);
                    }
                });
                thread.IsBackground = true;
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
        }

        private void SignalRWorker_OrderRemoved(object sender, FMS.Core.OrderEventArgs args)
        {
            if (args.OrderIds != null && args.OrderIds.Count > 0)
            {
                var thread = new Thread(() =>
                {
                    foreach (var i in args.OrderIds)
                    {
                        RemoveActiveOrder(i);
                    }
                });
                thread.IsBackground = true;
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set view which has current view model as data context
        /// <summary>
        public void SetParentView(IOrderListView parentView)
        {
            _parentView = parentView;
        }

        /// <summary>
        /// Update orders' display collection after data colelction changing
        /// <summary>
        private void UpdateDisplayDataCollection()
        {
            var thread = new Thread(() =>
            {
                var tempData = new List<IOrderModel>();

                SelectDataByOperationState(ref tempData);

                SelectDataByOperationType(ref tempData);

                DisplayDataCollection = new ObservableCollection<IOrderModel>(tempData);

                CommandManager.InvalidateRequerySuggested();
            });
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

        }

        /// <summary>
        /// Select data with current operation state
        /// </summary>
        private void SelectDataByOperationState(ref List<IOrderModel> tempData)
        {
            if (!string.IsNullOrEmpty(_currentOperationStateName))
            {
                if (_currentOperationStateName == "active")
                {
                    tempData = ActiveDataCollection;

                    SelectDataByFilter(ref tempData);
                }
                else
                {
                    tempData = HistoryDataCollection.Where(x => x.StateName == _currentOperationStateName).ToList();
                }
            }
        }

        /// <summary>
        /// Select data with current operation type
        /// </summary>
        private void SelectDataByOperationType(ref List<IOrderModel> tempData)
        {
            if (_currentOperationTypeId.HasValue)
            {
                tempData = tempData.Where(x => x.TypeId == _currentOperationTypeId).ToList();
            }
        }

        /// <summary>
        /// Select data which response to filter - just for active data? history data become filtered during loading from DB
        /// </summary>
        private void SelectDataByFilter(ref List<IOrderModel> tempData)
        {
            if (_currentFilter.IsFilterNotEmpty())
            {
                tempData = tempData.Where(x => _currentFilter.CheckFilter(x)).ToList();
            }
        }

        /// <summary>
        /// Set current filter
        /// </summary>
        private void SetFilter(bool IsFilterEmpty, IFilterModel CurrentFilter)
        {
            if (IsFilterEmpty)
            {
                _currentFilter.Reset();
            }
            else
            {
                _currentFilter = CurrentFilter;
            }
        }

        /// <summary>
        /// Load data using IOrderRepository by page number 
        /// </summary>
        private void LoadData()
        {
            OnShowLoader();

            var thread = new Thread(() =>
            {
                CheckCurrentPage();

                var historyData = _orderRepository.GetHistoryOrders(CurrentPage, _currentFilter, TaskClient.UrlData.DatabaseKey.ToString(), out _errorMessage).ToList();

                HistoryDataCollection = historyData != null ? new List<IOrderModel>(historyData) : new List<IOrderModel>();

                OnHideLoader();
            });
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        /// <summary>
        /// Check is current page equal to 0 and set it to 1
        /// </summary>
        private void CheckCurrentPage()
        {
            if (CurrentPage == 0)
            {
                CurrentPage = 1;
            }
        }

        /// <summary>
        /// Add new active order to collection converting from LaborDetail.ListItem
        /// </summary>
        private void AddActiveOrder(LaborDetail.ListItem order)
        {
            var item = _orderRepository.ConvertToWorkModel(order);
            if (item != null)
            {
                ActiveDataCollection.Add(item);

                UpdateDisplayDataCollection();
            }
        }

        /// <summary>
        /// Remove active order from collection by Id
        /// </summary>
        private void RemoveActiveOrder(int orderId)
        {
            if (ActiveDataCollection != null && ActiveDataCollection.Count > 0)
            {
                var items = ActiveDataCollection.Where(x => x.MasterNumber == orderId).ToList();
                if (items != null)
                {
                    foreach (var i in items)
                    {
                        ActiveDataCollection.Remove(i);
                    }

                    UpdateDisplayDataCollection();
                }
            }
        }

        /// <summary>
        /// Load active data using IOrderRepository
        /// </summary>
        private void LoadActiveData()
        {
            OnShowLoader();

            var thread = new Thread(() =>
            {
                var activeData = _orderRepository.GetActiveOrders(TaskClient.UrlData.DatabaseKey.ToString(), out _errorMessage);

                ActiveDataCollection = activeData != null ? new List<IOrderModel>(activeData) : new List<IOrderModel>();

                OnHideLoader();
            });
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void OnHideLoader()
        {
            _eventAggregator.GetEvent<PubSubEvent<LoaderEvent>>().Publish(new LoaderEvent(ViewState.Hide));
        }
        private void OnShowLoader()
        {
            _eventAggregator.GetEvent<PubSubEvent<LoaderEvent>>().Publish(new LoaderEvent(ViewState.Show));
        }

        private void OnHideStateAnimation()
        {
            _parentView.OnHideState();
        }
        private void OnHideTypeAnimation()
        {
            _parentView.OnHideType();
        }

        private void OnShowFilter()
        {
            _parentView.OnShowFilter();
        }
        private void OnHideFilter()
        {
            _parentView.OnHideFilter();
        }

        #endregion

        #region Command Executors

        private void EditCommandExecute(object obj)
        {
            var order = obj as IOrderModel;

            _eventAggregator.GetEvent<PubSubEvent<DetailEvent>>().Publish(new DetailEvent(DetailState.Edit, order.Id));
        }
        private bool EditCommandCanExecute(object obj)
        {
            var order = obj as IOrderModel;

            return order != null && order.StateName == "active";
        }

        private void DetailCommandExecute(object obj)
        {
            var order = obj as IOrderModel;

            _eventAggregator.GetEvent<PubSubEvent<DetailEvent>>().Publish(new DetailEvent(DetailState.View, order.Id, null, order.StateName));
        }
        private bool DetailCommandCanExecute(object obj)
        {
            return (obj as IOrderModel) != null;
        }

        #endregion

    }
}
