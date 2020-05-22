using FMS.Core;

namespace OrderAccounting.Common.SignalRWorker.Interfaces
{
    public interface ISignalRWorker
    {
        event AddOrderEventHandler OrderAdded;
        event AddOrderEventHandler OrderUpdated;
        event RemoveOrderEventHandler OrderRemoved;
    }
}
