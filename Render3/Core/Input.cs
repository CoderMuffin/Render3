using System;
using Render3.Async;
namespace Render3.Core
{
    public class Input
    {
        public Flow inputFlow;
        public event Action<char> OnKey;
        public Input Initialize(Func<char> read)
        {
            Flow.Go(() => {
                while(true)
                {
                    OnKey?.Invoke(read());
                }
            });
            return this;
        }
        public static char ReadConsole()
        {
            return Console.ReadKey().KeyChar;
        }
        //public Point2 screenMousePosition { get { return new Point2(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y); } set { System.Windows.Forms.Cursor.Position = value.ToWinFormsPoint(); } }
        //public Point2 mouseDelta { get { return new Point2(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y); } }
    }
}
