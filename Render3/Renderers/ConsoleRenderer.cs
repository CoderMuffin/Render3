using Render3.Core;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Render3.Renderers
{
    public class ConsoleRenderer : Renderer
    {
        Color[][] screenArr = new Color[0][];

        public override void DrawTriangle(Point2[] vertices, Color c)
        {
            PlotLine((int)vertices[0].x, (int)vertices[0].y/2, (int)vertices[1].x, (int)vertices[1].y/2);
            PlotLine((int)vertices[0].x, (int)vertices[0].y/2, (int)vertices[2].x, (int)vertices[2].y/2);
            PlotLine((int)vertices[2].x, (int)vertices[2].y/2, (int)vertices[1].x, (int)vertices[1].y/2);
        }

        public override void StartDrawing(Dimensions2 screenSize)
        {
            if (screenArr.Length != (int)screenSize.height)
            {
                screenArr = new Color[(int)screenSize.height][];
            }
            for (int index = 0; index < screenArr.Length; index++)
            {
                screenArr[index] = new Color[(int)screenSize.width];
            }
        }

        public override void StopDrawing()
        {
            StringBuilder s = new StringBuilder();

            foreach (Color[] content in screenArr)
            {
                foreach (Color c in content)
                {
                    s.Append(ColorToChar(c));
                }
                s.Append("\n");
            }
            //Console.Clear();
            Console.Write(s.ToString());
        }

        public void PlotLine(int x0, int y0, int x1, int y1)
        {
            int dx = Math.Abs(x1 - x0);
            int sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0);
            int sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;  /* error value e_xy */
            while (true)
            {   /* loop */
                screenArr[y0][x0]=new Color(1,1,1);
                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 >= dy)
                { /* e_xy+e_x > 0 */
                    err += dy;
                    x0 += sx;
                }
                if (e2 <= dx)
                {/* e_xy+e_y < 0 */
                    err += dx;
                    y0 += sy;
                }
            }
        }
        public char ColorToChar(Color c)
        {
            double avgValue = (c.r + c.g + c.b) / 3;
            if (avgValue < 0.1)
            {
                return ' ';
            }
            else if (avgValue < 0.2)
            {
                return '.';
            }
            else if (avgValue < 0.3)
            {
                return '~';
            }
            else if (avgValue < 0.5)
            {
                return '&';
            }
            else if (avgValue < 0.7)
            {
                return '%';
            }
            else if (avgValue < 0.8)
            {
                return '#';
            }
            else
            {
                return '@';
            }
        }
    }
}
