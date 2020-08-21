using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Components
{
    public class Component
    {
        public Core.SceneObject parent;
        public Component()
        {
        }
        public Component(Core.SceneObject parent)
        {
            this.parent = parent;
        }
    }
}
