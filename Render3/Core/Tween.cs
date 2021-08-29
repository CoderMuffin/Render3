using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Core
{
    /*public static class Tweener
    {
        public static void TweenTo<T>(ref T obj, T target, double ms) where T : ITweenable<T>
        {
            new System.Threading.Thread((ptr) => {
                T state = ((ITweenable<T>)ptr).GetTweenState();
                DateTime now = DateTime.Now;
                TimeSpan tsms = TimeSpan.FromMilliseconds(ms);
                while (DateTime.Now - now < tsms)
                {
                    ((ITweenable<T>)ptr).Lerp(state, target, (DateTime.Now - now).TotalMilliseconds / tsms.TotalMilliseconds);

                    System.Threading.Thread.Sleep(10);
                }
                ((ITweenable<T>)ptr).Lerp(state, target, 1);

            }).Start(obj);
        }

    }
    public interface ITweenable<T>
    {
        T GetTweenState();
        void Lerp(T from, T to, double progress);
    }*/
}
