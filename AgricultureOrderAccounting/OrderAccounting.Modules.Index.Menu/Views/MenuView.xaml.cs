using OrderAccounting.Modules.Index.Menu.ViewModels;
using System.Windows.Controls;
using System;
using System.Windows;

namespace OrderAccounting.Modules.Index.Menu.Views
{
    public partial class MenuView : UserControl, IMenuView
    {
        #region Routed Events

        public static readonly RoutedEvent FilterSetEvent = EventManager.RegisterRoutedEvent("FilterSet", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuView));

        public static readonly RoutedEvent FilterUnsetEvent = EventManager.RegisterRoutedEvent("FilterUnset", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuView));

        public static readonly RoutedEvent ShowFilterEvent = EventManager.RegisterRoutedEvent("ShowFilter", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuView));

        public static readonly RoutedEvent HideFilterEvent = EventManager.RegisterRoutedEvent("HideFilter", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuView));

        #endregion

        #region Events

        public event RoutedEventHandler FilterSet
        {
            add { AddHandler(FilterSetEvent, value); }
            remove { RemoveHandler(FilterSetEvent, value); }
        }

        public event RoutedEventHandler FilterUnset
        {
            add { AddHandler(FilterUnsetEvent, value); }
            remove { RemoveHandler(FilterUnsetEvent, value); }
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

        public MenuView(IMenuViewModel viewModel)
        {
            InitializeComponent();

            viewModel.SetParentView(this);

            DataContext = viewModel;
        }

        #endregion

        #region Methods

        private void RaiseFilterSetEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(FilterSetEvent);
            RaiseEvent(newEventArgs);
        }

        private void RaiseFilterUnsetEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(FilterUnsetEvent);
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

        public void OnFilterSet()
        {
            Dispatcher.BeginInvoke(new Action(() => { RaiseFilterSetEvent(); }));
        }

        public void OnFilterUnset()
        {
            Dispatcher.BeginInvoke(new Action(() => { RaiseFilterUnsetEvent(); }));
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
