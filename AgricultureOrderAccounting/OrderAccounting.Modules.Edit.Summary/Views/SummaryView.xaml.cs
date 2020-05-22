using OrderAccounting.Modules.Edit.Summary.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace OrderAccounting.Modules.Edit.Summary.Views
{
    /// <summary>
    /// Логика взаимодействия для SummaryView.xaml
    /// </summary>
    public partial class SummaryView : UserControl, ISummaryView
    {
        #region Routed Events

        public static readonly RoutedEvent ShowViewEvent = EventManager.RegisterRoutedEvent("ShowView", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SummaryView));

        public static readonly RoutedEvent HideViewEvent = EventManager.RegisterRoutedEvent("HideView", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SummaryView));

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

        public SummaryView(ISummaryViewModel viewModel)
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
