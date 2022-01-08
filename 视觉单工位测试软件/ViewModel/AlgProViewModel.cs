using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace 视觉单工位测试软件
{
    public class AlgProViewModel : ViewModelBase
    {
        #region 构造函数

        public AlgProViewModel()
        {
            ProList = new ObservableCollection<myBaseViewModel>();
            SelectedNode = -1;
        }

        #endregion
        
        #region 绑定参数

        /// <summary>
        /// 功能名称
        /// </summary>
        public string Name
        {
            get => GetProperty(() => Name);
            set => SetProperty(() => Name, value, () =>
            {
                //根据被选中项构建算法控件 在这里会有点复杂

                //镜筒类型
                XDocument config = XDocument.Load(filePath);
                config.Descendants("Name").ElementAt(0).SetValue(value);
                config.Save(filePath);

                //Growl.Info("当前被选中项：" + SelectedAlg.ToString());
            });
        }

        /// <summary>
        /// 步骤队列
        /// </summary>
        public ObservableCollection<myBaseViewModel> ProList
        {
            get => GetProperty(() => ProList);
            set => SetProperty(() => ProList, value,()=> 
            {
                
                
            });
        }

        /// <summary>
        /// 被选中节点
        /// </summary>
        public int SelectedNode
        {
            get => GetProperty(() => SelectedNode);
            set => SetProperty(() => SelectedNode, value,()=> 
            {
                
            });
        }

        /// <summary>
        /// 功能名称
        /// </summary>
        public UserControl strl
        {
            get => GetProperty(() => strl);
            set => SetProperty(() => strl, value);
        }

        #endregion

        #region 绑定方法

        /// <summary>
        /// 算法节点选择
        /// </summary>
        /// <param name="obj"></param>
        [AsyncCommand]
        public void SelCommand(object obj)
        {
            Growl.Info("算法节点选择");

            if (SelectedNode >= 0)
            {
                #region 侧边栏显示
#if false
                    switch (ProList[SelectedNode].ClassType)
                    {
                        case 1: strl = new LocationCtrl(); break;
                        case 2: strl = new MatchCtrl(); break;
                        case 3: strl = new AngleContrl(); break;
                        default:
                            strl = new AngleContrl(); break;
                    }
                    strl.DataContext = ProList[SelectedNode];
#endif
                #endregion

                #region 弹出窗显示
#if true

                UserControl str2;
                Grid view = new Grid()
                {
                    Width = 400
                };
                switch (ProList[SelectedNode].ClassType)
                {
                    case 1: str2 = new LocationCtrl(); break;
                    case 2: str2 = new MatchCtrl(); break;
                    case 3: str2 = new AngleContrl(); break;
                    default:
                        str2 = new AngleContrl(); break;
                }
                ProList[SelectedNode].Authority = System.Windows.Visibility.Visible;
                str2.DataContext = ProList[SelectedNode];

                str2.Margin = new System.Windows.Thickness(10);
                view.Children.Add(str2);


                if (window != null)
                {
                    window.Close();
                }
                window = new PopupWindow
                {
                    Topmost = true,
                    PopupElement = view,
                    WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                    AllowsTransparency = true,
                    WindowStyle = System.Windows.WindowStyle.None,
                    MinWidth = 0,
                    MinHeight = 0,
                    Title = ProList[SelectedNode].Name_C,
                };
                window.Show();
#endif
                #endregion
            }
        }

        /// <summary>
        /// 算法节点添加
        /// </summary>
        /// <param name="obj"></param>
        [AsyncCommand]
        public void AddCommand(object obj)
        {
            Growl.Info("添加算法节点");

            NodeMenuWindow nodeMenuWindow = new NodeMenuWindow();
            nodeMenuWindow.ShowDialog();
            myBaseViewModel newNode;
            switch (nodeMenuWindow.ret)
            {
                case 1:
                    newNode = new LocationViewModel();break;
                case 2:
                    newNode = new MatchViewModel(); break;
                case 3:
                    newNode = new AngleViewModel(); break;
                default:
                    newNode = new LocationViewModel();
                    break;
            }
            ProList.Add(newNode);

            Save();
        }

        /// <summary>
        /// 算法节点删除
        /// </summary>
        /// <param name="obj"></param>
        [AsyncCommand]
        public void DelCommand(object obj)
        {
            ProList.RemoveAt(SelectedNode);

            Save();
        }

        #endregion

        #region 公有方法
        
        /// <summary>
        /// 保存算法流程树
        /// </summary>
        public void Save()
        {
            string localFilePath = filePath;
            try
            {
                XDocument xdoc = new XDocument();
                XDeclaration xdec = new XDeclaration("1.0", "utf-8", "yes");
                xdoc.Declaration = xdec;

                XElement rootEle;
                XElement classEle;
                XElement childEle;

                //添加根节点
                rootEle = new XElement("AlgConfig");
                xdoc.Add(rootEle);

                classEle = new XElement("Name", Name);
                rootEle.Add(classEle);

                int i = 0;
                foreach (myBaseViewModel item in ProList)
                {
                    i++;
                    classEle = new XElement("Type", item.ClassType);
                    rootEle.Add(classEle);
                    //这里需要增加一个节点参数保存
                    item.Save(Path + "/Node" + i + ".xml");
                }

                xdoc.Save(localFilePath);
            }
            catch (Exception e)
            {
                Growl.Error(filePath + "创建失败！");
            }

           
            
            
        }

        /// <summary>
        /// 保存默认配置
        /// </summary>
        void InitErr()
        {
            MainWindow.ErrInfo(filePath + "丢失");

            //获得文件路径
            string localFilePath = "";
            localFilePath = filePath;
            try
            {
                XDocument xdoc = new XDocument();
                XDeclaration xdec = new XDeclaration("1.0", "utf-8", "yes");
                xdoc.Declaration = xdec;

                XElement rootEle;
                XElement classEle;
                XElement childEle;

                //添加根节点
                rootEle = new XElement("AlgConfig");
                xdoc.Add(rootEle);

                classEle = new XElement("Name", "未命名");
                rootEle.Add(classEle);

                xdoc.Save(localFilePath);
            }
            catch (Exception e)
            {
                Growl.Error(filePath + "创建失败！");
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="Index">文件名</param>
        public int Init(string path, int Index)
        {
            Path = path;
            filePath = path + "/AlgConfig.xml";
            if (XmlHelper.Exists(path, "AlgConfig.xml"))
            {
                XDocument Config = XDocument.Load(path + "/AlgConfig.xml");
                int NodeNum = Config.Descendants("Type").Count();
                Name = Config.Descendants("Name").ElementAt(0).Value;

                for (int i = 0; i < NodeNum; i++)
                {
                    int type = int.Parse(Config.Descendants("Type").ElementAt(i).Value);
                    myBaseViewModel algNode = null;
                    switch (type)
                    {
                        case 0: break;
                        case 1: algNode = new LocationViewModel(); break;
                        case 2: algNode = new MatchViewModel(); break;
                        case 3: algNode = new AngleViewModel(); break;
                        case 4: break;
                        case 5: break;
                        case 6: break;
                        case 7: break;
                        default:
                            break;
                    }
                    algNode.Init(Path + "/Node" + (i + 1) + ".xml");
                    ProList.Add(algNode);
                }
            }
            else
            {
                Growl.Error("算法" + Index + "初始化失败！");
                InitErr();
                return 1;
            }

            return 0;
        }

        #endregion

        #region 公有参数
        
        /// <summary>
        /// 参数保存路径
        /// </summary>
        string filePath;

        /// <summary>
        /// 参数保存文件夹
        /// </summary>
        string Path;

        /// <summary>
        /// 弹出框实体
        /// </summary>
        PopupWindow window;

        #endregion
    }
}
