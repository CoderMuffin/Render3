using System;
using Render3.Core;
//Thanks to http://www.technologicalutopia.com/sourcecode/xnageometry/quaternion.cs.htm
namespace Render3.Core
{
    public struct Quaternion : IEquatable<Quaternion>
    {
        public enum AngleUnit
        {
            Degrees,
            Double,
            Radians
        }
        public double x;
        public double y;
        public double z;
        public double w;

        static Quaternion identity = new Quaternion(0, 0, 0, 1);

        #region Custom overloads
        public static Direction3 operator *(Quaternion rotation, Direction3 value)
        {
            double x2 = rotation.x + rotation.x;
            double y2 = rotation.y + rotation.y;
            double z2 = rotation.z + rotation.z;

            double wx2 = rotation.w * x2;
            double wy2 = rotation.w * y2;
            double wz2 = rotation.w * z2;
            double xx2 = rotation.x * x2;
            double xy2 = rotation.x * y2;
            double xz2 = rotation.x * z2;
            double yy2 = rotation.y * y2;
            double yz2 = rotation.y * z2;
            double zz2 = rotation.z * z2;

            return new Direction3(
                value.x * (1.0f - yy2 - zz2) + value.y * (xy2 - wz2) + value.z * (xz2 + wy2),
                value.x * (xy2 + wz2) + value.y * (1.0f - xx2 - zz2) + value.z * (yz2 - wx2),
                value.x * (xz2 - wy2) + value.y * (yz2 + wx2) + value.z * (1.0f - xx2 - yy2));
        }
        public static Point3 operator *(Quaternion r, Point3 p)
        {
            return (r * new Direction3(p)).ToPoint3();
        }
        #endregion Custom overloads


        public Quaternion(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }


        public Quaternion(Direction3 vectorPart, double scalarPart)
        {
            this.x = vectorPart.x;
            this.y = vectorPart.y;
            this.z = vectorPart.z;
            this.w = scalarPart;
        }

        public static Quaternion Identity
        {
            get { return identity; }
        }


        public static Quaternion Add(Quaternion quaternion1, Quaternion quaternion2)
        {
            //Syderis
            Quaternion quaternion;
            quaternion.x = quaternion1.x + quaternion2.x;
            quaternion.y = quaternion1.y + quaternion2.y;
            quaternion.z = quaternion1.z + quaternion2.z;
            quaternion.w = quaternion1.w + quaternion2.w;
            return quaternion;
        }
        

        //Funcion aÃ±adida Syderis
        public static Quaternion Concatenate(Quaternion value1, Quaternion value2)
        {
            Quaternion quaternion;
            double x = value2.x;
            double y = value2.y;
            double z = value2.z;
            double w = value2.w;
            double num4 = value1.x;
            double num3 = value1.y;
            double num2 = value1.z;
            double num = value1.w;
            double num12 = (y * num2) - (z * num3);
            double num11 = (z * num4) - (x * num2);
            double num10 = (x * num3) - (y * num4);
            double num9 = ((x * num4) + (y * num3)) + (z * num2);
            quaternion.x = ((x * num) + (num4 * w)) + num12;
            quaternion.y = ((y * num) + (num3 * w)) + num11;
            quaternion.z = ((z * num) + (num2 * w)) + num10;
            quaternion.w = (w * num) - num9;
            return quaternion;

        }

        
        public void Concatenate(Quaternion i)
        {
            this=Concatenate(this, i);
        }

        
        public void Invert()
        {
            this.x = -this.x;
            this.y = -this.y;
            this.z = -this.z;
        }

        
        public Quaternion Inverse()
        {
            Quaternion quaternion;
            quaternion.x = -x;
            quaternion.y = -y;
            quaternion.z = -z;
            quaternion.w = w;
            return quaternion;
        }

        
        public static double AngleConvert(double d,AngleUnit current,AngleUnit target)
        {
            if (current==AngleUnit.Degrees)
            {
                d /= 360;
            } else if (current==AngleUnit.Radians) {
                d /= Math.PI * 2;
            }
            if (target == AngleUnit.Degrees)
            {
                d *= 360;
            }
            else if (target == AngleUnit.Radians)
            {
                d *= Math.PI * 2;
            }
            return d;
        }

        public static Quaternion CreateFromAxisAngle(Direction3 axis, double angle,AngleUnit inputUnit)
        {
            angle = AngleConvert(angle, inputUnit, AngleUnit.Radians);
            Quaternion quaternion;
            double num2 = angle * 0.5f;
            double num = (double)Math.Sin((double)num2);
            double num3 = (double)Math.Cos((double)num2);
            quaternion.x = axis.x * num;
            quaternion.y = axis.y * num;
            quaternion.z = axis.z * num;
            quaternion.w = num3;
            return quaternion;

        }
        private double CopySign(double x, double y) => y < 0 ? -x:x;
        public double[] ToEulerAngles()
        {
            Quaternion q = this;
            double[] angles = new double[3];

            // roll (z-axis rotation)
            double sinr_cosp = 2 * (q.w * q.x + q.y * q.z);
            double cosr_cosp = 1 - 2 * (q.x * q.x + q.y * q.y);
            angles[2] = Math.Atan2(sinr_cosp, cosr_cosp);

            // pitch (x-axis rotation)
            double sinp = 2 * (q.w * q.y - q.z * q.x);
            if (Math.Abs(sinp) >= 1)
                angles[0] = CopySign(Math.PI / 2, sinp); // use 90 degrees if out of range
            else
                angles[0] = Math.Asin(sinp);

            // yaw (y-axis rotation)
            double siny_cosp = 2 * (q.w * q.z + q.x * q.y);
            double cosy_cosp = 1 - 2 * (q.y * q.y + q.z * q.z);
            angles[1] = Math.Atan2(siny_cosp, cosy_cosp);

            return angles;
        }

        public static Quaternion Euler(double yaw, double pitch, double roll, AngleUnit inputUnit)
        {
            yaw = AngleConvert(yaw, inputUnit, AngleUnit.Radians);
            pitch = AngleConvert(pitch, inputUnit, AngleUnit.Radians);
            roll = AngleConvert(roll, inputUnit, AngleUnit.Radians);
            Quaternion quaternion;
            double num9 = roll * 0.5f;
            double num6 = (double)Math.Sin((double)num9);
            double num5 = (double)Math.Cos((double)num9);
            double num8 = pitch * 0.5f;
            double num4 = (double)Math.Sin((double)num8);
            double num3 = (double)Math.Cos((double)num8);
            double num7 = yaw * 0.5f;
            double num2 = (double)Math.Sin((double)num7);
            double num = (double)Math.Cos((double)num7);
            quaternion.x = ((num * num4) * num5) + ((num2 * num3) * num6);
            quaternion.y = ((num2 * num3) * num5) - ((num * num4) * num6);
            quaternion.z = ((num * num3) * num6) - ((num2 * num4) * num5);
            quaternion.w = ((num * num3) * num5) + ((num2 * num4) * num6);
            return quaternion;
        }

        public static Quaternion Divide(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion quaternion;
            double x = quaternion1.x;
            double y = quaternion1.y;
            double z = quaternion1.z;
            double w = quaternion1.w;
            double num14 = (((quaternion2.x * quaternion2.x) + (quaternion2.y * quaternion2.y)) + (quaternion2.z * quaternion2.z)) + (quaternion2.w * quaternion2.w);
            double num5 = 1f / num14;
            double num4 = -quaternion2.x * num5;
            double num3 = -quaternion2.y * num5;
            double num2 = -quaternion2.z * num5;
            double num = quaternion2.w * num5;
            double num13 = (y * num2) - (z * num3);
            double num12 = (z * num4) - (x * num2);
            double num11 = (x * num3) - (y * num4);
            double num10 = ((x * num4) + (y * num3)) + (z * num2);
            quaternion.x = ((x * num) + (num4 * w)) + num13;
            quaternion.y = ((y * num) + (num3 * w)) + num12;
            quaternion.z = ((z * num) + (num2 * w)) + num11;
            quaternion.w = (w * num) - num10;
            return quaternion;

        }

        public static void Divide(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
        {
            double x = quaternion1.x;
            double y = quaternion1.y;
            double z = quaternion1.z;
            double w = quaternion1.w;
            double num14 = (((quaternion2.x * quaternion2.x) + (quaternion2.y * quaternion2.y)) + (quaternion2.z * quaternion2.z)) + (quaternion2.w * quaternion2.w);
            double num5 = 1f / num14;
            double num4 = -quaternion2.x * num5;
            double num3 = -quaternion2.y * num5;
            double num2 = -quaternion2.z * num5;
            double num = quaternion2.w * num5;
            double num13 = (y * num2) - (z * num3);
            double num12 = (z * num4) - (x * num2);
            double num11 = (x * num3) - (y * num4);
            double num10 = ((x * num4) + (y * num3)) + (z * num2);
            result.x = ((x * num) + (num4 * w)) + num13;
            result.y = ((y * num) + (num3 * w)) + num12;
            result.z = ((z * num) + (num2 * w)) + num11;
            result.w = (w * num) - num10;

        }


        public static double Dot(Quaternion quaternion1, Quaternion quaternion2)
        {
            return ((((quaternion1.x * quaternion2.x) + (quaternion1.y * quaternion2.y)) + (quaternion1.z * quaternion2.z)) + (quaternion1.w * quaternion2.w));
        }

        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is Quaternion)
            {
                flag = this.Equals((Quaternion)obj);
            }
            return flag;
        }


        public bool Equals(Quaternion other)
        {
            return ((((this.x == other.x) && (this.y == other.y)) && (this.z == other.z)) && (this.w == other.w));
        }


        public override int GetHashCode()
        {
            return (((this.x.GetHashCode() + this.y.GetHashCode()) + this.z.GetHashCode()) + this.w.GetHashCode());
        }
        
        public double Length
        {
            get
            {
                {
                    double num = (((this.x * this.x) + (this.y * this.y)) + (this.z * this.z)) + (this.w * this.w);
                    return (double)Math.Sqrt((double)num);
                }
            }
        }


        public double LengthSquared
        {
            get
            {
                return ((((this.x * this.x) + (this.y * this.y)) + (this.z * this.z)) + (this.w * this.w));
            }
        }


        public static Quaternion Lerp(Quaternion quaternion1, Quaternion quaternion2, double amount)
        {
            double num = amount;
            double num2 = 1f - num;
            Quaternion quaternion = new Quaternion();
            double num5 = (((quaternion1.x * quaternion2.x) + (quaternion1.y * quaternion2.y)) + (quaternion1.z * quaternion2.z)) + (quaternion1.w * quaternion2.w);
            if (num5 >= 0f)
            {
                quaternion.x = (num2 * quaternion1.x) + (num * quaternion2.x);
                quaternion.y = (num2 * quaternion1.y) + (num * quaternion2.y);
                quaternion.z = (num2 * quaternion1.z) + (num * quaternion2.z);
                quaternion.w = (num2 * quaternion1.w) + (num * quaternion2.w);
            }
            else
            {
                quaternion.x = (num2 * quaternion1.x) - (num * quaternion2.x);
                quaternion.y = (num2 * quaternion1.y) - (num * quaternion2.y);
                quaternion.z = (num2 * quaternion1.z) - (num * quaternion2.z);
                quaternion.w = (num2 * quaternion1.w) - (num * quaternion2.w);
            }
            double num4 = (((quaternion.x * quaternion.x) + (quaternion.y * quaternion.y)) + (quaternion.z * quaternion.z)) + (quaternion.w * quaternion.w);
            double num3 = 1f / ((double)Math.Sqrt((double)num4));
            quaternion.x *= num3;
            quaternion.y *= num3;
            quaternion.z *= num3;
            quaternion.w *= num3;
            return quaternion;
        }


        public static Quaternion Slerp(Quaternion quaternion1, Quaternion quaternion2, double amount)
        {
            double num2;
            double num3;
            Quaternion quaternion;
            double num = amount;
            double num4 = (((quaternion1.x * quaternion2.x) + (quaternion1.y * quaternion2.y)) + (quaternion1.z * quaternion2.z)) + (quaternion1.w * quaternion2.w);
            bool flag = false;
            if (num4 < 0f)
            {
                flag = true;
                num4 = -num4;
            }
            if (num4 > 0.999999f)
            {
                num3 = 1f - num;
                num2 = flag ? -num : num;
            }
            else
            {
                double num5 = (double)Math.Acos((double)num4);
                double num6 = (double)(1.0 / Math.Sin((double)num5));
                num3 = ((double)Math.Sin((double)((1f - num) * num5))) * num6;
                num2 = flag ? (((double)-Math.Sin((double)(num * num5))) * num6) : (((double)Math.Sin((double)(num * num5))) * num6);
            }
            quaternion.x = (num3 * quaternion1.x) + (num2 * quaternion2.x);
            quaternion.y = (num3 * quaternion1.y) + (num2 * quaternion2.y);
            quaternion.z = (num3 * quaternion1.z) + (num2 * quaternion2.z);
            quaternion.w = (num3 * quaternion1.w) + (num2 * quaternion2.w);
            return quaternion;
        }


        public static Quaternion Subtract(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion quaternion;
            quaternion.x = quaternion1.x - quaternion2.x;
            quaternion.y = quaternion1.y - quaternion2.y;
            quaternion.z = quaternion1.z - quaternion2.z;
            quaternion.w = quaternion1.w - quaternion2.w;
            return quaternion;
        }

        public static Quaternion Multiply(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion quaternion;
            double x = quaternion1.x;
            double y = quaternion1.y;
            double z = quaternion1.z;
            double w = quaternion1.w;
            double num4 = quaternion2.x;
            double num3 = quaternion2.y;
            double num2 = quaternion2.z;
            double num = quaternion2.w;
            double num12 = (y * num2) - (z * num3);
            double num11 = (z * num4) - (x * num2);
            double num10 = (x * num3) - (y * num4);
            double num9 = ((x * num4) + (y * num3)) + (z * num2);
            quaternion.x = ((x * num) + (num4 * w)) + num12;
            quaternion.y = ((y * num) + (num3 * w)) + num11;
            quaternion.z = ((z * num) + (num2 * w)) + num10;
            quaternion.w = (w * num) - num9;
            return quaternion;
        }


        public static Quaternion Multiply(Quaternion quaternion1, double scaleFactor)
        {
            Quaternion quaternion;
            quaternion.x = quaternion1.x * scaleFactor;
            quaternion.y = quaternion1.y * scaleFactor;
            quaternion.z = quaternion1.z * scaleFactor;
            quaternion.w = quaternion1.w * scaleFactor;
            return quaternion;
        }


        public static void Multiply(ref Quaternion quaternion1, double scaleFactor, out Quaternion result)
        {
            result.x = quaternion1.x * scaleFactor;
            result.y = quaternion1.y * scaleFactor;
            result.z = quaternion1.z * scaleFactor;
            result.w = quaternion1.w * scaleFactor;
        }


        public Quaternion Negated
        {
            get
            {
                Quaternion quaternion2;
                quaternion2.x = -x;
                quaternion2.y = -y;
                quaternion2.z = -z;
                quaternion2.w = -w;
                return quaternion2;
            }
        }


        public void Normalize()
        {
            double num2 = (((this.x * this.x) + (this.y * this.y)) + (this.z * this.z)) + (this.w * this.w);
            double num = 1f / ((double)Math.Sqrt((double)num2));
            this.x *= num;
            this.y *= num;
            this.z *= num;
            this.w *= num;
        }


        public Quaternion Normalized
        {
            get
            {
                Quaternion quaternion2;
                double num2 = (((x * x) + (y * y)) + (z * z)) + (w * w);
                double num = 1f / ((double)Math.Sqrt((double)num2));
                quaternion2.x = x * num;
                quaternion2.y = y * num;
                quaternion2.z = z * num;
                quaternion2.w = w * num;
                return quaternion2;
            }
        }

        public static Quaternion operator +(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion quaternion;
            quaternion.x = quaternion1.x + quaternion2.x;
            quaternion.y = quaternion1.y + quaternion2.y;
            quaternion.z = quaternion1.z + quaternion2.z;
            quaternion.w = quaternion1.w + quaternion2.w;
            return quaternion;
        }
        

        public static Quaternion operator /(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion quaternion;
            double x = quaternion1.x;
            double y = quaternion1.y;
            double z = quaternion1.z;
            double w = quaternion1.w;
            double num14 = (((quaternion2.x * quaternion2.x) + (quaternion2.y * quaternion2.y)) + (quaternion2.z * quaternion2.z)) + (quaternion2.w * quaternion2.w);
            double num5 = 1f / num14;
            double num4 = -quaternion2.x * num5;
            double num3 = -quaternion2.y * num5;
            double num2 = -quaternion2.z * num5;
            double num = quaternion2.w * num5;
            double num13 = (y * num2) - (z * num3);
            double num12 = (z * num4) - (x * num2);
            double num11 = (x * num3) - (y * num4);
            double num10 = ((x * num4) + (y * num3)) + (z * num2);
            quaternion.x = ((x * num) + (num4 * w)) + num13;
            quaternion.y = ((y * num) + (num3 * w)) + num12;
            quaternion.z = ((z * num) + (num2 * w)) + num11;
            quaternion.w = (w * num) - num10;
            return quaternion;
        }


        public static bool operator ==(Quaternion quaternion1, Quaternion quaternion2)
        {
            return ((((quaternion1.x == quaternion2.x) && (quaternion1.y == quaternion2.y)) && (quaternion1.z == quaternion2.z)) && (quaternion1.w == quaternion2.w));
        }


        public static bool operator !=(Quaternion quaternion1, Quaternion quaternion2)
        {
            if (((quaternion1.x == quaternion2.x) && (quaternion1.y == quaternion2.y)) && (quaternion1.z == quaternion2.z))
            {
                return (quaternion1.w != quaternion2.w);
            }
            return true;
        }


        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            double x = left.w * right.x + left.x * right.w + left.y * right.z - left.z * right.y;
            double y = left.w * right.y + left.y * right.w + left.z * right.x - left.x * right.z;
            double z = left.w * right.z + left.z * right.w + left.x * right.y - left.y * right.x;
            double w = left.w * right.w - left.x * right.x - left.y * right.y - left.z * right.z;
            Quaternion result = new Quaternion(x, y, z, w);
            return result;
        }


        public static Quaternion operator *(Quaternion quaternion1, double scaleFactor)
        {
            Quaternion quaternion;
            quaternion.x = quaternion1.x * scaleFactor;
            quaternion.y = quaternion1.y * scaleFactor;
            quaternion.z = quaternion1.z * scaleFactor;
            quaternion.w = quaternion1.w * scaleFactor;
            return quaternion;
        }


        public static Quaternion operator -(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion quaternion;
            quaternion.x = quaternion1.x - quaternion2.x;
            quaternion.y = quaternion1.y - quaternion2.y;
            quaternion.z = quaternion1.z - quaternion2.z;
            quaternion.w = quaternion1.w - quaternion2.w;
            return quaternion;

        }


        public static Quaternion operator -(Quaternion quaternion)
        {
            Quaternion quaternion2;
            quaternion2.x = -quaternion.x;
            quaternion2.y = -quaternion.y;
            quaternion2.z = -quaternion.z;
            quaternion2.w = -quaternion.w;
            return quaternion2;
        }


        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(32);
            sb.Append("Render3.Core.Quaternion.ToString(): X:");
            sb.Append(this.x);
            sb.Append(", Y:");
            sb.Append(this.y);
            sb.Append(", Z:");
            sb.Append(this.z);
            sb.Append(", W:");
            sb.Append(this.w);
            return sb.ToString();
        }
        internal Direction3 xyz
        {
            get
            {
                return new Direction3(x, y, z);
            }

            set
            {
                x = value.x;
                y = value.y;
                z = value.z;
            }
        }


    }
}