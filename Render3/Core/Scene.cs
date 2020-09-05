using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render3.Components;
namespace Render3.Core
{
    public class Scene
    {
        public Camera camera;
        public Light light=new Light();
        public List<SceneObject> objects = new List<SceneObject>();
        public Scene(Camera c)
        {
            camera=c;
        }
    }
}
