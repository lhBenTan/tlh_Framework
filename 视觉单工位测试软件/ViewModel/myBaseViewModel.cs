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
    public class myBaseViewModel : ViewModelBase
    {
        public XDocument Config;

        public string Path;

        public myBaseViewModel()
        {

        }

        #region 公有参数

        /// <summary>
        /// 用于权限管理，这里只设两级权限
        /// </summary>
        public Visibility Authority
        {
            get => GetProperty(() => Authority);
            set => SetProperty(() => Authority, value);
        }

        /// <summary>
        /// 控件名称--这个最好在子类初始化的时候写入
        /// </summary>
        public string Name_C
        {
            get => GetProperty(() => Name_C);
            set => SetProperty(() => Name_C, value);
        }

        /// <summary>
        /// 控件ID
        /// </summary>
        public int ID_C
        {
            get => GetProperty(() => ID_C);
            set => SetProperty(() => ID_C, value);
        }

        /// <summary>
        /// 衍生类类型
        /// 0:测试类
        /// 1:待定
        /// </summary>
        public int ClassType
        {
            get => GetProperty(() => ClassType);
            set => SetProperty(() => ClassType, value);
        }

        /// <summary>
        /// 表示是否结束流程
        /// </summary>
        public int IsEnd
        {
            get => GetProperty(() => IsEnd);
            set => SetProperty(() => IsEnd, value);
        }

        /// <summary>
        /// 下一工位，默认为当前工位+1
        /// </summary>
        public int TheNext
        {
            get => GetProperty(() => TheNext);
            set => SetProperty(() => TheNext, value);
        }

        /// <summary>
        /// 算法异常时下一工位，默认为最后工位(图像输出工位)
        /// </summary>
        public int TheNextN
        {
            get => GetProperty(() => TheNext);
            set => SetProperty(() => TheNext, value);
        }

        #endregion

        #region 公有方法
        /// <summary>
        /// 基类功能
        /// </summary>
        /// <returns>下一工位位置</returns>
        public virtual int Function()
        {
            MessageBox.Show("基类功能！");

            return 0;
        }

        /// <summary>
        /// 参数初始化
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <returns>初始化结果 成功为true，反之为false</returns>
        public virtual bool Init(string path)
        {
            MessageBox.Show("基类功能！");

            return false;
        }

        public virtual void Save(string path)
        {
            MessageBox.Show("基类保存参数");
        }
        #endregion

    }
}
