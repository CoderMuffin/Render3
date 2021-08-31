#if WINFORMS //disable in render3 build settings - compilational compiler signals
using Render3.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Render3.Core;
namespace Render3.Renderers
{

    public class WinFormsRenderer : Renderer
    {
        private Pen gpen = new Pen(System.Drawing.Color.Black);
        private Graphics g;
        private Graphics bmg;
        SolidBrush brush=new SolidBrush(System.Drawing.Color.Black);

        Bitmap progress;
        public Form target;
        public WinFormsRenderer(Form target)
        {
            this.target = target;
            g = target.CreateGraphics();
        }
        public override void DrawTriangle(Point2[] vertices,Core.Color c)
        {
            Point[] points = new Point[vertices.Length];
            int i = 0;
            foreach (Point2 vertex in vertices)
            {
                points[i++] = vertex.ToWinFormsPoint();
            }
            if (((int)renderMode & 1) != 0)
            {
                if (!OutOfBounds(screenSize, vertices[0], vertices[1]))
                    bmg.DrawLine(gpen, points[0], points[1]);
                if (!OutOfBounds(screenSize, vertices[0], vertices[2]))
                    bmg.DrawLine(gpen, points[0], points[2]);
                if (!OutOfBounds(screenSize, vertices[2], vertices[1]))
                    bmg.DrawLine(gpen, points[2], points[1]);
            }
            if (((int)renderMode & 2) != 0)
            {
                brush.Color = System.Drawing.Color.FromArgb((int)(255 * c.a), (int)(255 * c.r), (int)(255 * c.g), (int)(255 * c.b));

                bmg.FillPolygon(brush, points);
            }
        }

        public override void StartDrawing()
        {
            progress = new Bitmap((int)screenSize.width,(int)screenSize.height);
            bmg = Graphics.FromImage(progress);
            using (SolidBrush brush = new SolidBrush(System.Drawing.Color.FromArgb(255, 255, 255)))
            {
                bmg.FillRectangle(brush, new Rectangle(new Point(0, 0), target.Size));
            }
        }

        public override void StopDrawing()
        {
            g.DrawImage(progress, new Point(0, 0));

        }
        public bool OutOfBounds(Dimensions2 bounds, Point2 p1, Point2 p2)
        {
            return (!new Rectangle(new Point(0, 0), bounds).IntersectsWith(new Rectangle(p1.ToWinFormsPoint(), new Size(1, 1)))) && (!new Rectangle(new Point(0, 0), bounds).IntersectsWith(new Rectangle(p2.ToWinFormsPoint(), new Size(1, 1))));
        }
    }
}
#endif