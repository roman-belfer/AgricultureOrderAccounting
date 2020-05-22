using OrderAccounting.Modules.Index.Paging.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace OrderAccounting.Modules.Index.Paging.Views
{
    public partial class PagingView : UserControl, IPagingView
    {
        #region Routed Events

        public static readonly RoutedEvent ShowViewEvent = EventManager.RegisterRoutedEvent("ShowView", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PagingView));

        public static readonly RoutedEvent HideViewEvent = EventManager.RegisterRoutedEvent("HideView", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PagingView));

        public static readonly RoutedEvent ShowFilterEvent = EventManager.RegisterRoutedEvent("ShowFilter", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PagingView));

        public static readonly RoutedEvent HideFilterEvent = EventManager.RegisterRoutedEvent("HideFilter", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PagingView));

        #endregion

        #region Events

        public event RoutedEventHandler ShowView
        {
            add { AddHandler(ShowViewEvent, value); }
            remove { RemoveHandler(ShowViewEvent, value); }
        }

        public event RoutedEventHandler HideView
        {
            add { AddHandler(HideViewEvent, value); }
            remove { RemoveHandler(HideViewEvent, value); }
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

        public PagingView(IPagingViewModel viewModel)
        {
            InitializeComponent();

            viewModel.SetParentView(this);

            DataContext = viewModel;
        }

        #region Methods

        private void RaiseShowViewEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(ShowViewEvent);
            RaiseEvent(newEventArgs);
        }

        private void RaiseHideViewEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(HideViewEvent);
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

        public void OnShow()
        {
            Dispatcher.BeginInvoke(new Action(() => { RaiseShowViewEvent(); }));
        }

        public void OnHide()
        {
            Dispatcher.BeginInvoke(new Action(() => { RaiseHideViewEvent(); }));
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
