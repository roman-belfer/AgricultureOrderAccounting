using Argo.DataAccess.All.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace OrderAccounting.Common.Infrastructure.Converters
{
    public class BlockToFieldsMulticonverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Count() > 0)
            {
                var fieldVersions = values[0] as List<FieldVersion.ListItem>;
                var fieldBlockId = values[1] as int?;
                int year = 0;
                if (values.Count() > 2 && values[2] != DependencyProperty.UnsetValue && values[2] != null)
                {
                    int.TryParse(values[2] as string, out year);
                }

                if (fieldVersions != null && fieldVersions.Count > 0)
                {
                    return fieldVersions.Where(x => (fieldBlockId.HasValue ? x.FVersField.FldBlockId == fieldBlockId : true) && (year != 0 ? x.FVersYear == year : true));
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
