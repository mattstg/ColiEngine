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

        public void RegAbilityPack(int packNum, List<Func<VagueObject, BodyMechanics, Material, AERetType>> BodyMechVsMaterial, List<Func<VagueObject, Explosion, Material, AERetType>> ExpVsMaterial
                                    , List<Func<VagueObject, BodyMechanics, Explosion, AERetType>> BodyMechVsExp, List<Func<VagueObject, KeyboardState, AERetType>> KeyActions,
                                      List<Func<VagueObject, BodyPart, Material, AERetType>> BodyPartVsMaterial, List<Func<VagueObject, BodyPart, Explosion, AERetType>> BodyPartVsExp)
        {
            switch(packNum)
            {
                case 4: //Human
                   // BmGActions.Add(BmGCreateExp);  create debug exp when hit Material
                    BodyMechVsMaterial.Add(callG);
                    BodyMechVsMaterial.Add(BodyHitsGround);
                    BodyMechVsExp.Add(BmColiExp);
                    KeyActions.Add(HumanKeyMove);
                   // BmGActions.Add(BodyHitsGround);
                    return;
                    
                case 5: //Exp
                    ExpVsMaterial.Add(ExpGAlterPath);
                    ExpVsMaterial.Add(callG);
                    BodyMechVsExp.Add(ExpColiBm);
                    return;
                
                case 6: //Material
                    ExpVsMaterial.Add(GcoliExp);
                    BodyMechVsMaterial.Add(GcoliBm);
                    return;

                case 7: //Body Part
                    
                    return;

                case 10: //Wings!
                    KeyActions.Add(HumanWingsInput);
                    BodyPartVsMaterial.Add(BodypartHitsGround);
                    return;


                default:
                    break;


            }

        }








        /////For now, all Events will be created here, they should be created elsewhere i guess... this way for custom skill generation
        private AERetType BmGCreateExp(VagueObject callingObj, BodyMechanics bm, Material Material)
        {          
                //used in body hitting Material, create explosion, apply on Material effects, or do nothing
                //Not used to exert normal forces on body, Material should do that, but most likely, this function, or another Material function, should call
                //grounds func since Material doesnt do coli testing
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

        private AERetType BodyHitsGround(VagueObject callingObj, BodyMechanics Bm, Material Material)
        {
            Vector2 dir = Statics.Converter.getMag(Bm.EI.momentum);
            
            //we now have direction/magnitude vector for movement
            //cases, direct collision, movement not covered by coli
            if (Bm.EI.coliHV[0])
            {
                //bounce in x direction & break Material
                Bm.ApplyForce(Enums.Force.ForceTypes.Dirt, new Vector2(Material.GetBounceForce(Bm.EI.momentum.X, nami.MoveTableByOffset(callingObj.coliBox,new S_XY((int)dir.X,0))), 0));
                
                
                //collision happened directly
            }
            else if(!Bm.EI.coliHV[0] && dir.X != 0) //indirect coli, apply friction
            {
                


            }


            if (Bm.EI.coliHV[1])
            {
                //bounce in x direction & break Material
                //ColiSys.Node node = callingObj.coliBox;
               // ColiSys.Node node = nami.MoveTableByOffset(callingObj.coliBox, new S_XY(0, (int)dir.Y));
               // Bm.ApplyForce(Enums.Force.ForceTypes.Dirt, new Vector2(0, Material.GetBounceForce(Bm.momentum.Y, node)));
                Bm.ApplyForce(Enums.Force.ForceTypes.Dirt, new Vector2(0, Material.GetBounceForce(Bm.EI.momentum.Y, nami.MoveTableByOffset(callingObj.coliBox, new S_XY(0, (int)dir.Y)))));
                //some reason, one of the pointers gets lost in the resulting Nami. transform when called inside the func, but not otherwise^


                //collision happened directly
            }
            else if (!Bm.EI.coliHV[1] && dir.Y != 0) //indirect coli, apply friction
            {



            }


            return new AERetType();
        }

        private AERetType BodypartHitsGround(VagueObject callingObj, BodyPart bp, Material Material)
        {
            Vector2 dir = Statics.Converter.getMag(bp.EI.momentum);
            

            //we now have direction/magnitude vector for movement
            //cases, direct collision, movement not covered by coli
            if (bp.EI.coliHV[0])
            {
                //bounce in x direction & break Material
                bp.Master.ApplyForce(Enums.Force.ForceTypes.Dirt, new Vector2(Material.GetBounceForce(bp.EI.momentum.X, nami.MoveTableByOffset(callingObj.coliBox, new S_XY((int)dir.X, 0))), 0));


                //collision happened directly
            }
            else if (!bp.EI.coliHV[0] && dir.X != 0) //indirect coli, apply friction
            {



            }


            if (bp.EI.coliHV[1])
            {
                //bounce in x direction & break Material
                //ColiSys.Node node = callingObj.coliBox;
                // ColiSys.Node node = nami.MoveTableByOffset(callingObj.coliBox, new S_XY(0, (int)dir.Y));
                // Bm.ApplyForce(Enums.Force.ForceTypes.Dirt, new Vector2(0, Material.GetBounceForce(Bm.momentum.Y, node)));
                bp.Master.ApplyForce(Enums.Force.ForceTypes.Dirt, new Vector2(0, Material.GetBounceForce(bp.EI.momentum.Y, nami.MoveTableByOffset(callingObj.coliBox, new S_XY(0, (int)dir.Y)))));
                //some reason, one of the pointers gets lost in the resulting Nami. transform when called inside the func, but not otherwise^


                //collision happened directly
            }
            else if (!bp.EI.coliHV[1] && dir.Y != 0) //indirect coli, apply friction
            {



            }

            return new AERetType();
        }

        private AERetType ExpGAlterPath(VagueObject callingObj, Explosion exp, Material Material)
        {
            //Exp
            //Exp hitting the Material
            //explosion hits Material, possibility of altering explosion path?
          //  Console.Out.Write("EXPLOSION HIT Material!");
            //nah should just be absorded, but bouncing would be cool if it doesnt break it, like
            //how Material bounces
            return new AERetType();

        }

        private AERetType callG(VagueObject callingObj, BodyMechanics bm, Material Material)
        {
            //Bm            
            //When body hits Material, body needs to call grounds AE
            Material.AE.TriggerEvent(bm,Material);


            return new AERetType();

        }

        private AERetType callG(VagueObject callingObj, Explosion exp, Material Material)
        {
            //Exp
            //When exp hits Material, exp needs to call grounds AE
            Material.AE.TriggerEvent(exp,Material);


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

        private AERetType GcoliExp(VagueObject callingObj, Explosion exp, Material Material)
        {
            //Material:
            //When the explosion hits the Material
            //Subtract dirt or do nothing

            return new AERetType();

        }

        private AERetType GcoliBm(VagueObject callingObj, BodyMechanics bm, Material Material)
         {
             //Material being hit by body, apply normal Forces back on bod,  or destroy and subtract from Material
             //this AE belongs to Material, but Material will not call it since it doesnt have coli check on update, 
             //the body that collides with Material should call this

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
