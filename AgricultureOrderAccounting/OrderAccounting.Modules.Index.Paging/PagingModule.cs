using OrderAccounting.Modules.Index.Paging.ViewModels;
using OrderAccounting.Modules.Index.Paging.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace OrderAccounting.Modules.Index.Paging
{
    public class PagingModule : IModule
    {
        #region Variables

        private readonly IRegionManager _regionManager;

        #endregion

        #region Initialization

        public PagingModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion("PagingRegion", typeof(IPagingView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IPagingView, PagingView>();
            containerRegistry.Register<IPagingViewModel, PagingViewModel>();
        }

        #endregion

    }
}
