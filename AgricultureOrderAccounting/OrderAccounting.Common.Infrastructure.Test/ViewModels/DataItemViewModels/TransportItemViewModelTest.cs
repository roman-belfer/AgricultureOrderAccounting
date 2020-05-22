using FMS.DataManagers.Interfaces;
using FMS.DataManagers.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrderAccounting.Common.Infrastructure.Models;
using OrderAccounting.Common.Infrastructure.ViewModels.OrderItemViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OrderAccounting.Common.Infrastructure.Test.ViewModels
{
    [TestClass]
    public class TransportItemViewModelTest
    {
        private TransportItemViewModel _transportItemViewModel;
        private IDirectoryManager _directoryManager = Mock.Of<IDirectoryManager>();

        public TransportItemViewModelTest()
        {
            _transportItemViewModel = new TransportItemViewModel(_directoryManager);
        }

        [TestMethod]
        public void IsDataValid_TransportCollectionIsEmpty_False()
        {
            _transportItemViewModel.TransportCollection = new ObservableCollection<TransportModel>();

            var result = _transportItemViewModel.IsDataValid();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDataValid_TransportCollectionIsValid_True()
        {
            var item = new TransportModel()
            {
                HasAggregate = true,
                DriverId = 1,
                Speed = 10,
                Width = 10,
                VehicleId = 1,
                UnitId = 1
            };

            _transportItemViewModel.TransportCollection = new ObservableCollection<TransportModel>() { item };

            var result = _transportItemViewModel.IsDataValid();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDataValid_TransportCollectionWithoutSpeed_False()
        {
            var item = new TransportModel()
            {
                HasAggregate = true,
                DriverId = 1,
                Width = 10,
                VehicleId = 1,
                UnitId = 1
            };

            _transportItemViewModel.TransportCollection = new ObservableCollection<TransportModel>() { item };

            var result = _transportItemViewModel.IsDataValid();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDataValid_TransportCollectionWithoutUnit_True()
        {
            var item = new TransportModel()
            {
                HasAggregate = false,
                DriverId = 1,
                Speed = 10,
                VehicleId = 1
            };

            _transportItemViewModel.TransportCollection = new ObservableCollection<TransportModel>() { item };

            var result = _transportItemViewModel.IsDataValid();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDataValid_TransportCollectionWithoutDriver_False()
        {
            var item = new TransportModel()
            {
                Speed = 10,
                VehicleId = 1
            };

            _transportItemViewModel.TransportCollection = new ObservableCollection<TransportModel>() { item };

            var result = _transportItemViewModel.IsDataValid();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDataValid_TransportCollectionWithoutWidth_False()
        {
            var item = new TransportModel()
            {
                HasAggregate = true,
                DriverId = 1,
                Speed = 10,
                VehicleId = 1,
                UnitId = 1
            };

            _transportItemViewModel.TransportCollection = new ObservableCollection<TransportModel>() { item };

            var result = _transportItemViewModel.IsDataValid();

            Assert.IsFalse(result);
        }

    }
}
