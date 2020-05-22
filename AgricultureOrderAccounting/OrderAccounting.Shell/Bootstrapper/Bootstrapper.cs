using FMS.DataManagers;
using FMS.DataManagers.Interfaces;
using OrderAccounting.Common.Infrastructure.Factories;
using OrderAccounting.Common.Infrastructure.Interfaces;
using OrderAccounting.Common.Infrastructure.Models;
using OrderAccounting.Common.Repository.Interfaces;
using OrderAccounting.Common.Repository.Repositories;
using OrderAccounting.Common.SignalRWorker.Interfaces;
using OrderAccounting.Common.SignalRWorker.Workers;
using OrderAccounting.Modules.Edit.Navigation;
using OrderAccounting.Modules.Edit.Summary;
using OrderAccounting.Modules.Editor;
using OrderAccounting.Modules.Filter;
using OrderAccounting.Modules.Index.Menu;
using OrderAccounting.Modules.Index.OrderList;
using OrderAccounting.Modules.Index.Paging;
using OrderAccounting.Modules.Loader;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;

namespace OrderAccounting.Shell
{
    public class Bootstrapper : PrismApplication
    {
        #region Variables

        private readonly DataBaseDirectoriesManager _directoryManager;

        private Shell _shellView;

        #endregion

        #region Initialization

        public Bootstrapper(DataBaseDirectoriesManager directoryManager)
        {
            _directoryManager = directoryManager;
        }

        #endregion

        #region Methods

        protected override Window CreateShell()
        {
            _shellView = new Shell();

            _shellView.Show();

            return _shellView;
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            var catalog = new ModuleCatalog();
            
            catalog.AddModule(typeof(MenuModule));
            catalog.AddModule(typeof(OrderListModule));
            catalog.AddModule(typeof(PagingModule));
            catalog.AddModule(typeof(FilterModule));
            catalog.AddModule(typeof(LoaderModule));

            catalog.AddModule(typeof(NavigationModule));
            catalog.AddModule(typeof(EditorModule));
            catalog.AddModule(typeof(SummaryModule));

            return catalog;
        }

        public Shell GetShell()
        {
            return _shellView;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IDirectoryManager>(_directoryManager);

            containerRegistry.Register<IOrderItemFactory, OrderItemFactory>();
            containerRegistry.Register<IOrderFactory, OrderFactory>();
            containerRegistry.Register<ISignalRWorker, OrderSignalRWorker>();
            containerRegistry.Register<IOrderRepository, OrderApiRepository>();
            containerRegistry.Register<IFilterModel, FilterModel>();
        }

        #endregion

    }
}
