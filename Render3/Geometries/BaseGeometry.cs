using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render3.Core;
namespace Render3.Geometries
{
    public class BaseGeometry:IGeometry
    {

        public List<Point3> vertices { get; set; }
        public List<Face> triangles { get { return t; } set { t = value; RecalculateVertices(); } }
        private List<Face> t;
        public void RecalculateVertices()
        {
            vertices = new List<Point3>();
            foreach (Face f in t)
            {
                vertices.AddRange(f.vertices);
            }
        }
        public BaseGeometry(params Face[] triangles)
        {
            this.triangles = new List<Face>();
            this.triangles.AddRange(triangles);
            RecalculateVertices();
        }
    }
}
