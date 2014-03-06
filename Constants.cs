using Structs;
//seeing how this is growing, there should be a data class called ground, which has different kinds, singleton librarian of sorts
//library will have ptr to all consts used in formulas, for physics and ex and can eventaully alter them, librarian not needed till end, but program with him in mind
//ex using a forumula with something u know is constant, make it static instead, it will be changed possibly
//eventaully all statics willbe put in some weird singleton class, taking away the need for statics, which will eventaully clum-p up the sys 
//guess ground will extend materials, a factory pattern will create these materials, so although there are some saved natural ones, we can make custom materials too

namespace Consts {
    /*class Ground
    {
        
        //so single object with these vars, a type, energy, 
        public static float DIRT_HP = 15;
        public static float DIRT_BOUNCE_THRESHHOLD = .33f; //bounce within 90-100%
        public static float DIRT_BOUNCE_FORCE_MULTIPLIER_LB = .2f;
        public static float DIRT_BOUNCE_FORCE_MULTIPLIER_UB = 1f;
        public static float DIRT_ABSORBPTION = 3; //times it own health worth of force repelling        

        public static float DirtColiForce(float ColiForce)  //this will merge all into one type as well, called in object
        {
            return _Internal.getBounceForce(ColiForce,DIRT_HP,DIRT_BOUNCE_FORCE_MULTIPLIER_LB,DIRT_BOUNCE_FORCE_MULTIPLIER_UB,DIRT_BOUNCE_THRESHHOLD,DIRT_ABSORBPTION);
        }


    }*/

     

    class World
    {
        public const float gravity = 60f;


    }

    public class TopScope
    {

        public static int WORLD_SIZE_X = 1920;// 1280;  //but world is 0-31? careful double check this
        public static int WORLD_SIZE_Y = 1080;//720; //important to note the world goes from 0 to size-1. in the hash table, it will be 0-24 ex, subject to change? not sure see where conveince goes


        //Statics, should they be ina  static file?

        public static Structs.S_XY GAME_SCALE;
        public static int GRAPHICS_BUFFER_WIDTH;
        public static int GRAPHICS_BUFFER_HEIGHT;

        public static int BRUSH_SIZE = 1;
        public const int BRUSH_MIN_SIZE = 1;
        public const int BRUSH_MAX_SIZE = 600;


    }






        //private internal classes
   /* class _Internal
    {//The bounce force can be calculated using real physics if need be, then user can alter those constants :D  
        public static float getBounceForce(float force,float hp,float lb, float ub, float thresh, float absorb)
        {
           float fMult = 1;

            if(force < hp*thresh)
                return force + 1; //not enough force to trigger break or bounce, return his force as ground Normal balancing out
            else
            {
                if(force < hp)
                {           //doesnt break it but bounces    
                    fMult = (ub-lb)*(force/hp - thresh) + lb;
                    return force + force* fMult;
                } else  //breaks it
                    if (force > hp * absorb)
                        return hp * absorb; //max force returnable
                    else
                        return force + 1;  //less than max, returns current force since all absorbed --future if dirt is object, return energy abosrded to the Material/Metals/Earth object
            }

        }



    }*/
}


    



