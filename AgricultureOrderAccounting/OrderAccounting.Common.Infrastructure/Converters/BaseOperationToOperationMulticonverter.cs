using FMS.DataManagers.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OrderAccounting.Common.Infrastructure.Converters
{
    public class BaseOperationToOperationMulticonverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null)
            {
                var directoryManager = values[0] as IDirectoryManager;
                var baseOperationId = values[1] as int?;

                var operations = directoryManager.Operations;

                if (operations != null && baseOperationId.HasValue)
                {
                    return operations.Where(x => x.BaseOperationID == baseOperationId).ToList();
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
