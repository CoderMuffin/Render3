using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using Render3.Async;
namespace Render3.Core
{
    public class EngineLoop : Render3Object
    {
        public Timer renderTimer;
        public double timeElapsed;
        public Scene scene;
        public event Action<double> RenderEvent;
        public EngineLoop(Scene s,double fps)
        {
            this.scene = s;
            Flow.Go(() => {
                Flow.Wait(500);
                renderTimer = Timer.Every(1000 / fps).Do(Render, false);
            });
           
            /*System.Timers.Timer t = new System.Timers.Timer();
            t.Interval = 10;
            t.Elapsed += UpdateTime;
            t.Enabled = true;*/
         
            
        }

        private void UpdateTime(Object o, System.Timers.ElapsedEventArgs e)
        {
            timeElapsed += 0.01;
        }
        private void Render(double deltaTime)
        {
            if (Debug.crashed)
            {
                return;
            }
            try
            {
                
                scene.camera.RenderMeshes(scene);
            } catch (InvalidOperationException)
            {
                Console.WriteLine("Render3.Renderer.EngineLoop.Render(): Screen is busy");
            }
            RenderEvent?.Invoke(deltaTime/1000);
        }
    }
}
