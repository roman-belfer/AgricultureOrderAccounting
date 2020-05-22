using FMS.DataManagers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrderAccounting.Common.Infrastructure.Factories;
using OrderAccounting.Common.Infrastructure.ViewModels.OrderItemViewModels;
using OrderAccounting.Common.Infrastructure.ViewModels.OrderViewModels;
using System.Linq;

namespace OrderAccounting.Common.Infrastructure.Test.ViewModels.WorkViewModels
{
    [TestClass]
    public class ManualWorkViewModelTest
    {
        private IDirectoryManager _directoryManager = Mock.Of<IDirectoryManager>();
        private IOrderItemFactory _orderItemFactory = Mock.Of<IOrderItemFactory>();
        private ManualWorkViewModel _manualWorkViewModel;
        
        [TestMethod]
        public void ManualWorkViewModel_ValidDataItemCount_True()
        {
            _manualWorkViewModel = new ManualWorkViewModel(_directoryManager, _orderItemFactory);

            var itemsCount = _manualWorkViewModel.DataCollection.Count;

            Assert.AreEqual(2, itemsCount);
        }

        [TestMethod]
        public void ManualWorkViewModel_EmployeeItemExist_True()
        {
            _manualWorkViewModel = new ManualWorkViewModel(_directoryManager, _orderItemFactory);

            var employeeItem = _manualWorkViewModel.DataCollection.FirstOrDefault(x => x is EmployeeItemViewModel);

            Assert.IsNotNull(employeeItem);
        }

        [TestMethod]
        public void ManualWorkViewModel_BaseOperationItemExist_True()
        {
            _manualWorkViewModel = new ManualWorkViewModel(_directoryManager, _orderItemFactory);

            var employeeItem = _manualWorkViewModel.DataCollection.FirstOrDefault(x => x is ManualOperationItemViewModel);

            Assert.IsNotNull(employeeItem);
        }
    }
}
