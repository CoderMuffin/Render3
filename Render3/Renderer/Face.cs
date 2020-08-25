using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render3.Core;
namespace Render3.Renderer
{
    public class Face
    {
        private int[] pv = new int[3];
        public int[] vertices { get { return pv; } set { if (value.Length != 3) throw new System.ArgumentException("Render3.Renderer.Face.vertices: Array should be of length 3"); else pv = value; } }
        public Direction3 normal;
        public Direction3 worldNormal;
        public Face(int a, int b, int c)
        {
            vertices[0] = a;
            vertices[1] = b;
            vertices[2] = c;
        }
        /// <summary>
        /// Geometric center. Note: This is not the realtime center, just the build. If you want the real time center, use worldCenter.
        /// </summary>
        public Point3 center;
        /// <summary>
        /// The center of the face on the mesh.
        /// </summary>
        public Point3 worldCenter;
    }
}
