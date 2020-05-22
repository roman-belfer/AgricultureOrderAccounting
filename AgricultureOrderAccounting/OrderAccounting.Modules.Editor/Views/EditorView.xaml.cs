using DevExpress.Xpf.Grid;
using OrderAccounting.Modules.Editor.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace OrderAccounting.Modules.Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для EditorView.xaml
    /// </summary>
    public partial class EditorView : UserControl, IEditorView
    {
        #region Routed Events

        public static readonly RoutedEvent ShowViewEvent = EventManager.RegisterRoutedEvent("ShowView", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EditorView));

        public static readonly RoutedEvent HideViewEvent = EventManager.RegisterRoutedEvent("HideView", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EditorView));

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

        public EditorView(IEditorViewModel viewModel)
        {
            GridControl.AllowInfiniteGridSize = true;

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
