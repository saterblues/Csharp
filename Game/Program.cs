using System;
using System.Collections.Generic;

using Game.System.Data;
using Game.System.Logic;
using Game.Demo;

namespace Game
{
    class Program
    {
        

        static void Main(string[] args)
        {

            Console.WriteLine("-------测试开始-------");
            try
            {
                test();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0},{1}", ex.Message, ex.StackTrace);
            }
            Console.WriteLine("-------测试结束-------");
            Console.ReadLine();
        }

        struct AAA {
            public AAA(List<int> aaa) { this.aaa = aaa; }
            public List<int> aaa;
        }

        static AAA GetAAA(AAA aa) {
            return aa;
        }

        static void test() {
            DemoTest demo = new DemoTest();
            demo.Test();
        }
    }
}
