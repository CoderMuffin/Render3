using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Render3.Core
{
    public static class Debug
    {
        internal static bool crashed = false;
        public static void Print<T>(T obj)
        {
            Console.WriteLine(obj);
        }
        public static void PrintArray<T>(T[] objs)
        {
            foreach (T obj in objs)
            {
                Print(obj);
            }
        }
        internal static void LaunchErrorForm()
        {
            if (crashed) return;
            crashed = true;
            System.Windows.Forms.Application.Run(new CrashForm());
        }
        public static void PrintArray<T>(List<T> objs)
        {
            foreach (T obj in objs)
            {
                Print(obj);
            }
        }
    }
}
