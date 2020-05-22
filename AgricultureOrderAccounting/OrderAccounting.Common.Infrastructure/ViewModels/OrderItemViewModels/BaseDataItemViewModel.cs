using FMS.Core;
using FMS.DataManagers.Interfaces;
using OrderAccounting.Common.Infrastructure.Interfaces;
using Prism.Mvvm;
using System;
using System.Windows;

namespace OrderAccounting.Common.Infrastructure.ViewModels.OrderItemViewModels
{
    public abstract class BaseDataItemViewModel : BindableBase, IOrderItemViewModel
    {
        #region Variables 

        protected IDirectoryManager _directoryManager;

        protected IOrderItemViewModel _nextData;

        #endregion

        #region Properties

        protected IOrderItemViewModel _previousData;
        public IOrderItemViewModel PreviousData
        {
            get { return _previousData; }
            set
            {
                if (_previousData != value)
                {
                    _previousData = value;
                    RaisePropertyChanged("PreviousData");
                }
            }
        }

        protected bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    RaisePropertyChanged("IsChecked");
                }
            }
        }

        protected bool _isValid;
        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    RaisePropertyChanged("IsValid");
                }
            }
        }

        protected string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    RaisePropertyChanged("Title");
                }
            }
        }

        private string _year;
        public string Year
        {
            get { return _year; }
            set
            {
                if (_year != value)
                {
                    _year = value;

                    if (_nextData != null)
                    {
                        _nextData.Year = _year;
                    }

                    RaisePropertyChanged("Year");
                }
            }
        }

        #endregion

        #region Commands

        public RellayCommand CommitCommand { get; set; }

        public RellayCommand ReturnCommand { get; set; }

        public RellayCommand AddCommand { get; set; }

        public RellayCommand RemoveCommand { get; set; }

        #endregion

        #region Events

        public event EventHandler DataChanged;

        #endregion

        #region Initialization

        public BaseDataItemViewModel(IDirectoryManager directoryManager)
        {
            _directoryManager = directoryManager;

            InitializeCommands();
        }

        protected virtual void InitializeCommands()
        {
            CommitCommand = new RellayCommand(CommitCommandExecute, CommitCommandCanExecute);
            ReturnCommand = new RellayCommand(ReturnCommandExecute, ReturnCommandCanExecute);
            AddCommand = new RellayCommand(AddCommandExecute, AddCommandCanExecute);
            RemoveCommand = new RellayCommand(RemoveCommandExecute);
        }

        #endregion

        #region Methods

        protected void OnDataChanged()
        {
            if (DataChanged != null)
            {
                DataChanged.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Show message box and returns if result YES
        /// </summary>
        protected bool IsRemoveCommited()
        {
            var result = MessageBox.Show("Ви дійсно бажаєте видалити елемент?", "Видалення!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);

            return result == MessageBoxResult.Yes;
        }

        public void SetPreviousData(IOrderItemViewModel previousData)
        {
            _previousData = previousData;

            Year = _previousData.Year;
        }

        public void SetNextData(IOrderItemViewModel nextData)
        {
            _nextData = nextData;
        }

        public abstract bool IsDataValid();

        public abstract int? GetOperationId();

        public abstract int? GetBaseOperationId();

        public abstract int? GetPhaseId();

        public abstract int? GetOperationTypeId();

        public abstract void SetObjectData(bool isTransportation);

        public abstract void ConvertDataToDTO<T>(T item) where T : class;

        public abstract void ConvertDataFromDTO<T>(T item) where T : class;

        #endregion

        #region Command Executors

        private void ReturnCommandExecute(object obj)
        {
            IsChecked = false;

            _previousData.IsChecked = true;
        }
        private bool ReturnCommandCanExecute(object obj)
        {
            return _previousData != null;
        }

        protected void CommitCommandExecute(object obj)
        {
            IsChecked = false;

            _nextData.IsChecked = true;
        }
        protected bool CommitCommandCanExecute(object obj)
        {
            return _nextData != null && IsDataValid();
        }

        protected abstract void AddCommandExecute(object obj);
        protected abstract bool AddCommandCanExecute(object obj);

        protected abstract void RemoveCommandExecute(object obj);

        #endregion

    }
}
