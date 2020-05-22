namespace OrderAccounting.Common.Infrastructure.Interfaces
{
    public interface IOrderModel
    {
        int Id { get; set; }

        int MasterNumber { get; set; }

        int TypeId { get; set; }
        
        string StateName { get; set; }

    }
}
