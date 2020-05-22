
namespace OrderAccounting.Modules.Index.Paging.Views
{
    public interface IPagingView
    {
        void OnHide();

        void OnShow();

        void OnShowFilter();

        void OnHideFilter();
    }
}
