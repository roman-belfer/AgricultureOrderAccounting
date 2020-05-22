using OrderAccounting.Modules.Index.Menu.ViewModels;
using OrderAccounting.Modules.Index.Menu.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace OrderAccounting.Modules.Index.Menu
{
    public class MenuModule : IModule
    {
        #region Variables

        private readonly IRegionManager _regionManager;

        #endregion

        #region Initialization

        public MenuModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion("MenuRegion", typeof(IMenuView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IMenuView, MenuView>();
            containerRegistry.Register<IMenuViewModel, MenuViewModel>();
        }

        #endregion

    }
}
