using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace 视觉单工位测试软件
{
    class TabNumConv : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int YTJ_XT_Nums = int.Parse(values[0].ToString());
                int YTJ_XT_ID = (int)values[1];
                if (YTJ_XT_ID <= YTJ_XT_Nums)
                { return Visibility.Visible; }
                else
                { return Visibility.Hidden; }
            }
            catch (Exception)
            {
                return Visibility.Hidden;
                
            }
           

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
