using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Csharp.Liqing.Log
{
    public class LQLog
    {
        public static void Log(string text)
        {
            System.Console.WriteLine(text);
        }
        public static void Log(string format,params object[] objs)
        {
            Log(string.Format(format, objs));
        }
    }
}
