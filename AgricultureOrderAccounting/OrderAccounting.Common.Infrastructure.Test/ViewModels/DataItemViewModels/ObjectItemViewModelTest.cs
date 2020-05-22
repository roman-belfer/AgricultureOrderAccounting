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
    public class ObjectItemViewModelTest
    {
        private ObjectItemViewModel _employeeItemViewModel;
        private IDirectoryManager _directoryManager = Mock.Of<IDirectoryManager>();

        public ObjectItemViewModelTest()
        {
            _employeeItemViewModel = new ObjectItemViewModel(_directoryManager);
        }

    }
}
