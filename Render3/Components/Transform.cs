using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render3.Components;
using Render3.Core;
namespace Render3.Components
{
    public class Transform : Render3.Components.Component
    {
        public void UpdateLocals()
        {
            foreach (SceneObject child in sceneObject.children)
            {
                child.transform.localPosition = child.transform.localPosition;
                child.transform.localRotation = child.transform.localRotation;
                child.transform.localScale = child.transform.localScale;
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

                UpdateLocals();
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
                UpdateLocals();
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
                UpdateLocals();
                if (sceneObject.GetComponent<Mesh>() != null)
                {
                    sceneObject.GetComponent<Mesh>().UpdateMesh();
                }
            }
        }
        private Quaternion _rotation = Quaternion.Identity;
        #endregion Global transform qualifiers
        #region Local transform modifiers
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
                if (sceneObject.parent == null)
                {
                    position = value;
                }
                else
                {
                    position = sceneObject.parent.transform.position + sceneObject.parent.transform.localRotation*value;
                }
                foreach (SceneObject child in sceneObject.children)
                {
                    child.transform.localPosition = child.transform.localPosition;
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
                foreach (SceneObject child in sceneObject.children)
                {
                    child.transform.localScale = child.transform.localScale;
                }
            }
        }
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
                    return sceneObject.parent.transform.rotation / rotation;
                }
            }
            set
            {
                if (sceneObject.parent == null)
                {
                    rotation = value;
                }
                else
                {
                    rotation = sceneObject.parent.transform.rotation * value;
                }
                
                foreach (SceneObject child in sceneObject.children)
                {
                    child.transform.localRotation = child.transform.localRotation;
                }
            }
        }
        #endregion Local transform qualifiers
        public Direction3 forward
        {
            get { return rotation * new Direction3(0, 0, 1); }
        }
        public Direction3 right
        {
            get { return rotation * new Direction3(1, 0, 0); }
        }
        public Direction3 up
        {
            get { return rotation * new Direction3(0, 1, 0); }
        }
    }
}
