
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Render3.Core;
namespace Render3.Renderer
{
    public enum CameraFovMode
    {
        Horizontal,
        Vertical
    }
    public enum RenderMode
    {
        Wireframe=1,
        Shaded=2
    }
    public class Camera : SceneObject
    {
        public Pen pen = new Pen(System.Drawing.Color.Black);
        public Size screenSize { get { return ss; } set { ss = value; fov = fov; } }
        private Size ss;
        public int clipNear;
        public CameraFovMode fovMode=CameraFovMode.Horizontal;
        private double afov;
        public Point3 TransformRelative(Point3 toTransform)
        {
            Point3 dir = toTransform - transform.position; // get point direction relative to pivot
            dir = Quaternion.Inverse(transform.rotation) * dir; // rotate it
            toTransform = dir + transform.position; // calculate rotated point

            return toTransform-transform.position;
        }
        public double fov
        {
            get
            {
                return afov;
            }
            set
            {
                afov = value;
                if (fovMode==0)
                {
                    eyeDist = Math.Tan(180 - (fov / 2)*Math.PI/180) * screenSize.Width;
                } else
                {
                    eyeDist = Math.Tan(180 - (fov / 2)*Math.PI/180) * screenSize.Height;
                }
            }
        }
        private double eyeDist;
        public Camera(double fov)
        {
            this.fov = fov;
        }
        public Point2 WorldToScreen(Point3 p)
        {
            p = TransformRelative(p);
            if (clipNear>p.z)
            {
                return new Point2(p.x*100000,p.y*100000);
            }
            try
            {
                return new Point2(p.x * (eyeDist / p.z), p.y * (eyeDist / p.z)) + new Point2(screenSize.Width / 2, screenSize.Height / 2); //Warning: Unsafe for camera position
            } catch (OverflowException)
            {
                return new Point2(-5, -5);
            }
        }
        public bool OutOfBounds(Size bounds,Point p1, Point p2)
        {
            return (!new Rectangle(new Point(0, 0), bounds).IntersectsWith(new Rectangle(p1, new Size(1, 1)))) && (!new Rectangle(new Point(0, 0), bounds).IntersectsWith(new Rectangle(p2, new Size(1, 1))));
        }
        public void RenderFaces(Scene scene, Bitmap b, Components.Mesh m)
        {
            foreach (Face f in m.geometry.triangles)
            {
                try
                {
                    using (Graphics target = Graphics.FromImage(b))
                    {
                        Point[] vertices = { WorldToScreen(m.worldVertices[f.vertices[0]]).ToPoint(), WorldToScreen(m.worldVertices[f.vertices[1]]).ToPoint(), WorldToScreen(m.worldVertices[f.vertices[2]]).ToPoint() };
                        if (!OutOfBounds(new Size(b.Width,b.Height),vertices[0], vertices[1]))
                            target.DrawLine(pen, vertices[0], vertices[1]);
                        if (!OutOfBounds(new Size(b.Width, b.Height), vertices[0], vertices[2]))
                            target.DrawLine(pen, vertices[0], vertices[2]);
                        if (!OutOfBounds(new Size(b.Width, b.Height), vertices[2], vertices[1]))
                            target.DrawLine(pen, vertices[2], vertices[1]);
                    }

                }
                catch (InvalidOperationException)
                {
                    Console.Write("Render3.Renderer.Camera.RenderFaces(): Display already in use");
                }
            }
        }
        public Bitmap RenderMeshes(Scene scene, Form target)
        {
            Bitmap b = new Bitmap(target.Size.Width,target.Size.Height);
            this.screenSize = target.Size;
            using (Graphics t = Graphics.FromImage(b))
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(255,255,255)))
                {
                    t.FillRectangle(brush, new Rectangle(new Point(0,0), target.Size));
                }
            }
            foreach (SceneObject o in scene.objects)
            {
                if (o.GetComponent<Components.Mesh>()!=null)
                    RenderFaces(scene, b, o.GetComponent<Components.Mesh>());
            }
            return b;
        }
    }
}
