using OrderAccounting.Modules.Editor.Views;

namespace OrderAccounting.Modules.Editor.ViewModels
{
    public interface IEditorViewModel
    {
        void SetParentView(IEditorView parentView);
    }
}
