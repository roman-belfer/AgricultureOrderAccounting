using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace OrderAccounting.Common.Infrastructure.Converters
{
    public class ActualPhaseFromDateMulticonverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null)
            {
                var actualPhases = values[0] as List<Argo.DataAccess.All.Models.ActualPhaseOfProduction.ListItem>;
                var dateFrom = values[1] as DateTime?;
                var dateTo = values[2] as DateTime?;

                if (actualPhases != null && dateFrom.HasValue && dateTo.HasValue)
                {
                    return actualPhases.Where(x => x.DateFrom <= dateFrom && x.DateTo >= dateFrom);
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
