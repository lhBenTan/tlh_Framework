#include "pch.h"
#include "CVAlgorithms.h"
#include "metrogolyCircle.h"

//0表示轮廓 1表示卡尺圆
#define LocationType 1

extern CVAlgorithms Postion;

CVAlgorithms::CVAlgorithms()
{
}

CVAlgorithms::~CVAlgorithms()
{
}

/*
卡尺圆定位，待修改 目前问题拟合圆不准确


#pragma region 参数转换
		int IsShow = atoi(input_Parameter[0]);
		float DefaultX = atof(input_Parameter[1]);
		float DefaultY = atof(input_Parameter[2]);
		float Scale = atof(input_Parameter[3]);
		float MaxOffset = atof(input_Parameter[4]);
		float MaxRadius = atof(input_Parameter[5]);
		float MinRadius = atof(input_Parameter[6]);
		float Threshold = atof(input_Parameter[7]);
#pragma endregion

#pragma region 本地参数
		Mat mask = Mat::zeros(src.size(), src.type());
		Mat temp = Mat::zeros(src.size(), src.type());
		int result = 0;
		float ActualX = 0;
		float ActualY = 0;

		float DeltaX = 0;
		float DeltaY = 0;

		Point2f center = Point2f(DefaultX, DefaultY);

		vector<vector<Point2i>> contours;
		vector<Vec4i> hierarchy;
		int item = -1;
#pragma endregion

#pragma region 图像预处理
#if (LocationType == 1)
		circle(mask, center, MaxRadius, Scalar(255), -1);
		circle(mask, center, MinRadius, Scalar(000), -1);

		src.copyTo(temp, mask);
		adaptiveThreshold(temp, temp, 255, ADAPTIVE_THRESH_MEAN_C, THRESH_BINARY, 3, Threshold);
#endif // (LocationType == 0)
#pragma endregion

#pragma region 输出图像选择
		if (IsShow == 0)
		{
			cvtColor(temp, dst, COLOR_GRAY2RGB);
		}
		else
		{
			cvtColor(src, dst, COLOR_GRAY2RGB);
		}
		circle(dst, center, MaxRadius, Scalar(0, 255, 0), 2, 1);
		circle(dst, center, MinRadius, Scalar(0, 255, 0), 2, 1);
#pragma endregion

#pragma region 坐标定位
#if (LocationType == 0)//轮廓定位
#pragma region 轮廓定位
		findContours(temp, contours, hierarchy, CV_RETR_TREE, CV_CHAIN_APPROX_NONE, Point(0, 0));
		for (size_t i = 0; i < contours.size(); i++)
		{

		}

		if (item < 0)
		{
			result++;
		}
#pragma endregion
#elif (LocationType == 1)
#pragma region 卡尺圆定位
		cv::Vec3i vecTmp = { (int)center.x, (int)center.y, (int)(MaxRadius + MinRadius) / 2 };
		int nV = (MaxRadius - MinRadius) / 2, nH = 5, nThreshold = Threshold;
		std::vector<cv::Point> stOutPt;
		gen_Metrology_Model_circle(src, vecTmp, nV, nH, nThreshold, stOutPt);

		for (size_t i = 0; i < stOutPt.size(); i++)
		{
			line(dst, stOutPt[i], stOutPt[i], Scalar(255, 255, 0), 4);
		}

		if (3 > stOutPt.size())
		{
			MyTextOut("有效点数不足，定位失败！", 1);
			return 1;
		}

		std::vector < cv::Point> inPoints; inPoints.clear();
		circleInfo fitCircle;
		ransc_fit_circle(src, stOutPt, 100, 5, 60, inPoints, fitCircle);

		for (size_t i = 0; i < inPoints.size(); i++)
		{
			line(dst, inPoints[i], inPoints[i], Scalar(0, 255, 255), 2);
		}

		if (3 > inPoints.size())
		{
			MyTextOut("拟合分数过低！定位失败！", 1);
			return 1;
		}
		ActualX = fitCircle.A;
		ActualY = fitCircle.B;

		if (0 >= inPoints.size())
		{
			result++;
		}
#pragma endregion
#endif // (LocationType == 0)

		DeltaX = (DefaultX - ActualX) * Scale;
		DeltaY = (DefaultY - ActualY) * Scale;
#pragma endregion

#pragma region 输出图像绘制

#if (LocationType == 1)
		circle(dst, cv::Point(fitCircle.A, fitCircle.B), fitCircle.C, Scalar(255, 0, 0), 2);

#endif

#pragma region 判断结果
		stringstream ss;
		ss << "中心坐标:(" << ActualX << "," << ActualY << ")" << endl;
		if (MaxOffset <= sqrt(DeltaX*DeltaX + DeltaX * DeltaX))
		{
			result++;
			MyTextOut("偏移量过大！请注意！", 1);
			MyTextOut(ss.str().c_str(), 1);
			return 1;
		}
		else
		{
			MyTextOut(ss.str().c_str(), 0);
		}
#pragma endregion


#pragma endregion

#pragma region 结果返回
		output_Parameter_Float[0] = result;
		output_Parameter_Float[1] = ActualX;
		output_Parameter_Float[2] = ActualY;
		output_Parameter_Float[3] = DeltaX;
		output_Parameter_Float[4] = DeltaY;

		Postion.center = Point(ActualX, ActualY);
#pragma endregion
*/

bool location(Mat src,Mat& dst, char** input_Parameter, float* output_Parameter_Float)
{
	try
	{
#pragma region 参数转换
		int IsShow = atoi(input_Parameter[0]);
		float DefaultX = atof(input_Parameter[1]);
		float DefaultY = atof(input_Parameter[2]);
		float Scale = atof(input_Parameter[3]);
		float MaxOffset = atof(input_Parameter[4]);
		float MaxRadius = atof(input_Parameter[5]);
		float MinRadius = atof(input_Parameter[6]);
		float Threshold = atof(input_Parameter[7]);
#pragma endregion

#pragma region 本地参数
		Mat mask = Mat::zeros(src.size(), src.type());
		Mat temp = Mat::zeros(src.size(), src.type());
		int result = 0;
		float ActualX = 0;
		float ActualY = 0;

		float DeltaX = 0;
		float DeltaY = 0;

		Point2f center = Point2f(DefaultX, DefaultY);

		vector<vector<Point2i>> contours;
		vector<Vec4i> hierarchy;
		int item = -1;
#pragma endregion

#pragma region 图像预处理
		circle(mask, center, MaxRadius, Scalar(255), -1);
		circle(mask, center, MinRadius, Scalar(000), -1);

		//adaptiveThreshold(src, temp, 255, ADAPTIVE_THRESH_MEAN_C, THRESH_BINARY, 7, Threshold);
		
		threshold(src, temp, Threshold, 255, THRESH_BINARY);
		temp.copyTo(temp, mask);
#pragma endregion

#pragma region 输出图片选择
		if (IsShow == 0)
		{
			cvtColor(temp, dst, COLOR_GRAY2RGB);
		}
		else
		{
			cvtColor(src, dst, COLOR_GRAY2RGB);
		}
		circle(dst, center, MaxRadius, Scalar(0, 255, 0), 2, 1);
		circle(dst, center, MinRadius, Scalar(0, 255, 0), 2, 1);
#pragma endregion

#pragma region 卡尺圆定位
		cv::Vec3i vecTmp = { (int)center.x, (int)center.y, (int)(MaxRadius + MinRadius) / 2 };
		int nV = (MaxRadius - MinRadius) / 2, nH = 5, nThreshold = Threshold;
		std::vector<cv::Point> stOutPt;
		gen_Metrology_Model_circle(temp, vecTmp, nV, nH, nThreshold, stOutPt);

		for (size_t i = 0; i < stOutPt.size(); i++)
		{
			line(dst, stOutPt[i], stOutPt[i], Scalar(255, 255, 0), 20);
		}

		if (3 > stOutPt.size())
		{
			MyTextOut("有效点数不足，定位失败！", 1);
			return 1;
		}

		std::vector < cv::Point> inPoints; inPoints.clear();
		circleInfo fitCircle;
		ransc_fit_circle(src, stOutPt, 816, 1, 70, inPoints, fitCircle);

		for (size_t i = 0; i < inPoints.size(); i++)
		{
			line(dst, inPoints[i], inPoints[i], Scalar(0, 255, 255), 10);
		}

		if (3 > inPoints.size())
		{
			MyTextOut("拟合分数过低！定位失败！", 1);
			return 1;
		}
		ActualX = fitCircle.A;
		ActualY = fitCircle.B;

		DeltaX = (DefaultX - ActualX) * Scale;
		DeltaY = (DefaultY - ActualY) * Scale;

		circle(dst, cv::Point(fitCircle.A, fitCircle.B), fitCircle.C, Scalar(255, 0, 0), 2);
#pragma endregion

#pragma region 判断结果
		stringstream ss;
		ss << "中心坐标:(" << ActualX << "," << ActualY << ")" << endl;
		if (MaxOffset <= sqrt(DeltaX*DeltaX + DeltaX * DeltaX))
		{
			result++;
			MyTextOut("偏移量过大！请注意！", 1);
			MyTextOut(ss.str().c_str(), 1);
			return 1;
		}
		else
		{
			MyTextOut(ss.str().c_str(), 0);
		}
#pragma endregion

#pragma region 结果返回
		output_Parameter_Float[0] = result;
		output_Parameter_Float[1] = ActualX;
		output_Parameter_Float[2] = ActualY;
		output_Parameter_Float[3] = DeltaX;
		output_Parameter_Float[4] = DeltaY;

		Postion.center = Point(ActualX, ActualY);

		return 0;
#pragma endregion
	}
	catch (const std::exception&)
	{
		output_Parameter_Float[0] = 1;
		return 1;
	}
}

bool match(Mat src, Mat& dst, char** input_Parameter, float* output_Parameter_Float)
{
	try
	{
#pragma region 参数转换
		
		int isGray = atoi(input_Parameter[0]);
		int isShow = atoi(input_Parameter[1]);
		float MaxRadius = atof(input_Parameter[2]);
		float MinRadius = atof(input_Parameter[3]);
		float Threshold = atof(input_Parameter[4]);
		float Threshold_INV = atof(input_Parameter[5]);
		float MatchingRate = atof(input_Parameter[6]);
		int Color = atoi(input_Parameter[7]);

		if (MaxRadius < 1)
		{
			output_Parameter_Float[0] = 0;
			return 0;
		}
#pragma endregion

#pragma region 本地参数
		Scalar color[6] =
		{
			Scalar(0x48, 0xEA, 0xE2),
			Scalar(0x27, 0x96, 0xEE),
			Scalar(0x48, 0xEA, 0x6D),
			Scalar(0xEA, 0xEA, 0x48),
			Scalar(0xEA, 0x83, 0x48),
			Scalar(0xEA, 0x48, 0xA0),
		};

		Point2f tempCenter = Postion.center;
		Mat temp, tempEx;

		int result = 0;
#pragma endregion

#pragma region 极坐标转化
		warpPolar(src, temp, cv::Size(MaxRadius + 5, 360), tempCenter, MaxRadius + 5, INTER_LINEAR + WARP_POLAR_LINEAR);

		//blur(temp, tempEx, Size(3, 501));

		//absdiff(temp, tempEx, temp);

		//threshold(temp, temp, 255, 255, THRESH_TOZERO_INV);
		//threshold(temp, temp, 120, 255, THRESH_BINARY);

		//Mat _tempMat;
		//cv::warpPolar(temp, _tempMat, src.size(), tempCenter, MaxRadius + 5, INTER_LINEAR + WARP_POLAR_LINEAR + WARP_INVERSE_MAP);
		//imshow("1", _tempMat);
		//waitKey(2);
#pragma endregion

#pragma region 灰度显示
		if (isGray == 0)
		{
			Mat _temp;
			cv::warpPolar(temp, _temp, src.size(), tempCenter, MaxRadius + 5, INTER_LINEAR + WARP_POLAR_LINEAR + WARP_INVERSE_MAP);

			cv::threshold(_temp, _temp, Threshold_INV, 255, THRESH_TOZERO_INV);
			cv::threshold(_temp, _temp, Threshold, 255, THRESH_BINARY);

			//circle(dst, tempCenter, MaxRadius, Scalar(0, 0, 255), 2);
			//circle(dst, tempCenter, MinRadius, Scalar(0, 0, 255), 2);

			cvtColor(_temp, dst, COLOR_GRAY2BGR);
		}
#pragma endregion

#pragma region Roi投影为线
		tempEx = temp(Rect(MinRadius, 0, MaxRadius - MinRadius, 360)).clone();

		reduce(tempEx, tempEx, 1, CV_REDUCE_AVG);
#pragma endregion

#pragma region 匹配度计算
		threshold(tempEx, tempEx, Threshold_INV, 1, THRESH_TOZERO_INV);
		threshold(tempEx, tempEx, Threshold, 1, THRESH_BINARY);

		int angleSum = cv::sum(tempEx)[0];
		float rate = angleSum / 360.0;
#pragma endregion

#pragma region 结果判定
		if (rate < MatchingRate)
		{
			result++;

			circle(dst, tempCenter, MaxRadius, Scalar(0, 0, 255), 2);
			circle(dst, tempCenter, MinRadius, Scalar(0, 0, 255), 2);

			for (size_t i = 0; i < tempEx.rows; i++)
			{
				if (tempEx.at<uchar>(i, 0) != 1)
				{
					int ang = (i + 90) % 360;
					float s = sin(ang * CV_PI / 180);
					float c = cos(ang * CV_PI / 180);

					Point2f end = tempCenter + MaxRadius * Point2f(s, -c);
					Point2f sta = tempCenter + MinRadius * Point2f(s, -c);

					line(dst, sta, end, Scalar(0, 0, 255), 1);
					//line(dst, sta, sta, Scalar(0, 0, 255), 6);
				}
			}
		}
		else
		{
			//良品根据前端选择是否显示
			if (isShow == 0)
			{
				circle(dst, tempCenter, MaxRadius, color[Color], 2);//0x48
				circle(dst, tempCenter, MinRadius, color[Color], 2);

				for (size_t i = 0; i < tempEx.rows; i++)
				{
					if (tempEx.at<uchar>(i, 0) != 1)
					{
						int ang = (i + 90) % 360;
						float s = sin(ang * CV_PI / 180);
						float c = cos(ang * CV_PI / 180);

						Point2f end = tempCenter + MaxRadius * Point2f(s, -c);
						Point2f sta = tempCenter + MinRadius * Point2f(s, -c);

						line(dst, sta, end, Scalar(0, 0, 255), 1);
						//line(dst, sta, sta, Scalar(0, 0, 255), 6);
					}
				}
			}
		}
#pragma endregion

#pragma region 结果返回
		output_Parameter_Float[0] = result;
		return result;
#pragma endregion
	}
	catch (const std::exception&)
	{
		MyTextOut("算法异常退出！", 1);
		output_Parameter_Float[0] = 1;
		return 1;
	}

}

bool angle(Mat src, Mat& dst, char** input_Parameter, float* output_Parameter_Float)
{
	try
	{
#pragma region 参数转换
		int isGray = atoi(input_Parameter[0]);
		float Threshold = atof(input_Parameter[1]);
		float Threshold_INV = atof(input_Parameter[2]);
		float MaxRadius = atof(input_Parameter[3]);
		float MinRadius = atof(input_Parameter[4]);
		float MaxLen = atof(input_Parameter[5]);
		float MinLen = atof(input_Parameter[6]);
#pragma endregion

#pragma region 本地参数
		Scalar color = Scalar(0xCD, 0xC3, 0xFD);

		Point2f tempCenter = Postion.center;
		Mat temp, diff;

		int result = 0;
		int angle = 0;
#pragma endregion

#pragma region 极坐标转化
		cv::warpPolar(src, temp, cv::Size(500, 360), tempCenter, 500, INTER_LINEAR + WARP_POLAR_LINEAR);

		//blur(temp, diff, Size(3, 501));

		//absdiff(temp, diff, temp);
#pragma endregion

#pragma region 灰度显示
		if (isGray == 0)
		{
			Mat _temp;
			cv::warpPolar(temp, _temp, src.size(), tempCenter, 500, INTER_LINEAR + WARP_POLAR_LINEAR + WARP_INVERSE_MAP);

			cv::threshold(_temp, _temp, Threshold_INV, 255, THRESH_TOZERO_INV);
			cv::threshold(_temp, _temp, Threshold, 255, THRESH_BINARY);

			cvtColor(_temp, dst, COLOR_GRAY2BGR);
		}
		circle(dst, tempCenter, MaxRadius, Scalar(0xCD, 0xC3, 0xFD), 2);
		circle(dst, tempCenter, MinRadius, Scalar(0xCD, 0xC3, 0xFD), 2);
#pragma endregion

#pragma region Roi投影为线
		diff = temp(Rect(MinRadius, 0, MaxRadius - MinRadius, 360)).clone();

		cv::reduce(diff, diff, 1, CV_REDUCE_AVG);

		cv::threshold(diff, diff, Threshold_INV, 255, THRESH_TOZERO_INV);
		cv::threshold(diff, diff, Threshold, 1, THRESH_BINARY);
#pragma endregion

#pragma region 计算角度
		Mat blob, labels, stats, centroids;
		int num = 0;
		int nccomps = connectedComponentsWithStats(diff, labels, stats, centroids, 4);

		for (size_t i = 0; i < stats.rows; i++)
		{
			int h = stats.at<int>(i, CC_STAT_HEIGHT);
			int y = stats.at<int>(i, CC_STAT_TOP);
			if (h > MinLen&&h < MaxLen)
			{
				int ang = (y + h / 2 + 90) % 360;
				angle = ang;
				float s = sin(ang * CV_PI / 180);
				float c = cos(ang * CV_PI / 180);

				Point2f end = tempCenter + MaxRadius * Point2f(s, -c);

				line(dst, tempCenter, end, Scalar(0, 0, 255), 1);

				num++;
			}
		}
#pragma endregion

#pragma region 缺口绘制
		for (size_t i = 0; i < diff.rows; i++)
		{
			if (diff.at<uchar>(i, 0) != 1)
			{
				int ang = (i + 90) % 360;
				float s = sin(ang * CV_PI / 180);
				float c = cos(ang * CV_PI / 180);

				Point2f end = tempCenter + MaxRadius * Point2f(s, -c);
				Point2f sta = tempCenter + MinRadius * Point2f(s, -c);

				line(dst, sta, end, Scalar(0, 0, 255), 1);
			}
		}
#pragma endregion

#pragma region 结果判定
		if (num == 0)
		{
			MyTextOut("未找到合适缺口！", 1);
			return 1;
		}
#pragma endregion

#pragma region 结果返回
		output_Parameter_Float[0] = result;
		output_Parameter_Float[1] = angle;

		stringstream ss;
		ss << "角度:" << angle << "°" << endl;
		MyTextOut(ss.str().c_str(), 0);
#pragma endregion
	}
	catch (const std::exception&)
	{
		MyTextOut("算法异常退出！", 1);
		output_Parameter_Float[0] = 1;

		return 1;
	}
}

//MyTextOut
//PutTextIn
bool MyTextOut(string str, int color)
{
	Postion.vTest.push_back(str);
	Postion.vcolor.push_back(color);
	return 0;
}

/*
功能：输出文字
参数：
	str-待输出内容
	color-文字颜色，0为绿色，1为红色
*/
bool PutTextIn(string str, int color)
{
	int w = Postion.dst.cols;
	int h = Postion.dst.rows;
	//字体大小
	int text_Size;
	text_Size = ((w* h / 10000 - 30) * 0.078 + 25) * 2;
	//位置
	Point text_Localtion01;
	text_Localtion01.x = text_Size / 3;
	text_Localtion01.y = text_Size / 3 + Postion.lines*text_Size;

	if (color != 0)
	{
		putTextZH(Postion.dst, str.c_str(), text_Localtion01, Scalar(0, 0, 255), text_Size, "黑体", 0);
	}
	else
	{
		putTextZH(Postion.dst, str.c_str(), text_Localtion01, Scalar(0, 255, 0), text_Size, "黑体", 0);
	}

	Postion.lines += 1;
	return 0;
}