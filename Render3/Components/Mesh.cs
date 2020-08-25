using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render3.Core;
using Render3.Geometries;
using Render3.Renderer;

namespace Render3.Components
{
    public class Mesh : Components.Component
    {
        public IGeometry geometry;

        public void UpdateMesh()
        {
            RecalculateWorldVertices();
            geometry.CalculateFaceCenters();
            geometry.CalculateFaceNormals();
            RecalculateWorldFaceCenters();
            RecalculateWorldFaceNormals();
        }

        public void RecalculateWorldVertices()
        {
            worldVertices.Clear();
            for (int i = 0; i < geometry.vertices.Count; i++)
            {
                worldVertices.Add((sceneObject.transform.rotation * (geometry.vertices[i] * sceneObject.transform.scale.ToPoint3())) + sceneObject.transform.position);
            }
        }

        public void RecalculateWorldFaceCenters()
        {
            foreach (Face f in geometry.triangles)
            {
                double x = (worldVertices[f.vertices[0]].x + worldVertices[f.vertices[1]].x + worldVertices[f.vertices[2]].x) / 3;
                double y = (worldVertices[f.vertices[0]].y + worldVertices[f.vertices[1]].y + worldVertices[f.vertices[2]].y) / 3;
                double z = (worldVertices[f.vertices[0]].z + worldVertices[f.vertices[1]].z + worldVertices[f.vertices[2]].z) / 3;
                f.worldCenter = new Point3(x, y, z);
            }
        }

        public void RecalculateWorldFaceNormals()
        {
            foreach (Face f in geometry.triangles)
            {
                Point3 V = worldVertices[f.vertices[1]] - worldVertices[f.vertices[0]];
                Point3 W = worldVertices[f.vertices[2]] - worldVertices[f.vertices[0]];
                f.worldNormal.x = ((V.y * W.z) - (V.z * W.y));
                f.worldNormal.y = ((V.z * W.x) - (V.x * W.z));
                f.worldNormal.z = ((V.x * W.y) - (V.y * W.x));
                f.worldNormal.Normalize();
            }
        }
        public List<Point3> worldVertices=new List<Point3>();
        public Mesh(IGeometry g)
        {
            geometry = g;
        }
    }
}
