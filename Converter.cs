using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Enums.Node;
using Structs;
using EntSys;

namespace Statics
{
    class Converter
    {
        public Converter() { }

        public static Rectangle SBox2Rect(S_Box a)
        {
            Rectangle toRet = new Rectangle(a.loc.x, a.loc.y, a.size.x, a.size.y);
            return toRet;
        }

        public static Enums.Navigation.Compass OverlapToCompass(OverlapType[] otype)
        {
            Enums.Navigation.Compass[,] coords = new Enums.Navigation.Compass[,]
            {{Enums.Navigation.Compass.N,Enums.Navigation.Compass.N,Enums.Navigation.Compass.N},
             {Enums.Navigation.Compass.W,Enums.Navigation.Compass.Center,Enums.Navigation.Compass.E},
             {Enums.Navigation.Compass.S,Enums.Navigation.Compass.S,Enums.Navigation.Compass.S}};

            S_XY value = new S_XY();
            if (otype[0] == OverlapType.Left)
                value.x = 0;
            else if (otype[0] == OverlapType.Right)
                value.x = 2;
            else
                value.x = 1;

            if (otype[1] == OverlapType.Left)
                value.y = 0;
            else if (otype[1] == OverlapType.Right)
                value.y = 2;
            else
                value.y = 1;

            return coords[value.y,value.x];

        }

        public static objBaseType ObjTypeToBaseType(objType o)
        {
            switch (o)
            {
                case objType.Body:
                case objType.Explosion:
                case objType.Ground:
                    return objBaseType.Ent;
                default:
                    return objBaseType.Misc;
                    


            }


        }

        public static Vector2 getMag(Vector2 v)
        {
            Vector2 tr = new Vector2();

            if (v.X == 0)
                tr.X = 0;
            else
                tr.X = (Math.Abs(v.X) / v.X);

            if (v.Y == 0)
                tr.Y = 0;
            else
                tr.Y = (Math.Abs(v.Y) / v.Y);


            return tr;



        }

    }
}
