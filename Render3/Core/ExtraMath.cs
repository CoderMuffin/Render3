using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Core
{
    public static class ExtraMath
    {
        public static double Lerp(double a, double b, double t)
        {
            t = t < 0 ? 0 : t;
            t = t > 1 ? 1 : t;
            return a + (b - a) * t;
        }
        public static bool BetweenInclusive(double a, double x, double b)
        {
            if (a>b)
            {
                double tmp = a;
                a = b;
                b = tmp;
            }
            return a <= x && x <= b;
        }
        public static bool BetweenExclusive(double a, double x, double b)
        {
            if (a > b)
            {
                double tmp = a;
                a = b;
                b = tmp;
            }
            return a < x && x < b;
        }
    }
}
