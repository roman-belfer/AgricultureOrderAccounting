using FMS.DataManagers.Interfaces;
using FMS.DataManagers.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace OrderAccounting.Common.Infrastructure.Converters
{
    public class VehicleTypeToModelMulticonverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null)
            {
                var vehicleTypeId = values[0] as int?;
                var directoryManager = values[1] as IDirectoryManager;

                var vehicleInformation = directoryManager.VehiclesCollection;

                if (vehicleTypeId.HasValue && vehicleInformation != null)
                {
                    return vehicleInformation.Where(x => x.TypeId == vehicleTypeId).ToList();
                }
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
