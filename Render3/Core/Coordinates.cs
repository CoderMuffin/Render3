using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Core
{
    public struct Direction3
    {
        public double x, y, z;
        public void SetX(double x) { this.x = x; }
        public void SetY(double y) { this.y = y; }
        public void SetZ(double z) { this.z = z; }
        #region Constructors
        public Direction3(double v)
        {
            this.x = v;
            this.y = v;
            this.z = v;
        }
        public Direction3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Direction3(Point3 p)
        {
            this.x = p.x;
            this.y = p.y;
            this.z = p.z;
        }
        #endregion Constructors
        public double magnitude
        {
            get
            {
                return Math.Sqrt(x * x + y * y + z * z);
            }
        }
        public void Normalize()
        {
            this /= new Direction3(magnitude);
        }
        public Direction3 normalized
        {
            get { return this / new Direction3(magnitude); }
        }
        #region Operator overloads
        public override bool Equals(object obj)
        {
            if (!(obj is Direction3))
            {
                return false;
            }

            var direction = (Direction3)obj;
            return x == direction.x &&
                   y == direction.y &&
                   z == direction.z;
        }

        public override int GetHashCode()
        {
            var hashCode = 373119288;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            hashCode = hashCode * -1521134295 + z.GetHashCode();
            return hashCode;
        }
        public static Direction3 operator +(Direction3 lhs, Direction3 rhs)
        {
            return new Direction3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }
        public static Direction3 operator -(Direction3 me)
        {
            return new Direction3(-me.x, -me.y, -me.z);
        }
        public static Direction3 operator -(Direction3 lhs, Direction3 rhs)
        {
            return new Direction3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }
        public static Direction3 operator *(Direction3 lhs, Direction3 rhs)
        {
            return new Direction3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
        }
        public static Direction3 operator /(Direction3 lhs, Direction3 rhs)
        {
            return new Direction3(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
        }
        public static bool operator ==(Direction3 lhs, Direction3 rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }
        public static bool operator !=(Direction3 lhs, Direction3 rhs)
        {
            return !(lhs == rhs);
        }
        public static Direction3 operator !(Direction3 me)
        {
            return -me;
        }
        #endregion Operator overloads
        public Point3 ToPoint3()
        {
            return new Point3(x, y, z);
        }

        public override string ToString()
        {
            return "Render3.Core.Direction3.ToString(): " + x.ToString() + "," + y.ToString() + "," + z.ToString();
        }
    }
    public struct Point2
    {
        public double x, y;
        public void SetX(double x) { this.x = x; }
        public void SetY(double y) { this.y = y; }
        public Point2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public Point ToWinFormsPoint()
        {
            return new Point(Convert.ToInt32(x), Convert.ToInt32(y));
        }

        public float[] ToOpenTKFloatArr()
        {
            return new float[] { (float)x, (float)y, 0 };
        }
        public override string ToString()
        {
            return "Render3.Core.Point2.ToString(): " + x.ToString() + "," + y.ToString();
        }

        #region Operator overloads
        public static Point2 operator +(Point2 lhs, Point2 rhs)
        {
            return new Point2(lhs.x + rhs.x, lhs.y + rhs.y);
        }
        public static Point2 operator -(Point2 lhs, Point2 rhs)
        {
            return new Point2(lhs.x - rhs.x, lhs.y - rhs.y);
        }
        public static Point2 operator -(Point2 me)
        {
            return new Point2(-me.x, -me.y);
        }
        public static Point2 operator *(Point2 lhs, Point2 rhs)
        {
            return new Point2(lhs.x * rhs.x, lhs.y * rhs.y);
        }
        public static Point2 operator /(Point2 lhs, Point2 rhs)
        {
            return new Point2(lhs.x / rhs.x, lhs.y / rhs.y);
        }
        public static bool operator ==(Point2 lhs, Point2 rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }
        public static bool operator !=(Point2 lhs, Point2 rhs)
        {
            return !(lhs == rhs);
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Point2))
            {
                return false;
            }

            var point = (Point2)obj;
            return x == point.x &&
                   y == point.y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1502939027;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }



        public static Point2 operator !(Point2 me)
        {
            return me * new Point2(-1, -1);
        }
        #endregion Operator overloads
        public bool In(Dimensions2 inside)
        {
            return ExtraMath.BetweenExclusive(inside.topLeft.x,x,inside.bottomRight.x)&&ExtraMath.BetweenExclusive(inside.topLeft.y,y,inside.bottomRight.y);
        }
    }
    public struct Point3
    {
        public double x, y, z;
        public void SetX(double x) { this.x = x; }
        public void SetY(double y) { this.y = y; }
        public void SetZ(double z) { this.z = z; }
        public Point3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Point3(Direction3 d)
        {
            this.x = d.x;
            this.y = d.y;
            this.z = d.z;
        }
        public override string ToString()
        {
            return "Render3.Core.Point3.ToString(): " + x.ToString() + "," + y.ToString() + "," + z.ToString();
        }


        #region Operator overloads
        public static Point3 operator +(Point3 lhs, Point3 rhs)
        {
            return new Point3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }
        public static Point3 operator -(Point3 me)
        {
            return new Point3(-me.x, -me.y, -me.z);
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
        public static bool operator ==(Point3 lhs, Point3 rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }
        public static bool operator !=(Point3 lhs, Point3 rhs)
        {
            return !(lhs == rhs);
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Point3))
            {
                return false;
            }

            var point = (Point3)obj;
            return x == point.x &&
                   y == point.y &&
                   z == point.z;
        }

        public override int GetHashCode()
        {
            int hashCode = 373119288;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            hashCode = hashCode * -1521134295 + z.GetHashCode();
            return hashCode;
        }
        public static Point3 operator !(Point3 me)
        {
            return -me;
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
        public Dimensions2(double width, double height)
            {
                this.topLeft = new Point2();
                this.bottomRight = new Point2(width,height);
            }
        public static implicit operator Size(Dimensions2 me)
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
