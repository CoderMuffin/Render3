using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Core
{
    public interface IGeometry
    {
        List<Face> Triangles { get; set; }
        List<Point3> Vertices { get; set; }
    }
}
