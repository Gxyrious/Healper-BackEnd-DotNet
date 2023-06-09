#include "pch.h"
#include "encryption.h"

#include <string>
#include <cstring>
#include "md5.h"

using std::string;

const char* encryption(const char* msg)
{
	string res = MD5(msg).toStr();

	char* resPtr = new char[res.length() + 1];
	strncpy_s(resPtr, res.length() + 1, res.c_str(), res.length());
	resPtr[res.length()] = '\0';
	return resPtr;
}

void release(const char* ptr)
{
	delete[] ptr;
}