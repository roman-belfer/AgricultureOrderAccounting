using Argo.DataAccess.LaborDetail.Model;
using FMS.DataManagers.Interfaces;
using FMS.DataManagers.Models;
using OrderAccounting.Common.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace OrderAccounting.Common.Infrastructure.ViewModels.OrderItemViewModels
{
    public class EmployeeItemViewModel : BaseDataItemViewModel, IOrderItemViewModel
    {
        #region Variables

        private List<LaborDetailEmployee.Item> _LaborDetailEmployee;

        #endregion

        #region Properties

        private ObservableCollection<EmployeeModel> _employeeCollection;
        public ObservableCollection<EmployeeModel> EmployeeCollection
        {
            get { return _employeeCollection; }
            set
            {
                if (value != null)
                {
                    if (_employeeCollection != value)
                    {
                        _employeeCollection = value;
                        _employeeCollection.CollectionChanged += EmployeeCollectionChanged;
                    }
                }
            }
        }

        #endregion

        #region Event Executors

        private void EmployeeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnDataChanged();
        }

        #endregion

        #region Initialization

        public EmployeeItemViewModel(IDirectoryManager directoryManager) : base(directoryManager)
        {
            Title = "Допоміжний персонал";

            EmployeeCollection = new ObservableCollection<EmployeeModel>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns validation bool value if employees data is not empty
        /// </summary>
        public override bool IsDataValid()
        {
            IsValid = false;

            if (EmployeeCollection == null || EmployeeCollection.Count == 0)
            {
                return false;
            }

            foreach (var employee in EmployeeCollection)
            {
                IsValid = employee != null && employee.Id != 0;

                if (!IsValid) break;
            }

            return IsValid;
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

        protected override void RemoveCommandExecute(object obj)
        {
            if (!IsRemoveCommited())
            {
                return;
            }

            var employee = obj as EmployeeModel;

            if (EmployeeCollection != null && EmployeeCollection.Count > 0 && employee != null)
            {
                EmployeeCollection.Remove(employee);
            }
        }

        protected override void AddCommandExecute(object obj)
        {
            var employee = obj as EmployeeModel;

            EmployeeCollection.Add(employee);
        }
        protected override bool AddCommandCanExecute(object obj)
        {
            var employee = obj as EmployeeModel;

            return employee != null && !EmployeeCollection.Contains(employee);
        }

        public override void ConvertDataToDTO<T>(T item)
        {
            if (typeof(T) == typeof(LaborDetail.Item))
            {
                var model = (item as LaborDetail.Item);

                model.Employees = new List<LaborDetailEmployee.Item>();
                foreach (var emp in EmployeeCollection)
                {
                    LaborDetailEmployee.Item laborEmployee = _LaborDetailEmployee != null ? _LaborDetailEmployee.FirstOrDefault(x => x.Employee.Identity == emp.Id) : null;
                    model.Employees.Add(new LaborDetailEmployee.Item()
                    {
                        Identity = laborEmployee != null ? laborEmployee.Identity : 0,
                        Employee = new Argo.DataAccess.All.Models.Employee.ListItem() { Identity = emp.Id }
                    });
                }
            }
        }

        public override void ConvertDataFromDTO<T>(T item)
        {
            if (typeof(T) == typeof(LaborDetail.Item))
            {
                var model = (item as LaborDetail.Item);
                _LaborDetailEmployee = model.Employees.ToList();
                for (int i = 0; i < _LaborDetailEmployee.Count; i++)
                {
                    var employee = _directoryManager.EmployeesCollection.FirstOrDefault(x => x.Id == _LaborDetailEmployee[i].Employee.Identity);
                    if (employee != null)
                    {
                        EmployeeCollection.Add(employee);
                    }
                }
            }
        }

        #endregion

    }
}
