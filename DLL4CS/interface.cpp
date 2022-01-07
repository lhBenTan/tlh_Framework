#include "pch.h"

#include "interface.h"
CVAlgorithms Postion;
 
bool MV_Upload(int nHeight, int nWidth, BmpBuf &data, int colorType)
{
	//Postion.src = Mat(nHeight, nWidth, CV_8UC1, data.data_Input);//默认非点胶后相机提供的原始图像为黑白图像
	Postion.src = imread("G:\\待分类\\0922\\903\\20200922 (1).bmp", 0);
	cvtColor(Postion.src, Postion.dst, COLOR_GRAY2RGB);
	Postion.lines = 0;
	return 1;
}

bool MV_EntryPoint(int type, char** input_Parameter, float* output_Parameter_Float)
{
	

	switch (type)
	{
	case 0:return location(Postion.src, Postion.dst, input_Parameter, output_Parameter_Float); break;
	case 1:return match(Postion.src, Postion.dst, input_Parameter, output_Parameter_Float); break;
	case 2:return angle(Postion.src, Postion.dst, input_Parameter, output_Parameter_Float); break;
	case 3:break;
	case 4:break;
	default:
		break;
	}

	return 0;
}
#define DATE __DATE__

bool MV_Download(BmpBuf &data)
{
	while (Postion.vTest.size())
	{
		string _str = Postion.vTest.back();
		Postion.vTest.pop_back();
		int n = Postion.vcolor.back();
		Postion.vcolor.pop_back();

		PutTextIn((char*)_str.c_str(), n);
	}


	int size = Postion.dst.total() * Postion.dst.elemSize();
	data.size = size;
	data.h = Postion.dst.rows;
	data.w = Postion.dst.cols;

	data.data_Output = (uchar *)calloc(size, sizeof(uchar));
	std::memcpy(data.data_Output, Postion.dst.data, size * sizeof(BYTE));

	return 0;
}

bool MV_TextOut(char* str, int color)
{
	return MyTextOut(str, color);
}

bool MV_Release(BmpBuf &data)
{
	delete data.data_Output;
	data.data_Output = NULL;

	data.size = 0;
	return 0;
}