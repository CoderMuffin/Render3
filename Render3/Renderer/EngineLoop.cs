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
    public class EngineLoop
    {
        public System.Timers.Timer renderTimer;
        public Scene scene;
        public Form f1;
        private Graphics g;
        public delegate void tick();
        public event tick TickEvent;
        public EngineLoop(Scene s,Form f1)
        {
            this.f1 = f1;
            g = f1.CreateGraphics();
            this.scene = s;
            renderTimer = new System.Timers.Timer();
            renderTimer.Interval = 16;
            renderTimer.Elapsed += Render;
            renderTimer.Enabled = true;
        }
        public void Render(Object o, System.Timers.ElapsedEventArgs e)
        {
                try
                {
                    TickEvent();
                } catch (NullReferenceException)
                {
                    
                }
                g.DrawImage(scene.camera.RenderMeshes(scene, f1),new Point(0,0));
        }
    }
}
