using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Core
{
    public enum CameraFovMode
    {
        Horizontal,
        Vertical
    }
    public class Camera
    {
        public Pen pen = new Pen(Color.Black);
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
        public Camera(double fov,Size screenSize)
        {
            this.fov = fov;
            this.screenSize = screenSize;
        }
        public Point2 WorldToScreen(Point3 p)
        {
            return new Point2(p.x * (eyeDist / p.z), p.y * (eyeDist / p.z))+new Point2(screenSize.Width / 2, screenSize.Height / 2);
        }
        public void RenderVertices(Scene scene,Graphics target)
        {
            foreach (Mesh m in scene.meshes)
            {
                foreach (Point3 p in m.geometry.vertices)
                {
                    try
                    {
                        target.FillRectangle(new SolidBrush(Color.Black), new Rectangle(scene.camera.WorldToScreen(p).ToPoint(), new Size(1, 1)));
                    }
                    catch (InvalidOperationException)
                    {
                        Console.Write("Render3.Core.Camera.RenderVertices(): Display already in use");
                    }
                }
            }
        }
        public void RenderFaces(Scene scene, Graphics target, IGeometry g)
        {
            foreach (Face f in g.triangles)
            {
                try
                {
                    target.DrawLine(pen, scene.camera.WorldToScreen(f.vertices[0]).ToPoint(), scene.camera.WorldToScreen(f.vertices[1]).ToPoint());
                    target.DrawLine(pen, scene.camera.WorldToScreen(f.vertices[0]).ToPoint(), scene.camera.WorldToScreen(f.vertices[2]).ToPoint());
                    target.DrawLine(pen, scene.camera.WorldToScreen(f.vertices[2]).ToPoint(), scene.camera.WorldToScreen(f.vertices[1]).ToPoint());
                }
                catch (InvalidOperationException)
                {
                    Console.Write("Render3.Core.Camera.RenderFaces(): Display already in use");
                }
            }
        }
        public void RenderMeshes(Scene scene, Graphics target)
        {
            foreach (Mesh m in scene.meshes)
            {
                RenderFaces(scene,target,m.geometry);
            }
        }
    }
}
