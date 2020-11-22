using Render3.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Core
{
    public class SceneObject : Render3Object
    {
        public System.Collections.ObjectModel.ReadOnlyCollection<SceneObject> children { get { _children.RemoveAll(x => x.parent != this); return _children.AsReadOnly(); } }
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
            if (component == null)
            {
                throw new NullReferenceException("Render3.Core.SceneObject.AddComponent(): Component was null");
            }
            if (GetComponent<T>() == null)
            {
                components.Add(component);
                component.sceneObject = this;
                return;
            }
            throw new InvalidOperationException("Render3.Core.SceneObject.AddComponent(): Component " + component.GetType().FullName + " is already added");
        }
        private void AC<T>(T c) where T : Component
        {
            AddComponent(c);
        }
        private T GC<T>() where T : Component
        {
            return GetComponent<T>();
        }
        public void AddComponent(Component component, Type t)
        {
            GetType().GetMethod("AC", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(t).Invoke(this, new object[] { component });
        }
        public Component GetComponent(Component component, Type t)
        {
            return (Component)GetType().GetMethod("GC", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreReturn).MakeGenericMethod(t).Invoke(this, new object[] { component });
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
        public List<T> GetDescendantComponents<T>(bool requireEnabled = false) where T : Component
        {
            List<T> cc = new List<T>();
            foreach (SceneObject child in children)
            {
                foreach (T c in child.GetDescendantComponents<T>(requireEnabled))
                {
                    cc.Add(c);
                }
                if (child.GetComponent<T>() != null)
                {
                    if (requireEnabled && child.GetComponent<T>().enabled)
                        cc.Add(child.GetComponent<T>());
                    else if (!requireEnabled)
                        cc.Add(child.GetComponent<T>());
                }
            }
            return cc;
        }
        public SceneObject(SceneObject parent)
        {
            this.parent = parent;
            AddComponent(transform);
        }
        public SceneObject()
        {
            AddComponent(transform);
        }
        public Transform transform = new Transform();
        public SceneObject parent { get { return _parent; } set { if (_parent!=null) _parent._children.RemoveAll(x => x.UUID == UUID); if (value!=null) value._children.Add(this); _parent = value; transform.ForceRecalcLocals(); } }
        private SceneObject _parent;
        private List<SceneObject> _children = new List<SceneObject>();
    }
}
