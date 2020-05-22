using FMS.Services.NetTcpDirectoryServiceReference;
using FMS.DataManagers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrderAccounting.Common.Infrastructure.ViewModels.OrderItemViewModels;
using System;
using System.Collections.Generic;
using Argo.DataAccess.All.Models;

namespace OrderAccounting.Common.Infrastructure.Test.ViewModels
{
    [TestClass]
    public class DetailItemViewModelTest
    {
        private DetailItemViewModel _detailItemViewModel;

        public DetailItemViewModelTest()
        {
            var directoryManager = Mock.Of<IDirectoryManager>(x => x.Operations == new List<Operation.ListItem>());

            _detailItemViewModel = new DetailItemViewModel(directoryManager);
        }

        [TestMethod]
        public void IsDataValid_DataIsValid_True()
        {
            _detailItemViewModel.ActualPhaseId = 0;
            _detailItemViewModel.OperationTypeId = 0;
            _detailItemViewModel.DateFrom = DateTime.Now;
            _detailItemViewModel.DateTo = DateTime.Now.AddDays(1);

            var result = _detailItemViewModel.IsDataValid();

            Assert.IsTrue(result);
        }
        
        //Test methods IsDataValid_ActualPhaseIsNull_True and IsDataValid_OperationTypeIsNull_True
        //should be uncommented when the ActualPhases and Operation Types will be added to system as dictionaries
        //and showed on view

        //[TestMethod]
        //public void IsDataValid_ActualPhaseIsNull_True()
        //{
        //    _detailItemViewModel.OperationTypeId = 0;
        //    _detailItemViewModel.DateFrom = DateTime.Now;
        //    _detailItemViewModel.DateTo = DateTime.Now.AddDays(1);

        //    var result = _detailItemViewModel.IsDataValid();

        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void IsDataValid_OperationTypeIsNull_True()
        //{
        //    _detailItemViewModel.ActualPhaseId = 0;
        //    _detailItemViewModel.DateFrom = DateTime.Now;
        //    _detailItemViewModel.DateTo = DateTime.Now.AddDays(1);

        //    var result = _detailItemViewModel.IsDataValid();

        //    Assert.IsTrue(result);
        //}

        [TestMethod]
        public void IsDataValid_DateIsNotValid_False()
        {
            _detailItemViewModel.ActualPhaseId = 0;
            _detailItemViewModel.OperationTypeId = 0;
            _detailItemViewModel.DateFrom = DateTime.Now;
            _detailItemViewModel.DateTo = DateTime.Now.AddDays(-1);

            var result = _detailItemViewModel.IsDataValid();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDataValid_DateToIsEmpty_False()
        {
            _detailItemViewModel.ActualPhaseId = 0;
            _detailItemViewModel.OperationTypeId = 0;
            _detailItemViewModel.DateFrom = DateTime.Now;
            _detailItemViewModel.DateTo = null;

            var result = _detailItemViewModel.IsDataValid();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDataValid_DateFromIsEmpty_False()
        {
            _detailItemViewModel.ActualPhaseId = 0;
            _detailItemViewModel.OperationTypeId = 0;
            _detailItemViewModel.DateFrom = null;
            _detailItemViewModel.DateTo = DateTime.Now;

            var result = _detailItemViewModel.IsDataValid();

            Assert.IsFalse(result);
        }
    }
}
