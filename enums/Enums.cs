﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enums
{
    namespace Force
    {
        enum ForceTypes{Internal,NaturalLaw}



    }

     namespace Node
    {
        public enum Bounds { l, u };
        public enum ENode { adj, dwn };
        public enum copyTypes { copyNode, copyDwn, copyAdj, copyBoth };
        public enum OverlapType { Right, Left, OEA, AEO, Equals, Before, After };
        public enum Shape { Circle, Square, ConsoleIn, Human };

    }

     namespace ColiObjTypes
     {
         
         public enum ColiTypes {None, Dirt, Magic};
         public enum Ground { Dirt, Stone };
         public enum Magic { Fire, Stone };

     }


}
