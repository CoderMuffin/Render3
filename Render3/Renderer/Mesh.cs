using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render3.Core;
namespace Render3.Renderer
{
    public class Mesh
    {
        public IGeometry geometry;
        public Point3 position
        {
            get
            {
                return p;
            }
            set
            {
                p = value;
                RecalculateWorldVertices();
                geometry.CalculateFaceNormals();
            }
        }

        public void RecalculateWorldVertices()
        {
            worldVertices.Clear();
            for (int i = 0; i < geometry.vertices.Count; i++)
            {
                worldVertices.Add(geometry.vertices[i] + position);
            }
        }

        private Point3 p;
        public Point3 eulerAngles;
        public Quaternion rotation;
        public List<Point3> worldVertices=new List<Point3>();
        public Mesh(IGeometry g)
        {
            geometry = g;
        }
    }
}
