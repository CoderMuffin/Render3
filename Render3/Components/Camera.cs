
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Render3.Core;
using Render3.Components;
using Render3.Renderers;
namespace Render3.Components
{

    public class Camera : Component
    {
        public Renderer renderer;
        public enum CameraFovMode
        {
            Horizontal,
            Vertical
        }

        public enum CameraMatrix
        {
            Perspective,
            Orthographic
        }
        int err = 0;
        public Dimensions2 screenSize { get { return ss; } set { ss = value; fov = fov; } }
        private Dimensions2 ss=new Dimensions2(100,100);
        public int clipNear;
        public CameraFovMode fovMode = CameraFovMode.Horizontal;
        private double afov; private double eyeDist;
        List<Mesh> sceneMeshes = new List<Mesh>();
        List<Mesh> orderedMeshes = new List<Mesh>();
        List<Face> orderedTriangles = new List<Face>();
        private Point3 TransformRelative(Point3 toTransform)
        {
            Point3 dir = toTransform - sceneObject.transform.position; // get point direction relative to pivot
            dir = (sceneObject.transform.rotation).Inverse() * dir; // rotate it
            toTransform = dir + sceneObject.transform.position; // move it back

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
                    eyeDist = Math.Tan(180 - (fov / 2) * Math.PI / 180) * screenSize.width;
                }
                else
                {
                    eyeDist = Math.Tan(180 - (fov / 2) * Math.PI / 180) * screenSize.height;
                }
            }
        }

        public Camera(double fov,Renderer renderer,Dimensions2 screenSize)
        {
            this.fov = fov;
            this.renderer = renderer;
            this.screenSize = screenSize;
        }
        public Point2 WorldToScreen(Point3 p)
        {
            p = TransformRelative(p);
            if (clipNear > p.z)
            {
                return new Point2(p.x * 100000, p.y * 100000);
            }
            return new Point2(p.x * (eyeDist / p.z), p.y * (eyeDist / p.z)) + new Point2(screenSize.width / 2, screenSize.height / 2);
            
        }
        public static bool OutOfBounds(Dimensions2 bounds, Point2 p1, Point2 p2)
        {
            return (!new Rectangle(new Point(0, 0), bounds).IntersectsWith(new Rectangle(p1.ToWinFormsPoint(), new Size(1, 1)))) && (!new Rectangle(new Point(0, 0), bounds).IntersectsWith(new Rectangle(p2.ToWinFormsPoint(), new Size(1, 1))));
        }
        public void RenderFaces(Scene s, Mesh m)
        {
            orderedTriangles.Clear();
            foreach (Face f in m.geometry.Triangles)
            {
                int i = 0;
                foreach (Face toCompare in orderedTriangles)
                {
                    if (TransformRelative(f.worldCenter).z < TransformRelative(toCompare.worldCenter).z) break;
                    i++;
                }
                orderedTriangles.Insert(i, f);
            }
            for (int i = orderedTriangles.Count - 1; i > 0; i--)
            {
                Face f = orderedTriangles[i];
                try
                {
                    Point2[] vertices = { WorldToScreen(m.worldVertices[f.Vertices[0]]), WorldToScreen(m.worldVertices[f.Vertices[1]]), WorldToScreen(m.worldVertices[f.Vertices[2]]) };
                    double color = ((f.worldNormal - (-s.light.direction.normalized))).magnitude / 2;
                    renderer.DrawTriangle(vertices,Core.Color.Merge(m.color, Core.Color.Merge(new Core.Color(1 - color, 1 - color, 1 - color), s.light.color)));
                    
                }
                /*catch (InvalidOperationException)
                {
                    Console.WriteLine("[X] Render3.Renderer.Camera.RenderFaces(): Display already in use"); Disable batch rendering
                }*/
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("[!] Render3.Core.Camera.RenderFaces(): Render boot error.");
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
        public void RenderMeshes(Scene scene)
        {

            //this.screenSize = target.Size;
            renderer.UpdateScreenSize(screenSize);
            renderer.StartDrawing();

            /*foreach (SceneObject o in scene.objects)
            {
                if (o.GetComponent<Mesh>()!=null&&o.GetComponent<Mesh>().enabled==true)
                    RenderFaces( b, o.GetComponent<Mesh>());
            }*/
            sceneMeshes.Clear();
            //TODO: order faces???//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            foreach (SceneObject o in scene.objects)
            {
                sceneMeshes.AddRange(o.GetDescendantComponents<Mesh>(true));
                if (o.GetComponent<Mesh>() != null && o.GetComponent<Mesh>().enabled == true)
                    sceneMeshes.Add(o.GetComponent<Mesh>());
            }
            orderedMeshes.Clear();
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
                RenderFaces(scene, m);
            }
            err--;
            err = Math.Min(err, 0);
            renderer.StopDrawing();
        }
    }
}
