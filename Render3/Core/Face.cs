namespace Render3.Core
{
    public class Face
    {
        public Components.Mesh mesh;
        public IGeometry geometry;
        private int[] pv = new int[3];
        public int[] Vertices { get { return pv; } set { if (value.Length != 3) throw new System.ArgumentException("Render3.Renderer.Face.vertices: Array should be of length 3"); else pv = value; } }
        public Direction3 normal
        {
            get
            {
                Point3 V = geometry.Vertices[Vertices[1]] - geometry.Vertices[Vertices[0]];
                Point3 W = geometry.Vertices[Vertices[2]] - geometry.Vertices[Vertices[0]];
                Direction3 n;
                n.x = ((V.y * W.z) - (V.z * W.y));
                n.y = ((V.z * W.x) - (V.x * W.z));
                n.z = ((V.x * W.y) - (V.y * W.x));
                return n.normalized;
            }
        }
        public Direction3 WorldNormal
        {
            get
            {
                Point3 V = mesh.worldVertices[Vertices[1]] - mesh.worldVertices[Vertices[0]];
                Point3 W = mesh.worldVertices[Vertices[2]] - mesh.worldVertices[Vertices[0]];
                Direction3 wn;
                wn.x = ((V.y * W.z) - (V.z * W.y));
                wn.y = ((V.z * W.x) - (V.x * W.z));
                wn.z = ((V.x * W.y) - (V.y * W.x));
                return wn.normalized;
            }
        }
        public Face(IGeometry g, int a, int b, int c)
        {
            geometry = g;
            Vertices[0] = a;
            Vertices[1] = b;
            Vertices[2] = c;
        }
        /// <summary>
        /// Geometric center. Note: This is not the real time center, just the build. If you want the real time center, use worldCenter.
        /// </summary>
        public Point3 center
        {
            get
            {
                Point3 c;
                c.x = (geometry.Vertices[Vertices[0]].x + geometry.Vertices[Vertices[1]].x + geometry.Vertices[Vertices[2]].x) / 3;
                c.y = (geometry.Vertices[Vertices[0]].y + geometry.Vertices[Vertices[1]].y + geometry.Vertices[Vertices[2]].y) / 3;
                c.z = (geometry.Vertices[Vertices[0]].z + geometry.Vertices[Vertices[1]].z + geometry.Vertices[Vertices[2]].z) / 3;
                return c;
            }
        }
        /// <summary>
        /// The center of the face on the mesh.
        /// </summary>
        public Point3 WorldCenter
        {
            get
            {
                Point3 wc;
                wc.x = (mesh.worldVertices[Vertices[0]].x + mesh.worldVertices[Vertices[1]].x + mesh.worldVertices[Vertices[2]].x) / 3;
                wc.y = (mesh.worldVertices[Vertices[0]].y + mesh.worldVertices[Vertices[1]].y + mesh.worldVertices[Vertices[2]].y) / 3;
                wc.z = (mesh.worldVertices[Vertices[0]].z + mesh.worldVertices[Vertices[1]].z + mesh.worldVertices[Vertices[2]].z) / 3;
                return wc;
            }
        }
    }
}
