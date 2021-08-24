using Render3.Core;
namespace Render3.Renderers
{
    public abstract class Renderer
    {
        public abstract void DrawTriangle(Point2[] vertices,Color c);
        public abstract void StartDrawing(Dimensions2 screenSize);
        public abstract void StopDrawing();
        public RenderMode renderMode=RenderMode.Shaded;
    }
    public enum RenderMode
    {
        None = 0,
        Wireframe = 1,
        Shaded = 2
    }
}
