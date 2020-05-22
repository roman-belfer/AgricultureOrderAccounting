using Argo.DataAccess.All.Models;
using Argo.DataAccess.LaborDetail.Model;
using FMS.DataManagers.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace OrderAccounting.Common.Infrastructure.ViewModels.OrderItemViewModels
{
    public class ManualOperationItemViewModel : BaseDataItemViewModel
    {
        #region Variables

        private List<LaborDetailManualOperation.Item> _LaborDetailManualOperation;

        #endregion

        #region Properties

        private List<object> _manualOperations;
        public List<object> ManualOperations
        {
            get { return _manualOperations; }
            set
            {
                if (value != null)
                {
                    if (_manualOperations != value)
                    {
                        _manualOperations = value;
                        OnManualOperationsChanged();
                    }
                }
            }
        }


        private string _displayManualOperations;
        public string DisplayManualOperations
        {
            get { return _displayManualOperations; }
            set
            {
                if (value != null)
                {
                    if (_displayManualOperations != value)
                    {
                        _displayManualOperations = value;
                        RaisePropertyChanged("DisplayManualOperations");
                    }
                }
            }
        }

        #endregion

        #region Event Executors

        private void OnManualOperationsChanged()
        {
            OnDataChanged();

            DisplayManualOperations = string.Join(", ", ManualOperations.Select(x => (x as ManualOperation.ListItem).ManOperationName).ToList());
        }

        #endregion

        #region Initializiation

        public ManualOperationItemViewModel(IDirectoryManager directoryManager) : base(directoryManager)
        {
            Title = "Базова ручна операція";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns validation bool value if employees data is not empty
        /// </summary>
        public override bool IsDataValid()
        {
            IsValid = false;

            if (ManualOperations == null || ManualOperations.Count == 0)
            {
                return false;
            }

            foreach (var employee in ManualOperations)
            {
                IsValid = employee is ManualOperation.ListItem;

                if (!IsValid) break;
            }

            return IsValid;
        }

        public override void ConvertDataToDTO<T>(T item)
        {
            if (typeof(T) == typeof(LaborDetail.Item))
            {
                var model = (item as LaborDetail.Item);

                model.ManualOperations = new List<LaborDetailManualOperation.Item>();
                foreach (var manOper in ManualOperations)
                {
                    var oper = manOper as ManualOperation.ListItem;
                    LaborDetailManualOperation.Item laborManualOperation = _LaborDetailManualOperation != null ? _LaborDetailManualOperation.FirstOrDefault(x => x.Operation.Identity == oper.Identity) : null;
                    model.ManualOperations.Add(new LaborDetailManualOperation.Item()
                    {
                        Identity = laborManualOperation != null ? laborManualOperation.Identity : 0,
                        Operation = oper
                    });
                }
            }
        }

        public override void ConvertDataFromDTO<T>(T item)
        {
            if (typeof(T) == typeof(LaborDetail.Item))
            {
                var model = (item as LaborDetail.Item);

                _LaborDetailManualOperation = model.ManualOperations.ToList();
                ManualOperations = new List<object>();
                for (int i = 0; i < _LaborDetailManualOperation.Count; i++)
                {
                    ManualOperations.Add(_LaborDetailManualOperation[i].Operation);
                }
            }
        }

        public override int? GetOperationId()
        {
            return null;
        }

        public override int? GetBaseOperationId()
        {
            return null;
        }

        public override int? GetPhaseId()
        {
            return null;
        }

        public override int? GetOperationTypeId()
        {
            return null;
        }

        public override void SetObjectData(bool isTransportation)
        { }

        #endregion

        #region Command Executors

        protected override void AddCommandExecute(object obj)
        { }
        protected override bool AddCommandCanExecute(object obj)
        {
            return false;
        }

        protected override void RemoveCommandExecute(object obj)
        { }

        #endregion

    }
}
