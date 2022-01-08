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
    class MatchViewModel:myBaseViewModel
    {
        #region 构造函数
        public MatchViewModel()
        {
            Name_C = "圆周灰度匹配";
            Authority = Visibility.Collapsed;
            ClassType = 2;

            IsGray = -1;
            IsShow = -1;

            MaxRadius = float.MaxValue;
            MinRadius = float.MaxValue;
            Threshold = 255;
            Threshold_INV = 0;
            MatchingRate = 0.5F;
            Color = -1;
        }
        #endregion
        
        #region 前台参数

        public int IsGray
        {
            get => GetProperty(() => IsGray);
            set => SetProperty(() => IsGray, value, () => 
            {
                str[0] = value.ToString();
                
                if (Config != null)
                {
                    Config.Descendants("isGray").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }

        public int IsShow
        {
            get => GetProperty(() => IsShow);
            set => SetProperty(() => IsShow, value, () => 
            {
                str[1] = value.ToString();
                
                if (Config != null)
                {
                    Config.Descendants("IsShow").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }
        
        public float MaxRadius
        {
            get => GetProperty(() => MaxRadius);
            set => SetProperty(() => MaxRadius, value, () => 
            {
                str[2] = value.ToString();
                
                if (Config != null)
                {
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
                str[3] = value.ToString();
                
                if (Config != null)
                {
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
                str[4] = value.ToString();
                
                if (Config != null)
                {
                    Config.Descendants("Threshold").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }

        public float Threshold_INV
        {
            get => GetProperty(() => Threshold_INV);
            set => SetProperty(() => Threshold_INV, value, () => 
            {
                str[5] = value.ToString();
                
                if (Config != null)
                {
                    Config.Descendants("Threshold_INV").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }

        public float MatchingRate
        {
            get => GetProperty(() => MatchingRate);
            set => SetProperty(() => MatchingRate, value, () => 
            {
                
                str[6] = value.ToString();
                
                if (Config != null)
                {
                    Config.Descendants("MatchingRate").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }

        public int Color
        {
            get => GetProperty(() => Color);
            set => SetProperty(() => Color, value, () => 
            {
                str[7] = value.ToString();
                
                if (Config != null)
                {
                    Config.Descendants("Color").ElementAt(0).SetValue(value);
                    Config.Save(Path);
                }
            });
        }

        public string Name
        {
            get => GetProperty(() => Name);
            set => SetProperty(() => Name, value,()=>
            {
                Name_C = "圆周灰度匹配-" + Name;
                Config.Descendants("Name").ElementAt(0).SetValue(value);
                Config.Save(Path);
            });
        }

        public string ErrInf
        {
            get => GetProperty(() => ErrInf);
            set => SetProperty(() => ErrInf, value,()=>
            {
                Config.Descendants("ErrInf").ElementAt(0).SetValue(value);
                Config.Save(Path);
            });
        }
        #endregion

        #region 公有参数
        string[] str = new string[8];

        /// <summary>
        /// 是否继续向后流动
        /// </summary>
        public bool isContinue = false;
        #endregion

        #region 公有方法
        public override int Function()
        {
            str[0] = IsGray.ToString();
            str[1] = IsShow.ToString();
            str[2] = MaxRadius.ToString();
            str[3] = MinRadius.ToString();
            str[4] = Threshold.ToString();
            str[5] = Threshold_INV.ToString();
            str[6] = MatchingRate.ToString();
            str[7] = Color.ToString();

            float[] reParam = new float[1];
            try
            {
                
                bool ret = CVAlgorithms.MV_EntryPoint(1, str, ref reParam[0]);

                isContinue = (reParam[0] != 0);

                if (!isContinue)
                {

                }
                else
                {
                    string text = "错误可能为:" + ErrInf;
                    int color = 1;
                    CVAlgorithms.MV_TextOut(text, color);
                }
            }
            catch (Exception)
            {

                return 1;
            }
            
            return (int)reParam[0];
        }

        public override bool Init(string path)
        {
            try
            {
                Path = path;
                Config = XDocument.Load(Path);
                str = new string[8];

                IsGray          = int.Parse(Config.Descendants("isGray").ElementAt(0).Value);
                IsShow          = int.Parse(Config.Descendants("IsShow").ElementAt(0).Value);
                MaxRadius       = float.Parse(Config.Descendants("MaxRadius").ElementAt(0).Value);
                MinRadius       = float.Parse(Config.Descendants("MinRadius").ElementAt(0).Value);
                Threshold       = float.Parse(Config.Descendants("Threshold").ElementAt(0).Value);
                Threshold_INV   = float.Parse(Config.Descendants("Threshold_INV").ElementAt(0).Value);
                MatchingRate    = float.Parse(Config.Descendants("MatchingRate").ElementAt(0).Value);
                Color           = int.Parse(Config.Descendants("Color").ElementAt(0).Value);

                Name = Config.Descendants("Name").ElementAt(0).Value;
                ErrInf = Config.Descendants("ErrInf").ElementAt(0).Value;
                
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

                classEle = new XElement("isGray", IsGray);
                rootEle.Add(classEle);

                classEle = new XElement("IsShow", IsShow);
                rootEle.Add(classEle);

                classEle = new XElement("MaxRadius", MaxRadius);
                rootEle.Add(classEle);

                classEle = new XElement("MinRadius", MinRadius);
                rootEle.Add(classEle);

                classEle = new XElement("Threshold", Threshold);
                rootEle.Add(classEle);

                classEle = new XElement("Threshold_INV", Threshold_INV);
                rootEle.Add(classEle);

                classEle = new XElement("MatchingRate", MatchingRate);
                rootEle.Add(classEle);

                classEle = new XElement("Color", Color);
                rootEle.Add(classEle);

                classEle = new XElement("Name", Name);
                rootEle.Add(classEle);

                classEle = new XElement("ErrInf", ErrInf);
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
