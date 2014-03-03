using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enums
{
    namespace Force
    {
        public enum ForceTypes { Coli, Internal, NaturalLaw, Dirt }



    }

    namespace Global{

        public enum VoidableTypes {Explosion }

    }

    namespace Navigation
    {
        public enum Compass { N, NE, E, SE, S, SW, W, NW, Center};


        
    }

     namespace Node
    {
         public enum Bounds { l, u };
        public enum ENode { adj, dwn };
        public enum copyTypes { copyNode, copyDwn, copyAdj, copyBoth };
        public enum OverlapType { Right, Left, OEA, AEO, Equals, Before, After };
        

    }

     namespace ColiObjTypes
     {
         
         public enum ColiTypes {None, Dirt, Magic, Explosion};
         public enum Ground { Dirt, Stone };
         public enum Magic { Fire, Stone };

     }


}

namespace EntSys
{
    public enum objType { Body = 0, Explosion = 1, Ground = 2, None = 99 };
    public enum objBaseType { Ground, Ent, Misc };
    public enum objSpecificType { Ent = 0, Sprite = 1, Body = 2, Bm = 3, Human = 4, Exp = 5, Ground = 6, BodyPart = 7, Material = 8 };
    public enum dir { up = 0, right = 1, down = 2, left = 3 }
    
}
