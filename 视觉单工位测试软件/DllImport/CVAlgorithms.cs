using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace 视觉单工位测试软件
{
    class CVAlgorithms
    {
        public struct BmpBuf
        {
            public IntPtr pData;
            public int size;
            public IntPtr pData_IntPtr;
            //public int size_IntPtr;
            public int Height;
            public int Width;
        }

        /// <summary>
        /// 上传原始图片
        /// </summary>
        /// <param name="nWidth">图像宽度</param>
        /// <param name="nHeight">图像高度</param>
        /// <param name="bmpBuf">图像数据</param>
        /// <param name="ColorType">图像颜色类型</param>
        /// <returns></returns>
        [DllImport("DLL4CS.dll", CharSet = CharSet.Ansi, EntryPoint = "MV_Upload", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MV_Upload(int nWidth, int nHeight,ref BmpBuf bmpBuf,int ColorType);

        /// <summary>
        /// 识别参数入口
        /// </summary>
        /// <param name="type">
        /// 0表示定位算法
        /// 1表示匹配算法
        /// 2表示角度算法
        /// 3表示单个判胶算法
        /// 4表示整盘判胶算法
        /// </param>
        /// <param name="input_Parameter">
        /// 输入参数
        /// </param>
        /// <param name="output_Parameter_Float">
        /// 输出参数
        /// </param>
        /// <returns></returns>
        [DllImport("DLL4CS.dll", CharSet = CharSet.Ansi, EntryPoint = "MV_EntryPoint", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MV_EntryPoint(int type, string[] input_Parameter, ref float output_Parameter_Float);

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="bmpBuf">图像数据</param>
        /// <returns></returns>
        [DllImport("DLL4CS.dll", CharSet = CharSet.Ansi, EntryPoint = "MV_Download", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MV_Download(ref BmpBuf bmpBuf);

        /// <summary>
        /// 输出文字至画面
        /// </summary>
        /// <param name="str">输出内容</param>
        /// <param name="color">文字颜色</param>
        /// <returns></returns>
        [DllImport("DLL4CS.dll", CharSet = CharSet.Ansi, EntryPoint = "MV_TextOut", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MV_TextOut(string str,int color);

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="data"></param>
        [DllImport("DLL4CS.dll", EntryPoint = "MV_Release", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MV_Release(ref BmpBuf data);
    }
}
