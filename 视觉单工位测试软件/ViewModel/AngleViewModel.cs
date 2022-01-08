using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace 视觉单工位测试软件
{
    class AngleViewModel:myBaseViewModel
    {
        #region 构建函数
        public AngleViewModel()
        {
            Name_C = "单缺口查找";
            Authority = Visibility.Collapsed;
            ClassType = 3;

            isGray = -1;
            Threshold = int.MaxValue;
            Threshold_INV = int.MaxValue;
            MaxRadius = int.MaxValue;
            MinRadius = int.MaxValue;
            MaxLen = int.MaxValue;
            MinLen = int.MaxValue;
        }
        #endregion
        
        #region 前台参数

        public int isGray
        {
            get => GetProperty(() => isGray);
            set => SetProperty(() => isGray, value, () => 
            {
                str[0] = value.ToString();
                
                if (Config != null)
                {
                    Config.Descendants("isGray").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }

        public int Threshold
        {
            get => GetProperty(() => Threshold);
            set => SetProperty(() => Threshold, value, () => 
            {
                str[1] = value.ToString();
                
                if (Config != null)
                {
                    Config.Descendants("Threshold").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }

        public int Threshold_INV
        {
            get => GetProperty(() => Threshold_INV);
            set => SetProperty(() => Threshold_INV, value, () => 
            {
                str[2] = value.ToString();
                
                if (Config != null)
                {
                    Config.Descendants("Threshold_INV").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }

        public int MaxRadius
        {
            get => GetProperty(() => MaxRadius);
            set => SetProperty(() => MaxRadius, value, () => 
            {
                str[3] = value.ToString();
               
                if (Config != null)
                {
                    Config.Descendants("MaxRadius").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }

        public int MinRadius
        {
            get => GetProperty(() => MinRadius);
            set => SetProperty(() => MinRadius, value, () => 
            {
                str[4] = value.ToString();
                
                if (Config != null)
                {
                    Config.Descendants("MinRadius").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }

        public int MaxLen
        {
            get => GetProperty(() => MaxLen);
            set => SetProperty(() => MaxLen, value, () => 
            {
                str[5] = value.ToString();
                
                if (Config != null)
                {
                    Config.Descendants("MaxLen").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }

        public int MinLen
        {
            get => GetProperty(() => MinLen);
            set => SetProperty(() => MinLen, value, () => 
            {
                str[6] = value.ToString();
                
                if (Config != null)
                {
                    Config.Descendants("MinLen").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }
        #endregion

        #region 公用参数
        
        string[] str = new string[7];

        /// <summary>
        /// 输出角度
        /// </summary>
        float angle = 0;

        /// <summary>
        /// 是否继续向后流动
        /// </summary>
        public bool isContinue = false;
        #endregion

        #region 公用方法

        public override int Function()
        {
            str[0] = isGray.ToString();
            str[1] = Threshold.ToString();
            str[2] = Threshold_INV.ToString();
            str[3] = MaxRadius.ToString();
            str[4] = MinRadius.ToString();
            str[5] = MaxLen.ToString();
            str[6] = MinLen.ToString();

            float[] reParam = new float[2];
            bool ret = CVAlgorithms.MV_EntryPoint(2, str, ref reParam[0]);

            isContinue = (reParam[0] != 0);
            angle = reParam[1];
            return (int)reParam[0];
        }

        public override bool Init(string path)
        {
            try
            {
                Path = path;
                Config = XDocument.Load(Path);

                isGray = int.Parse(Config.Descendants("isGray").ElementAt(0).Value);
                Threshold = int.Parse(Config.Descendants("Threshold").ElementAt(0).Value);
                Threshold_INV = int.Parse(Config.Descendants("Threshold_INV").ElementAt(0).Value);
                MaxRadius = int.Parse(Config.Descendants("MaxRadius").ElementAt(0).Value);
                MinRadius = int.Parse(Config.Descendants("MinRadius").ElementAt(0).Value);
                MaxLen = int.Parse(Config.Descendants("MaxLen").ElementAt(0).Value);
                MinLen = int.Parse(Config.Descendants("MinLen").ElementAt(0).Value);
            }
            catch (Exception)
            {
                MessageBox.Show("配置[" + path + "]数据已丢失", "警告");
                return false;
            }
            return true;
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

                classEle = new XElement("isGray", isGray);
                rootEle.Add(classEle);

                classEle = new XElement("Threshold", Threshold);
                rootEle.Add(classEle);

                classEle = new XElement("Threshold_INV", Threshold_INV);
                rootEle.Add(classEle);

                classEle = new XElement("MaxRadius", MaxRadius);
                rootEle.Add(classEle);

                classEle = new XElement("MinRadius", MinRadius);
                rootEle.Add(classEle);

                classEle = new XElement("MaxLen", MaxLen);
                rootEle.Add(classEle);

                classEle = new XElement("MinLen", MinLen);
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
