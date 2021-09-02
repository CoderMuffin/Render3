using Render3.Core;
using Render3.Geometries;
using Render3.Components;
using Render3.Renderers;

namespace CR3TestProject
{
    public static class MainClass
    {
        static SceneObject camera;
        static SceneObject cube1 = new SceneObject();
        static SceneObject cubeChild = new SceneObject();
        public static void Main(string[] _)
        {
            //new Input().Initialize(Input.ReadConsole).OnKey += (c) => { System.Console.WriteLine(c); };
            //camera.AddComponent(new Camera(60));
            camera = new Camera(60, new ConsoleRenderer(), new Dimensions2(30, 20)).DefaultObject();
            Scene scene = new Scene(camera.GetComponent<Camera>());
            camera.GetComponent<Camera>().renderer.renderMode = RenderMode.Shaded;
            EngineLoop e = new EngineLoop(scene, 10);

            e.RenderEvent += OnUpdate; // Add the OnUpdate event for every time the camera renders

            cube1.AddComponent(new Mesh(BaseGeometry.CubeGeometry())); // Add a cube geometry to cube1
            cube1.GetComponent<Mesh>().color = new Color(1, 0, 1); // Set the color of cube1's mesh
            cube1.worldPosition += new Point3(0, 0, 3); // Change the worldPosition of cube1
            scene.objects.Add(cube1); // Add cube1 to the scene

            camera.worldPosition = new Point3(0, -2, 0);
            camera.worldRotation = Quaternion.Euler(-30, 0, 0, Quaternion.AngleUnit.Degrees);
            //scene.light.color = Color.FromArgb(0, 255, 0);

            //cubeChild.parent = cube1; // Set the parent of cubeChild to be cube1
            cubeChild.localScale = new Direction3(0.5, 0.5, 1);
            cubeChild.AddComponent(new Mesh(BaseGeometry.CubeGeometry()));
            cubeChild.localPosition = new Point3(0, 0, 3);
            cubeChild.localRotation = Quaternion.Euler(145, 0, 0, Quaternion.AngleUnit.Degrees);
            cubeChild.GetComponent<Mesh>().color = new Color(0, 1, 1);

            SceneObject occ = new SceneObject();
            //occ.parent = cubeChild;
            occ.AddComponent(new Mesh(BaseGeometry.CubeGeometry()));
            occ.localPosition = new Point3(0, 0, 3);
            occ.GetComponent<Mesh>().color = new Color(0, 1, 0);
            occ.worldScale = new Direction3(1, 1, 1);
            //Tweener.TweenTo(ref cube1._localworldPosition,new Point3(0,0,0),5000);
            //scene.objects.Add(occ);

            //c.parent = o;*/
            //c.localworldPosition = new Point3(0, 0, 5);
            while (true) { }
        }
        public static void OnUpdate(double deltaTime)
        {
            //cube1.scale -= new Direction3(0.01, 0.01, 0.01);
            cube1.worldRotation *= Quaternion.Euler(0, deltaTime * 36, 0, Quaternion.AngleUnit.Degrees);
            camera.worldPosition += new Point3(0, deltaTime * 0.25, deltaTime * 0.2);
            //cubeChild.localRotation*=Quaternion.Euler(1,2,3,Quaternion.AngleUnit.Degrees);
            //camera.worldPosition = new Point3(0,0,camera.worldPosition.z+0.1);
        }
    }
}
