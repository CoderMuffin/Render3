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
                component.parent = this;
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
            GetComponent<T>().parent = null;
            components.Remove(GetComponent<T>());
        }
        public SceneObject()
        {
            AddComponent(transform);
        }
        public Transform transform = new Transform();
    }
}
