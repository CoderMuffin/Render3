using Render3.Core;
namespace Render3.Renderers
{
    public abstract class Renderer
    {
        protected Dimensions2 screenSize;
        public void UpdateScreenSize(Dimensions2 screenSize)
        {
            this.screenSize = screenSize;
        }
        protected bool InBounds(Point2 p)
        {
            return p.In(screenSize);
        }
        public abstract void DrawTriangle(Point2[] vertices, Color c);
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
