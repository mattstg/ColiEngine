using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structs;
using Microsoft.Xna.Framework;
using Enums.Force;
using BodyParts;

namespace EntSys
{
    public class BodyMechanics : Body
    {
        
        Global.Bus bus = Global.Bus.Instance;
        private PhysSys.Physics phys = PhysSys.Physics.Instance;
        //private ColiSys.NodeManipulator nami = ColiSys.NodeManipulator.Instance;

        public BodyMechanics() { }
        public BodyMechanics(DNA dna)
        {
            //set the instructor to be prepped as if being fed a string of values
            //some things only affected by full ints, so passing doubles can make a good scope/buffer level
            ForceCnstr(dna);
            
        }

        private void _DNADecoder(DNA dna)
        {
            //Need DNA copier

        }

        public void ApplyForce(Enums.Force.ForceTypes forceType,Vector2 mag)
        {
            //force type can be reacted differently depending on body stats or(both) states
            curForce += mag;
          

        }

        protected void ForceCnstr(DNA dna)
        {
            base.ForceCnstr(dna);
            _DNADecoder(dna);
        }

        public void Update(float rt)
        {
            //things that apply force
           // _UpdatePerSec(rt);
           // _ApplyForceToVelo();
            phys.applyNaturalLaws(this, mass, rt);

           // _CheckAllColi();
           // _ApplyForceToVelo();
           // _MoveAndCheckVelo(rt);

            _ApplyForceToVelo();
            ColiAndMoveFunc(rt);
            //things that mod velo
                  
            //_MoveUpdate();
            PlaceInBounds();
            //do all calcs with force
            curForce = new Vector2(0,0); //reset 0,0
            
            base.Update(rt);
        }

       /* private void _UpdatePerSec(float rt)
        {
            
           // if (timerOncePerSec.ready)
           // {

            //    phys.applyNaturalLaws(this, mass); //applys force

           // }
        }*/

        private Vector2 _ApplyForceToVelo()
        {
            //eventaully connect with weight and such
            //F=MA UP IN HERE
            Vector2 toRet = curForce / mass;
            velo += toRet;
            curForce.X = 0; curForce.Y = 0; //all force used into velo
            return toRet;

        }

        private void _MoveUpdate()
        {
            
            //Then coli check to see if should move         
            rawOffSet += velo;
            offset = new S_XY((int)rawOffSet.X, (int)rawOffSet.Y);

            //nami check
        }

        
       

        private float _RetHighest(Vector2 a)
        {
            if (Math.Abs(a.X) > Math.Abs(a.Y))
                return a.X;
            else 
                return a.Y;
        }
        private int _RetHighest(S_XY a)
        {
            if (Math.Abs(a.x) > Math.Abs(a.y))
                return a.x;
            else
                return a.y;
        }

        private S_XY _diffToColiSpot(S_Box lastColiCheck)
        {
            S_XY movePartsByOffset = new S_XY();
            if (lastColiCheck != null)
            {
                
                if (lastColiCheck.loc.x != offset.x)
                {
                    //offset.x = lastColiCheck.loc.x;
                    movePartsByOffset.x = -1;

                }
                else if (size.x != lastColiCheck.size.x) //going to right
                {
                    //offset.x++;
                    movePartsByOffset.x = 1;
                }
                else if (lastColiCheck.loc.x != offset.x)
                    Console.Out.Write("unexpected value");



                if (lastColiCheck.loc.y != offset.y)
                {
                   // offset.y = lastColiCheck.loc.y; //up
                    movePartsByOffset.y = -1;

                }
                else if (size.y != lastColiCheck.size.y) //going down
                {
                    //offset.y++;
                    movePartsByOffset.y = 1;
                }
                else if (offset.y != lastColiCheck.loc.y)
                    Console.Out.Write("unexpected value");

               
            }
            return movePartsByOffset;
        }

        private bool[] _GetDirCol(Vector2 turnVelo,VagueObject ht,ColiSys.Node colibox)
        {      
            if (turnVelo.X == 0)
                return new bool[] { false, true };
            if (turnVelo.Y == 0)
                return new bool[] { true, false };

            bool[] toRet = new bool[]{false,false};
            
            ColiSys.DiagMotion d = new ColiSys.DiagMotion((int)(Math.Abs(turnVelo.X) / turnVelo.X), 0, colibox);
            if (ht.Coli(d.RetNextBox()))
                  toRet[0] = true;
                
            
            ColiSys.DiagMotion d2 = new ColiSys.DiagMotion(0, (int)(Math.Abs(turnVelo.Y) / turnVelo.Y), colibox);
            if (ht.Coli(d2.RetNextBox()))
                  toRet[1] = true;

            if (!toRet[0] && !toRet[1])
                return new bool[] { true, true };

            return toRet;

        }

        private bool[] _GetDirCol(S_XY turnVelo, VagueObject ht, ColiSys.Node colibox)
        {
            return _GetDirCol(new Vector2(turnVelo.x, turnVelo.y), ht, colibox);
        }
        

        public void ColiAndMoveFunc(float rt)
        {
            int expectedNumOfCycles;
            Vector2 tRawOffset = rawOffSet + velo * (rt / 1000);
            //Vector2 tRawOffset = rawOffSet + new Vector2(3,1);
            Vector2 tROdiff = tRawOffset - rawOffSet;
            S_XY veloThisCycle = new S_XY((int)(tRawOffset.X - offset.x), (int)(tRawOffset.Y - offset.y));
            expectedNumOfCycles = Math.Abs(_RetHighest(veloThisCycle));
            float trem = rt;
            float tTick = trem / expectedNumOfCycles; //by highest velo
            ColiSys.DiagMotion dMot = new ColiSys.DiagMotion(veloThisCycle.x, veloThisCycle.y, this.coliBox);
            

            if(veloThisCycle.x != 0 || veloThisCycle.y != 0)
                while (trem > 0)
                {
                    VagueObject connecter = new VagueObject(); //need to assign?
                    S_Box checkHere = dMot.RetNextBox(); //second call, last loc changed by two?
                    bool ColiHappend = false;

                    Collidables.ResetIT();
                    if(checkHere != null)
                        while (Collidables.GetNext(ref connecter))
                        { //checking against each Collidable in bodyMechs list
                            bool thisCollided = connecter.Coli(checkHere);
                            List<BodyPart> collidedParts = new List<BodyPart>();
                            foreach ( BodyPart bp in bodyParts)
                                bp.CheckColi(checkHere.loc - offset,connecter,collidedParts);
                            if(thisCollided || collidedParts.Count != 0)
                            {
                                //COLI HAS OCCURED// Since bodyparts and Ent were handled seperatly unfortunatly, do
                                //two sets of calculations
                                EI.PointsOfContact = collidedParts.Count;
                                if (thisCollided)
                                {
                                    EI.PointsOfContact++;
                                    EI.coliHV = _GetDirCol(veloThisCycle, connecter, this.coliBox);  //i should be snaped to location, so i just need to check around me
                                    EI.momentum = velo * (totalMass / EI.PointsOfContact);
                                    //Console.Out.WriteLine(velo);
                                    switch (connecter.specificType)
                                    {
                                        case objSpecificType.Bm:
                                            break;
                                        case objSpecificType.Body:
                                            break;
                                        case objSpecificType.Ent:
                                            break;
                                        case objSpecificType.Exp:
                                            AE.TriggerEvent(this, connecter.getObj<Explosion>());
                                            break;
                                        case objSpecificType.Material:
                                            AE.TriggerEvent(this, connecter.getObj<Material>());
                                            break;
                                        case objSpecificType.Human:
                                            break;
                                        case objSpecificType.Sprite:
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                foreach (BodyPart bp in collidedParts)
                                {
                                    bp.EI.totalMass = EI.totalMass;
                                    bp.EI.PointsOfContact = EI.PointsOfContact;
                                    bp.EI.momentum = velo * (totalMass / EI.PointsOfContact);
                                    bp.EI.coliHV = _GetDirCol(veloThisCycle, connecter, bp.coliBox);

                                    if (bp.EI.coliHV[0] || bp.EI.coliHV[1])
                                        switch (connecter.specificType)
                                        {
                                            case objSpecificType.Bm:
                                                break;
                                            case objSpecificType.Body:
                                                break;
                                            case objSpecificType.Ent:
                                                break;
                                            case objSpecificType.Exp:
                                                bp.AE.TriggerEvent(bp, connecter.getObj<Explosion>());
                                                break;
                                            case objSpecificType.Material:                                                
                                                bp.AE.TriggerEvent(bp, connecter.getObj<Material>());
                                                break;
                                            case objSpecificType.Human:
                                                //Collision with another human occured
                                                break;
                                            case objSpecificType.Sprite:
                                                break;
                                            default:
                                                break;
                                        }
                                    else //something went really weird, no coli detected
                                    {
                                        Console.Out.WriteLine("no coli detected second time round");
                                    }


                                }

                                //ColiHV = _GetDirCol(turnVelo, connecter);
                                ColiHappend = true;
                                //ApplyForce(ForceTypes.Internal, new Vector2(0, -(float)Math.Abs(velo.Y * mass * 2)));
                                //debug reaction, bounce up
                                //Multiple coli dectected on same turn can stack odly, example the -,
                                //In here, the action events will be called

                            }
                    }
                    if (ColiHappend)
                    {
                        //cause a coli happened, we need to reapply the forces that happened above
                        rawOffSet.X = offset.x;
                        rawOffSet.Y = offset.y;

                        _ApplyForceToVelo();
                        veloThisCycle.x = (int)((trem / 1000) * velo.X);
                        veloThisCycle.y = (int)((trem / 1000) * velo.Y);
                        dMot = new ColiSys.DiagMotion(veloThisCycle.x, veloThisCycle.y, this.coliBox);
                        expectedNumOfCycles = Math.Abs(_RetHighest(veloThisCycle));
                        if (expectedNumOfCycles != 0)
                            tTick = trem / expectedNumOfCycles; //by highest velo
                        else
                            trem = 0;


                    }
                    else //Succesful no Coli move, move the Ent and all peices foward
                    {
                        
                            trem -= tTick;

                            //debug test
                            //if (offset.y != dMot.RetLast().loc.y + 1 && dMot.RetLast().size.y != 15 && dMot.RetLast().size.x != 5)
                              //  Console.Out.WriteLine("breakpoint");
                            if (checkHere != null)
                            {
                                S_XY movePartsBy = _diffToColiSpot(checkHere);
                               // S_XY movePartsBy = Statics.Converter.getMag(velo
                                MoveAllBy(movePartsBy);
                                rawOffSet += velo* (tTick / 1000);
                                
                                //rawOffSet.X += movePartsBy.x;
                                //rawOffSet.Y += movePartsBy.y;
                            }
                    }
                }
              else //else there was not enough velo to go foward
              {
                  rawOffSet = tRawOffset;

              }




        }


        //////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////
        //////////////////////////ACTIONS CALLED BY PLAYER/AI SCOPE //////////////////////

        public void MoveInDir(dir moveDir)
        {
            switch(moveDir)
            {
                case dir.up:
                    ApplyForce(ForceTypes.Internal, new Vector2(0, -moveForce));
                    break;
                case dir.right:
                     ApplyForce(ForceTypes.Internal, new Vector2(moveForce, 0));
                    break;
                case dir.left:
                    ApplyForce(ForceTypes.Internal, new Vector2(-moveForce, 0));
                    break;
                case dir.down:
                     ApplyForce(ForceTypes.Internal, new Vector2(0, moveForce));
                    break;
                default:
                    Console.Out.WriteLine("non dir key pressed");
                    break;
                  
         }
        }
        public void ApplyForceOppOfHeading(ForceTypes ft,float force)
        {
            Enums.Navigation.Compass compass = GetHeading();
            


        }



        //////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////
    }
}