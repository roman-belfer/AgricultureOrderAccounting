namespace OrderAccounting.Common.Infrastructure.Interfaces
{
    public interface IFilterModel
    {
        /// <summary>
        /// Return bool value if filter data is not empty
        /// </summary>
        bool IsFilterNotEmpty();

        /// <summary>
        /// Return bool value if order model contains data according to the filter
        /// </summary>
        bool CheckFilter<T>(T orderModel);

        /// <summary>
        /// Turn all properties to NULL
        /// </summary>
        void Reset();

        T ConvertFromModel<T>() where T : class;
    }
}
