using Structs;


namespace Consts {
    

    class World
    {
        public const float gravity = 0.01f;


    }

    public class TopScope
    {

        public const int WORLD_SIZE_X = 320;  //but world is 0-31? careful double check this
        public const int WORLD_SIZE_Y = 180; //important to note the world goes from 0 to size-1. in the hash table, it will be 0-24 ex, subject to change? not sure see where conveince goes


        //Statics, should they be ina  static file?

        public static Structs.S_XY GAME_SCALE;
        public static int GRAPHICS_BUFFER_WIDTH;
        public static int GRAPHICS_BUFFER_HEIGHT;

        public static int BRUSH_SIZE = 1;
        public const int BRUSH_MIN_SIZE = 1;
        public const int BRUSH_MAX_SIZE = 60;


    }
}
    



