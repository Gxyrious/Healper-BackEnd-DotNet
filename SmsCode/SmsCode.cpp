#include "pch.h"

#include "SmsCode.h"

#include <iostream>
#include <random>

int SmsCode::RandomCodeGenerator::generateCode()
{
    std::random_device rd;
    std::mt19937 gen(rd());
    std::uniform_int_distribution<int> dis(1000, 9999);
    return dis(gen);
}
