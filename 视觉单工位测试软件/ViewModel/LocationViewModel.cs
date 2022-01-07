using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace 视觉单工位测试软件
{
    public class LocationViewModel:myBaseViewModel
    {
        #region 构造函数
        public LocationViewModel()
        {
            Name_C = "卡尺圆定位";
            Authority = Visibility.Collapsed;
            ClassType = 1;

            IsShow = 1;
            DefaultX = float.MaxValue;
            DefaultY = float.MaxValue;
            Scale = float.MaxValue;
            MaxOffset = float.MaxValue;
            MaxRadius = float.MaxValue;
            MinRadius = float.MaxValue;
            Threshold = 128;
        }
        #endregion
        
        #region 前台参数

        public int IsShow
        {
            get => GetProperty(() => IsShow);
            set => SetProperty(() => IsShow, value,()=> 
            {
                str[0] = value.ToString();
                if (Path!=null)
                {
                    Config = XDocument.Load(Path);
                    Config.Descendants("IsShow").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }

        public float DefaultX
        {
            get => GetProperty(() => DefaultX);
            set => SetProperty(() => DefaultX, value, () => 
            {
                str[1] = value.ToString();
                
                if (Path != null)
                {
                    Config = XDocument.Load(Path);
                    Config.Descendants("DefaultX").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }

        public float DefaultY
        {
            get => GetProperty(() => DefaultY);
            set => SetProperty(() => DefaultY, value, () => 
            {
                str[2] = value.ToString();
               
                if (Path != null)
                {
                    Config = XDocument.Load(Path);
                    Config.Descendants("DefaultY").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }

        public float Scale
        {
            get => GetProperty(() => Scale);
            set => SetProperty(() => Scale, value, () => 
            {
                str[3] = value.ToString();
               
                if (Path != null)
                {
                    Config = XDocument.Load(Path);
                    Config.Descendants("Scale").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }

        public float MaxOffset
        {
            get => GetProperty(() => MaxOffset);
            set => SetProperty(() => MaxOffset, value, () => 
            {
                str[4] = value.ToString();
                
                if (Path != null)
                {
                    Config = XDocument.Load(Path);
                    Config.Descendants("MaxOffset").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }
    
        public float MaxRadius
        {
            get => GetProperty(() => MaxRadius);
            set => SetProperty(() => MaxRadius, value, () => 
            {
                str[5] = value.ToString();
               
                if (Path != null)
                {
                    Config = XDocument.Load(Path);
                    Config.Descendants("MaxRadius").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }

        public float MinRadius
        {
            get => GetProperty(() => MinRadius);
            set => SetProperty(() => MinRadius, value, () => 
            {
                str[6] = value.ToString();
                
                if (Path != null)
                {
                    Config = XDocument.Load(Path);
                    Config.Descendants("MinRadius").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }

        public float Threshold
        {
            get => GetProperty(() => Threshold);
            set => SetProperty(() => Threshold, value, () => 
            {
                str[7] = value.ToString();
               
                if (Path != null)
                {
                    Config = XDocument.Load(Path);
                    Config.Descendants("Threshold").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }
        #endregion
        
        #region 公有参数
        public string[] str = new string[8];

        /// <summary>
        /// 是否继续向后流动
        /// </summary>
        public bool isContinue = false;

        /// <summary>
        /// X方向补偿(单位：像素)
        /// </summary>
        public float ActualX = 0;

        /// <summary>
        /// Y方向补偿(单位：像素)
        /// </summary>
        public float ActualY = 0;

        /// <summary>
        /// X方向补偿(单位：毫米)
        /// </summary>
        public float OffsetX_MM = 0;

        /// <summary>
        /// Y方向补偿(单位：毫米)
        /// </summary>
        public float OffsetY_MM = 0;
        #endregion

        #region 覆写方法
        public override int Function()
        {
            float[] reParam = new float[5];
            bool ret;//= CVAlgorithms.MV_EntryPoint(0, str, ref reParam[0]);
            ret = CVAlgorithms.MV_EntryPoint(0, str, ref reParam[0]);

            isContinue = (reParam[0] != 0);
            ActualX = reParam[1];
            ActualY = reParam[2];

            OffsetX_MM = reParam[3];
            OffsetY_MM = reParam[4];
            /*                 输出结果至下位机                      */

            /*********************************************************/

            return (int)reParam[0];
        }

        public override bool Init(string path)
        {
            try
            {
                Path = path;
                Config = XDocument.Load(Path);

                IsShow = int.Parse(Config.Descendants("IsShow").ElementAt(0).Value);
                DefaultX = float.Parse(Config.Descendants("DefaultX").ElementAt(0).Value);
                DefaultY = float.Parse(Config.Descendants("DefaultY").ElementAt(0).Value);
                Scale = float.Parse(Config.Descendants("Scale").ElementAt(0).Value);
                MaxOffset = float.Parse(Config.Descendants("MaxOffset").ElementAt(0).Value);
                MaxRadius = float.Parse(Config.Descendants("MaxRadius").ElementAt(0).Value);
                MinRadius = float.Parse(Config.Descendants("MinRadius").ElementAt(0).Value);
                Threshold = float.Parse(Config.Descendants("Threshold").ElementAt(0).Value);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                MessageBox.Show("配置[" + path + "]数据已丢失", "警告");
            }
            return false;
        }

        public override void Save(string path)
        {
            Path = path;

            string localFilePath = Path;
            try
            {
                XDocument xdoc = new XDocument();
                XDeclaration xdec = new XDeclaration("1.0", "utf-8", "yes");
                xdoc.Declaration = xdec;

                XElement rootEle;
                XElement classEle;
                XElement childEle;

                //添加根节点
                rootEle = new XElement("Node");
                xdoc.Add(rootEle);

                classEle = new XElement("IsShow", IsShow);
                rootEle.Add(classEle);

                classEle = new XElement("DefaultX", DefaultX);
                rootEle.Add(classEle);

                classEle = new XElement("DefaultY", DefaultY);
                rootEle.Add(classEle);

                classEle = new XElement("Scale", Scale);
                rootEle.Add(classEle);

                classEle = new XElement("MaxOffset", MaxOffset);
                rootEle.Add(classEle);

                classEle = new XElement("MaxRadius", MaxRadius);
                rootEle.Add(classEle);

                classEle = new XElement("MinRadius", MinRadius);
                rootEle.Add(classEle);

                classEle = new XElement("Threshold", Threshold);
                rootEle.Add(classEle);

                xdoc.Save(localFilePath);
            }
            catch (Exception e)
            {

            }
        }

        #endregion

    }
}
