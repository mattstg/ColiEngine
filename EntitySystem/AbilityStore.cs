using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BodyParts;
using Structs;
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
        ColiSys.NodeManipulator nami = ColiSys.NodeManipulator.Instance;

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
                                    , List<Func<VagueObject, BodyMechanics, Explosion, AERetType>> BmExpActions, List<Func<VagueObject, KeyboardState, AERetType>> KeyActions,
                                      List<Func<VagueObject, BodyPart, Ground, AERetType>> BodypartGroundActions, List<Func<VagueObject, BodyPart, Explosion, AERetType>> BodypartExpActions)
        {
            switch(packNum)
            {
                case 4: //Human
                   // BmGActions.Add(BmGCreateExp);  create debug exp when hit ground
                    BmGActions.Add(callG);
                    BmGActions.Add(BodyHitsGround);
                    BmExpActions.Add(BmColiExp);
                    KeyActions.Add(HumanKeyMove);
                   // BmGActions.Add(BodyHitsGround);
                    return;
                    
                case 5: //Exp
                    ExpGActions.Add(ExpGAlterPath);
                    ExpGActions.Add(callG);
                    BmExpActions.Add(ExpColiBm);
                    return;
                
                case 6: //Ground
                    ExpGActions.Add(GcoliExp);
                    BmGActions.Add(GcoliBm);
                    return;

                case 7: //Body Part
                    
                    return;

                case 10: //Wings!
                    KeyActions.Add(HumanWingsInput);
                    BodypartGroundActions.Add(BodypartHitsGround);
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

            return new AERetType();
        }

        private AERetType BodyHitsGround(VagueObject callingObj, BodyMechanics Bm, Ground ground)
        {
            Vector2 dir = Statics.Converter.getMag(Bm.EI.momentum);
            
            //we now have direction/magnitude vector for movement
            //cases, direct collision, movement not covered by coli
            if (Bm.EI.coliHV[0])
            {
                //bounce in x direction & break ground
                Bm.ApplyForce(Enums.Force.ForceTypes.Dirt, new Vector2(ground.GetBounceForce(Bm.EI.momentum.X, nami.MoveTableByOffset(callingObj.coliBox,new S_XY((int)dir.X,0))), 0));
                
                
                //collision happened directly
            }
            else if(!Bm.EI.coliHV[0] && dir.X != 0) //indirect coli, apply friction
            {
                


            }


            if (Bm.EI.coliHV[1])
            {
                //bounce in x direction & break ground
                //ColiSys.Node node = callingObj.coliBox;
               // ColiSys.Node node = nami.MoveTableByOffset(callingObj.coliBox, new S_XY(0, (int)dir.Y));
               // Bm.ApplyForce(Enums.Force.ForceTypes.Dirt, new Vector2(0, ground.GetBounceForce(Bm.momentum.Y, node)));
                Bm.ApplyForce(Enums.Force.ForceTypes.Dirt, new Vector2(0, ground.GetBounceForce(Bm.EI.momentum.Y, nami.MoveTableByOffset(callingObj.coliBox, new S_XY(0, (int)dir.Y)))));
                //some reason, one of the pointers gets lost in the resulting Nami. transform when called inside the func, but not otherwise^


                //collision happened directly
            }
            else if (!Bm.EI.coliHV[1] && dir.Y != 0) //indirect coli, apply friction
            {



            }


            return new AERetType();
        }

        private AERetType BodypartHitsGround(VagueObject callingObj, BodyPart bp, Ground ground)
        {
            Vector2 dir = Statics.Converter.getMag(bp.EI.momentum);
            

            //we now have direction/magnitude vector for movement
            //cases, direct collision, movement not covered by coli
            if (bp.EI.coliHV[0])
            {
                //bounce in x direction & break ground
                bp.Master.ApplyForce(Enums.Force.ForceTypes.Dirt, new Vector2(ground.GetBounceForce(bp.EI.momentum.X, nami.MoveTableByOffset(callingObj.coliBox, new S_XY((int)dir.X, 0))), 0));


                //collision happened directly
            }
            else if (!bp.EI.coliHV[0] && dir.X != 0) //indirect coli, apply friction
            {



            }


            if (bp.EI.coliHV[1])
            {
                //bounce in x direction & break ground
                //ColiSys.Node node = callingObj.coliBox;
                // ColiSys.Node node = nami.MoveTableByOffset(callingObj.coliBox, new S_XY(0, (int)dir.Y));
                // Bm.ApplyForce(Enums.Force.ForceTypes.Dirt, new Vector2(0, ground.GetBounceForce(Bm.momentum.Y, node)));
                bp.Master.ApplyForce(Enums.Force.ForceTypes.Dirt, new Vector2(0, ground.GetBounceForce(bp.EI.momentum.Y, nami.MoveTableByOffset(callingObj.coliBox, new S_XY(0, (int)dir.Y)))));
                //some reason, one of the pointers gets lost in the resulting Nami. transform when called inside the func, but not otherwise^


                //collision happened directly
            }
            else if (!bp.EI.coliHV[1] && dir.Y != 0) //indirect coli, apply friction
            {



            }

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



        private AERetType HumanWingsInput(VagueObject callingObj, KeyboardState ks)
        {
            //The calling object here must be of type BodyPart
            //keyboard input events do not have that fault security
            BodyPart bp = callingObj.getObj<BodyPart>();
            BodyMechanics bm = bp.Master;    
            
            if (ks.IsKeyDown(Keys.Up))
                    bm.ApplyForce(Enums.Force.ForceTypes.Internal,new Vector2(0,-60));

                return new AERetType(); 
        }
        


    }
}
