using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
 * Pack 0, default pack for player
 * Pack 1, default pack for explosions
 * 
 */
namespace EntSys
{
    class AbilityStore
    {
        Global.Bus bus = Global.Bus.Instance;


        private static AbilityStore instance;
        private AbilityStore() { }
        public static AbilityStore Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AbilityStore();
                }
                return instance;
            }
        }  
        
        public void RegAbilityPack(int packNum, List<Func<Object,BodyMechanics,Ground,bool>> bodGroundActions,List<Func<Object,Explosion, Ground, bool>> expGroundActions)
        {
            switch(packNum)
            {
                case 0:
                    bodGroundActions.Add(bg1);
                    return;
                    
                case 1:
                    expGroundActions.Add(eg1);
                    return;

                default:
                    break;


            }

        }








        /////For now, all Events will be created here, they should be created elsewhere i guess... this way for custom skill generation
        private bool bg1(Object callingObj,BodyMechanics bod, Ground ground)
        {          

                bus.LoadPassenger(new Explosion(bod.trueEntShape, new Structs.S_XY(bod.offsetCopy.x, bod.offsetCopy.y + 14),50,null));
                Console.Out.Write("YO SHIT BE HAPPENING");   
            return true;
        }

        private bool eg1(Object callingObj, Explosion exp, Ground ground)
        {
            Console.Out.Write("EXPLOSION HIT GROUND!");
            return true;

        }

    }
}
