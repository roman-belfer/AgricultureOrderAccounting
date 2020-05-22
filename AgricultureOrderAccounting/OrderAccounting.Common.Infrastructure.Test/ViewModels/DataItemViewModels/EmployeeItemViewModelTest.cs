using FMS.DataManagers.Interfaces;
using FMS.DataManagers.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrderAccounting.Common.Infrastructure.ViewModels.OrderItemViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OrderAccounting.Common.Infrastructure.Test.ViewModels
{
    [TestClass]
    public class EmployeeItemViewModelTest
    {
        private EmployeeItemViewModel _employeeItemViewModel;
        private IDirectoryManager _directoryManager = Mock.Of<IDirectoryManager>(x => x.EmployeesCollection == new List<EmployeeModel>() { new EmployeeModel() { Id = 1} });

        public EmployeeItemViewModelTest()
        {
            _employeeItemViewModel = new EmployeeItemViewModel(_directoryManager);
        }

        [TestMethod]
        public void IsDataValid_EmployeeCollectionIsEmpty_False()
        {
            _employeeItemViewModel.EmployeeCollection = new ObservableCollection<EmployeeModel>();

            var result = _employeeItemViewModel.IsDataValid();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDataValid_EmployeeCollectionIsValid_True()
        {
            _employeeItemViewModel.EmployeeCollection = new ObservableCollection<EmployeeModel>() { new EmployeeModel() { Id = 1} };

            var result = _employeeItemViewModel.IsDataValid();

            Assert.IsTrue(result);
        }

    }
}
