#pragma once
#include "putText.h"
#include "opencv.hpp"
#include <opencv2/imgproc/types_c.h>
using namespace cv;
using namespace std;

class CVAlgorithms
{
public:
	CVAlgorithms();
	~CVAlgorithms();

	Mat src;
	Mat dst;
	int lines;

	Point2f center;

	vector<string> vTest;
	vector<int> vcolor;
private:

};

bool location(Mat src, Mat& dst, char** input_Parameter, float* output_Parameter_Float);
bool match(Mat src, Mat& dst, char** input_Parameter, float* output_Parameter_Float);
bool angle(Mat src, Mat& dst, char** input_Parameter, float* output_Parameter_Float);
bool MyTextOut(string str, int color);
bool PutTextIn(string str, int color);