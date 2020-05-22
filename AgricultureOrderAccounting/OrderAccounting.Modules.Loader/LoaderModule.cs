using OrderAccounting.Modules.Loader.ViewModels;
using OrderAccounting.Modules.Loader.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace OrderAccounting.Modules.Loader
{
    public class LoaderModule : IModule
    {
        #region Variables

        private IRegionManager _regionManager;

        #endregion

        #region Initialization

        public LoaderModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion("LoaderRegion", typeof(ILoaderView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ILoaderView, LoaderView>();
            containerRegistry.Register<ILoaderViewModel, LoaderViewModel>();
        }

        #endregion

    }
}
