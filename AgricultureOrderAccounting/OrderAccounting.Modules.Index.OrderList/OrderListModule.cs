using OrderAccounting.Modules.Index.OrderList.ViewModels;
using OrderAccounting.Modules.Index.OrderList.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace OrderAccounting.Modules.Index.OrderList
{
    public class OrderListModule : IModule
    {
        #region Variables

        private readonly IRegionManager _regionManager;

        #endregion

        #region Initialization

        public OrderListModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion("OrderListRegion", typeof(IOrderListView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IOrderListView, OrderListView>();
            containerRegistry.Register<IOrderListViewModel, OrderListViewModel>();
        }

        #endregion

    }
}
