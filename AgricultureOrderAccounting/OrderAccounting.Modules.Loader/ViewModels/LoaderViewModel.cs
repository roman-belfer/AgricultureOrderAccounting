using OrderAccounting.Common.Infrastructure.Enums;
using OrderAccounting.Common.Infrastructure.Events;
using OrderAccounting.Modules.Loader.Views;
using Prism.Events;

namespace OrderAccounting.Modules.Loader.ViewModels
{
    public class LoaderViewModel : ILoaderViewModel
    {
        #region Variables

        private ILoaderView _parentView;

        private IEventAggregator _eventAggregator;

        #endregion

        #region Initialization

        public LoaderViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<PubSubEvent<LoaderEvent>>().Subscribe(LoaderTriggered);
        }

        private void LoaderTriggered(LoaderEvent obj)
        {
            if (obj.ViewState == ViewState.Show)
            {
                OnShowView();
            }
            else
            {
                OnHideView();
            }
        }

        #endregion

        #region Methods

        public void SetParentView(ILoaderView parentView)
        {
            _parentView = parentView;
        }

        private void OnShowView()
        {
            _parentView.OnShowView();
        }

        private void OnHideView()
        {
            _parentView.OnHideView();
        }

        #endregion

    }
}
