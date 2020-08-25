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
        public List<Face> triangles { get; set; }
        public BaseGeometry(params Face[] triangles)
        {
            vertices = new List<Point3>();
            this.triangles = new List<Face>();
            this.triangles.AddRange(triangles);
            CalculateFaceNormals();
            CalculateFaceCenters();
        }
        public void CalculateFaceNormals()
        {
            foreach (Face f in triangles)
            {
                Point3 V = vertices[f.vertices[1]] - vertices[f.vertices[0]];
                Point3 W = vertices[f.vertices[2]] - vertices[f.vertices[0]];
                f.normal.x = ((V.y * W.z) - (V.z * W.y));
                f.normal.y = ((V.z * W.x) - (V.x * W.z));
                f.normal.z = ((V.x * W.y) - (V.y * W.x));
                f.normal.Normalize();
            }
        }
        public void CalculateFaceCenters()
        {
            foreach (Face f in triangles)
            {
                double x = (vertices[f.vertices[0]].x + vertices[f.vertices[1]].x + vertices[f.vertices[0]].x) / 3;
                double y = (vertices[f.vertices[0]].y + vertices[f.vertices[1]].y + vertices[f.vertices[2]].y) / 3;
                double z = (vertices[f.vertices[0]].z + vertices[f.vertices[1]].z + vertices[f.vertices[2]].z) / 3;
                f.center = new Point3(x, y, z);
            }
        }
    }
}
