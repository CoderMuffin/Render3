using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render3.Core;
using Render3.Renderer;
namespace Render3.Geometries
{
    public class BaseGeometry:IGeometry
    {
        public List<Point3> vertices { get; set; }
        public List<Face> triangles { get { return t; } set { t = value; } }
        private List<Face> t;
        public BaseGeometry(params Face[] triangles)
        {
            vertices = new List<Point3>();
            this.triangles = new List<Face>();
            this.triangles.AddRange(triangles);
        }
        public void CalculateFaceNormals()
        {
            foreach (Face f in triangles)
            {
                Point3 V = f.vertices[1] - f.vertices[0];
                Point3 W = f.vertices[2] - f.vertices[1];
                f.normal.x = ((V.y * W.z) - (V.z * W.y));
                f.normal.y = ((V.z * W.x) - (V.x * W.z));
                f.normal.y = ((V.x * W.y) - (V.y * W.x));
            }
        }
    }
}
