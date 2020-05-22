using FMS.Services.NetTcpDirectoryServiceReference;
using FMS.DataManagers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrderAccounting.Common.Infrastructure.ViewModels.OrderItemViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAccounting.Common.Infrastructure.Test.ViewModels
{
    [TestClass]
    public class OperationItemViewModelTest
    {
        private OperationItemViewModel _baseOperationItemViewModel;

        public OperationItemViewModelTest()
        {
            var directoryManager = Mock.Of<IDirectoryManager>(x => x.OperationsInformation == new List<OperationsNetTcp>());

            _baseOperationItemViewModel = new OperationItemViewModel(directoryManager);
        }

        [TestMethod]
        public void IsDataValid_BaseOperationNotNull_True()
        {
            _baseOperationItemViewModel.BaseOperationId = 0;

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
