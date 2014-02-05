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
        
        public void RegAbilityPack(int packNum, List<Func<Object,BodyMechanics,Ground,bool>> BmGActions,List<Func<Object,Explosion, Ground, bool>> ExpGActions
                                    ,List<Func<Object,BodyMechanics,Explosion,bool>> BmExpActions)
        {
            switch(packNum)
            {
                case 0: //Bm
                    BmGActions.Add(BmGCreateExp);  
                    BmGActions.Add(callG);
                    BmExpActions.Add(BmColiExp);
                    
                    return;
                    
                case 1: //Exp
                    ExpGActions.Add(ExpGAlterPath);
                    ExpGActions.Add(callG);
                    BmExpActions.Add(ExpColiBm);
                    return;
                
                case 2: //Ground
                    ExpGActions.Add(GcoliExp);
                    BmGActions.Add(GcoliBm);
                    return;


                default:
                    break;


            }

        }








        /////For now, all Events will be created here, they should be created elsewhere i guess... this way for custom skill generation
        private bool BmGCreateExp(Object callingObj,BodyMechanics bm, Ground ground)
        {          
                //used in body hitting ground, create explosion, apply on ground effects, or do nothing
                //Not used to exert normal forces on body, ground should do that, but most likely, this function, or another ground function, should call
                //grounds func since ground doesnt do coli testing
                bus.LoadPassenger(new Explosion(bm.trueEntShape, new Structs.S_XY(bm.offsetCopy.x, bm.offsetCopy.y + 14),50,null));
                Console.Out.Write("YO SHIT BE HAPPENING");   
            return true;
        }

        private bool ExpGAlterPath(Object callingObj, Explosion exp, Ground ground)
        {
            //Exp
            //Exp hitting the ground
            //explosion hits ground, possibility of altering explosion path?
            Console.Out.Write("EXPLOSION HIT GROUND!");
            return true;

        }

        private bool callG(Object callingObj,BodyMechanics bm, Ground ground)
        {
            //Bm            
            //When body hits ground, body needs to call grounds AE
            ground.AE.TriggerEvent(bm,ground);


            return false;

        }

        private bool callG(Object callingObj,Explosion exp, Ground ground)
        {
            //Exp
            //When exp hits ground, exp needs to call grounds AE
            ground.AE.TriggerEvent(exp,ground);


            return false;

        }

        private bool BmColiExp(Object callingObj,BodyMechanics bm, Explosion exp)
        {
            //Body
            //Body being hit by Explosion
            //Body choses how to react to exp? This way could cancel force or have abilities that stop it, but then again, this could be inside of
            //exp as well and call method forcing an explosion on body.. which would prob call an AE anyways.. 


            return false;

        }

        private bool ExpColiBm(Object callingObj,BodyMechanics bm, Explosion exp)
        {
            //Exp
            //Body being hit by Explosion
            //Since forces are handled by body already, this should send the status/special effects? maybe this should do force as well, and 
            //just call the funcs for bm here? hmm

            return false;

        }

        private bool GcoliExp(Object callingObj,Explosion exp, Ground ground)
        {
            //Ground:
            //When the explosion hits the ground
            //Subtract dirt or do nothing

            return false;

        }

         private bool GcoliBm(Object callingObj,BodyMechanics bm, Ground ground)
         {
             //Ground being hit by body, apply normal Forces back on bod,  or destroy and subtract from ground
             //this AE belongs to ground, but ground will not call it since it doesnt have coli check on update, 
             //the body that collides with ground should call this

             //Apply Forces or check if breaks
             return true;
         }


    }
}
