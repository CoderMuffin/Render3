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
        public Color lastColor;
        public bool gloriousTechnicolor;
        public override void DrawTriangle(Point2[] vertices, Color c)
        {
            if (((int)renderMode & 2) != 0)
                FillTriangle(vertices[0], vertices[1], vertices[2], c);
            if (((int)renderMode & 1) != 0)
            {
                PlotLine((int)vertices[0].x, (int)vertices[0].y / 2, (int)vertices[1].x, (int)vertices[1].y / 2);
                PlotLine((int)vertices[0].x, (int)vertices[0].y / 2, (int)vertices[2].x, (int)vertices[2].y / 2);
                PlotLine((int)vertices[2].x, (int)vertices[2].y / 2, (int)vertices[1].x, (int)vertices[1].y / 2);
            }
        }

        public override void StartDrawing()
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
                if (InBounds(new Point2(x0,y0)))
                    screenArr[y0][x0] = new Color(1, 1, 1);
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
        void FillTriangle(Point2 a,Point2 b,Point2 c,Color d)
        {
            double ab=Math.Abs(a.y - b.y);
            double bc=Math.Abs(b.y - c.y);
            double ac=Math.Abs(a.y - c.y);
            //one should be sum of others
            Point2[] primary;
            Point2[] secondary;
            Point2[] tertiary;
            if (ac>=ab+bc)
            {
                //ca primary
                primary = new Point2[] { a, c };
                secondary = new Point2[] { a, b };
                tertiary = new Point2[] { b, c };
            } else if (ab>=bc+ac)
            {
                //ab primary
                primary = new Point2[] { a, b };
                secondary = new Point2[] { b, c };
                tertiary = new Point2[] { a, c };
            } else
            {
                //bc primary or we made a monumental screwup
                primary = new Point2[] { b, c };
                secondary = new Point2[] { a, c };
                tertiary = new Point2[] { a, b };
            }
            int x0 = (int)primary[0].x;
            int x1 = (int)primary[1].x;
            int y0 = (int)primary[0].y/2;
            int y1 = (int)primary[1].y/2;
            List<Point2> points = new List<Point2>();
            int dx = Math.Abs(x1 - x0);
            int sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0);
            int sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;  /* error value e_xy */
            while (true)
            {   /* loop */
                points.Add(new Point2(x0, y0));
                if (InBounds(new Point2(x0, y0)))
                    screenArr[y0][x0] = d;

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
            Dictionary<int,int> points2 = new Dictionary<int,int>();
            x0 = (int)secondary[0].x;
            x1 = (int)secondary[1].x;
            y0 = (int)secondary[0].y/2;
            y1 = (int)secondary[1].y/2;
            dx = Math.Abs(x1 - x0);
            sx = x0 < x1 ? 1 : -1;
            dy = -Math.Abs(y1 - y0);
            sy = y0 < y1 ? 1 : -1;
            err = dx + dy;  /* error value e_xy */
            while (true)
            {   /* loop */
                points2[y0]=x0;
                if (InBounds(new Point2(x0, y0)))
                    screenArr[y0][x0] = d;

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
            x0 = (int)tertiary[0].x;
            x1 = (int)tertiary[1].x;
            y0 = (int)tertiary[0].y/2;
            y1 = (int)tertiary[1].y/2;
            dx = Math.Abs(x1 - x0);
            sx = x0 < x1 ? 1 : -1;
            dy = -Math.Abs(y1 - y0);
            sy = y0 < y1 ? 1 : -1;
            err = dx + dy;  /* error value e_xy */
            while (true)
            {   /* loop */
                points2[y0] = x0;
                if (InBounds(new Point2(x0, y0)))
                    screenArr[y0][x0] = d;

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
            foreach (Point2 p in points)
            {
                int tx0 = (int)p.x;
                int tx1 = points2[(int)p.y];
                if (tx0>tx1) {
                    int tmp = tx0;
                    tx0 = tx1;
                    tx1 = tmp;
                }
                for (int x=tx0; x<tx1; x++)
                {
                    screenArr[(int)p.y][x] = d;
                }
            }
        }
        public string ColorToChar(Color c)
        {
            if (gloriousTechnicolor) {
                if (!(lastColor==c)) {
                    lastColor=c;
                    return "\x1b[38;2;"+((int)(c.r*255))+";"+((int)(c.g*255))+";"+((int)(c.b*255))+"m\u2588";
                } else {
                    return "\u2588";
                }
            } else {
                double avgValue = (c.r + c.g + c.b) / 3;
            if (avgValue < 0.1)
            {
                return " ";
            }
            else if (avgValue < 0.2)
            {
                return ".";
            }
            else if (avgValue < 0.3)
            {
                return "~";
            }
            else if (avgValue < 0.5)
            {
                return "&";
            }
            else if (avgValue < 0.7)
            {
                return "%";
            }
            else if (avgValue < 0.8)
            {
                return "#";
            }
            else
            {
                return "@";
            }
            }
        }
    }
}
