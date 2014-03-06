using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace Structs
{
    

    namespace Navigation
    {
        public class Compass
        {
            private Enums.Navigation.Compass[,] comp;
            public Enums.Navigation.Compass dir;


            public Compass()
            {
                comp = new Enums.Navigation.Compass[3, 3]{{Enums.Navigation.Compass.NW,Enums.Navigation.Compass.N,Enums.Navigation.Compass.NE},
                                                         {Enums.Navigation.Compass.W,Enums.Navigation.Compass.Center,Enums.Navigation.Compass.E},
                                                         {Enums.Navigation.Compass.SW,Enums.Navigation.Compass.S,Enums.Navigation.Compass.SE}};

            }
            
            public Enums.Navigation.Compass SetCompass(int[] t)
            {
                dir = comp[t[0],t[1]];
                return dir;
            }


        }


    }

    public class S_XY
    {

        //S_XY moo = S_XY(5,5)
        //moo.x = 4;
        public int x;
        public int y;

        public S_XY() { x = 0; y = 0; }
        public S_XY(int tx, int ty) { x = tx; y = ty; }
       // public S_XY(float tx, float ty) { x = (int)tx; y = (int)ty; }
        public S_XY(S_XY copyMe)
        {
            x = copyMe.x;
            y = copyMe.y;
        }
        

        public string GenString()
        {
            return "x: " + x + " y: " + y;
        }

        public static S_XY operator /(S_XY s1, float s2)
        {
            return new S_XY((int)(s1.x / s2), (int)(s1.y / s2));

        }
        public static S_XY operator +(S_XY s1, S_XY s2)
        {
            return new S_XY(s1.x + s2.x, s1.y + s2.y);
        }

        public static S_XY operator +(S_XY s1, int s2)
        {
            return new S_XY(s1.x + s2, s1.y + s2);
        }

        public static S_XY operator *(S_XY s1, int mult)
        {
            return new S_XY(s1.x * mult, s1.y * mult);
        }
        public static S_XY operator -(S_XY s1, S_XY s2)
        {
            return new S_XY(s1.x - s2.x, s1.y - s2.y);
        }
        
      

    }

    public class S_Box
    {

        //S_Box moo = new S_Box(new S_XY(),new S_XY());   how to init shortcut
        // moo.loc.x = 5;
        // moo.size.y = -2;


        //error to look out for, becomes null out of scope, if that is case, make a copy constr 
        public S_XY loc;
        public S_XY size;

        public S_Box() { loc = new S_XY(); size = new S_XY(); }
        public S_Box(S_XY tLoc, S_XY tSize) { loc = tLoc; size = tSize; }
        public S_Box(int locx, int locy, int sizex, int sizey) { loc = new S_XY(locx, locy); size = new S_XY(sizex, sizey); }
        public S_Box(int locx, int locy, int sizeLocx, int sizeLocy, bool tsize)
        {
            if (tsize)
            {
                loc = new S_XY(locx, locy); size = new S_XY(sizeLocx, sizeLocy);
            }
            else
            {
                loc = new S_XY(locx, locy); size = new S_XY(sizeLocx - locx + 1, sizeLocy - locy + 1);
            }
        }
        public S_Box(S_Box copyMe)
        {
            this.loc = new S_XY(copyMe.loc);
            this.size = new S_XY(copyMe.size);

        }

        public string GenString()
        {
            return "loc: " + loc.GenString() + " size: " + size.GenString();
        }

    }

    
    /*
    class ColiListConnector
    {
        

        public ColiSys.Hashtable hashTable;
        public Enums.ColiObjTypes.ColiTypes coliType;
        
        private EntSys.Ground _ground;


        public ColiListConnector(ColiSys.Hashtable ptable, Enums.ColiObjTypes.ColiTypes pcoliType)
        {
            hashTable = ptable;
            coliType = pcoliType;
        }

        public ColiListConnector(EntSys.Ground g)
        {
            _ground = g;
            hashTable = g.htable;
            coliType = Enums.ColiObjTypes.ColiTypes.Dirt;
        }

        public object retObj()
        {
            return _ground;

        }*
    }*/

}


namespace EntSys
{
    struct AERetType { };
    struct DamageType
    {
        float piercing;
        float blunt;
        float groundShrapnel;


    }
}