namespace OrderAccounting.Common.Infrastructure.Interfaces
{
    public interface IProcessable
    {
        int Index { get; set; }

        /// <summary>
        /// Returns integer value as percent of filled data
        /// </summary>
        int GetProcessed();
    }
}
