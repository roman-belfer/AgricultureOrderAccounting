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
    public class BasicWorkViewModelTest
    {
        private IDirectoryManager _directoryManager = Mock.Of<IDirectoryManager>();
        private IOrderItemFactory _orderItemFactory = Mock.Of<IOrderItemFactory>();
        private BasicWorkViewModel _basicWorkViewModel;

        [TestMethod]
        public void BasicWorkViewModel_ValidDataItemCount_True()
        {
            _basicWorkViewModel = new BasicWorkViewModel(_directoryManager, _orderItemFactory);

            var itemsCount = _basicWorkViewModel.DataCollection.Count;

            Assert.AreEqual(4, itemsCount);
        }

        [TestMethod]
        public void BasicWorkViewModel_DetailItemExist_True()
        {
            _basicWorkViewModel = new BasicWorkViewModel(_directoryManager, _orderItemFactory);

            var employeeItem = _basicWorkViewModel.DataCollection.FirstOrDefault(x => x is DetailItemViewModel);

            Assert.IsNotNull(employeeItem);
        }

        [TestMethod]
        public void BasicWorkViewModel_OperationItemExist_True()
        {
            _basicWorkViewModel = new BasicWorkViewModel(_directoryManager, _orderItemFactory);

            var employeeItem = _basicWorkViewModel.DataCollection.FirstOrDefault(x => x is OperationItemViewModel);

            Assert.IsNotNull(employeeItem);
        }

        //[TestMethod]
        //public void BasicWorkViewModel_StuffItemExist_True()
        //{
        //    _basicWorkViewModel = new BasicWorkViewModel<BasicWorkModel>(_directoryManager);

        //    var employeeItem = _basicWorkViewModel.DataCollection.FirstOrDefault(x => x is StuffItemViewModel);

        //    Assert.IsNotNull(employeeItem);
        //}

        [TestMethod]
        public void BasicWorkViewModel_TransportItemExist_True()
        {
            _basicWorkViewModel = new BasicWorkViewModel(_directoryManager, _orderItemFactory);

            var employeeItem = _basicWorkViewModel.DataCollection.FirstOrDefault(x => x is TransportItemViewModel);

            Assert.IsNotNull(employeeItem);
        }

        [TestMethod]
        public void BasicWorkViewModel_ObjectItemExist_True()
        {
            _basicWorkViewModel = new BasicWorkViewModel(_directoryManager, _orderItemFactory);

            var objectItem = _basicWorkViewModel.DataCollection.FirstOrDefault(x => x is ObjectItemViewModel);

            Assert.IsNotNull(objectItem);
        }
    }
}