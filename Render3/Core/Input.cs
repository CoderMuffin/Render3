using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Core
{
    public class Input
    {
        
        public Point2 screenMousePosition { get { return new Point2(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y); } set { System.Windows.Forms.Cursor.Position = value.ToPoint(); } }
        //public Point2 mouseDelta { get { return new Point2(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y); } }
    }
}
