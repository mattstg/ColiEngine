
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
            public S_XY(S_XY copyMe)
            {
                x = copyMe.x;
                y = copyMe.y;
            }

            public string GenString()
            {
                return "x: " + x + " y: " + y;
            }

        }
    }
