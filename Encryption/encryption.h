#pragma once

#include "pch.h"
#include <string>
#include <cstring>
using std::string;

extern "C" __declspec(dllexport) const char* encryption(const char* message);

extern "C" __declspec(dllexport) void release(const char* ptr);