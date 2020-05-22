using FMS.Services.NetTcpDirectoryServiceReference;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace OrderAccounting.Common.Infrastructure.Converters
{
    public class ObjectTypeToOutlineMulticonverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Count() > 0)
            {
                var outlines = values[0] as List<OutlinesNetTcp>;
                var objectTypeId = values[1] as int?;

                if (outlines != null && outlines.Count > 0 && objectTypeId.HasValue)
                {
                    return outlines.Where(x => x.OutObjectTypeId == objectTypeId);
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
