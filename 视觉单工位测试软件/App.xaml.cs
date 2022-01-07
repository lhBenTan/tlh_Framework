using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace 视觉单工位测试软件
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        System.Threading.Mutex mutex;

        public App()
        {
            Application.Current.DispatcherUnhandledException += app_DispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //bool ret;
            //mutex = new Mutex(true, "视觉单工位测试软件", out ret);
            //if (!ret)
            //{
            //    Environment.Exit(0);
            //}
            base.OnStartup(e);
        }

        public void app_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("软件异常!即将退出，错误信息请拍照记录：\n"+e.Exception.StackTrace,"警告");
            

            FileStream file;
            XmlHelper.Exists("./Log", "Log.txt");
            file = new FileStream("./Log/Log.txt", FileMode.Append, FileAccess.Write);

            string OutStr = "[";
            OutStr += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss::ff");
            OutStr += "]错误发生在" + e.Exception.TargetSite+"@@"+e.Exception.Message;
            StreamWriter sw = new StreamWriter(file);


            sw.WriteLine(OutStr);
            sw.Flush();
            sw.Close();
            file.Close();

            e.Handled = false;
        }
    }
}
