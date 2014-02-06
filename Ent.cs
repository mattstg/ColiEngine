namespace ColiSys
{
    public class Ent
    {
        int[] loc = new int[2]; //location of the top OverlapType.Left of the entity
        int[] size = new int[2]; //the size of the entity

        public Ent(int ox, int oy, int sx, int sy)
        {
            loc[0] = ox;
            loc[1] = oy;
            size[0] = sx;
            size[1] = sy;
        }

        public Ent()
        { //default cunstructor creates 1x1 entity @ (0,0)
            loc[0] = 0;
            loc[1] = 0;
            size[0] = 2;
            size[1] = 2;
        }

        public Ent(Ent E)
        { //copy constructor
            loc = E.getLoc();
            size = E.getSize();
        }


        //Get and Sets
        public void setLoc(int x, int y)
        {
            loc[0] = x;
            loc[1] = y;
        }

        public int[] getLoc()
        {
            return loc;
        }

        public void setSize(int x, int y)
        {
            size[0] = x;
            size[1] = y;
        }

        public int[] getSize()
        {
            return size;
        }

        public string GenString()
        {
            return "width:" + size[0] + "  hight:" + size[1] + "  & @:" + "(" + loc[0] + " , " + loc[1] + ")";
        }

    }
}