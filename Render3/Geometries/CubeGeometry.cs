using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render3.Core;
namespace Render3.Geometries
{
    public class CubeGeometry : BaseGeometry
    {
        public CubeGeometry() : base()
        {
            vertices.AddRange(new Point3[]{
                new Point3(-1, -1, -1),
                new Point3(1, -1, -1),
                new Point3(1, 1, -1),
                new Point3(-1, 1, -1),
                new Point3(-1, 1, 1),
                new Point3(1, 1, 1),
                new Point3(1, -1, 1),
                new Point3(-1, -1, 1),
            });
            triangles.AddRange(new Face[]
            {
                new Face(0, 2, 1), //face front
	            new Face(0, 3, 2),
                new Face(2, 3, 4),//face top
	            new Face(2, 4, 5),
                new Face(1, 2, 5),//face right
	            new Face(1, 5, 6),
                new Face(0, 7, 4),//face left
	            new Face(0, 4, 3),
                new Face(5, 4, 7),//face back
	            new Face(5, 7, 6),
                new Face(0, 6, 7),//face bottom
	            new Face(0, 1, 6),
            });
        }
    }
}
