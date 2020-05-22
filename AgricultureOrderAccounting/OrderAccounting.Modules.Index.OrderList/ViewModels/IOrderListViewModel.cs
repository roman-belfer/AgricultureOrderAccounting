using OrderAccounting.Modules.Index.OrderList.Views;

namespace OrderAccounting.Modules.Index.OrderList.ViewModels
{
    public interface IOrderListViewModel
    {
        void SetParentView(IOrderListView parentView);
    }
}
