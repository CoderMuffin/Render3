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
        private Point3 p;
        public Point3 position
        {
            get
            {
                return p;
            }
            set
            {
                p = value;
                if (parent.GetComponent<Mesh>() != null)
                {
                    parent.GetComponent<Mesh>().RecalculateWorldVertices();
                    parent.GetComponent<Mesh>().geometry.CalculateFaceNormals();
                }
            }
        }
        public Direction3 scale
        {
            get
            {
                return s;
            }
            set
            {
                s = value;
                if (parent.GetComponent<Mesh>() != null)
                    parent.GetComponent<Mesh>().RecalculateWorldVertices();
            }
        }
        private Direction3 s = new Direction3(1, 1, 1);
        public Quaternion rotation
        {
            get
            {
                return r;
            }
            set
            {
                r = value;
                if (parent.GetComponent<Mesh>() != null)
                {
                    parent.GetComponent<Mesh>().RecalculateWorldVertices();
                    parent.GetComponent<Mesh>().geometry.CalculateFaceNormals();
                }
            }
        }
        private Quaternion r = Quaternion.Identity;
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
