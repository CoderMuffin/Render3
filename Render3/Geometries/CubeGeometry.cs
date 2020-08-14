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
        public CubeGeometry() :

            base(
                //bottom
                new Face(new Point3(1, 1, 1), new Point3(1, 1, -1), new Point3(-1, 1, -1)),
                new Face(new Point3(1, 1, 1), new Point3(-1, 1, 1), new Point3(-1, 1, -1)),
                //front
                new Face(new Point3(-1, -1, -1), new Point3(1, 1, -1), new Point3(-1, 1, -1)),
                new Face(new Point3(-1, -1, -1), new Point3(1, 1, -1), new Point3(1, -1, -1)),
                //left
                new Face(new Point3(-1, -1, -1), new Point3(-1, 1, 1), new Point3(1, 1, -1)),
                new Face(new Point3(-1, -1, -1), new Point3(-1, 1, 1), new Point3(1, -1, 1)),
                //top
                new Face(new Point3(-1, -1, -1), new Point3(-1, -1, 1), new Point3(1, -1, 1)),
                new Face(new Point3(-1, -1, -1), new Point3(1, -1, -1), new Point3(1, -1, 1)),
                //back
                new Face(new Point3(1, 1, 1), new Point3(-1, -1, 1), new Point3(1, -1, 1)),
                new Face(new Point3(1, 1, 1), new Point3(-1, -1, 1), new Point3(-1, 1, 1)),
                //right
                new Face(new Point3(1, 1, 1), new Point3(1, -1, -1), new Point3(-1, -1, 1)),
                new Face(new Point3(1, 1, 1), new Point3(1, -1, -1), new Point3(-1, 1, -1))
            )

        {
        }
    }
}
