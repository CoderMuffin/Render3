using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Components
{
    public abstract class Component
    {
        public bool enabled { get { return _enabled; } set { this._enabled = false; EnabledChanged(value); } }

        protected virtual void EnabledChanged(bool value)
        {
            
        }

        protected bool _enabled=true;
        public Core.SceneObject sceneObject;
        public Core.SceneObject DefaultObject()
        {
            Core.SceneObject c = new Core.SceneObject();
            c.AddComponent(this,GetType());
            return c;
        }
        public Component()
        {
        }
        public Component(Core.SceneObject parent)
        {
            this.sceneObject = parent;
        }
    }
}
