using FMS.Core;
using FMS.DataManagers.Interfaces;
using OrderAccounting.Common.Infrastructure.Enums;
using OrderAccounting.Common.Infrastructure.Events;
using OrderAccounting.Common.Infrastructure.Interfaces;
using OrderAccounting.Common.Infrastructure.Models;
using OrderAccounting.Modules.Index.Menu.Views;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OrderAccounting.Modules.Index.Menu.ViewModels
{
    public class MenuViewModel : BindableBase, IMenuViewModel
    {
        #region Properties

        private bool _isFilterSet = false;
        public bool IsFilterSet
        {
            get { return _isFilterSet; }
            set
            {
                if (_isFilterSet != value)
                {
                    _isFilterSet = value;

                    if (_isFilterSet)
                    {
                        OnFilterSet();
                    }
                    else
                    {
                        OnFilterUnset();
                    }

                    RaisePropertyChanged("IsFilterSet");
                }
            }
        }

        private ObservableCollection<ValueModel> _statesDataSource;
        public ObservableCollection<ValueModel> StatesDataSource
        {
            get { return _statesDataSource; }
            set
            {
                if (_statesDataSource != value)
                {
                    _statesDataSource = value;
                    RaisePropertyChanged("StatesDataSource");
                }
            }
        }

        private ObservableCollection<ValueModel> _operationTypesDataSource;
        public ObservableCollection<ValueModel> OperationTypesDataSource
        {
            get { return _operationTypesDataSource; }
            set
            {
                if (_operationTypesDataSource != value)
                {
                    _operationTypesDataSource = value;
                    RaisePropertyChanged("OperationTypesDataSource");
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

        #endregion

        #region Variables

        private IEventAggregator _eventAggregator;

        private IMenuView _parentView;

        private IFilterModel _currentFilter;

        #endregion

        #region Commands

        public RellayCommand CreateCommand { get; set; }

        public RellayCommand FilterCommand { get; set; }

        public RellayCommand ClearFilterCommand { get; set; }

        public RellayCommand OperationStateCommand { get; set; }

        public RellayCommand OperationTypeCommand { get; set; }

        #endregion

        #region Initialization

        public MenuViewModel(IEventAggregator eventAggregator, IDirectoryManager directoryManager)
        {
            _eventAggregator = eventAggregator;

            _directoryManager = directoryManager;

            InitializeEvents(_eventAggregator);

            InitializeCollections(_directoryManager);

            InitializeCommands();
        }

        private void InitializeEvents(IEventAggregator _eventAggregator)
        {
            _eventAggregator.GetEvent<PubSubEvent<FilterEvent>>().Subscribe(FilterTriggered);
            _eventAggregator.GetEvent<PubSubEvent<FilterVisibilityEvent>>().Subscribe(FilterVisibilityTriggered);
        }

        private void InitializeCollections(IDirectoryManager _directoryManager)
        {

            var typesSource = new List<ValueModel>()
                {
                new ValueModel(0, "Основні"),
                new ValueModel(1, "Допоміжні")
            };

            OperationTypesDataSource = new ObservableCollection<ValueModel>(typesSource);
        }

        private void InitializeCommands()
        {
            CreateCommand = new RellayCommand(CreateCommandExecute);
            FilterCommand = new RellayCommand(FilterCommandExecute);
            ClearFilterCommand = new RellayCommand(ClearFilterCommandExecute, ClearFilterCommandCanExecute);

            OperationStateCommand = new RellayCommand(OperationStateCommandExecute, OperationStateCommandCanExecute);
            OperationTypeCommand = new RellayCommand(OperationTypeCommandExecute, OperationCommandCanExecute);
        }

        #endregion

        #region Methods

        public void SetParentView(IMenuView parentView)
        {
            _parentView = parentView;
        }

        private void OnShowFilter()
        {
            _eventAggregator.GetEvent<PubSubEvent<FilterVisibilityEvent>>().Publish(new FilterVisibilityEvent(true));

            _parentView.OnShowFilter();
        }

        private void OnHideFilter()
        {
            _parentView.OnHideFilter();
        }

        private void OnShowCreator()
        {
            _eventAggregator.GetEvent<PubSubEvent<DetailEvent>>().Publish(new DetailEvent(DetailState.Create));
        }

        private void OnFilterSet()
        {
            _parentView.OnFilterSet();
        }

        private void OnFilterUnset()
        {
            _parentView.OnFilterUnset();
        }

        private void FilterTriggered(FilterEvent obj)
        {
            if (!obj.IsFilterEmpty)
            {
                IsFilterSet = true;
            }

            _currentFilter = obj.CurrentFilter;

            OnHideFilter();
        }

        private void FilterVisibilityTriggered(FilterVisibilityEvent obj)
        {
            if (!obj.IsVisible)
            {
                OnHideFilter();
            }
        }

        #endregion

        #region Command Executors

        private void FilterCommandExecute(object obj)
        {
            OnShowFilter();
        }

        private void CreateCommandExecute(object obj)
        {
            OnShowCreator();
        }

        private void ClearFilterCommandExecute(object obj)
        {
            IsFilterSet = false;

            _currentFilter.Reset();

            _eventAggregator.GetEvent<PubSubEvent<FilterEvent>>().Publish(new FilterEvent(true, _currentFilter));
        }
        private bool ClearFilterCommandCanExecute(object obj)
        {
            return IsFilterSet;
        }

        private void OperationStateCommandExecute(object obj)
        {
            var stateName = obj as string;

            if (stateName != "active")
            {
                _eventAggregator.GetEvent<PubSubEvent<PagingEvent>>().Publish(new PagingEvent(ViewState.Show));
            }
            else
            {
                _eventAggregator.GetEvent<PubSubEvent<PagingEvent>>().Publish(new PagingEvent(ViewState.Hide));
            }

            _eventAggregator.GetEvent<PubSubEvent<OperationStateEvent>>().Publish(new OperationStateEvent(stateName));
        }
        private bool OperationStateCommandCanExecute(object obj)
        {
            var stateName = obj as string;

            return !string.IsNullOrEmpty(stateName);
        }


        private void OperationTypeCommandExecute(object obj)
        {
            _eventAggregator.GetEvent<PubSubEvent<OperationTypeEvent>>().Publish(new OperationTypeEvent((int)obj));
        }
        private bool OperationCommandCanExecute(object obj)
        {
            var id = obj as int?;

            return id.HasValue;
        }


        #endregion

    }
}
