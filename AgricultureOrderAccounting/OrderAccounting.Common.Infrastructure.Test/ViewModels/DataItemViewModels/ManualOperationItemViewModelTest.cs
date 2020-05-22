using FMS.Services.NetTcpDirectoryServiceReference;
using FMS.DataManagers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrderAccounting.Common.Infrastructure.ViewModels.OrderItemViewModels;
using System.Collections.Generic;
using Argo.DataAccess.All.Models;

namespace OrderAccounting.Common.Infrastructure.Test.ViewModels
{
    [TestClass]
    public class ManualOperationItemViewModelTest
    {
        private ManualOperationItemViewModel _baseOperationItemViewModel;

        public ManualOperationItemViewModelTest()
        {
            var directoryManager = Mock.Of<IDirectoryManager>(x => x.OperationsInformation == new List<OperationsNetTcp>());

            _baseOperationItemViewModel = new ManualOperationItemViewModel(directoryManager);
        }

        [TestMethod]
        public void IsDataValid_BaseOperationNotNull_True()
        {
            _baseOperationItemViewModel.ManualOperations = new List<object>() { new ManualOperation.ListItem() };

            var result = _baseOperationItemViewModel.IsDataValid();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDataValid_BaseOperationNull_False()
        {
            var result = _baseOperationItemViewModel.IsDataValid();

            Assert.IsFalse(result);
        }
    }
}
