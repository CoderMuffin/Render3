using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render3.Core;

namespace Render3.Geometries
{
    public class BaseGeometry:Render3Object,IGeometry
    {
        public static BaseGeometry CubeGeometry()
        {
            BaseGeometry bg = new BaseGeometry();

            bg.Vertices.AddRange(new Point3[]{
                new Point3(-1, -1, -1),
                new Point3(1, -1, -1),
                new Point3(1, 1, -1),
                new Point3(-1, 1, -1),
                new Point3(-1, 1, 1),
                new Point3(1, 1, 1),
                new Point3(1, -1, 1),
                new Point3(-1, -1, 1),
            });
            bg.triangles.AddRange(new Face[]
            {
                new Face(bg, 0, 2, 1), //face front
	            new Face(bg, 0, 3, 2),
                new Face(bg, 2, 3, 4),//face top
	            new Face(bg, 2, 4, 5),
                new Face(bg, 1, 2, 5),//face right
	            new Face(bg, 1, 5, 6),
                new Face(bg, 0, 7, 4),//face left
	            new Face(bg, 0, 4, 3),
                new Face(bg, 5, 4, 7),//face back
	            new Face(bg, 5, 7, 6),
                new Face(bg, 0, 6, 7),//face bottom
	            new Face(bg, 0, 1, 6),
            });
            return bg;
        }
        public List<Point3> Vertices { get; set; }
        public List<Face> triangles { get; set; }
        public BaseGeometry(params Face[] triangles)
        {
            Vertices = new List<Point3>();
            this.triangles = new List<Face>();
            this.triangles.AddRange(triangles);
        }
    }
}
