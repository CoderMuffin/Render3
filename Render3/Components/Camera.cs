
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Render3.Core;
using Render3.Components;
namespace Render3.Components
{

    public class Camera : Component
    {
        public enum CameraFovMode
        {
            Horizontal,
            Vertical
        }
        public enum RenderMode
        {
            None = 0,
            Wireframe = 1,
            Shaded = 2
        }
        int err = 0;
        public RenderMode renderMode = RenderMode.Shaded;
        private Pen gpen = new Pen(System.Drawing.Color.Black);
        public Size screenSize { get { return ss; } set { ss = value; fov = fov; } }
        private Size ss;
        public int clipNear;
        public CameraFovMode fovMode = CameraFovMode.Horizontal;
        private double afov;
        public Point3 TransformRelative(Point3 toTransform)
        {
            Point3 dir = toTransform - sceneObject.transform.position; // get point direction relative to pivot
            dir = Quaternion.Inverse(sceneObject.transform.rotation) * dir; // rotate it
            toTransform = dir + sceneObject.transform.position; // calculate rotated point

            return toTransform - sceneObject.transform.position;
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
                if (fovMode == 0)
                {
                    eyeDist = Math.Tan(180 - (fov / 2) * Math.PI / 180) * screenSize.Width;
                }
                else
                {
                    eyeDist = Math.Tan(180 - (fov / 2) * Math.PI / 180) * screenSize.Height;
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
            if (clipNear > p.z)
            {
                return new Point2(p.x * 100000, p.y * 100000);
            }
            try
            {
                return new Point2(p.x * (eyeDist / p.z), p.y * (eyeDist / p.z)) + new Point2(screenSize.Width / 2, screenSize.Height / 2);
            }
            catch (OverflowException)
            {
                return new Point2(-5, -5);
            }
        }
        public bool OutOfBounds(Size bounds, Point p1, Point p2)
        {
            return (!new Rectangle(new Point(0, 0), bounds).IntersectsWith(new Rectangle(p1, new Size(1, 1)))) && (!new Rectangle(new Point(0, 0), bounds).IntersectsWith(new Rectangle(p2, new Size(1, 1))));
        }
        public void RenderFaces(Scene s,Bitmap b, Mesh m)
        {
            List<Face> orderedTriangles = new List<Face>();
            foreach (Face f in m.geometry.Triangles)
            {
                int i = 0;
                foreach (Face toCompare in orderedTriangles)
                {
                    if (TransformRelative(f.WorldCenter).z < TransformRelative(toCompare.WorldCenter).z) break;
                    i++;
                }
                orderedTriangles.Insert(i, f);
            }
            foreach (Face f in orderedTriangles.Reverse<Face>())
            {

                try
                {
                    //throw new ArgumentOutOfRangeException(); Force crash
                    using (Graphics target = Graphics.FromImage(b))
                    {
                        Point[] vertices = { WorldToScreen(m.worldVertices[f.Vertices[0]]).ToPoint(), WorldToScreen(m.worldVertices[f.Vertices[1]]).ToPoint(), WorldToScreen(m.worldVertices[f.Vertices[2]]).ToPoint() };
                        if (((int)renderMode & 1) != 0)
                        {
                            if (!OutOfBounds(new Size(b.Width, b.Height), vertices[0], vertices[1]))
                                target.DrawLine(gpen, vertices[0], vertices[1]);
                            if (!OutOfBounds(new Size(b.Width, b.Height), vertices[0], vertices[2]))
                                target.DrawLine(gpen, vertices[0], vertices[2]);
                            if (!OutOfBounds(new Size(b.Width, b.Height), vertices[2], vertices[1]))
                                target.DrawLine(gpen, vertices[2], vertices[1]);
                        }
                        if (((int)renderMode & 2) != 0)
                        {
                            double color = ((f.WorldNormal - (-s.light.direction.normalized))).magnitude / 2;
                            Color c = Color.FromArgb((int)(255 * (1 - color)), (int)(255 * (1 - color)), (int)(255 * (1 - color)));
                            target.FillPolygon(new SolidBrush(ColorMerge(m.color, ColorMerge(c,s.light.color))), vertices);
                        }
                    }

                }
                /*catch (InvalidOperationException)
                {
                    Console.WriteLine("[X] Render3.Renderer.Camera.RenderFaces(): Display already in use"); Disable batch rendering
                }*/
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("[!] Render3.Renderer.Camera.RenderFaces(): Render boot error.");
                    err += 2;
                    if (err > 10)
                    {
                        Console.WriteLine("------------------------" + "\n" +
                                          "-----Please restart-----" + "\n" +
                                          "------------------------");
                        Debug.LaunchErrorForm();
                    }
                }
                /*foreach (Mesh childMesh in m.sceneObject.GetDescendantComponents<Mesh>(true))
                {
                    RenderFaces(scene, b, childMesh);
                }*/
            }
        }
        public static Color ColorMerge(Color a, Color b)
        {
            return Color.FromArgb(a.A * b.A / 255, a.R * b.R / 255, a.G * b.G / 255, a.B * b.B / 255);
        }
        public List<Mesh> GetFlatMeshTree(List<Mesh> submeshTree)
        {
            List<Mesh> ms = new List<Mesh>();
            foreach (Mesh m in submeshTree)
            {
                ms.AddRange(GetFlatMeshTree(m.sceneObject.GetDescendantComponents<Mesh>(true)));
                if (m.enabled)
                    ms.Add(m);
            }
            return ms;
        }
        public Bitmap RenderMeshes(Scene scene, Form target)
        {
            Bitmap b = new Bitmap(target.Size.Width, target.Size.Height);
            this.screenSize = target.Size;
            using (Graphics t = Graphics.FromImage(b))
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
                {
                    t.FillRectangle(brush, new Rectangle(new Point(0, 0), target.Size));
                }
            }
            /*foreach (SceneObject o in scene.objects)
            {
                if (o.GetComponent<Mesh>()!=null&&o.GetComponent<Mesh>().enabled==true)
                    RenderFaces( b, o.GetComponent<Mesh>());
            }*/
            List<Mesh> sceneMeshes = new List<Mesh>();
            List<Face> sceneFaces = new List<Face>();
            foreach (SceneObject o in scene.objects)
            {
                sceneMeshes.AddRange(o.GetDescendantComponents<Mesh>(true));
                if (o.GetComponent<Mesh>() != null && o.GetComponent<Mesh>().enabled == true)
                    sceneMeshes.Add(o.GetComponent<Mesh>());
            }
            List<Mesh> orderedMeshes = new List<Mesh>();
            foreach (Mesh m in sceneMeshes)
            {
                int i = 0;
                foreach (Mesh toCompare in orderedMeshes)
                {
                    if (TransformRelative(m.sceneObject.transform.position).z > TransformRelative(toCompare.sceneObject.transform.position).z) break;
                    i++;
                }
                orderedMeshes.Insert(i, m);
            }
            foreach (Mesh m in orderedMeshes)
            {
                RenderFaces(scene,b, m);
            }
            err--;
            err = Math.Min(err, 0);
            return b;
        }
    }
}
