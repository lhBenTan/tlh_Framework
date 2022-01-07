using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.IO;
using System.Xml.Linq;
using 视觉单工位测试软件.ViewModel;

namespace 视觉单工位测试软件
{
    class MainViewModel:ViewModelBase
    {
        #region 弃用
#if false
        void createNewFile(string str)
        {
            //获得文件路径
            string localFilePath = "";
            localFilePath = str;
            System.Xml.Linq.XDocument xdoc = new XDocument();
            XDeclaration xdec = new XDeclaration("1.0", "utf-8", "yes");
            xdoc.Declaration = xdec;

            //添加根节点
            XElement rootEle = new XElement("StdValue");
            xdoc.Add(rootEle);

            //给根节点添加子节点
            XElement classEle = new XElement("JT_Para_01", "0");
            //XAttribute attrClass = new XAttribute("No", 1);
            //classEle.Add(attrClass);
            rootEle.Add(classEle);

            xdoc.Save(localFilePath);
        }

        public MainViewModel()
        {
            viewModel = new ObservableCollection<myBaseViewModel>();
            string[] path = new string[8];

            path[0] = "./Loc1.xml";
            path[1] = "./Mat1.xml";
            path[2] = "./Mat2.xml";
            path[3] = "./Mat3.xml";
            path[4] = "./Mat4.xml";
            path[5] = "./Mat5.xml";
            path[6] = "./Mat6.xml";
            path[7] = "./Ang1.xml";

            //for (int i = 0; i < 8; i++)
            //{
            //    createNewFile(path[i]);
            //}
            //int n = 0;
            //LocationVM = new LocationViewModel(path[n++]);
            ////六个匹配添加
            //MatchVM = new ObservableCollection<MatchViewModel>();
            //for (int i = 0; i < 6; i++)
            //{
            //    MatchVM.Add(new MatchViewModel(path[n++]));
            //}
            //AngleVM = new AngleViewModel(path[n++]);

            int n = 0;
            viewModel.Add(new LocationViewModel());
            for (int i = 0; i < 6; i++)
            {
                viewModel.Add(new MatchViewModel());
            }
            viewModel.Add(new AngleViewModel());

            foreach (myBaseViewModel item in viewModel)
            {
                item.Init(path[n++]);
            }

            Authority = 0;
        }

        public int Authority
        {
            get => GetProperty(() => Authority);
            set => SetProperty(() => Authority, value,()=> 
            {
                foreach (myBaseViewModel item in viewModel)
                {
                    item.Authority = (value == 0 ? Visibility.Collapsed : Visibility.Visible); 
                }
            });
        }

        public ObservableCollection<myBaseViewModel> viewModel
        {
            get => GetProperty(() => viewModel);
            set => SetProperty(() => viewModel, value);
        }

        public LocationViewModel LocationVM
        {
            get => GetProperty(() => LocationVM);
            set => SetProperty(() => LocationVM, value);
        }

        public ObservableCollection<MatchViewModel> MatchVM
        {
            get => GetProperty(() => MatchVM);
            set => SetProperty(() => MatchVM, value);
        }

        public AngleViewModel AngleVM
        {
            get => GetProperty(() => AngleVM);
            set => SetProperty(() => AngleVM, value);
        }
        
        public BitmapSource bitmap
        {
            get => GetProperty(() => bitmap);
            set => SetProperty(() => bitmap, value);
        }

        public void DoSomethinng(object sender, EventArgs e)
        {
            //Thread thread = new Thread(() =>
            //{

            //});
            //thread.Start();

            CVAlgorithms.BmpBuf bmpBuf = new CVAlgorithms.BmpBuf();
            bool flag = CVAlgorithms.MV_Upload(0, 0, ref bmpBuf, 0);
            if (flag)
            {
                Somethinng();
                ///////////////////////////////////
                ///这里输出结果
                ///////////////////////////////////
                CVAlgorithms.MV_Download(ref bmpBuf);

                ///////////////////////////////////

                byte[] ys = new byte[bmpBuf.size];
                Marshal.Copy(bmpBuf.pData, ys, 0, bmpBuf.size);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    MyImShow(bmpBuf.Width, bmpBuf.Height, ys);
                });
                ////////////////////////////////////
                CVAlgorithms.MV_Release(ref bmpBuf);
            }
            else
            {
                //图像上传错误
            }
        }

        public bool Somethinng()
        {
            int flag = 0;

            foreach (myBaseViewModel item in viewModel)
            {
                flag = item.Function();
                if (flag != 0)
                {
                    return false;
                }
            }
            return true;
        }

        DispatcherTimer timer = new DispatcherTimer();

        [AsyncCommand]
        public void DoSomethinngCommand(object obj)
        {
            

            if (!timer.IsEnabled)
            {
                timer.Interval = new TimeSpan(600);
                timer.Tick += DoSomethinng;
                timer.Start();

                Growl.Info("开始测试");
            }
            else
            {
                timer.Stop();

                Growl.Info("停止测试");
            }


        }

        public void MyImShow(int w, int h, byte[] rawArry)
        {
            //int rawStride = ((w * PixelFormats.Bgr24.BitsPerPixel) + 7) / 8;
            //mut.WaitOne();
            //bitmap = BitmapSource.Create(w, h, 24, 24, PixelFormats.Bgr24, null, rawArry, rawStride);
            //mut.ReleaseMutex();

            int rawStride = ((w * PixelFormats.Bgr24.BitsPerPixel) + 7) / 8;
            bitmap = BitmapSource.Create(w, h, 24, 24, PixelFormats.Bgr24, null, rawArry, rawStride);

            //GC.Collect();
        }
#endif
        #endregion
        
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainViewModel()
        {
            int ret;
            CamNum = -1;
            SeletedCam = -1;
            InterfaceNum = 0;
            CamID = new int[16];
            for (int i = 0; i < 16; i++) CamID[i] = i;

            if (XmlHelper.Exists(path, "Config.xml"))
            {
                ret = Init(path + "/Config.xml");
                if (ret > 0)
                {
                    Application.Current.Shutdown(0);
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                }
            }
            else
            {
                Growl.Error("软件初始化失败！");
                InitErr();
            }
            SeletedCam = 0;

            HiKhelper.Search();
        }

        #endregion
        
        #region Commands

        /// <summary>
        /// 最小化软件界面
        /// </summary>
        /// <param name="obj"></param>
        [AsyncCommand]
        public void HidingCommand(object obj)
        {
            Growl.Info("最小化系统界面");
        }

        /// <summary>
        /// 唤出Log界面
        /// </summary>
        /// <param name="obj"></param>
        [AsyncCommand]
        public void LogCommand(object obj)
        {
            var listView = new System.Windows.Controls.ListBox()
            {
                DisplayMemberPath = "Message",
                SelectedValuePath = "ID"
            };
            var scrollViewer = new ScrollViewer()
            {
                Height = 800,
                Content = listView
            };

            var window = new PopupWindow
            {
                Topmost = true,
                PopupElement = scrollViewer,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                AllowsTransparency = true,
                WindowStyle = WindowStyle.None,
                MinWidth = 0,
                MinHeight = 0,
                Title = "异常日志",
            };

            ObservableCollection<LogInfo> txt = new ObservableCollection<LogInfo>();
            using (StreamReader sr = new StreamReader("./Log/Log.txt", Encoding.UTF8))
            {
                int lineCount = 0;
                while (0 < sr.Peek())
                {
                    lineCount++;
                    string temp = sr.ReadLine();
                    txt.Insert(0, new LogInfo() { ID = lineCount, Message = temp });
                }
            }
            listView.ItemsSource = txt;
            window.Show();
        }

        /// <summary>
        /// 开关指令
        /// </summary>
        /// <param name="obj"></param>
        [AsyncCommand]
        public void SwitchCommand(object obj)
        {
            //网络连接

            //相机连接
            for (int i = 0; i <= CamNum; i++)
            {
                MultiView.DictPanel[i].camParamViewModel.Connect();
            }
            
        }

        /// <summary>
        /// 软件配置界面唤出
        /// </summary>
        /// <param name="obj"></param>
        [AsyncCommand]
        public void SwConfigCommand(object obj)
        {
            Growl.Info("配置界面开启");

            ConfigWindow configWindow = new ConfigWindow();
            configWindow.DataContext = this;
            configWindow.Show();
        }

        /// <summary>
        /// 算法配置界面唤出
        /// </summary>
        /// <param name="obj"></param>
        [AsyncCommand]
        public void AlgorithmCommand(object obj)
        {
            Growl.Info("算法设置界面开启");
        }

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name="obj"></param>
        [AsyncCommand]
        public void LoadCommand(object obj)
        {
            Growl.Info("导入配置");
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="obj"></param>
        [AsyncCommand]
        public void SaveCommand(object obj)
        {
            Growl.Info("存储配置");
        }

        /// <summary>
        /// 软件退出
        /// </summary>
        /// <param name="obj"></param>
        [AsyncCommand]
        public void ExitCommand(object obj)
        {
            for (int i = 0; i <= CamNum; i++)
            {
                MultiView.DictPanel[i].camParamViewModel.HKcamera.Disconnect();
            }

            Application.Current.Shutdown(-1);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 初始化失败时新建文件
        /// </summary>
        void InitErr()
        {
            MainWindow.ErrInfo(path + "/Config.xml" + "丢失");
            try
            {
                //获得文件路径
                string localFilePath = "";
                localFilePath = path + "/Config.xml";
                XDocument xdoc = new XDocument();
                XDeclaration xdec = new XDeclaration("1.0", "utf-8", "yes");
                xdoc.Declaration = xdec;

                XElement rootEle;
                XElement classEle;
                XElement childEle;

                //添加根节点
                rootEle = new XElement("Config");
                xdoc.Add(rootEle);

                classEle = new XElement("CamNum", 0);
                rootEle.Add(classEle);

                for (int i = 0; i < 4; i++)
                {
                    classEle = new XElement("Net-" + i);
                    rootEle.Add(classEle);


                    childEle = new XElement("IP", "192.168.1.1");
                    classEle.Add(childEle);

                    childEle = new XElement("Port", "1000");
                    classEle.Add(childEle);

                    childEle = new XElement("IsUsing", "false");
                    classEle.Add(childEle);

                    childEle = new XElement("HeartBeat", "1000");
                    classEle.Add(childEle);
                }

                xdoc.Save(localFilePath);
            }
            catch (Exception)
            {

                Growl.Error("软件默认配置创建失败！");
            }

            HandyControl.Controls.MessageBox.Show("软件配置丢失！软件将自启多次补全配置！", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);

            Application.Current.Shutdown(0);
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="filePath"></param>
        int Init(string filePath)
        {
            int ret = 0;
            XDocument Config = XDocument.Load(filePath);

            CamNum = int.Parse(Config.Descendants("CamNum").ElementAt(0).Value);

            for (int i = 0; i < 4; i++)
            {
                try
                {
                    string ip = Config.Descendants("IP").ElementAt(i).Value;
                    int port = int.Parse(Config.Descendants("Port").ElementAt(i).Value);
                    bool isUsing = bool.Parse(Config.Descendants("IsUsing").ElementAt(i).Value);
                    int heartBeat = int.Parse(Config.Descendants("HeartBeat").ElementAt(i).Value);
                }
                catch (Exception e)
                {
                    //Growl.Error(e.Source + ":" + e.Message + "\n" + e.StackTrace);
                }

            }

            for (int i = 0; i < 16; i++)
            {
                ret += MultiView.DictPanel[i].Init(path + "/Camera" + i);
            }

            return ret;
        }

        #endregion

        #region 绑定参数
        
        public struct LogInfo
        {
            public int ID { set; get; }
            public string Message { set; get; }
        }

        /// <summary>
        /// 配置根目录
        /// </summary>
        private readonly string path = "./Para";

        /// <summary>
        /// 通讯设置结构体
        /// </summary>
        public struct Interface
        {
            public bool IsUsing;
            public string IP;
            public string Port;
            //串口信息暂未添加
        }
        
        /// <summary>
        /// 软件配置界面-相机数目(0-15)
        /// </summary>
        public int CamNum
        {
            get => GetProperty(() => CamNum);
            set => SetProperty(() => CamNum, value,()=> 
            {
                if (value >= 0)
                {
                    MultiView.SetCurrentModel(CamNum + 1);
                    XDocument config = XDocument.Load(path + "/Config.xml");
                    config.Descendants("CamNum").ElementAt(0).SetValue(value);
                    config.Save(path + "/Config.xml");
                }
            });
        }

        /// <summary>
        /// 相机ID
        /// </summary>
        public int[] CamID
        {
            get => GetProperty(() => CamID);
            set => SetProperty(() => CamID, value);
        }
        
        /// <summary>
        /// 软件配置界面-接口启用数目(0-4)
        /// </summary>
        public int InterfaceNum
        {
            get => GetProperty(() => InterfaceNum);
            set => SetProperty(() => InterfaceNum, value);
        }

        /// <summary>
        /// 全部通讯接口信息
        /// </summary>
        public ObservableCollection<Interface> Interfaces
        {
            get => GetProperty(() => Interfaces);
            set => SetProperty(() => Interfaces, value);
        }

        /// <summary>
        /// 当前被选中相机
        /// </summary>
        public int SeletedCam
        {
            get => GetProperty(() => SeletedCam);
            set => SetProperty(() => SeletedCam, value,()=> 
            {
                if (value >= 0)
                {
                    camParamViewModel = MultiView.DictPanel[SeletedCam].camParamViewModel;
                    //Growl.Info("选中相机" + SeletedCam);
                }
            });
        }
       
        /// <summary>
        /// 前台绑定参数
        /// </summary>
        public CamParamViewModel camParamViewModel
        {
            get => GetProperty(() => camParamViewModel);
            set => SetProperty(() => camParamViewModel, value);
        }

        #endregion
    }
}
