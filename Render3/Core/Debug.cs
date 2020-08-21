using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Core
{
    public static class Debug
    {
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
        public static void PrintArray<T>(List<T> objs)
        {
            foreach (T obj in objs)
            {
                Print(obj);
            }
        }
    }
}
