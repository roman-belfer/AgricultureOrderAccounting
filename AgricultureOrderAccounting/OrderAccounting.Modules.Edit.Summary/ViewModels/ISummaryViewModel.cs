using OrderAccounting.Modules.Edit.Summary.Views;

namespace OrderAccounting.Modules.Edit.Summary.ViewModels
{
    public interface ISummaryViewModel
    {
        void SetParentView(ISummaryView parentView);
    }
}
