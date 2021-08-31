using System;
using System.Collections.Generic;
using Render3.Async;
namespace Render3.Core
{
    public class Input
    {
        public bool this[char ch] {
            get => down.Contains(ch);
        }
        private HashSet<char> down = new HashSet<char>();
        public Flow inputFlow;
        public event Action<char> OnKey;
        public Input Initialize(Func<char> read)
        {
            Flow.Go(() => {
                while(true)
                {
                    char ch = read();
                    down.Add(ch);
                    OnKey?.Invoke(ch);
                }
            });
            return this;
        }
        public static char ReadConsole()
        {
            return Console.ReadKey(true).KeyChar;
        }
        public void Flush()
        {
            down.Clear();
        }
        //public Point2 screenMousePosition { get { return new Point2(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y); } set { System.Windows.Forms.Cursor.Position = value.ToWinFormsPoint(); } }
        //public Point2 mouseDelta { get { return new Point2(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y); } }
    }
}
