using Amib.Threading;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace 视觉单工位测试软件.ViewModel
{
    public class CamParamViewModel : ViewModelBase
    {

        #region 海康相机相关

        /// <summary>
        /// 智能线程池，用于处理回调图像算法
        /// </summary>
        private SmartThreadPool stp;

        /// <summary>
        /// 线程锁对象
        /// </summary>
        private static readonly object objlock = new object();

        CVAlgorithms.BmpBuf bmpBuf = new CVAlgorithms.BmpBuf();

        /// <summary>
        /// 线程执行函数
        /// </summary>
        /// <param name="i"></param>
        /// <param name="e"></param>
        private void MV_STPAction(int i, HiKhelper.MV_IM_INFO e)
        {

            bmpBuf.Width = e.width;
            bmpBuf.Height = e.height;
            CVAlgorithms.MV_Upload(e.width, e.height, ref bmpBuf, 3);

            
            foreach (var item in AlgPros.ElementAt(0).ProList)
            {
                if (0 != item.Function()) break;
            }
            CVAlgorithms.MV_Download(ref bmpBuf);
            
            //显示
            Application.Current.Dispatcher.Invoke(() =>
            {
               int size = e.nFrameLen;
                if (ImSrc_test == null || ImSrc_test.Width != bmpBuf.Width || ImSrc_test.Height != bmpBuf.Height)
                {
                    if (size > 3 * bmpBuf.Width * bmpBuf.Height / 2) 
                        ImSrc_test = new WriteableBitmap(bmpBuf.Width, bmpBuf.Height, 96.0, 96.0, PixelFormats.Bgr24, null);
                    else
                        ImSrc_test = new WriteableBitmap(bmpBuf.Width, bmpBuf.Height, 24.0, 24.0, PixelFormats.Gray8, null);
                }
                
                lock (objlock)
                {
                    if ((e.pData != (IntPtr)0x00000000))
                    {
                        ImSrc_test.Lock();
                        ImSrc_test.WritePixels(new Int32Rect(0, 0, bmpBuf.Width, bmpBuf.Height), bmpBuf.pData, size, ImSrc_test.BackBufferStride);
                        ImSrc_test.AddDirtyRect(new Int32Rect(0, 0, bmpBuf.Width, bmpBuf.Height));
                        ImSrc_test.Unlock();

                    }
                }
            });

            //这里是固定的
            CVAlgorithms.MV_Release(ref bmpBuf);
        }

        /// <summary>
        /// 回调函数，回调帧为YUV格式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hik_MV_OnOriFrameInvoked(object sender, HiKhelper.MV_IM_INFO e)
        {
            stp.QueueWorkItem(new Amib.Threading.Action<int, HiKhelper.MV_IM_INFO>(MV_STPAction), 0, e, Amib.Threading.WorkItemPriority.Normal);
        }

        /// <summary>
        /// 海康相机实例
        /// </summary>
        public HiKhelper HKcamera = new HiKhelper();
        
        /// <summary>
        /// 连接相机
        /// </summary>
        public void Connect()
        {
            if (HKcamera.isConnect == false)
            {
                if (0 == HKcamera.Connect(SelectedCam))
                {
                    IsCamLink = true;
                    CamTriggerMode = true;
                    Growl.Success("相机" + CamID + "连接成功");
                }
                else
                {
                    Growl.Warning("相机" + CamID + "连接失败");
                    IsCamLink = false;
                }
            }
            else
            {
                HKcamera.Disconnect();
                IsCamLink = false;
            }
        }

        /// <summary>
        /// 连接相机
        /// </summary>
        /// <param name="obj"></param>
        [AsyncCommand]
        public void ConnectCommand(object obj)
        {
            Connect();
        }

        #endregion

        #region 构造函数
        
        //这里导入海康函数库后还需要增加
        public CamParamViewModel()
        {
            AlgID = new int[8];
            for (int i = 0; i < 8; i++) AlgID[i] = i + 1;

            IsCamLink = false;
            CamTriggerMode = false;

            CamInfoExs = new ObservableCollection<CamInfoEx>();
            AlgPros = new ObservableCollection<AlgProViewModel>();

            
            camInfos = HiKhelper.CamInfos;

            stp = new SmartThreadPool
            {
                MaxThreads = 1
            };

            HKcamera.MV_OnOriFrameInvoked += Hik_MV_OnOriFrameInvoked;
        }

        #endregion
        
        #region 本地方法

        void InitErr()
        {
            MainWindow.ErrInfo(filePath+"丢失");
            try
            {
                //获得文件路径
                string localFilePath = "";
                localFilePath = filePath;
                XDocument xdoc = new XDocument();
                XDeclaration xdec = new XDeclaration("1.0", "utf-8", "yes");
                xdoc.Declaration = xdec;

                XElement rootEle;
                XElement classEle;
                XElement childEle;

                //添加根节点
                rootEle = new XElement("CamConfig");
                xdoc.Add(rootEle);

                classEle = new XElement("AlgNum", 1);
                rootEle.Add(classEle);

                classEle = new XElement("Name", "未命名");
                rootEle.Add(classEle);

                classEle = new XElement("Exposure", 100);
                rootEle.Add(classEle);

                classEle = new XElement("Gain", 100);
                rootEle.Add(classEle);

                classEle = new XElement("Delay", 100);
                rootEle.Add(classEle);

                classEle = new XElement("PreDelay", 100);
                rootEle.Add(classEle);

                classEle = new XElement("SelectedCam", 0);
                rootEle.Add(classEle);

                xdoc.Save(localFilePath);
            }
            catch (Exception e)
            {
                Growl.Error("相机" + CamID + "默认配置导入失败！");
            }
        }

        public int Init(string path)
        {
            int ret = 0;
            filePath = path + "/CamConfig.xml";
            if (XmlHelper.Exists(path, "CamConfig.xml"))
            {
                XDocument Config = XDocument.Load(path + "/CamConfig.xml");
                AlgNum = int.Parse(Config.Descendants("AlgNum").ElementAt(0).Value);
                
                Name = Config.Descendants("Name").ElementAt(0).Value;
                CamInfoExs.Add(new CamInfoEx()
                {
                    Exposure = int.Parse(Config.Descendants("Exposure").ElementAt(0).Value),
                    Gain = int.Parse(Config.Descendants("Gain").ElementAt(0).Value),
                    Delay = int.Parse(Config.Descendants("Delay").ElementAt(0).Value),
                    PreDelay = int.Parse(Config.Descendants("PreDelay").ElementAt(0).Value),
                });

                for (int i = 0; i < 8; i++)
                {
                    AlgPros.Add(new AlgProViewModel());
                    ret += AlgPros[i].Init(path + "/Algorithms" + i.ToString(), i);
                }
                SelectedItem = AlgPros[0];

                SelectedCam = int.Parse(Config.Descendants("SelectedCam").ElementAt(0).Value);
            }
            else
            {
                Growl.Error("相机" + CamID + "初始化失败！");
                InitErr();
                ret++;
            }

            return ret;
        }

        #endregion

        #region 本地参数
        
        public int CamID = 0;

        public string filePath;

        #endregion

        #region 绑定参数

        #region 相机相关

        /// <summary>
        /// 相机名称
        /// </summary>
        public string Name
        {
            get => GetProperty(() => Name);
            set => SetProperty(() => Name, value);
        }

        /// <summary>
        /// 相机配置--这里直接从相机读取or从本地读取写入相机
        /// </summary>
        public struct CamInfoEx
        {
            /// <summary>
            /// 相机号
            /// </summary>
            public int Item { set; get; }
            public int Exposure { set; get; }
            public float Gain { set; get; }
            public float Delay { set; get; }
            public float PreDelay { set; get; }
            //根据实际需求待继续完善
        }

        /// <summary>
        /// 全部相机参数
        /// </summary>
        public ObservableCollection<CamInfoEx> CamInfoExs
        {
            get => GetProperty(() => CamInfoExs);
            set => SetProperty(() => CamInfoExs, value);
        }

        /// <summary>
        /// 相机信息 用于连接相机
        /// </summary>
        public ObservableCollection<HiKhelper.CamInfo> camInfos
        {
            get => GetProperty(() => camInfos);
            set => SetProperty(() => camInfos, value);
        }

        /// <summary>
        /// 被选中相机号
        /// </summary>
        public int SelectedCam
        {
            get => GetProperty(() => SelectedCam);
            set => SetProperty(() => SelectedCam, value, () =>
            {
                //Growl.Info("算法数：" + value);

                //镜筒类型
                XDocument config = XDocument.Load(filePath);
                config.Descendants("SelectedCam").ElementAt(0).SetValue(value);
                config.Save(filePath);
            });
        }

        /// <summary>
        /// 相机是否已连接 false表示未连接
        /// </summary>
        public bool IsCamLink
        {
            get => GetProperty(() => IsCamLink);
            set => SetProperty(() => IsCamLink, value);
        }

        /// <summary>
        /// 相机触发模式选择
        /// 目前只设置连续触发以及软件触发两种
        /// </summary>
        public bool CamTriggerMode
        {
            get => GetProperty(() => CamTriggerMode);
            set => SetProperty(() => CamTriggerMode, value);
        }

        #endregion

        #region 算法相关

        /// <summary>
        /// Image控件显示的资源
        /// </summary>
        public WriteableBitmap ImSrc_test
        {
            get => GetProperty(() => ImSrc_test);
            set => SetProperty(() => ImSrc_test, value);
        }

        /// <summary>
        /// 算法数--目前内部算法最多8个
        /// </summary>
        public int AlgNum
        {
            get => GetProperty(() => AlgNum);
            set => SetProperty(() => AlgNum, value,()=> 
            {
                //Growl.Info("算法数：" + value);

                //镜筒类型
                XDocument config = XDocument.Load(filePath);
                config.Descendants("AlgNum").ElementAt(0).SetValue(value);
                config.Save(filePath);
            });
        }

        /// <summary>
        /// 算法ID
        /// </summary>
        public int[] AlgID
        {
            get => GetProperty(() => AlgID);
            set => SetProperty(() => AlgID, value);
        }
        
        /// <summary>
        /// 当前所选算法
        /// </summary>
        public int SelectedAlg
        {
            get => GetProperty(() => SelectedAlg);
            set => SetProperty(() => SelectedAlg, value, () =>
            {
                //根据被选中项构建算法控件 在这里会有点复杂
                //Growl.Info("当前被选中项：" + SelectedAlg.ToString());
                if (SelectedAlg < 0)
                {
                    SelectedItem = AlgPros[0];
                }
                else if(SelectedAlg < AlgPros.Count)
                    SelectedItem = AlgPros[SelectedAlg];
            });
        }

        /// <summary>
        /// 当前被选中项
        /// </summary>
        public AlgProViewModel SelectedItem
        {
            get => GetProperty(() => SelectedItem);
            set => SetProperty(() => SelectedItem, value, () =>
            {
                //根据被选中项构建算法控件 在这里会有点复杂
                //Growl.Info("当前被选中项：" + SelectedAlg.ToString());
            });
        }

        /// <summary>
        /// 算法流程
        /// </summary>
        public ObservableCollection<AlgProViewModel> AlgPros
        {
            get => GetProperty(() => AlgPros);
            set => SetProperty(() => AlgPros, value);
        }
        #endregion

        #region 通讯端口

        /// <summary>
        /// 通讯端口
        /// </summary>
        public int Port
        {
            get => GetProperty(() => Port);
            set => SetProperty(() => Port, value);
        }

        #endregion

        #endregion
    }
}
