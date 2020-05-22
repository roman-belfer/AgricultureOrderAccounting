using FMS.DataManagers.Interfaces;
using FMS.Services;
using OrderAccounting.Common.Infrastructure.Enums;
using OrderAccounting.Common.Infrastructure.Events;
using OrderAccounting.Common.Infrastructure.Events.EditEvents;
using OrderAccounting.Common.Infrastructure.Factories;
using OrderAccounting.Common.Infrastructure.Interfaces;
using OrderAccounting.Common.Repository.Interfaces;
using OrderAccounting.Modules.Editor.Views;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace OrderAccounting.Modules.Editor.ViewModels
{
    /// <summary>
    /// Editor
    /// </summary>
    public class EditorViewModel : BindableBase, IEditorViewModel
    {
        #region Variables

        /// <summary>
        /// Set in True when Editor send data about current OrderViewModel
        /// Set in False when Editor is ready to get data about OrderViewModel from other source
        /// </summary>
        private bool _sendData;

        private bool _isDetailPublished;

        private string _errorMessage;

        private IEditorView _parentView;

        private IEventAggregator _eventAggregator;

        private IOrderRepository _orderRepository;

        private IOrderFactory _orderFactory;

        #endregion

        #region Properties

        /// <summary>
        /// List of showed/edited/created view models
        /// </summary>
        private List<IOrderViewModel> _orderCollection = new List<IOrderViewModel>();

        /// <summary>
        /// Current showed/edited/created view model
        /// </summary>
        public IOrderViewModel _orderViewModel;
        public IOrderViewModel OrderViewModel
        {
            get { return _orderViewModel; }
            set
            {
                if (_orderViewModel != value)
                {
                    _orderViewModel = value;
                    _orderViewModel.DataChanged += OrderDataChanged;

                    OrderDataChanged(_orderViewModel, new EventArgs());

                    RaisePropertyChanged("OrderViewModel");
                }
            }
        }

        private IDirectoryManager _directoryManager;
        public IDirectoryManager DirectoryManager
        {
            get { return _directoryManager; }
            set
            {
                if (_directoryManager != value)
                {
                    _directoryManager = value;

                    RaisePropertyChanged("DirectoryManager");
                }
            }
        }

        /// <summary>
        /// Describes is view able to be editable
        /// </summary>
        private bool _isEditable;
        public bool IsEditable
        {
            get { return _isEditable; }
            set
            {
                if (_isEditable != value)
                {
                    _isEditable = value;
                    RaisePropertyChanged("IsEditable");
                }
            }
        }

        #endregion

        #region Initialization

        public EditorViewModel(IEventAggregator eventAggregator, IDirectoryManager directoryManager,
            IOrderRepository orderRepository, IOrderFactory orderFactory)
        {
            _eventAggregator = eventAggregator;

            _orderRepository = orderRepository;

            _orderFactory = orderFactory;

            DirectoryManager = directoryManager;

            InitializeEvents(_eventAggregator);
        }

        private void InitializeEvents(IEventAggregator _eventAggregator)
        {
            _eventAggregator.GetEvent<PubSubEvent<DetailEvent>>().Subscribe(DetailTriggered);

            _eventAggregator.GetEvent<PubSubEvent<EditNavigationEvent>>().Subscribe(EditNavigationTriggered);

            _eventAggregator.GetEvent<PubSubEvent<EditOrderEvent>>().Subscribe(EditOrderTriggered);

            _eventAggregator.GetEvent<PubSubEvent<SaveEvent>>().Subscribe(SaveEventTriggered);

            _eventAggregator.GetEvent<PubSubEvent<ChangeStatusEvent>>().Subscribe(ChangeStatusEventTriggered);

            _eventAggregator.GetEvent<PubSubEvent<AdditionalEvent>>().Subscribe(AdditionalEventTriggered);
        }

        #endregion

        #region Event Executors

        void OrderDataChanged(object sender, EventArgs e)
        {
            _sendData = true;

            _eventAggregator.GetEvent<PubSubEvent<EditOrderEvent>>().Publish(new EditOrderEvent(sender as IOrderViewModel));

            _sendData = false;

            _eventAggregator.GetEvent<PubSubEvent<NavigationProcessEvent>>().Publish(new NavigationProcessEvent(sender as IProcessable));
        }

        #endregion

        #region Event Triggers

        private void DetailTriggered(DetailEvent obj)
        {
            ChangeEditorViewState(obj.DetailState, obj.OrderId);
        }

        private void EditNavigationTriggered(EditNavigationEvent obj)
        {
            SetCurrentOrder(obj.WorkTypeId, obj.Index);
        }

        private void EditOrderTriggered(EditOrderEvent obj)
        {
            SetCurrentOrder(obj.CurrentOrder);
        }

        private void SaveEventTriggered(SaveEvent obj)
        {
            SaveOrder();
        }

        private void ChangeStatusEventTriggered(ChangeStatusEvent obj)
        {
            OnShowLoader();

            var thread = new Thread(() =>
            {
                if (_orderRepository.ChangeOrderStatus(_orderCollection.FirstOrDefault().LaborDetailId, obj.StatusId, TaskClient.UrlData.DatabaseKey.ToString(), out _errorMessage))
                {
                    _eventAggregator.GetEvent<PubSubEvent<DetailEvent>>().Publish(new DetailEvent(DetailState.Hide));
                }
                else
                {
                    (_parentView as DispatcherObject).Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show(_errorMessage, "Не вдалося змінити статус наряду!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }));
                }

                OnHideLoader();
            });
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void AdditionalEventTriggered(AdditionalEvent obj)
        {
            SetCurrentOrder(obj.Id, obj.Index);
        }

        #endregion

        #region Methods

        public void SetParentView(IEditorView parentView)
        {
            _parentView = parentView;
        }

        private void OnHideLoader()
        {
            _eventAggregator.GetEvent<PubSubEvent<LoaderEvent>>().Publish(new LoaderEvent(ViewState.Hide));
        }

        private void OnShowLoader()
        {
            _eventAggregator.GetEvent<PubSubEvent<LoaderEvent>>().Publish(new LoaderEvent(ViewState.Show));
        }

        /// <summary>
        /// Show Editor view
        /// </summary>
        private void OnShowView()
        {
            _parentView.OnShowView();
        }

        /// <summary>
        /// Hide Editor view and reset orders' data
        /// </summary>
        private void OnHideView()
        {
            _orderCollection = new List<IOrderViewModel>();

            _parentView.OnHideView();
        }

        /// <summary>
        /// Initialize or deinitialize Editor getting editorState
        /// </summary>
        private void ChangeEditorViewState(DetailState editorState, int? orderId)
        {
            if (_isDetailPublished) return;

            if (editorState == DetailState.Create)
            {
                OnShowView();

                IsEditable = true;
            }
            else if (editorState == DetailState.Edit && orderId.HasValue)
            {
                ShowDetails(orderId.Value, DetailState.Edit);
            }
            else if (editorState == DetailState.View && orderId.HasValue)
            {
                ShowDetails(orderId.Value, DetailState.View);
            }
            else if (editorState == DetailState.Hide)
            {
                OnHideView();
            }

        }

        private void ShowDetails(int orderId, DetailState detailState)
        {
            var thread = new Thread(() =>
            {
                OnShowLoader();

                LoadOrderDetail(orderId);

                _isDetailPublished = true;

                _eventAggregator.GetEvent<PubSubEvent<DetailEvent>>().Publish(new DetailEvent(detailState, orderId, _orderCollection));

                _isDetailPublished = false;

                IsEditable = detailState != DetailState.View;

                if (_orderCollection != null && _orderCollection.Count > 0)
                {
                    foreach (var i in _orderCollection)
                    {
                        _eventAggregator.GetEvent<PubSubEvent<NavigationProcessEvent>>().Publish(new NavigationProcessEvent(i as IProcessable));
                    }
                }

                OnHideLoader();
            });
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        /// <summary>
        /// Load order detail by orderId using IOrderRepository
        /// </summary>
        private void LoadOrderDetail(int orderId)
        {
            var data = _orderRepository.GetMasterOrder(orderId, TaskClient.UrlData.DatabaseKey.ToString(), out _errorMessage);
            if (data != null && data.Count() > 0 && string.IsNullOrEmpty(_errorMessage))
            {
                _orderCollection = new List<IOrderViewModel>(data);

                OnShowView();
            }
            else
            {
                (_parentView as DispatcherObject).Dispatcher.BeginInvoke(new Action(() =>
                {
                    MessageBox.Show(_errorMessage, "Не вдалося завантажити дані!", MessageBoxButton.OK, MessageBoxImage.Warning);

                    _eventAggregator.GetEvent<PubSubEvent<DetailEvent>>().Publish(new DetailEvent(DetailState.Hide));
                }));
            }
        }

        /// <summary>
        /// Save order using IOrderRepository
        /// </summary>
        private void SaveOrder()
        {
            OnShowLoader();

            var thread = new Thread(() =>
            {
                if (_orderRepository.SaveOrder(_orderCollection, TaskClient.UrlData.DatabaseKey.ToString(), out _errorMessage))
                {
                    _eventAggregator.GetEvent<PubSubEvent<DetailEvent>>().Publish(new DetailEvent(DetailState.Hide));
                }
                else
                {
                    (_parentView as DispatcherObject).Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show(_errorMessage, "Не вдалося зберегти дані!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }));
                }

                OnHideLoader();
            });
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        /// <summary>
        /// Set current OrderViewModel if _sendData is set to False and Editor
        /// is ready to get data from other source
        /// </summary>
        private void SetCurrentOrder(IOrderViewModel viewModel)
        {
            if (!_sendData)
            {
                OrderViewModel = viewModel;
            }
        }

        /// <summary>
        /// Set current OrderViewModel based on input workTypeId
        /// </summary>
        private void SetCurrentOrder(int orderTypeId, int? index = null)
        {
            IOrderViewModel order;

            if (index.HasValue)
            {
                var orders = _orderCollection.ToList();
                if (orders != null && orders.Count >= index.Value + 1)
                {
                    order = orders.FirstOrDefault(x => x.Index == index.Value);
                }
                else
                {
                    order = null;
                }
            }
            else
            {
                order = _orderCollection.FirstOrDefault(x => x.Type == orderTypeId);
            }

            if (order != null)
            {
                OrderViewModel = order;
            }
            else
            {
                OrderViewModel = _orderFactory.CreateOrderByType(orderTypeId, index.HasValue ? index.Value : _orderCollection.Count);

                _orderCollection.Add(OrderViewModel);
            }

        }

        #endregion

    }
}
