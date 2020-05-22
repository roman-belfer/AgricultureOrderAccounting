namespace OrderAccounting.Common.Infrastructure.Models
{
    public class ValueModel
    {
        public int? Id { get; set; }
        public object Value { get; set; }

        public ValueModel() { }

        public ValueModel(int id, object value)
        {
            Id = id;
            Value = value;
        }
    }
}
