
using System;
using System.Collections.Generic;
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
        public double clipNear=0.1f;
        public CameraFovMode fovMode = CameraFovMode.Horizontal;
        private double afov; private double eyeDist;
        List<Mesh> sceneMeshes = new List<Mesh>();
        Sorter<double, (Mesh, Face)> sortedTriangles = new Sorter<double, (Mesh, Face)>();
        private Point3 TransformRelative(Point3 toTransform)
        {
            Point3 dir = toTransform - sceneObject.worldPosition; // get point direction relative to pivot
            dir = (sceneObject.worldRotation).Inverse() * dir; // rotate it
            toTransform = dir + sceneObject.worldPosition; // move it back

            return toTransform - sceneObject.worldPosition;
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
        #region Obsolete
        /*public void RenderFaces(Scene s, Mesh m)
        {
            sortedTriangles.Clear();
            foreach (Face f in m.geometry.triangles)
            {
                //sortedTriangles.Add(-TransformRelative(f.worldCenter).z,f);
                //int i = 0;
                //foreach (Face toCompare in orderedTriangles)
                //{
                //    if (TransformRelative(f.worldCenter).z < TransformRelative(toCompare.worldCenter).z) break;
                //    i++;
                //}
                //orderedTriangles.Insert(i, f);
            }
            //for (int i = orderedTriangles.Count - 1; i >= 0; i--)
            /*foreach (KeyValuePair<double,Face> kv in sortedTriangles.Walk())
            {
                Face f = kv.Value;
                //Face f = orderedTriangles[i];
                //try
                //{
                    Point2[] vertices = { WorldToScreen(m.worldVertices[f.Vertices[0]]), WorldToScreen(m.worldVertices[f.Vertices[1]]), WorldToScreen(m.worldVertices[f.Vertices[2]]) };
                    double color = ((f.worldNormal - (-s.light.direction.normalized))).magnitude / 2;
                    renderer.DrawTriangle(vertices,Core.Color.Merge(m.color, Core.Color.Merge(new Core.Color(1 - color, 1 - color, 1 - color), s.light.color)));
                    */
                //}
                /*catch (InvalidOperationException)
                {
                    Console.WriteLine("[X] Render3.Renderer.Camera.RenderFaces(): Display already in use"); Disable batch rendering
                }*/
                /*catch (ArgumentOutOfRangeException)
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
                }*/
                /*foreach (Mesh childMesh in m.sceneObject.GetDescendantComponents<Mesh>(true))
                {
                    RenderFaces(scene, b, childMesh);
                }*/
           //}
        //}
        #endregion

        /*        public List<Mesh> GetFlatMeshTree(List<Mesh> submeshTree)
                {
                    List<Mesh> ms = new List<Mesh>();
                    foreach (Mesh m in submeshTree)
                    {
                        ms.AddRange(GetFlatMeshTree(m.sceneObject.GetDescendantComponents<Mesh>(true)));
                        if (m.enabled)
                            ms.Add(m);
                    }
                    return ms;
                }*/
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
            sortedTriangles.Clear();
            foreach (Mesh m in sceneMeshes)
                foreach (Face f in m.geometry.triangles)
                    sortedTriangles.Add(-TransformRelative(f.worldCenter).z,(m,f));

            foreach (KeyValuePair<double, (Mesh, Face)> kv in sortedTriangles.Walk())
            {
                Face f = kv.Value.Item2;
                Mesh m = kv.Value.Item1;
                //Face f = orderedTriangles[i];
                //try
                //{
                if (TransformRelative(f.worldCenter).z < clipNear) continue;
                Point2[] vertices = { WorldToScreen(m.worldVertices[f.Vertices[0]]), WorldToScreen(m.worldVertices[f.Vertices[1]]), WorldToScreen(m.worldVertices[f.Vertices[2]]) };
                double color = ((f.worldNormal - (-scene.light.direction.normalized))).magnitude / 2;
                renderer.DrawTriangle(vertices, Color.Merge(m.color, Color.Merge(new Color(1 - color, 1 - color, 1 - color), scene.light.color)));
            }
            err--;
            err = Math.Min(err, 0);
            renderer.StopDrawing();
        }
    }
}
