using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Core
{
    public class Scene
    {
        public Camera camera;
        public List<Mesh> meshes = new List<Mesh>();
        public Scene()
        {

        }
    }
}
