using FMS.Core;
using FMS.DataManagers.Interfaces;
using FMS.Services;
using OrderAccounting.Common.Infrastructure.Enums;
using OrderAccounting.Common.Infrastructure.Events;
using OrderAccounting.Common.Infrastructure.Events.EditEvents;
using OrderAccounting.Common.Infrastructure.Interfaces;
using OrderAccounting.Common.Repository.Interfaces;
using OrderAccounting.Modules.Edit.Navigation.Views;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace OrderAccounting.Modules.Edit.Navigation.ViewModels
{
    public class NavigationViewModel : BindableBase, INavigationViewModel
    {
        #region Variables

        private INavigationView _parentView;

        private IEventAggregator _eventAggregator;

        private IDirectoryManager _directoryManager;

        private IOrderRepository _orderRepository;

        private bool? _canBeCanceled;

        private bool _isActive;

        #endregion

        #region Properties

        private ObservableCollection<NavigationModel> _operationTypesDataSource;
        public ObservableCollection<NavigationModel> OperationTypesDataSource
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

        #region Commands

        public RellayCommand HideCommand { get; set; }

        public RellayCommand AcceptCommand { get; set; }

        public RellayCommand FinishCommand { get; set; }

        public RellayCommand CancelCommand { get; set; }

        public RellayCommand AddAdditionalCommand { get; set; }

        #endregion

        #region Initialization

        public NavigationViewModel(IEventAggregator eventAggregator, IDirectoryManager directoryManager, IOrderRepository orderRepository)
        {
            _eventAggregator = eventAggregator;

            _directoryManager = directoryManager;

            _orderRepository = orderRepository;

            InitializeEvents(_eventAggregator);

            InitializeCommands();
        }

        private void InitalizeDataCollection()
        {
            var typesSource = new List<NavigationModel>();

            var basicType = new NavigationModel(_eventAggregator, 0, "Основна", null, true) { Index = 0 };
            typesSource.Add(basicType);

            var manualType = new NavigationModel(_eventAggregator, 1, "Ручна", basicType) { Index = 1 };
            typesSource.Add(manualType);

            var additionalType = new NavigationModel(_eventAggregator, 2, "Допоміжна", basicType) { Index = 2 };
            typesSource.Add(additionalType);

            OperationTypesDataSource = new ObservableCollection<NavigationModel>(typesSource);
        }

        private void InitializeCommands()
        {
            HideCommand = new RellayCommand(HideCommandExecute);
            AcceptCommand = new RellayCommand(AcceptCommandExecute, AcceptCommandCanExecute);
            CancelCommand = new RellayCommand(CancelCommandExecute, CancelCommandCanExecute);
            FinishCommand = new RellayCommand(FinishCommandExecute, FinishCommandCanExecute);
            AddAdditionalCommand = new RellayCommand(AddAdditionalCommandExecute, AddAdditionalCommandCanExecute);
        }

        private void InitializeEvents(IEventAggregator _eventAggregator)
        {
            _eventAggregator.GetEvent<PubSubEvent<DetailEvent>>().Subscribe(DetailTriggered);
            _eventAggregator.GetEvent<PubSubEvent<NavigationProcessEvent>>().Subscribe(NavigationProcessTriggered);
            _eventAggregator.GetEvent<PubSubEvent<NavigationTypeEvent>>().Subscribe(NavigationTypeTriggered);
            _eventAggregator.GetEvent<PubSubEvent<NavigationTypeEvent>>().Subscribe(NavigationTypeTriggered);
        }

        #endregion

        #region Event Triggers

        private void DetailTriggered(DetailEvent obj)
        {
            ChangeNavigationViewState(obj.DetailState, obj.OrderId, obj.OrderCollection);
        }

        private void NavigationProcessTriggered(NavigationProcessEvent obj)
        {
            SetProcessValue(obj.Processable);
        }

        private void NavigationTypeTriggered(NavigationTypeEvent obj)
        {
            ChangeNavigationType(obj.TypeId);
        }

        #endregion

        #region Methods

        public void SetParentView(INavigationView parentView)
        {
            _parentView = parentView;
        }

        /// <summary>
        /// Change navigation type bu input TypeId
        /// </summary>
        private void ChangeNavigationType(int typeId)
        {
            var currentType = OperationTypesDataSource.FirstOrDefault(x => x.IsChecked);
            if (currentType != null && currentType.Process == 100)
            {
                currentType = OperationTypesDataSource.FirstOrDefault(x => x.Id == typeId);
                if (currentType != null)
                {
                    currentType.IsChecked = true;
                }
            }
        }

        /// <summary>
        /// Set process state from input IProcessable
        /// </summary>
        private void SetProcessValue(IProcessable processable)
        {
            if (OperationTypesDataSource != null)
            {
                var currentType = OperationTypesDataSource.FirstOrDefault(x => x.Index == processable.Index);

                if (currentType != null)
                {
                    currentType.Process = processable.GetProcessed();
                }
            }
        }

        /// <summary>
        /// Initialize or deinitialize Editor getting editorState
        /// </summary>
        private void ChangeNavigationViewState(DetailState summaryState, int? orderId, List<IOrderViewModel> orders)
        {
            if (summaryState == DetailState.Create)
            {
                IsEditable = true;

                InitalizeDataCollection();

                if (OperationTypesDataSource != null)
                {
                    foreach (var i in OperationTypesDataSource)
                    {
                        i.Reset(IsEditable);
                    }
                }

                OnShowView();
            }
            else if (summaryState == DetailState.Edit && orderId.HasValue)
            {
                IsEditable = true;

                if (orders != null && orders.Count > 0)
                {
                    var typesSource = new List<NavigationModel>();

                    int index = 0;

                    foreach (var i in orders)
                    {
                        var basicType = new NavigationModel(_eventAggregator, i.Type, i.ToString(), null, i.Type == 0) { Process = 100, Index = index };
                        typesSource.Add(basicType);
                        index++;
                    }

                    if (typesSource.FirstOrDefault(x => x.Id == 1) == null)
                    {
                        var manualType = new NavigationModel(_eventAggregator, 1, "Ручна", typesSource.FirstOrDefault()) { Index = typesSource.Count };
                        typesSource.Add(manualType);
                    }

                    if (typesSource.FirstOrDefault(x => x.Id == 2) == null)
                    {
                        var additionalType = new NavigationModel(_eventAggregator, 2, "Допоміжна", typesSource.FirstOrDefault()) { Index = typesSource.Count };
                        typesSource.Add(additionalType);
                    }

                    OperationTypesDataSource = new ObservableCollection<NavigationModel>(typesSource);

                    var items = OperationTypesDataSource.Where(x => orders.Select(y => y.Type).Contains(x.Id));
                    if (items != null && items.Count() > 0)
                    {
                        foreach (var i in items)
                        {
                            i.Process = 100;
                        }
                    }
                }

                OnShowView();
            }
            else if (summaryState == DetailState.View && orderId.HasValue)
            {
                IsEditable = false;

                if (orders != null && orders.Count > 0)
                {
                    var typesSource = new List<NavigationModel>();

                    int index = 0;

                    foreach (var i in orders)
                    {
                        var basicType = new NavigationModel(_eventAggregator, i.Type, i.ToString(), null, i.Type == 0) { Process = 100, Index = index };
                        typesSource.Add(basicType);
                        index++;
                    }

                    OperationTypesDataSource = new ObservableCollection<NavigationModel>(typesSource);

                    var items = OperationTypesDataSource.Where(x => orders.Select(y => y.Type).Contains(x.Id));
                    if (items != null && items.Count() > 0)
                    {
                        foreach (var i in items)
                        {
                            i.Process = 100;
                        }
                    }

                    var status = _directoryManager.LaborStatus.FirstOrDefault(x => x.Identity == orders.FirstOrDefault().StatusId);
                    if (status != null)
                    {
                        _isActive = status.Name == "active";
                    }

                }

                OnShowView();

                _canBeCanceled = IsOrderCanBeCanceled(orderId.Value);
            }
            else if (summaryState == DetailState.Hide)
            {
                OnHideView();
            }

            if (OperationTypesDataSource != null)
            {
                var currentType = OperationTypesDataSource.FirstOrDefault();
                if (currentType != null)
                {
                    currentType.IsChecked = true;
                }
            }

        }

        private bool? IsOrderCanBeCanceled(int orderId)
        {
            return _orderRepository.IsOrderCanBeCanceled(orderId, TaskClient.UrlData.DatabaseKey.ToString());
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

        #region Command Executors

        private void HideCommandExecute(object obj)
        {
            if (IsEditable)
            {
                var result = MessageBox.Show("Ви дійсно бажаєте скасувати опрацювання наряду без збереження? Всі дані будуть втрачені!", "Скасування!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);
                if (result != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            _eventAggregator.GetEvent<PubSubEvent<DetailEvent>>().Publish(new DetailEvent(DetailState.Hide));

            OnHideView();
        }

        private void AcceptCommandExecute(object obj)
        {
            _eventAggregator.GetEvent<PubSubEvent<SaveEvent>>().Publish(new SaveEvent());
        }
        private bool AcceptCommandCanExecute(object obj)
        {
            var canExecute = OperationTypesDataSource != null && OperationTypesDataSource.Count > 0 && OperationTypesDataSource.Where(x => x.Process != 100 && x.Process != 0).Count() == 0 &&
                OperationTypesDataSource.FirstOrDefault(x => x.Process == 100) != null;

            return canExecute;
        }

        private void FinishCommandExecute(object obj)
        {
            var result = MessageBox.Show("Ви дійсно бажаєте завершити дію наряду? В подальшому дані будуть не доступні для редагування!", "Завершення!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
            {
                _eventAggregator.GetEvent<PubSubEvent<ChangeStatusEvent>>().Publish(new ChangeStatusEvent(_directoryManager.LaborStatus.FirstOrDefault(x => x.Name == "finish").Identity));
            }
        }
        private bool FinishCommandCanExecute(object obj)
        {
            return _isActive && _canBeCanceled.HasValue && !_canBeCanceled.Value;
        }

        private void CancelCommandExecute(object obj)
        {
            var result = MessageBox.Show("Ви дійсно бажаєте скасувати дію наряду?", "Скасування!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
            {
                _eventAggregator.GetEvent<PubSubEvent<ChangeStatusEvent>>().Publish(new ChangeStatusEvent(_directoryManager.LaborStatus.FirstOrDefault(x => x.Name == "cancel").Identity));
            }
        }
        private bool CancelCommandCanExecute(object obj)
        {
            return _isActive && _canBeCanceled.HasValue && _canBeCanceled.Value;
        }

        private void AddAdditionalCommandExecute(object obj)
        {
            var name = string.Format("Допоміжна {0}", OperationTypesDataSource.Count - 2);
            var index = OperationTypesDataSource.Count;

            var item = new NavigationModel(_eventAggregator, 2, name, OperationTypesDataSource.FirstOrDefault(), true) { Index = index };
            item.Reset(IsEditable);

            OperationTypesDataSource.All(x => x.IsChecked = false);

            OperationTypesDataSource.Add(item);

            _eventAggregator.GetEvent<PubSubEvent<AdditionalEvent>>().Publish(new AdditionalEvent(2, index));

        }
        private bool AddAdditionalCommandCanExecute(object obj)
        {
            if (_isEditable && OperationTypesDataSource != null && OperationTypesDataSource.Count > 0)
            {
                return OperationTypesDataSource.Where(x => x.Id != 1).All(x => x.Process == 100);
            }

            return false;
        }

        #endregion

    }
}
