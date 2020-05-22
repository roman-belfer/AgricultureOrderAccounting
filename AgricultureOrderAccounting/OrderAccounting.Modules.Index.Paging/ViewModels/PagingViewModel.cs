using OrderAccounting.Common.Infrastructure.Enums;
using OrderAccounting.Common.Infrastructure.Events;
using OrderAccounting.Modules.Index.Paging.Views;
using Prism.Events;
using Prism.Mvvm;

namespace OrderAccounting.Modules.Index.Paging.ViewModels
{
    public class PagingViewModel : BindableBase, IPagingViewModel
    {
        #region Properties

        private int _pageNumber = 1;
        public int PageNumber
        {
            get { return _pageNumber; }
            set
            {
                //if (_pageNumber != value)
                {
                    _pageNumber = value;

                    OnPageNumberChanged(_pageNumber);
                    RaisePropertyChanged("PageNumber");
                }
            }
        }

        #endregion

        #region Variables

        private IPagingView _parentView;

        private IEventAggregator _eventAggregator;

        #endregion

        #region Initialization

        public PagingViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<PubSubEvent<PagingEvent>>().Subscribe(PagingTriggered);

            _eventAggregator.GetEvent<PubSubEvent<FilterEvent>>().Subscribe(FilterTriggered);
            _eventAggregator.GetEvent<PubSubEvent<FilterVisibilityEvent>>().Subscribe(FilterVisibilityTriggered);
        }

        #endregion

        #region Methods

        public void SetParentView(IPagingView parentView)
        {
            _parentView = parentView;
        }

        private void OnHide()
        {
            _parentView.OnHide();
        }

        private void OnShow()
        {
            _parentView.OnShow();
        }

        private void OnPageNumberChanged(int pageNumber)
        {
            _eventAggregator.GetEvent<PubSubEvent<PageChangedEvent>>().Publish(new PageChangedEvent(pageNumber));
        }

        private void PagingTriggered(PagingEvent obj)
        {
            if (obj.ViewState == ViewState.Show)
            {
                OnShow();
            }
            else
            {
                OnHide();
            }
        }

        private void FilterVisibilityTriggered(FilterVisibilityEvent obj)
        {
            if (obj.IsVisible)
            {
                OnShowFilter();
            }
            else
            {
                OnHideFilter();
            }
        }

        private void FilterTriggered(FilterEvent obj)
        {
            OnHideFilter();
        }

        private void OnShowFilter()
        {
            _parentView.OnShowFilter();
        }

        private void OnHideFilter()
        {
            _parentView.OnHideFilter();
        }

        #endregion

    }
}
