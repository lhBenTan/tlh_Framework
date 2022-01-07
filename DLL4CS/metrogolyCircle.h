//#include "stdafx.h"
#include "pch.h"
/**************************************************************************
**File name		: metrogolyCircle.h
**Author		: Bigshow Zheng
**Date			: 2021-03-16
**Description   : find circle
**
**History       :
**
**
*******************************************************************************/
#pragma once
#include <vector>
#include <math.h>
#include "opencv2/opencv.hpp"


/**
*@brief   ������Բ�Ļ�������
*@ A	  ��Բ��X����
*@ B	  ��Բ��Y����
*@ C	  ��Բ�İ��
*@ Info	  ���жϹ���Բ�Ƿ�ɹ�
*/
typedef struct circleInfo
{
	float A;							                 //����a
	float B;							                 //����b
	float C;							                 //����c
	bool Info;						                     //�Ƿ��ҵ�Բ�ı�־λ

	circleInfo() :A(), B(), C(), Info() {}               //�޲����Ĺ��캯�������ʼ��ʱ����
	circleInfo(float a, float b, float c, bool is_info)
		:A(a), B(b), C(c), Info(is_info) {}              //�вι���
};



/**
*@brief                   : ��ȡROIԲ��
*@inputPic                ������ͼƬ
*@mainCircle              : �������λԲ�ġ��뾶
*@nPerpendicularTHreshold : ����Բ��ֱƫ����Ҿ���
*@nTangetialThreshold     ������Բ����ƫ����Ҿ���
*@nMeasureThreshold       �������ݶȲ�����ֵ
*@stOutPoint              ��������ҵ��ĵ�
*@return                  ����
*/
void gen_Metrology_Model_circle(cv::Mat &inputPic, cv::Vec3i &mainCircle,
	int nPerpendicularTHreshold, int nTangetialThreshold,
	int nMeasureThreshold, std::vector<cv::Point> &stOutPoint);


/**
*@brief       : ���Բ��Ϣ
*@input       ������ͼƬ
*@allPoints   ����������е�___gen_Metrology_Model_circle�����еĲ���stOutPoint
*@maxIterCnt  ����������
*@disTH       ����ɢ������ֵ
*@minScore    ����ͷ�ֵ
*@inPoints    ��ʹ�õ㼯
*@fitCircle   ����ϵ�Բ
*@return      ����
*/
void ransc_fit_circle(cv::Mat &input, std::vector<cv::Point> &allPoints,
	int maxIterCnt, float disTH, float minScor, std::vector<cv::Point> & inPoints, circleInfo& fitCircle);


/**call function

	 cv::Mat img = cv::imread("./11.bmp"));
	 if (img.channels() != 1) cv::cvtColor(img, img, cv::COLOR_BGR2GRAY);
	 cv::Vec3i vecTmp = { 1965, 627, 50 };
	 int nV = 30, nH = 5, nThreshold = 10;
	 std::vector<cv::Point> stOutPt;
	 gen_Metrology_Model_circle(img, vecTmp, nV, nH, nThreshold, stOutPt);

	 std::vector < cv::Point> inPoints; inPoints.clear();
	 circleInfo fitCircle;
	 ransc_fit_circle(img, stOutPt, 100, 60, 30, inPoints, fitCircle);

	 cv::circle(img, cv::Point(fitCircle.A, fitCircle.B), fitCircle.C, cv::Scalar(255));

*/