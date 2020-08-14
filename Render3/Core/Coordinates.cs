using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Core
{
    public struct Point2
    {
        public double x, y;
        public Point2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public Point ToPoint()
        {
            return new Point((int)Math.Round(x), (int)Math.Round(y));
        }
        public override string ToString()
        {
            return "Render3.Core.Point2.ToString(): " + x.ToString() + "," + y.ToString();
        }
        #region Operator overloads
        public static implicit operator Point2(int i)
        {
            return new Point2(i, i);
        }
        public static Point2 operator +(Point2 lhs, Point2 rhs)
        {
            return new Point2(lhs.x + rhs.x, lhs.y + rhs.y);
        }
        public static Point2 operator -(Point2 lhs, Point2 rhs)
        {
            return new Point2(lhs.x - rhs.x, lhs.y - rhs.y);
        }
        public static Point2 operator *(Point2 lhs, Point2 rhs)
        {
            return new Point2(lhs.x * rhs.x, lhs.y * rhs.y);
        }
        public static Point2 operator /(Point2 lhs, Point2 rhs)
        {
            return new Point2(lhs.x / rhs.x, lhs.y / rhs.y);
        }
        public static Point2 operator !(Point2 me)
        {
            return me * -1;
        }
        #endregion Operator overloads
    }
    public struct Point3
    {
        public double x, y, z;
        public Point3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public override string ToString()
        {
            return "Render3.Core.Point3.ToString(): " + x.ToString() + "," + y.ToString() + "," + z.ToString();
        }
        #region Operator overloads
        public static implicit operator Point3(int i)
        {
            return new Point3(i, i, i);
        }
        public static Point3 operator +(Point3 lhs, Point3 rhs)
        {
            return new Point3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }
        public static Point3 operator -(Point3 lhs, Point3 rhs)
        {
            return new Point3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }
        public static Point3 operator *(Point3 lhs, Point3 rhs)
        {
            return new Point3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
        }
        public static Point3 operator /(Point3 lhs, Point3 rhs)
        {
            return new Point3(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
        }
        public static Point3 operator !(Point3 me)
        {
            return me*-1;
        }
        #endregion Operator overloads
    }
    public struct Dimensions2
    {
        public Point2 topLeft;
        public Point2 bottomRight;
        public double width { get { return Math.Abs(topLeft.x - bottomRight.x); } }
        public double height { get { return Math.Abs(topLeft.y - bottomRight.y); } }
        public Dimensions2(Point2 topLeft, Point2 bottomRight)
        {
            this.topLeft = topLeft;
            this.bottomRight = bottomRight;
        }
        public static implicit operator SizeF(Dimensions2 me)
        {
            return new Size((int)Math.Round(me.width), (int)Math.Round(me.height));
        }
    }
    public struct Dimensions3
    {
        public Point3 topLeft;
        public Point3 bottomRight;
        public double width { get { return Math.Abs(topLeft.x - bottomRight.x); } }
        public double height { get { return Math.Abs(topLeft.y - bottomRight.y); } }
        public double depth { get { return Math.Abs(topLeft.z - bottomRight.z); } }
        public Dimensions3(Point3 topLeft, Point3 bottomRight)
        {
            this.topLeft = topLeft;
            this.bottomRight = bottomRight;
        }
    }
}
