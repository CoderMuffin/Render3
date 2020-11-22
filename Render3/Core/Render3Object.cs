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
    }
}
