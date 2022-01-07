using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace 视觉单工位测试软件
{
    /// <summary>
    /// ParamWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ParamWindow : Window
    {
        public ParamWindow()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //int index = (sender as TabControl).SelectedIndex;
            //MainViewModel mainvm = (this.DataContext as MainViewModel);

            //switch (index)
            //{
            //    case 0:
            //        {
            //            foreach (MatchViewModel item in mainvm.MatchVM)
            //            {
            //                item.IsShow = 1;
            //                item.IsGray = 1;
            //            }
            //            mainvm.AngleVM.isGray = 1;
            //        }
            //        break;
            //    case 1:
            //        {
            //            mainvm.LocationVM.IsShow = 1;
            //            mainvm.AngleVM.isGray = 1;
            //        }
            //        break;
            //    case 2:
            //        {
            //            mainvm.LocationVM.IsShow = 1;
            //            foreach (MatchViewModel item in mainvm.MatchVM)
            //            {
            //                item.IsShow = 1;
            //                item.IsGray = 1;
            //            }
            //        }
            //        break;
            //    case 3:break;

            //    default:
            //        break;
            //}

        }

        private void TabControl_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //int index = (sender as TabControl).SelectedIndex;
            //MainViewModel mainvm = (this.DataContext as MainViewModel);
            //for (int i = 0; i < 6; i++)
            //{
            //    if (i != index)
            //    {
            //        mainvm.MatchVM[i].IsGray = 1;
            //        mainvm.MatchVM[i].IsShow = 1;
            //    }
            //}
        }
    }
}
