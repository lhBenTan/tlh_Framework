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
    /// NodeMenuWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NodeMenuWindow : Window
    {
        public int ret = 0;
        public NodeMenuWindow()
        {
            InitializeComponent();
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ret = int.Parse((string)(sender as ListBoxItem).Tag);
            
            this.Close();
        }
    }
}
