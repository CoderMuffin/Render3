using Render3.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Render3.Core
{

    public class WinFormsRenderer : Renderer
    {
        private Pen gpen = new Pen(System.Drawing.Color.Black);
        private Graphics g;
        private Graphics bmg;
        SolidBrush brush=new SolidBrush(System.Drawing.Color.Black);

        private Dimensions2 screenSizeCache;
        Bitmap progress;
        public Form target;
        public WinFormsRenderer(Form target)
        {
            this.target = target;
            g = target.CreateGraphics();
        }
        public override void Clean()
        {
            using (SolidBrush brush = new SolidBrush(System.Drawing.Color.FromArgb(255, 255, 255)))
            {
                bmg.FillRectangle(brush, new Rectangle(new Point(0, 0), target.Size));
            }
        }
        public override void DrawTriangle(Point2[] vertices,Color c)
        {
            Point[] points = new Point[vertices.Length];
            int i = 0;
            foreach (Point2 vertex in vertices)
            {
                points[i++] = vertex.ToPoint();
            }
            if (((int)renderMode & 1) != 0)
            {
                if (!Camera.OutOfBounds(screenSizeCache, vertices[0], vertices[1]))
                    bmg.DrawLine(gpen, points[0], points[1]);
                if (!Camera.OutOfBounds(screenSizeCache, vertices[0], vertices[2]))
                    bmg.DrawLine(gpen, points[0], points[2]);
                if (!Camera.OutOfBounds(screenSizeCache, vertices[2], vertices[1]))
                    bmg.DrawLine(gpen, points[2], points[1]);
            }
            if (((int)renderMode & 2) != 0)
            {
                brush.Color = System.Drawing.Color.FromArgb((int)(255 * c.a), (int)(255 * c.r), (int)(255 * c.g), (int)(255 * c.b));

                bmg.FillPolygon(brush, points);
            }
        }

        public override void StartDrawing(Dimensions2 screenSize)
        {
            this.screenSizeCache = screenSize;
            progress = new Bitmap((int)screenSize.width,(int)screenSize.height);
            bmg = Graphics.FromImage(progress);
        }

        public override void StopDrawing()
        {
            g.DrawImage(progress, new Point(0, 0));
            
        }
    }
}
