using FMS.DataManagers.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace OrderAccounting.Common.Infrastructure.Converters
{
    public class VehicleTypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var vehicle = value as VehicleModel;

                if (vehicle != null)
                {
                    return GetImageSourceByTypeId(vehicle.TypeId);
                }
                else if((value as int?).HasValue)
                {
                    return GetImageSourceByTypeId((int)value);
                }
                
            }

            return "pack://application:,,,/OrderAccounting.Common.Infrastructure;component/Images/vehicle.png";
        }

        private string GetImageSourceByTypeId(int typeId)
        {
            switch (typeId)
            {
                case 1:
                case 8:
                case 11:
                    return "pack://application:,,,/OrderAccounting.Common.Infrastructure;component/Images/tank.png";
                case 2:
                    return "pack://application:,,,/OrderAccounting.Common.Infrastructure;component/Images/truck.png";
                case 3:
                    return "pack://application:,,,/OrderAccounting.Common.Infrastructure;component/Images/harvester.png";
                case 4:
                    return "pack://application:,,,/OrderAccounting.Common.Infrastructure;component/Images/crane.png";
                case 5:
                    return "pack://application:,,,/OrderAccounting.Common.Infrastructure;component/Images/hatchback.png";
                case 7:
                    return "pack://application:,,,/OrderAccounting.Common.Infrastructure;component/Images/sprayer.png";
                case 10:
                    return "pack://application:,,,/OrderAccounting.Common.Infrastructure;component/Images/tractor.png";
                case 13:
                    return "pack://application:,,,/OrderAccounting.Common.Infrastructure;component/Images/plane.png";
            }

            return "pack://application:,,,/OrderAccounting.Common.Infrastructure;component/Images/truck.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
