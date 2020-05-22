using OrderAccounting.Modules.Edit.Navigation.Views;

namespace OrderAccounting.Modules.Edit.Navigation.ViewModels
{
    public interface INavigationViewModel
    {
        void SetParentView(INavigationView parentView);
    }
}
