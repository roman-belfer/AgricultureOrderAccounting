using FMS.DataManagers.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace OrderAccounting.Common.Infrastructure.Converters
{
    public class UnitTypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var unit = value as UnitModel;

                if (unit != null)
                {
                    if (unit.TypeId > 0)
                    {
                        return GetImageSourceByTypeId(unit.TypeId);
                    }
                }
                else if ((value as int?).HasValue)
                {
                    return GetImageSourceByTypeId((int)value);
                }
            }

            return "pack://application:,,,/OrderAccounting.Common.Infrastructure;component/Images/aggregate.png";
        }

        private string GetImageSourceByTypeId(int typeId)
        {
            return "pack://application:,,,/OrderAccounting.Common.Infrastructure;component/Images/unit.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
