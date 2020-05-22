namespace OrderAccounting.Modules.Index.Menu.Views
{
    public interface IMenuView
    {
        void OnFilterSet();
        void OnFilterUnset();

        void OnShowFilter();
        void OnHideFilter();
    }
}
