using OrderAccounting.Modules.Loader.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace OrderAccounting.Modules.Loader.Views
{
    /// <summary>
    /// Логика взаимодействия для LoaderView.xaml
    /// </summary>
    public partial class LoaderView : UserControl, ILoaderView
    {
        #region Routed Events

        public static readonly RoutedEvent ShowViewEvent = EventManager.RegisterRoutedEvent("ShowView", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LoaderView));

        public static readonly RoutedEvent HideViewEvent = EventManager.RegisterRoutedEvent("HideView", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LoaderView));

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

        #endregion

        #region Initialization

        public LoaderView(ILoaderViewModel viewModel)
        {
            InitializeComponent();

            viewModel.SetParentView(this);

            DataContext = viewModel;
        }

        #endregion

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

        public void OnShowView()
        {
            Dispatcher.BeginInvoke(new Action(() => { RaiseShowViewEvent(); }));
        }

        public void OnHideView()
        {
            Dispatcher.BeginInvoke(new Action(() => { RaiseHideViewEvent(); }));
        }

        #endregion
    }
}
