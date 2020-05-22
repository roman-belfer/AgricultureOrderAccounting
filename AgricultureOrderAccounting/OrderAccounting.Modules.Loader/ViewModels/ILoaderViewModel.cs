using OrderAccounting.Modules.Loader.Views;

namespace OrderAccounting.Modules.Loader.ViewModels
{
    public interface ILoaderViewModel
    {
        void SetParentView(ILoaderView parentView);
    }
}
