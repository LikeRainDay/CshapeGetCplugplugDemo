using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CshapeGetCplugplugDemo
{
    static class Program
    {
        //注意使用DLL导入时 需要引用using System.Runtime.InteropServices;声明    extern 声明当前这个Add函数为外部引用
        [DllImport(@"E:\GitHubSample\CshapeGetCplugplugDemo\Debug\CplugplugDemo.dll", EntryPoint = "Add")]
        extern static int Add(int a, int b);
        //由于传递的是指针因此需要使用unsafe
        [DllImport(@"E:\GitHubSample\CshapeGetCplugplugDemo\Debug\CplugplugDemo.dll", EntryPoint = "WriteString")]
        extern unsafe static void WriteString(char* c);
        [DllImport(@"E:\GitHubSample\CshapeGetCplugplugDemo\Debug\CplugplugDemo.dll", EntryPoint = "AddInt")]
        extern unsafe static void AddInt(int* i);
        [DllImport(@"E:\GitHubSample\CshapeGetCplugplugDemo\Debug\CplugplugDemo.dll", EntryPoint = "AddIntArray")]
        extern unsafe static void AddIntArray(int* firstElement, int arraylength);
        [DllImport(@"E:\GitHubSample\CshapeGetCplugplugDemo\Debug\CplugplugDemo.dll", EntryPoint = "GetArrayFromCPP")]
        extern unsafe static int* GetArrayFromCPP();



        //（方便C++调用C#的方法

        //定义一个委托，返回值为空，存在一个整型参数）
        public delegate void CSCallback(int tick);
        //定义一个用于回调的方法，与前面定义的猥琐的原理一样
        //该方法会被C++所调用
        static void CSCallbackFunction(int tick)
        {
            Console.WriteLine(tick.ToString());
        }
        //定义一个委托类型的实例，
        //在主程序中该委托实例将指向前面定义个CSCallbackFunction方法
        static CSCallback callback;
        [DllImport(@"E:\GitHubSample\CshapeGetCplugplugDemo\Debug\CplugplugDemo.dll", EntryPoint = "SetCallback")]
        extern  static void SetCallback(CSCallback callback);

        //结构体传递
        /**
         *要传递的成员为公有的值类型字段
          *  C#中结构体字段类型与C++结构体中的字段类型相兼容
           * C#结构中的字段顺序与C++结构体中的字段顺序相同，要保证该功能，需要将C#结构体标记为[StructLayout( LayoutKind.Sequential)]
          */
          [StructLayout(LayoutKind.Sequential)]
          struct Vector3
        {
            public float X, Y, Z;
        }
        [DllImport(@"E:\GitHubSample\CshapeGetCplugplugDemo\Debug\CplugplugDemo.dll", EntryPoint = "SendStructFromCSToCPP")]
        extern static void SendStructFromCSToCPP(Vector3 vector);







        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            int c = Add(1, 2);
            Console.WriteLine(c);
            /**
             *unsafe需要在你项目右键 属性中设置 允许不安全代码
             */
            unsafe
            {
                //在传递字符串时，将字符串所在的内存固化
                //并取出字符串数组的指针 
                /**
                 *fixed 语句禁止垃圾回收器重定位可移动的变量。
                 * fixed 语句只能出现在不安全的上下文中。Fixed 还可用于创建固定大小的缓冲区。
                 */
                fixed (char*p = &("helo".ToCharArray()[0]))
                {
                    WriteString(p);
                }

                //调用C++的AddInt 方法
                int i = 10;
                AddInt(&i);
                Console.WriteLine(i);

                //调用C++中的AddIntArray方法将C#中的数据传递到C++中，并在C++中输出
                int[] CSArray = new int[10];
                for(int iArr = 0; iArr < 10; iArr++)
                {
                    CSArray[iArr] = iArr;
                }
                fixed(int * pCSArray = &CSArray[0])
                {
                    AddIntArray(pCSArray, 10);
                }
                //调用C++中的GetArrayFromCPP方法获取一个C++中建立的数组
                int* pArrayPointer = null;
                pArrayPointer = GetArrayFromCPP();
                for(int iarr = 0; iarr < 10; iarr++)
                {
                    Console.WriteLine(*pArrayPointer);
                    pArrayPointer++;
                }

            }

            //在CS的主程序中让callback指向CSCallbackFunction方法，代码如下所示：
            //调用委托所指向的方法
            callback = CSCallbackFunction;
            //将委托传递给C++  SetCallback方法被执行后，在C#中定义的CSCallbackFunction就会被C++所调用。
            SetCallback(callback);




            //建立一个Vector3的实例
            Vector3 vector = new Vector3() { X = 10, Y = 20, Z = 30 };
            SendStructFromCSToCPP(vector);



            Console.Read();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

  

    }
}
