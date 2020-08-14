using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Core
{
    public class Mesh
    {
        public IGeometry geometry;
        public Point3 position;
        public Point3 eulerAngles;
        public Quaternion rotation;
        public Mesh(IGeometry g)
        {
            geometry = g;
        }
    }
}
