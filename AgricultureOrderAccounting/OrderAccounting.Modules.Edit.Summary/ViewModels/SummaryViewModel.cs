using OrderAccounting.Common.Infrastructure.Enums;
using OrderAccounting.Common.Infrastructure.Events;
using OrderAccounting.Common.Infrastructure.Events.EditEvents;
using OrderAccounting.Common.Infrastructure.Interfaces;
using OrderAccounting.Modules.Edit.Summary.Views;
using Prism.Events;
using Prism.Mvvm;
using System;

namespace OrderAccounting.Modules.Edit.Summary.ViewModels
{
    public class SummaryViewModel : BindableBase, ISummaryViewModel
    {
        #region Variables

        private ISummaryView _parentView;

        private IEventAggregator _eventAggregator;

        /// <summary>
        /// Set in True when Editor send data about current OrderViewModel
        /// Set in False when Editor is ready to get data about OrderViewModel from other source
        /// </summary>
        private bool _sendData;

        #endregion

        #region Properties

        /// <summary>
        /// Current showed/edited/created view model
        /// </summary>
        private IOrderViewModel _orderViewModel;
        public IOrderViewModel OrderViewModel
        {
            get { return _orderViewModel; }
            set
            {
                if (_orderViewModel != value)
                {
                    _orderViewModel = value;
                    _orderViewModel.DataChanged += OrderDataChanged;

                    RaisePropertyChanged("OrderViewModel");
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

        public SummaryViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            InitializeEvents(_eventAggregator);
        }

        private void InitializeEvents(IEventAggregator _eventAggregator)
        {
            _eventAggregator.GetEvent<PubSubEvent<DetailEvent>>().Subscribe(DetailTriggered);
            _eventAggregator.GetEvent<PubSubEvent<EditOrderEvent>>().Subscribe(EditOrderTriggered);
        }

        #endregion

        #region Event Executors

        void OrderDataChanged(object sender, EventArgs e)
        {
            _sendData = true;

            _eventAggregator.GetEvent<PubSubEvent<EditOrderEvent>>().Publish(new EditOrderEvent(sender as IOrderViewModel));

            _sendData = false;
        }

        #endregion

        #region Event Triggers

        private void DetailTriggered(DetailEvent obj)
        {
            ChangeSummaryViewState(obj.DetailState, obj.OrderId);
        }

        private void EditOrderTriggered(EditOrderEvent obj)
        {
            SetCurrentOrder(obj.CurrentOrder);
        }

        #endregion

        #region Methods

        public void SetParentView(ISummaryView parentView)
        {
            _parentView = parentView;
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
        /// Initialize or deinitialize Editor getting editorState
        /// </summary>
        private void ChangeSummaryViewState(DetailState summaryState, int? orderId)
        {
            if (summaryState == DetailState.Create)
            {
                OnShowView();

                IsEditable = true;
            }
            else if (summaryState == DetailState.Edit && orderId.HasValue)
            {
                OnShowView();

                IsEditable = true;
            }
            else if (summaryState == DetailState.View && orderId.HasValue)
            {
                OnShowView();

                IsEditable = false;
            }
            else if (summaryState == DetailState.Hide)
            {
                OnHideView();
            }
        }

        private void OnShowView()
        {
            _parentView.OnShowView();
        }

        private void OnHideView()
        {
            _parentView.OnHideView();
        }

        #endregion

    }
}
