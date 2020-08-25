using Render3.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Core
{
    public class SceneObject
    {
        public List<SceneObject> children { get { _children.RemoveAll(x=>x.parent != this); return _children; } set { _children.RemoveAll(x => x.parent != this); _children = value; } }
        public List<Component> components = new List<Component>();
        public T GetComponent<T>() where T : Component
        {
            foreach (Component c in components)
            {
                if (c is T) return (T)c;
            }
            return null;
        }
        public void AddComponent<T>(T component) where T : Component
        {
            if (component==null)
            {
                throw new NullReferenceException("Render3.Core.SceneObject.AddComponent(): Component was null");
            }
            if (GetComponent<T>()==null)
            {
                components.Add(component);
                component.sceneObject = this;
                return;
            }
            throw new InvalidOperationException("Render3.Core.SceneObject.AddComponent(): Component is already added");
        }
        public void RemoveComponent<T>() where T : Component
        {
            if (GetComponent<T>() == null)
            {
                throw new InvalidOperationException("Render3.Core.SceneObject.RemoveComponent(): The object does not have this component");
            }
            GetComponent<T>().sceneObject = null;
            components.Remove(GetComponent<T>());
        }
        public SceneObject(SceneObject parent)
        {
            this.parent = parent;
            AddComponent(transform);
        }
        public SceneObject() {
            AddComponent(transform);
        }
        public Transform transform = new Transform();
        public SceneObject parent { get { return _parent; } set { value.children.Add(this); _parent = value; } }
        private SceneObject _parent;
        private List<SceneObject> _children = new List<SceneObject>();
    }
}
