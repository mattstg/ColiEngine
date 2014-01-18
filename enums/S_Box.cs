namespace ColiSys
{
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
        public S_Box(int locx, int locy, int sizeLocx, int sizeLocy, bool tsize) {
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


}