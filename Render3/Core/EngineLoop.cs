using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Render3.Core
{
    public class EngineLoop
    {
        public System.Timers.Timer renderTimer;
        public Graphics canvas;
        public Panel canvasPanel;
        public Scene scene;
        public EngineLoop(Scene s,Panel c)
        {
            this.scene = s;
            this.canvas = c.CreateGraphics();
            this.canvasPanel = c;
            renderTimer = new System.Timers.Timer();
            renderTimer.Interval = 16;
            renderTimer.Elapsed += Render;
            renderTimer.Enabled = true;
        }
        public void Render(Object o, System.Timers.ElapsedEventArgs e)
        {
            canvas.Clear(Color.AliceBlue);
            scene.camera.RenderGeometries(scene, canvas);
        }
    }
}
