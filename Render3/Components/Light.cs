using Render3.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Components
{
    public class Light
    {
        public Direction3 direction=new Direction3(1,1,2);
        public System.Drawing.Color color=System.Drawing.Color.FromArgb(255,255,255);
    }
}
