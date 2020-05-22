using OrderAccounting.Modules.Filter.ViewModels;
using OrderAccounting.Modules.Filter.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace OrderAccounting.Modules.Filter
{
    public class FilterModule : IModule
    {
        #region Variables

        private IRegionManager _regionManager;

        #endregion

        #region Initialization

        public FilterModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion("FilterRegion", typeof(IFilterView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IFilterView, FilterView>();
            containerRegistry.Register<IFilterViewModel, FilterViewModel>();
        }

        #endregion

    }
}
