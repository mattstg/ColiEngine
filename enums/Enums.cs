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
    public enum objSpecificType { Ent = 0, Sprite = 1, Material = 2, Body = 3, Bm = 4, Human = 5, Exp = 6, BodyPart = 7, Ground = 6 };
    public enum dir { up = 0, right = 1, down = 2, left = 3 }



    class ObjType
    {
        objSpecificType type;
        public ObjType() { }
        public ObjType(objSpecificType sType)
        {
            type = sType;
        }


        public static bool operator >(ObjType o1, ObjType o2)
        {
            if(o1.type != objSpecificType.BodyPart && o2.type != objSpecificType.BodyPart) //then just compare by values
            {
                return ((int)o1.type > (int)o2.type);


            } else { //special case
                int a = (int)o1.type;
                int b = (int)o2.type;

                if (o1.type == objSpecificType.BodyPart)
                    a = 4;
                if (o2.type == objSpecificType.BodyPart)
                    b = 4;

                return a > b;

            }

        }

        public static bool operator <(ObjType o1, ObjType o2)
        {
            if (o1.type != o2.type)
            {
                return !(o1 > o2);

            }
            return false;
        }

        public static bool operator ==(ObjType o1, ObjType o2)
        {
            return (o1.type == o2.type);
        }

        public static bool operator >=(ObjType o1, ObjType o2)
        {
            return (o1 > o2) || (o1 == o2);
        }

        public static bool operator <=(ObjType o1, ObjType o2)
        {
            return (o1 < o2) || (o1 == o2);

        }

        public static bool operator !=(ObjType o1, ObjType o2)
        {
            return (o1.type != o2.type);
        }



    }

    
}
