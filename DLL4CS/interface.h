#pragma once



#include "CVAlgorithms.h"

struct BmpBuf
{
	unsigned char* data_Output;
	int size;
	unsigned char* data_Input;
	int h;
	int w;

};

//bool MV_TextOut(string str, int color);
bool MV_TextOut(char* str, int color);