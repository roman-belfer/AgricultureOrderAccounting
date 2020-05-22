using OrderAccounting.Modules.Filter.Views;

namespace OrderAccounting.Modules.Filter.ViewModels
{
    public interface IFilterViewModel
    {
        void SetParentView(IFilterView parentView);
    }
}
