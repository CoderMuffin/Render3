using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Core
{
    public class Face
    {
        private Point3[] pv = new Point3[3];
        public Point3[] vertices { get { return pv; } set { if (value.Length != 3) throw new System.ArgumentException("Render3.Core.Face.vertices: Array should be of length 3"); else pv = value; } }
        public Face(Point3 a, Point3 b, Point3 c)
        {
            vertices[0] = a;
            vertices[1] = b;
            vertices[2] = c;
        }
        public Face(Point3 a, Point3 b, Point3 c, Point3 center)
        {
            vertices[0] = a+center;
            vertices[1] = b+center;
            vertices[2] = c+center;
        }
    }
}
