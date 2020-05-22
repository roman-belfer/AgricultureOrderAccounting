using OrderAccounting.Modules.Edit.Summary.ViewModels;
using OrderAccounting.Modules.Edit.Summary.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace OrderAccounting.Modules.Edit.Summary
{
    public class SummaryModule : IModule
    {
        #region Variables

        private IRegionManager _regionManager;

        #endregion

        #region Initialization

        public SummaryModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion("SummaryRegion", typeof(ISummaryView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ISummaryView, SummaryView>();
            containerRegistry.Register<ISummaryViewModel, SummaryViewModel>();
        }

        #endregion

    }
}
