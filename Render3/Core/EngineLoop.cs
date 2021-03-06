﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Render3.Core;
namespace Render3.Core
{
    public class EngineLoop : Render3Object
    {
        public System.Timers.Timer renderTimer;
        public double timeElapsed;
        public Scene scene;
        public Form f1;
        private Graphics g;
        public delegate void tick();
        public event tick RenderEvent;
        public EngineLoop(Scene s,Form f1)
        {
            this.f1 = f1;
            g = f1.CreateGraphics();
            this.scene = s;
            System.Timers.Timer ts = new System.Timers.Timer();
            ts.Interval = 500;
            ts.Elapsed += RenderStart;
            ts.Enabled = true;
            renderTimer = new System.Timers.Timer();
            renderTimer.Interval = 16;
            renderTimer.Elapsed += Render;
           
            renderTimer.Enabled = false;
            /*System.Timers.Timer t = new System.Timers.Timer();
            t.Interval = 10;
            t.Elapsed += UpdateTime;
            t.Enabled = true;*/
         
            
        }

        private void RenderStart(Object o, System.Timers.ElapsedEventArgs e)
        {
            renderTimer.Enabled = true;
        }
        private void UpdateTime(Object o, System.Timers.ElapsedEventArgs e)
        {
            timeElapsed += 0.01;
        }
        public void Render(Object o, System.Timers.ElapsedEventArgs e)
        {
            if (Debug.crashed)
            {
                return;
            }
            try
            {
                g.DrawImage(scene.camera.RenderMeshes(scene, f1), new Point(0, 0));
            } catch (InvalidOperationException)
            {
                Console.WriteLine("Render3.Renderer.EngineLoop.Render(): Screen is busy");
            }
            if (RenderEvent != null)
            {
                RenderEvent();
            }
        }
    }
}
