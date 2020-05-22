using OrderAccounting.Modules.Edit.Navigation.ViewModels;
using OrderAccounting.Modules.Edit.Navigation.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace OrderAccounting.Modules.Edit.Navigation
{
    public class NavigationModule : IModule
    {
        #region Variables

        private readonly IRegionManager _regionManager;

        #endregion

        #region Initialization

        public NavigationModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion("NavigationRegion", typeof(INavigationView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<INavigationView, NavigationView>();
            containerRegistry.Register<INavigationViewModel, NavigationViewModel>();
        }

        #endregion

    }
}
