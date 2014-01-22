using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Enums.Node;
using Structs;

namespace ColiSys
{
    class Converter
    {
        public Converter() { }

        public static Rectangle SBox2Rect(S_Box a)
        {
            Rectangle toRet = new Rectangle(a.loc.x, a.loc.y, a.size.x, a.size.y);
            return toRet;
        }


    }
}
