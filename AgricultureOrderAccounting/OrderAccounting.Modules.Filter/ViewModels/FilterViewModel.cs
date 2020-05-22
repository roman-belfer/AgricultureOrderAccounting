using FMS.Core;
using FMS.DataManagers.Interfaces;
using OrderAccounting.Common.Infrastructure.Events;
using OrderAccounting.Common.Infrastructure.Interfaces;
using OrderAccounting.Modules.Filter.Views;
using Prism.Events;
using Prism.Mvvm;

namespace OrderAccounting.Modules.Filter.ViewModels
{
    /// <summary>
    /// Filter
    /// </summary>
    public class FilterViewModel : BindableBase, IFilterViewModel
    {
        #region Variables

        private IEventAggregator _eventAggregator;

        private IFilterView _parentView;

        #endregion

        #region Properties

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

        private IFilterModel _currentFilter;
        public IFilterModel CurrentFilter
        {
            get { return _currentFilter; }
            set
            {
                if (_currentFilter != value)
                {
                    _currentFilter = value;
                    RaisePropertyChanged("CurrentFilter");
                }
            }
        }

        #endregion

        #region Command

        public RellayCommand FilterCommand { get; set; }

        public RellayCommand HideCommand { get; set; }

        #endregion

        #region Initialization

        public FilterViewModel(IDirectoryManager directoryManager, IEventAggregator eventAggregator, IFilterModel filterModel)
        {
            DirectoryManager = directoryManager;

            CurrentFilter = filterModel;

            InitializeEvents(eventAggregator);

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            FilterCommand = new RellayCommand(FilterCommandExecute, FilterCommandCanExecute);
            HideCommand = new RellayCommand(HideCommandExecute);
        }

        private void InitializeEvents(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<PubSubEvent<FilterVisibilityEvent>>().Subscribe(FilterVisibilityTriggered);
            _eventAggregator.GetEvent<PubSubEvent<FilterEvent>>().Subscribe(FilterTriggered);
        }

        #endregion

        #region Methods

        public void SetParentView(IFilterView parentView)
        {
            _parentView = parentView;
        }

        #endregion

        #region Event Triggers

        private void FilterTriggered(FilterEvent obj)
        {
            if (obj.IsFilterEmpty)
            {
                CurrentFilter.Reset();
            }
        }

        private void FilterVisibilityTriggered(FilterVisibilityEvent obj)
        {
            if (obj.IsVisible)
            {
                _parentView.OnShowView();
            }
        }

        #endregion

        #region Command Executors

        private void FilterCommandExecute(object obj)
        {
            _eventAggregator.GetEvent<PubSubEvent<FilterEvent>>().Publish(new FilterEvent(false, CurrentFilter));

            _parentView.OnHideView();
        }
        private bool FilterCommandCanExecute(object obj)
        {
            return CurrentFilter.IsFilterNotEmpty();
        }

        private void HideCommandExecute(object obj)
        {
            _eventAggregator.GetEvent<PubSubEvent<FilterVisibilityEvent>>().Publish(new FilterVisibilityEvent(false));

            _parentView.OnHideView();
        }

        #endregion

    }
}
