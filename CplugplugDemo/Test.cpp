#include "stdafx.h"
#include "Test.h"
#include <iostream>

TESTCPPDLL_API int __stdcall Add(int a, int b) {

	return a + b;
}

TESTCPPDLL_API void __stdcall WriteString(wchar_t* content) {

	std::cout << content;
}


TESTCPPDLL_API void __stdcall AddInt(int *i) {

	(*i)++;
 }
TESTCPPDLL_API void __stdcall AddIntArray(int *firstname, int arraylength) {
	int * currentPointer = firstname;
	for (int i = 0; i < arraylength; i++) {
		std::cout << *currentPointer;
		currentPointer++;
	}
	std::cout << std::endl;

 }

int *arrPtr;
TESTCPPDLL_API int* __stdcall GetArrayFromCPP() {
	arrPtr = new int[10];
	for (int i = 0; i < 10; i++)
	{
		arrPtr[i] = i;
	}
	return arrPtr;
 }


TESTCPPDLL_API void SetCallback(CPPCallback callback) {
	int tick = 50;
	//下面代码是对C#中委托进行调用
	callback(tick);
}

TESTCPPDLL_API void __stdcall SendStructFromCSToCPP(Vector3 vector3) {
	std::cout << "got vector3 in cpp,x:";

	std::cout << vector3.X;
	std::cout << ",Y:";
	std::cout << vector3.Y;
	std::cout << ",Z:";
	std::cout << vector3.Z;

}