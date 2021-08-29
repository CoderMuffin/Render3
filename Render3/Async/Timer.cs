using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Async
{
    public struct TimerEvery
    {
        public double ms;
        public Timer Do(Action<double> todo, bool triggerInitial) => new Timer(ms, todo, triggerInitial);

        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
    public class Timer
    {
        public static TimerEvery Every(double ms) => new TimerEvery { ms = ms };
        public static double ms { get => (DateTime.Now - new DateTime(0)).TotalMilliseconds; }
        public static double ToMS(DateTime t) => (t - new DateTime(0)).TotalMilliseconds;


        public Flow flow;
        public Timer(double ms,Action<double> todo,bool triggerInitial)
        {
            flow=Flow.Go(() => {
                if (triggerInitial) todo(0);
                System.Timers.Timer t = new System.Timers.Timer();
                t.Enabled = true;
                t.Interval = ms;
                DateTime lastTime=DateTime.Now;
                t.Elapsed += (s, e) => {
                    todo((e.SignalTime - lastTime).TotalMilliseconds);
                    lastTime = e.SignalTime;
                };
            });
        }
    }
}
