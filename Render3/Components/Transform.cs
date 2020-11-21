using Render3.Core;
using System;
namespace Render3.Components
{
    public class Transform : Render3.Components.Component
    {
        public void UpdateChildLocals()
        {
            foreach (SceneObject child in sceneObject.children)
            {
                Point3 rotated = (rotation * child.transform._assignedLP);
                child.transform._position = position + (rotated);
                child.transform._rotation = (rotation*child.transform.localRotation);
                //if (child.transform.rotation.Dot())
                {
                 //   child.transform._rotation = (rotation * child.transform.localRotation).Normalized.Negated;
                }
                child.transform._scale = scale*child.transform.localScale;
                if (child.GetComponent<Mesh>() != null)
                {
                    child.GetComponent<Mesh>().UpdateMesh();
                }
                child.transform.UpdateChildLocals();
            }
        }
        #region Global transform modifiers
        private Point3 _position;
        public Point3 position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;

                UpdateChildLocals();
                if (sceneObject.GetComponent<Mesh>() != null)
                {
                    sceneObject.GetComponent<Mesh>().UpdateMesh();
                }
            }
        }
        public Direction3 scale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
                UpdateChildLocals();
                if (sceneObject.GetComponent<Mesh>() != null)
                {
                    sceneObject.GetComponent<Mesh>().UpdateMesh();
                }
            }
        }
        private Direction3 _scale = new Direction3(1, 1, 1);
        public Quaternion rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
                UpdateChildLocals();
                if (sceneObject.GetComponent<Mesh>() != null)
                {
                    sceneObject.GetComponent<Mesh>().UpdateMesh();
                }
            }
        }
        private Quaternion _rotation = Quaternion.Identity;
        #endregion Global transform qualifiers
        #region Local transform modifiers
        private Point3 _assignedLP;
        public Point3 localPosition
        {
            get
            {
                if (sceneObject.parent == null)
                {
                    return position;
                }
                else
                {
                    return position - sceneObject.parent.transform.position;
                }
            }
            set
            {
                _assignedLP = value;
                if (sceneObject.parent == null)
                {
                    position = value;
                }
                else
                {
                    Point3 rotated = (sceneObject.parent.transform.rotation * value);
                    position = sceneObject.parent.transform.position + (rotated);
                }
            }
        }
        public Direction3 localScale
        {
            get
            {
                if (sceneObject.parent == null)
                {
                    return scale;
                } else {
                    return scale / sceneObject.parent.transform.scale;
                }
            }
            set
            {
                if (sceneObject.parent == null)
                {
                    scale = value;
                }
                else
                {
                    scale = sceneObject.parent.transform.scale * value;
                }
            }
        }
        private Quaternion _localRotation = Quaternion.Identity;
        public Quaternion localRotation
        {
            get
            {
                if (sceneObject.parent == null)
                {
                    return rotation;
                }
                else
                {
                    return _localRotation;
                    Quaternion Q = sceneObject.parent.transform.rotation / rotation;
                    return Q;
                }
            }
            set
            {
                if (sceneObject.parent == null)
                {
                    rotation = value;
                    _localRotation = value;
                }
                else
                {
                    _localRotation = value;
                    rotation = sceneObject.parent.transform.rotation * value;
                }
            }
        }
        #endregion Local transform qualifiers
        public Direction3 Forward
        {
            get { return rotation * new Direction3(0, 0, 1); }
        }
        public Direction3 Right
        {
            get { return rotation * new Direction3(1, 0, 0); }
        }
        public Direction3 Up
        {
            get { return rotation * new Direction3(0, 1, 0); }
        }
    }
}
