
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Linq;
using 视觉单工位测试软件.ViewModel;

namespace 视觉单工位测试软件
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel mainVM;

        Queue<myBaseViewModel> qViewModel = new Queue<myBaseViewModel>();

        static public void ErrInfo(string str)
        {
            FileStream file;
            XmlHelper.Exists("./Log", "Log.txt");
            file = new FileStream("./Log/Log.txt", FileMode.Append, FileAccess.Write);

            string OutStr = "[";
            OutStr += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss::ff");
            OutStr += "]" + str;
            StreamWriter sw = new StreamWriter(file);


            sw.WriteLine(OutStr);
            sw.Flush();
            sw.Close();
            file.Close();
        }
        
        public MainWindow()
        {
            InitializeComponent();
           
            Loaded += (s, e) =>
            {
                hWindow.grid.Tag = 1;
                MultiView.InitGrid(hWindow.grid, 16);

                mainVM = new MainViewModel();

                this.DataContext = mainVM;
            };

            //////////////////////////////
        }
        
    }
}

