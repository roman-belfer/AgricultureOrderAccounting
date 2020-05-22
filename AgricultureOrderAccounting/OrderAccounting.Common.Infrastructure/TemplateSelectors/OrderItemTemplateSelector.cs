using OrderAccounting.Common.Infrastructure.ViewModels.OrderItemViewModels;
using System.Windows;
using System.Windows.Controls;

namespace OrderAccounting.Common.Infrastructure.TemplateSelectors
{
    public class OrderItemTemplateSelector : DataTemplateSelector
    {
        #region Properties

        public DataTemplate DetailTemplate { get; set; }
        public DataTemplate ManualOperationTemplate { get; set; }
        public DataTemplate OperationTemplate { get; set; }
        public DataTemplate StuffTemplate { get; set; }
        public DataTemplate TransportTemplate { get; set; }
        public DataTemplate ObjectTemplate { get; set; }
        public DataTemplate EmployeeTemplate { get; set; }

        #endregion

        #region Methods

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                if (item is DetailItemViewModel)
                {
                    return DetailTemplate;
                }
                else if (item is OperationItemViewModel)
                {
                    return OperationTemplate;
                }
                else if (item is ManualOperationItemViewModel)
                {
                    return ManualOperationTemplate;
                }
                else if (item is StuffItemViewModel)
                {
                    return StuffTemplate;
                }
                else if (item is TransportItemViewModel)
                {
                    return TransportTemplate;
                }
                else if (item is ObjectItemViewModel)
                {
                    return ObjectTemplate;
                }
                else if (item is EmployeeItemViewModel)
                {
                    return EmployeeTemplate;
                }
            }

            return null;
        }

        #endregion

    }
}
