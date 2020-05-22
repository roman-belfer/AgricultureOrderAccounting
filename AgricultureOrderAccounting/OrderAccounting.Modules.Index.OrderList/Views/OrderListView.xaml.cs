using OrderAccounting.Modules.Index.OrderList.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace OrderAccounting.Modules.Index.OrderList.Views
{
    /// <summary>
    /// Логика взаимодействия для OrderListView.xaml
    /// </summary>
    public partial class OrderListView : UserControl, IOrderListView
    {
        #region Routed Events

        public static readonly RoutedEvent HideStateEvent = EventManager.RegisterRoutedEvent("HideState", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(OrderListView));

        public static readonly RoutedEvent HideTypeEvent = EventManager.RegisterRoutedEvent("HideType", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(OrderListView));

        public static readonly RoutedEvent ShowFilterEvent = EventManager.RegisterRoutedEvent("ShowFilter", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(OrderListView));

        public static readonly RoutedEvent HideFilterEvent = EventManager.RegisterRoutedEvent("HideFilter", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(OrderListView));

        #endregion

        #region Events

        public event RoutedEventHandler HideState
        {
            add { AddHandler(HideStateEvent, value); }
            remove { RemoveHandler(HideStateEvent, value); }
        }

        public event RoutedEventHandler HideType
        {
            add { AddHandler(HideTypeEvent, value); }
            remove { RemoveHandler(HideTypeEvent, value); }
        }

        public event RoutedEventHandler ShowFilter
        {
            add { AddHandler(ShowFilterEvent, value); }
            remove { RemoveHandler(ShowFilterEvent, value); }
        }

        public event RoutedEventHandler HideFilter
        {
            add { AddHandler(HideFilterEvent, value); }
            remove { RemoveHandler(HideFilterEvent, value); }
        }

        #endregion

        #region Initialization

        public OrderListView(IOrderListViewModel viewModel)
        {
            InitializeComponent();

            viewModel.SetParentView(this);

            DataContext = viewModel;
        }

        #endregion

        #region Methods

        private void RaiseHideStateEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(HideStateEvent);
            RaiseEvent(newEventArgs);
        }

        private void RaiseHideTypeEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(HideTypeEvent);
            RaiseEvent(newEventArgs);
        }

        private void RaiseShowFilterEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(ShowFilterEvent);
            RaiseEvent(newEventArgs);
        }

        private void RaiseHideFilterEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(HideFilterEvent);
            RaiseEvent(newEventArgs);
        }

        public void OnHideState()
        {
            Dispatcher.BeginInvoke(new Action(() => { RaiseHideStateEvent(); }));
        }

        public void OnHideType()
        {
            Dispatcher.BeginInvoke(new Action(() => { RaiseHideTypeEvent(); }));
        }

        public void OnShowFilter()
        {
            Dispatcher.BeginInvoke(new Action(() => { RaiseShowFilterEvent(); }));
        }

        public void OnHideFilter()
        {
            Dispatcher.BeginInvoke(new Action(() => { RaiseHideFilterEvent(); }));
        }

        #endregion

    }
}
