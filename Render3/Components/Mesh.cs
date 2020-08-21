using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render3.Core;
using Render3.Geometries;

namespace Render3.Components
{
    public class Mesh : Components.Component
    {
        public IGeometry geometry;


        public void RecalculateWorldVertices()
        {
            worldVertices.Clear();
            for (int i = 0; i < geometry.vertices.Count; i++)
            {
                worldVertices.Add((parent.transform.rotation * (geometry.vertices[i] * parent.transform.scale.ToPoint3())) + parent.transform.position);
            }
        }
        public List<Point3> worldVertices=new List<Point3>();
        public Mesh(IGeometry g)
        {
            geometry = g;
        }
    }
}
