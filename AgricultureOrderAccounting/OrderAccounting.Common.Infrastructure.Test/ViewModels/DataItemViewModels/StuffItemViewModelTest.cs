using FMS.DataManagers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrderAccounting.Common.Infrastructure.ViewModels.OrderItemViewModels;

namespace OrderAccounting.Common.Infrastructure.Test.ViewModels
{
    [TestClass]
    public class StuffItemViewModelTest
    {
        private StuffItemViewModel _employeeItemViewModel;
        private IDirectoryManager _directoryManager = Mock.Of<IDirectoryManager>();

        public StuffItemViewModelTest()
        {
            _employeeItemViewModel = new StuffItemViewModel(_directoryManager);
        }

    }
}
