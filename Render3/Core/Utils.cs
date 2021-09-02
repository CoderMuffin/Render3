using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Core
{
    public static class Utils
    {
        public static void Decoration<T>(IDecorator<T> a,Action<T> b)
        {
            a.Decorate(b);
        }
        public static TReturn Decoration<TReturn, T>(IDecorator<TReturn, T> a, Func<TReturn, T> b)
        {
            return a.RDecorate(b);
        }
    }
    public interface IDecorator<T>
    {
        void Decorate(Action<T> decoration);
    }
    public interface IDecorator<TReturn,T>
    {
        TReturn RDecorate(Func<TReturn,T> decoration);
    }
}
