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
    public class AdditionalWorkViewModelTest
    {
        private IDirectoryManager _directoryManager = Mock.Of<IDirectoryManager>();
        private IOrderItemFactory _orderItemFactory = Mock.Of<IOrderItemFactory>();
        private AdditionalWorkViewModel _workViewModel;

        [TestMethod]
        public void AdditionalWorkViewModel_ValidDataItemCount_True()
        {
            _workViewModel = new AdditionalWorkViewModel(_directoryManager, _orderItemFactory);

            var itemsCount = _workViewModel.DataCollection.Count;

            Assert.AreEqual(3, itemsCount);
        }

        [TestMethod]
        public void AdditionalWorkViewModel_OperationItemExist_True()
        {
            _workViewModel = new AdditionalWorkViewModel(_directoryManager, _orderItemFactory);

            var employeeItem = _workViewModel.DataCollection.FirstOrDefault(x => x is OperationItemViewModel);

            Assert.IsNotNull(employeeItem);
        }

        //[TestMethod]
        //public void ManualWorkViewModel_StuffItemExist_True()
        //{
        //    _basicWorkViewModel = new BasicWorkViewModel(_directoryManager);

        //    var employeeItem = _basicWorkViewModel.DataCollection.FirstOrDefault(x => x is StuffItemViewModel);

        //    Assert.IsNotNull(employeeItem);
        //}

        [TestMethod]
        public void AdditionalWorkViewModel_TransportItemExist_True()
        {
            _workViewModel = new AdditionalWorkViewModel(_directoryManager, _orderItemFactory);

            var employeeItem = _workViewModel.DataCollection.FirstOrDefault(x => x is TransportItemViewModel);

            Assert.IsNotNull(employeeItem);
        }

        [TestMethod]
        public void AdditionalWorkViewModel_ObjectItemExist_True()
        {
            _workViewModel = new AdditionalWorkViewModel(_directoryManager, _orderItemFactory);

            var objectItem = _workViewModel.DataCollection.FirstOrDefault(x => x is ObjectItemViewModel);

            Assert.IsNotNull(objectItem);
        }
    }
}