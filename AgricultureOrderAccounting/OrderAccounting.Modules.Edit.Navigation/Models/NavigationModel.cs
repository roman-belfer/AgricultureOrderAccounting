using FMS.Core;
using OrderAccounting.Common.Infrastructure.Events.EditEvents;
using Prism.Events;
using Prism.Mvvm;

namespace OrderAccounting.Modules.Edit.Navigation
{
    public class NavigationModel : BindableBase
    {
        #region Variables

        private IEventAggregator _eventAggregator;

        private bool _isEditable;

        #endregion

        #region Properties

        public int Id { get; set; }

        public string Title { get; set; }

        public int Index { get; set; }

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }

            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;

                    if (IsChecked)
                    {
                        _eventAggregator.GetEvent<PubSubEvent<EditNavigationEvent>>().Publish(new EditNavigationEvent(Id, Index));
                    }

                    RaisePropertyChanged("IsChecked");
                }
            }
        }

        private int _process = 0;
        public int Process
        {
            get { return _process; }

            set
            {
                if (_process != value)
                {
                    _process = value;
                    RaisePropertyChanged("Process");
                }
            }
        }

        public NavigationModel Parrent { get; set; }

        #endregion

        #region Commands

        public RellayCommand CheckCommand { get; set; }

        #endregion

        #region Initialization

        public NavigationModel(IEventAggregator eventAggregator, int id, string title, NavigationModel parrent = null, bool? isChecked = null)
        {
            _eventAggregator = eventAggregator;

            InitializeCommands();

            Id = id;
            Title = title;
            Parrent = parrent;
            IsChecked = isChecked.HasValue ? isChecked.Value : false;
        }

        private void InitializeCommands()
        {
            CheckCommand = new RellayCommand(CheckCommandExecute, CheckCommandCanExecute);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reset Process to 0 and set editable mode by input isEditable
        /// </summary>
        public void Reset(bool isEditable, bool isExist = false)
        {
            Process = isExist ? 100 : 0;
            _isEditable = isEditable;
        }

        #endregion

        #region Command Executors

        private void CheckCommandExecute(object obj)
        { }

        private bool CheckCommandCanExecute(object obj)
        {
            bool isValid = Process == 100;

            if (_isEditable)
            {
                return (Parrent != null && Parrent.Process == 100) || isValid || IsChecked;
            }
            else
            {
                return true;
            }
        }

        #endregion

    }
}
