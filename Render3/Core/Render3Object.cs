using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Core
{
    public abstract class Render3Object
    {
        public string UUID = null;
        public Render3Object()
        {
            UUID = Guid.NewGuid().ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is Render3Object @object &&
                   UUID == @object.UUID;
        }

        public override int GetHashCode()
        {
            return 2006673922 + EqualityComparer<string>.Default.GetHashCode(UUID);
        }

        public static bool operator ==(Render3Object obj,Render3Object obj2)
        {
            if (obj is null) return obj2 is null;
            if (obj2 is null) return false;
            return obj.UUID == obj2.UUID;
        }
        public static bool operator !=(Render3Object obj, Render3Object obj2)
        {
            if (obj is null) return !(obj2 is null);
            if (obj2 is null) return true;
            return obj.UUID != obj2.UUID;
        }
    }
}
