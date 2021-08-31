using System;
using System.Diagnostics;

namespace Render3.Core
{
    public struct Color
    {

        public static Color Merge(Color a, Color b)
        {
            return new Color(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);
        }

        private double _r;
        public double r { get { return _r; } set { if (value >= 0 && value <= 1) _r = value; else throw new InvalidOperationException("r should be between 0 and 1"); } }

        public Color(double r, double g, double b) : this()
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = 1;
        }

        public Color(double r, double g, double b, double a) : this(r, g, b)
        {
            this.a = a;
        }

        private double _g;
        public double g { get { return _g; } set { if (value >= 0 && value <= 1) _g = value; else throw new InvalidOperationException("g should be between 0 and 1"); } }
        private double _b;
        public double b
        {
            get { return _b; }
            set
            {
                if (value >= 0 && value <= 1) _b = value; else throw new InvalidOperationException("b should be between 0 and 1");
            }
        }
        private double _a;
        public double a { get { return _a; } set { if (value >= 0 && value <= 1) _a = value; else throw new InvalidOperationException("a should be between 0 and 1"); } }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public static bool operator==(Color a,Color b)
        {
            return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
        }

        public static bool operator !=(Color a, Color b)
        {
            return a.r != b.r || a.g != b.g || a.b != b.b || a.a != b.a;
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}