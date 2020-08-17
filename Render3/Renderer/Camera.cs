
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
    public class Camera
    {
        public Pen pen = new Pen(System.Drawing.Color.Black);
        public Size screenSize { get { return ss; } set { ss = value; fov = fov; } }
        private Size ss;
        public CameraFovMode fovMode=CameraFovMode.Horizontal;
        private double afov;
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
            return new Point2(p.x * (eyeDist / p.z), p.y * (eyeDist / p.z))+new Point2(screenSize.Width / 2, screenSize.Height / 2);
        }
        public void RenderVertices(Scene scene,Graphics target)
        {
            foreach (Mesh m in scene.meshes)
            {
                foreach (Point3 p in m.worldVertices)
                {
                    try
                    {
                        //target.FillRectangle(new SolidBrush(System.Drawing.Color.Black), new Rectangle(WorldToScreen(p).ToPoint(), new Size(1, 1)));
                    }
                    catch (InvalidOperationException)
                    {
                        Console.Write("Render3.Renderer.Camera.RenderVertices(): Display already in use");
                    }
                }
            }
        }
        public void RenderFaces(Scene scene, Bitmap b, Mesh m)
        {
            foreach (Face f in m.geometry.triangles)
            {
                try
                {
                    using (Graphics target = Graphics.FromImage(b))
                    {
                        target.DrawLine(pen, WorldToScreen(m.worldVertices[f.vertices[0]]).ToPoint(), WorldToScreen(m.worldVertices[f.vertices[1]]).ToPoint());
                        target.DrawLine(pen, WorldToScreen(m.worldVertices[f.vertices[0]]).ToPoint(), WorldToScreen(m.worldVertices[f.vertices[2]]).ToPoint());
                        target.DrawLine(pen, WorldToScreen(m.worldVertices[f.vertices[2]]).ToPoint(), WorldToScreen(m.worldVertices[f.vertices[1]]).ToPoint());
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
            foreach (Mesh m in scene.meshes)
            {
                RenderFaces(scene, b, m);
            }
            return b;
        }
    }
}
