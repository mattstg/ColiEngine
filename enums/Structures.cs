using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace Structs
{
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

    class ColiListConnector
    {
        

        public ColiSys.Hashtable hashTable;
        public Enums.ColiObjTypes.ColiTypes coliType;

        public ColiListConnector(ColiSys.Hashtable ptable, Enums.ColiObjTypes.ColiTypes pcoliType)
        {
            hashTable = ptable;
            coliType = pcoliType;
        }
    }

}
