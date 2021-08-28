using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Core
{
/*    public static class Tweener
    {
        unsafe internal class Armor<T> where T : unmanaged {
            internal T* t;
            public Armor(ref T obj)
            {
                fixed (T* objptr = &obj) {
                    t = objptr;
                }
            }
        }
        public static unsafe void TweenTo<T>(ref T obj, T target, double ms) where T : unmanaged, ITweenable<T>
        {
            new System.Threading.Thread((ptr) => {
                dynamic armor = *(((Armor<T>)ptr).t);
                T state = armor.GetTweenState();
                DateTime now = DateTime.Now;
                TimeSpan tsms = TimeSpan.FromMilliseconds(ms);
                while (DateTime.Now - now < tsms)
                {
                    armor.Lerp(armor, state, target, (DateTime.Now - now).Milliseconds / tsms.TotalMilliseconds);

                    System.Threading.Thread.Sleep(10);
                    Console.WriteLine(armor.GetTweenState());
                }
                armor.Lerp(state, target, 1);

            }).Start(new Armor<T>(ref obj));
        }

    }
    public interface ITweenable<T> where T : unmanaged
    {
        T GetTweenState();
        void Lerp(dynamic obj,T from, T to, double progress);
    }*/
}
