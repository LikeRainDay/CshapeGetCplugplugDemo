#pragma once
///TESTCPPDLL_API 定义了后面的内容，当然也可以不使用"__declspec(dllexport)"意思是将后面修饰的内容定义为DLL中要导出的内容
#define TESTCPPDLL_API __declspec(dllexport)
//extern "C"代表了该函数在编译和连接时使用C语言的方式，以保证函数名不变
extern "C" TESTCPPDLL_API int  __stdcall Add(int a, int b);


//__stdcall 声明调用方法
extern "C" TESTCPPDLL_API void __stdcall WriteString(wchar_t* content);

//指针的传递
//传入一个整型指针，并将其所有指向的内容加1
extern "C" TESTCPPDLL_API void __stdcall AddInt(int *i);
//传入一个整型数组指针以及数组长度，遍历每一个元素并输出
extern "C" TESTCPPDLL_API void __stdcall AddIntArray(int *firstname,int arraylength);
//在C++中生成一个整型数组，并且数组指针返回给C#
extern "C" TESTCPPDLL_API int* __stdcall GetArrayFromCPP();

//定义一个函数指针
typedef void(__stdcall *CPPCallback)(int tick);
//定义一个用来设置函数指针的方法，并在该函数中调用C#中传递过来的委托
extern "C" TESTCPPDLL_API void SetCallback(CPPCallback callback);


struct  Vector3
{
	float X, Y, Z;
};
extern "C" TESTCPPDLL_API void __stdcall SendStructFromCSToCPP(Vector3 vector3);
