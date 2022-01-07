using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace 视觉单工位测试软件.ViewModel
{
    /// <summary>
    /// 基类模板
    /// </summary>
    class TextViewModel:myBaseViewModel
    {
        #region 构造函数
        public TextViewModel()
        {
            //0表示测试类
            ClassType = 0;
            //默认最高级显示
            Authority = Visibility.Visible;

            //这里预赋值是为了避免输入为0时str初始化异常
            //或者不预先定义str[]，而是每次调用前定义----代码量上好像没变化
            Value1 = int.MaxValue;
            Value2 = float.MaxValue;
            Value3 = "1";
        }
        #endregion
        
        #region 函数传递数组
        public string[] Strs = new string[3];
        #endregion

        /*
         * 前台参数不直接绑定数组，是为了避免勿删参数导致程序异常
         */
        #region 前台绑定参数
        public int Value1
        {
            get => GetProperty(() => Value1);
            set => SetProperty(() => Value1, value,()=> 
            {
                Strs[0] = Value1.ToString();
            });
        }

        public float Value2
        {
            get => GetProperty(() => Value2);
            set => SetProperty(() => Value2, value, () =>
            {
                Strs[1] = Value2.ToString();
            });
        }

        public string Value3
        {
            get => GetProperty(() => Value3);
            set => SetProperty(() => Value3, value, () =>
            {
                Strs[2] = Value3.ToString();
            });
        }
        #endregion

        #region 待覆写程序
        /// <summary>
        /// 重写子类功能
        /// </summary>
        /// <returns>下一工位位置</returns>
        public override int Function()
        {
            try
            {
                MessageBox.Show("子类功能！", "提示");

                //如果算法结果NG，则直接进入
                if (false)
                {
                    return TheNextN;
                }
            }
            catch (Exception)
            {
                //算法过程中已失败，在图面上输出错误提示，跳过剩余测试
                return TheNextN;
            }
            return TheNext;
        }

        /// <summary>
        /// 参数初始化
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <returns>初始化结果 成功为true，反之为false</returns>
        public override bool Init(string path)
        {
            try
            {
                Strs = new string[3];
                this.Config = XDocument.Load(path);
                Value1 = int.Parse(Config.Descendants("Value1").ElementAt(0).Value);
                Value2 = float.Parse(Config.Descendants("Value2").ElementAt(0).Value);
                Value3 = Config.Descendants("Value3").ElementAt(0).Value;
            }
            catch (Exception)
            {
                MessageBox.Show("配置[" + path + "]数据已丢失", "警告");
                return false;
            }

            return true;
        }
        #endregion


    }
}
