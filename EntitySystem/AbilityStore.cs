﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BodyParts;
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

        public void RegAbilityPack(int packNum, List<Func<VagueObject, BodyMechanics, Ground, AERetType>> BmGActions, List<Func<VagueObject, Explosion, Ground, AERetType>> ExpGActions
                                    , List<Func<VagueObject, BodyMechanics, Explosion, AERetType>> BmExpActions, List<Func<VagueObject, KeyboardState, AERetType>> KeyActions)
        {
            switch(packNum)
            {
                case 0: //Bm
                    BmGActions.Add(BmGCreateExp);  
                    BmGActions.Add(callG);
                    BmExpActions.Add(BmColiExp);
                    KeyActions.Add(HumanKeyMove);
                   // BmGActions.Add(BodyHitsGround);
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
        private AERetType BmGCreateExp(VagueObject callingObj, BodyMechanics bm, Ground ground)
        {          
                //used in body hitting ground, create explosion, apply on ground effects, or do nothing
                //Not used to exert normal forces on body, ground should do that, but most likely, this function, or another ground function, should call
                //grounds func since ground doesnt do coli testing
                bus.LoadPassenger(new Explosion(bm.trueEntShape, new Structs.S_XY(bm.offsetCopy.x, bm.offsetCopy.y + 14),50,null));
                Console.Out.Write("YO SHIT BE HAPPENING");
                return new AERetType();
        }

        //requirement, min Human, is there  a way to force requirements?
        private AERetType HumanKeyMove(VagueObject callingObj, KeyboardState ks)
        {
            //Calling this will be an entity. So applying force to it based on its body's restrictions

            HumanPlayer human = callingObj.getObj<HumanPlayer>();
            if (ks.IsKeyDown(human.keymap.down))
                human.MoveInDir(dir.down);
            if (ks.IsKeyDown(human.keymap.right))
                human.MoveInDir(dir.right);
            if (ks.IsKeyDown(human.keymap.left))
                human.MoveInDir(dir.left);
            if(human.HasBodyPart(BodyPartType.Wings))
                 if (ks.IsKeyDown(human.keymap.up))
                     human.MoveInDir(dir.up);

            return new AERetType();
        }

        private AERetType BodyHitsGround(VagueObject callingObj, BodyMechanics Bm, Ground ground)
        {


            return new AERetType();
        }

        private AERetType ExpGAlterPath(VagueObject callingObj, Explosion exp, Ground ground)
        {
            //Exp
            //Exp hitting the ground
            //explosion hits ground, possibility of altering explosion path?
          //  Console.Out.Write("EXPLOSION HIT GROUND!");
            //nah should just be absorded, but bouncing would be cool if it doesnt break it, like
            //how ground bounces
            return new AERetType();

        }

        private AERetType callG(VagueObject callingObj, BodyMechanics bm, Ground ground)
        {
            //Bm            
            //When body hits ground, body needs to call grounds AE
            ground.AE.TriggerEvent(bm,ground);


            return new AERetType();

        }

        private AERetType callG(VagueObject callingObj, Explosion exp, Ground ground)
        {
            //Exp
            //When exp hits ground, exp needs to call grounds AE
            ground.AE.TriggerEvent(exp,ground);


            return new AERetType();

        }

        private AERetType BmColiExp(VagueObject callingObj, BodyMechanics bm, Explosion exp)
        {
            //Body
            //Body being hit by Explosion
            //Body choses how to react to exp? This way could cancel force or have abilities that stop it, but then again, this could be inside of
            //exp as well and call method forcing an explosion on body.. which would prob call an AE anyways.. 


            return new AERetType();

        }

        private AERetType ExpColiBm(VagueObject callingObj, BodyMechanics bm, Explosion exp)
        {
            //Exp
            //Body being hit by Explosion
            //Since forces are handled by body already, this should send the status/special effects? maybe this should do force as well, and 
            //just call the funcs for bm here? hmm

            return new AERetType(); 

        }

        private AERetType GcoliExp(VagueObject callingObj, Explosion exp, Ground ground)
        {
            //Ground:
            //When the explosion hits the ground
            //Subtract dirt or do nothing

            return new AERetType();

        }

        private AERetType GcoliBm(VagueObject callingObj, BodyMechanics bm, Ground ground)
         {
             //Ground being hit by body, apply normal Forces back on bod,  or destroy and subtract from ground
             //this AE belongs to ground, but ground will not call it since it doesnt have coli check on update, 
             //the body that collides with ground should call this

             //Apply Forces or check if breaks
             return new AERetType();
         }


    }
}