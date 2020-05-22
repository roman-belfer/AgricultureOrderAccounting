using OrderAccounting.Modules.Index.Paging.Views;

namespace OrderAccounting.Modules.Index.Paging.ViewModels
{
    public interface IPagingViewModel
    {
        void SetParentView(IPagingView parentView);
    }
}
