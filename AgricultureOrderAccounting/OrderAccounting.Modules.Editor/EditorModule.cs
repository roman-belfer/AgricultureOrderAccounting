using OrderAccounting.Modules.Editor.ViewModels;
using OrderAccounting.Modules.Editor.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace OrderAccounting.Modules.Editor
{
    public class EditorModule : IModule
    {
        #region Variables

        private IRegionManager _regionManager;

        #endregion

        #region Initialization

        public EditorModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion("EditorRegion", typeof(IEditorView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IEditorView, EditorView>();
            containerRegistry.Register<IEditorViewModel, EditorViewModel>();
        }

        #endregion

    }
}
