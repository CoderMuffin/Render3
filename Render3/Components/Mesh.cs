using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render3.Core;
using Render3.Geometries;


namespace Render3.Components
{
    public class Mesh : Component
    {
        public IGeometry geometry
        {
            get { return _geometry; }
            set { if (_geometry != null) _geometry.Triangles.ForEach(x => x.mesh = null); _geometry = value; _geometry.Triangles.ForEach(x => x.mesh = this); }
        }
        private IGeometry _geometry;
        public Color color = new Color(0.5, 0.5, 0.5);
        public void UpdateMesh()
        {
            RecalculateWorldVertices();
        }

        public void RecalculateWorldVertices()
        {
            worldVertices.Clear();
            for (int i = 0; i < geometry.Vertices.Count; i++)
            {
                worldVertices.Add((sceneObject.transform.rotation * (geometry.Vertices[i] * sceneObject.transform.scale.ToPoint3())) + sceneObject.transform.position);
            }
        }
        //internal List<Point3> orderedTriangles= new List<Point3>();
        public List<Point3> worldVertices = new List<Point3>();
        public Mesh(IGeometry g)
        {
            geometry = g;
        }
    }
}
