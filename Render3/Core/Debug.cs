using System;
using System.Collections.Generic;

namespace Render3.Core
{
    public static class Debug
    {
        public static bool enableLog = true;
        internal static bool crashed = false;
        public static void Print<T>(T obj)
        {
            if (enableLog)
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
        public static void PrintList<T>(List<T> objs)
        {
            foreach (T obj in objs)
            {
                Print(obj);
            }
        }
        public static void Dump(this object o)
        {
            Print(o);
        }
    }
}
