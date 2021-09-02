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
                throw new NullReferenceException("Render3.Core.AddComponent(): Component was null");
            }
            if (GetComponent<T>() == null)
            {
                components.Add(component);
                component.sceneObject = this;
                return;
            }
            throw new InvalidOperationException("Render3.Core.AddComponent(): Component " + component.GetType().FullName + " is already added");
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
                throw new InvalidOperationException("Render3.Core.RemoveComponent(): The object does not have this component");
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
        }
        public SceneObject()
        {
        }
        internal void UpdateChildGlobals()
        {
            foreach (SceneObject child in children)
            {
                Point3 rotated = (worldRotation * (child._localPosition * worldScale.ToPoint3()));
                child._position = (worldPosition + rotated);
                child._rotation = (worldRotation * child.localRotation);
                //if (child.rotation.Dot())
                {
                    //   child._rotation = (rotation * child.localRotation).Normalized.Negated;
                }
                child._scale = worldScale * child.localScale;
                if (child.GetComponent<Mesh>() != null)
                {
                    child.GetComponent<Mesh>().UpdateMesh();
                }
                child.UpdateChildGlobals();
            }
        }

        public void ForceRecalcLocals()
        {
            localPosition = localPosition;
            localRotation = localRotation;
            localScale = localScale;
        }

        #region Global transform modifiers
        private Point3 _position;
        public Point3 worldPosition
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                /*if (parent != null)
                {
                    _localPosition = value - parent.position;
                }
                else
                {
                    _localPosition = value;
                }*/
                UpdateChildGlobals();
                if (GetComponent<Mesh>() != null)
                {
                    GetComponent<Mesh>().UpdateMesh();
                }
            }
        }
        public Direction3 worldScale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
                if (parent != null)
                {
                    _localScale = value / parent.worldScale;
                }
                else
                {
                    _localScale = value;
                }
                UpdateChildGlobals();
                if (GetComponent<Mesh>() != null)
                {
                    GetComponent<Mesh>().UpdateMesh();
                }
            }
        }
        private Direction3 _scale = new Direction3(1, 1, 1);
        public Quaternion worldRotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
                /*if (parent != null)
                {
                    _localRotation = value / parent.rotation;
                }
                else
                {
                    _localRotation = value;
                }*/
                UpdateChildGlobals();
                if (GetComponent<Mesh>() != null)
                {
                    GetComponent<Mesh>().UpdateMesh();
                }
            }
        }
        private Quaternion _rotation = Quaternion.Identity;
        #endregion Global transform qualifiers
        #region Local transform modifiers
        private Point3 _localPosition;
        public Point3 localPosition
        {
            get
            {
                if (parent == null)
                {
                    return worldPosition;
                }
                else
                {
                    return worldPosition - parent.worldPosition;
                }
            }
            set
            {
                _localPosition = value;
                if (parent == null)
                {
                    worldPosition = value;
                }
                else
                {
                    Point3 rotated = (parent.worldRotation * value);
                    worldPosition = parent.worldPosition + (rotated);
                }
                UpdateChildGlobals();
                if (GetComponent<Mesh>() != null)
                {
                    GetComponent<Mesh>().UpdateMesh();
                }
            }
        }
        private Direction3 _localScale = new Direction3(1);
        public Direction3 localScale
        {
            get
            {
                if (parent == null)
                {
                    return worldScale;
                }
                else
                {
                    return _localScale;
                }
            }
            set
            {
                _localScale = value;
                if (parent == null)
                {

                    worldScale = value;
                }
                else
                {
                    worldScale = parent.worldScale * value;
                }

                UpdateChildGlobals();
                if (GetComponent<Mesh>() != null)
                {
                    GetComponent<Mesh>().UpdateMesh();
                }
            }
        }
        private Quaternion _localRotation = Quaternion.Identity;
        public Quaternion localRotation
        {
            get
            {
                if (parent == null)
                {
                    return worldRotation;
                }
                else
                {
                    return _localRotation;
                }
            }
            set
            {

                _localRotation = value;
                if (parent == null)
                {
                    worldRotation = value;
                }
                else
                {
                    worldRotation = parent.worldRotation * value;
                }

                UpdateChildGlobals();
                if (GetComponent<Mesh>() != null)
                {
                    GetComponent<Mesh>().UpdateMesh();
                }
            }
        }
        #endregion Local transform qualifiers
        public Direction3 Forward
        {
            get { return worldRotation * new Direction3(0, 0, 1); }
        }
        public Direction3 Right
        {
            get { return worldRotation * new Direction3(1, 0, 0); }
        }
        public Direction3 Up
        {
            get { return worldRotation * new Direction3(0, 1, 0); }
        }
        public SceneObject parent { get { return _parent; } set { if (_parent!=null) _parent._children.RemoveAll(x => x.UUID == UUID); if (value!=null) value._children.Add(this); _parent = value; ForceRecalcLocals(); } }
        private SceneObject _parent;
        private List<SceneObject> _children = new List<SceneObject>();
    }
}
