using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render3.Renderer;
namespace Render3.Core
{
    public interface IGeometry
    {
        List<Face> triangles { get; set; }
        List<Point3> vertices { get; set; }
        void CalculateFaceNormals();
    }
}
