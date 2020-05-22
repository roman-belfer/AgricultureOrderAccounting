using OrderAccounting.Modules.Index.Menu.Views;

namespace OrderAccounting.Modules.Index.Menu.ViewModels
{
    public interface IMenuViewModel
    {
        void SetParentView(IMenuView parentView);
    }
}
