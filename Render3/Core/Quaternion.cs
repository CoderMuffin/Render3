using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Core
{
    public class Quaternion
    {
        public double w = 0;
        public double x = 0;
        public double y = 0;
        public double z = 0;
        public static readonly Quaternion identity = new Quaternion(0,0,0,0);
        public Quaternion(double w,double x,double y, double z)
        {
            this.w = w;
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
