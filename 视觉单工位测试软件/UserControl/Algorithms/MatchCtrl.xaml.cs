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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 视觉单工位测试软件
{
    /// <summary>
    /// MatchCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class MatchCtrl : UserControl
    {
        public MatchCtrl()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ComboBox comboBox = sender as ComboBox;

            //ComboBoxItem comboBoxItem = comboBox.SelectedItem as ComboBoxItem;
            //comboBox.Background = comboBoxItem.Background;


        }
    }
}
