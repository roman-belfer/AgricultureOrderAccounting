namespace OrderAccounting.Modules.Index.OrderList.Views
{
    public interface IOrderListView
    {
        void OnHideState();
        void OnHideType();

        void OnShowFilter();
        void OnHideFilter();
    }
}
