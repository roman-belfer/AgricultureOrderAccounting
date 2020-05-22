using FMS.DataManagers.Interfaces;
using FMS.Services.NetTcpDirectoryServiceReference;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrderAccounting.Common.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace OrderAccounting.Common.Infrastructure.Test.Models
{
    [TestClass]
    public class FilterModelTest
    {
        private FilterModel _filterModel = new Mock<FilterModel>().Object;

        private OrderModel _basicModel;

        public FilterModelTest()
        {
            var directoryManager = Mock.Of<IDirectoryManager>(x => x.OperationsInformation == new List<OperationsNetTcp>());

            _basicModel = new OrderModel(directoryManager);
        }

        [TestMethod]
        public void IsFilterNotEmpty_FilterDataEmpty_False()
        {
            var false_result = _filterModel.IsFilterNotEmpty();

            Assert.IsFalse(false_result);
        }

        [TestMethod]
        public void IsFilterNotEmpty_FilterOperationIdSet_True()
        {
            _filterModel.OperationId = 10;

            var true_result = _filterModel.IsFilterNotEmpty();

            Assert.IsTrue(true_result);
        }

        [TestMethod]
        public void IsFilterNotEmpty_InvalidDate_False()
        {
            _filterModel.DateFrom = DateTime.Now;
            _filterModel.DateTo = DateTime.Now.AddDays(-1);

            var false_result = _filterModel.IsFilterNotEmpty();

            Assert.IsFalse(false_result);
        }

        [TestMethod]
        public void IsFilterNotEmpty_ValidDate_True()
        {
            _filterModel.DateFrom = DateTime.Now.AddDays(-1);
            _filterModel.DateTo = DateTime.Now;

            var true_result = _filterModel.IsFilterNotEmpty();

            Assert.IsTrue(true_result);
        }

        [TestMethod]
        public void CheckFilter_ValidBasicModel_True()
        {
            _filterModel.OperationId = 10;
            _basicModel.OperationId = 10;

            var true_result = _filterModel.CheckFilter(_basicModel);

            Assert.IsTrue(true_result);
        }

        [TestMethod]
        public void CheckFilter_InvalidBasicModel_False()
        {
            _filterModel.OperationId = 10;

            var false_result = _filterModel.CheckFilter(_basicModel);

            Assert.IsFalse(false_result);
        }

        [TestMethod]
        public void Reset_True()
        {
            _filterModel.OperationId = 10;
            _filterModel.ActualPhaseId = 1;
            _filterModel.BaseOperationId = 2;
            _filterModel.DateFrom = DateTime.Now;
            _filterModel.DateTo = DateTime.Now;
            _filterModel.OperationTypeId = 9;
            _filterModel.UnitId = 5;
            _filterModel.VehicleId = 9;

            _filterModel.Reset();

            Assert.IsNull(_filterModel.OperationId);
            Assert.IsNull(_filterModel.ActualPhaseId);
            Assert.IsNull(_filterModel.BaseOperationId);
            Assert.IsNull(_filterModel.DateFrom);
            Assert.IsNull(_filterModel.DateTo);
            Assert.IsNull(_filterModel.OperationTypeId);
            Assert.IsNull(_filterModel.UnitId);
            Assert.IsNull(_filterModel.VehicleId);
        }
    }
}
