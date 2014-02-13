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
    class BodyMechanics:Body
    {
        protected ActionEvent AE;
        Global.Bus bus = Global.Bus.Instance;
        private PhysSys.Physics phys = PhysSys.Physics.Instance;
        //private ColiSys.NodeManipulator nami = ColiSys.NodeManipulator.Instance;

        public BodyMechanics() { }
        public BodyMechanics(DNA EntDNA, DNA SprDNA, DNA BodDNA, DNA dna)
        {
            //set the instructor to be prepped as if being fed a string of values
            //some things only affected by full ints, so passing doubles can make a good scope/buffer level
            ForceCnstr(EntDNA,SprDNA,BodDNA,dna);
            
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

        protected void ForceCnstr(DNA EntDNA, DNA SprDNA, DNA BodDNA, DNA dna)
        {
            base.ForceCnstr(EntDNA,SprDNA,BodDNA);
            _DNADecoder(dna);
        }

        public void Update(float rt)
        {
            //things that apply force
            _UpdatePerSec(rt);
            _ApplyForceToVelo();

           // _CheckAllColi();
           // _ApplyForceToVelo();
           // _MoveAndCheckVelo(rt);
            newColiAndMoveFunc(rt);
            //things that mod velo
                  
            //_MoveUpdate();
            PlaceInBounds();
            //do all calcs with force
            curForce = new Vector2(0,0); //reset 0,0
            
            base.Update(rt);
        }

        private void _UpdatePerSec(float rt)
        {
            if (timerOncePerSec.ready)
            {

                phys.applyNaturalLaws(this, mass); //applys force

            }
        }

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
                if (lastColiCheck.loc.x < offset.x)
                {
                    //offset.x = lastColiCheck.loc.x;
                    movePartsByOffset.x = -1;

                }
                else if (size.x < lastColiCheck.size.x) //going to right
                {
                    //offset.x++;
                    movePartsByOffset.x = 1;
                }
                else if (lastColiCheck.loc.x != offset.x)
                    Console.Out.Write("unexpected value");

                if (lastColiCheck.loc.y < offset.y)
                {
                   // offset.y = lastColiCheck.loc.y; //up
                    movePartsByOffset.y = -1;

                }
                else if (size.y < lastColiCheck.size.y) //going down
                {
                    //offset.y++;
                    movePartsByOffset.y = 1;
                }
                else if (offset.y != lastColiCheck.loc.y)
                    Console.Out.Write("unexpected value");

               
            }
            return movePartsByOffset;
        }

        private bool[] _GetDirCol(Vector2 turnVelo,VagueObject ht)
        {
            bool[] toRet = new bool[]{false,false};
            if (turnVelo.X != 0)
            {
                ColiSys.DiagMotion d = new ColiSys.DiagMotion((int)(Math.Abs(turnVelo.X) / turnVelo.X), 0, this.coliBox);
                if (ht.Coli(d.RetNextBox()))
                {
                    toRet[0] = true;
                }
            }
            if (turnVelo.Y != 0)
            {
                ColiSys.DiagMotion d2 = new ColiSys.DiagMotion(0, (int)(Math.Abs(turnVelo.Y) / turnVelo.Y), this.coliBox);
                if (ht.Coli(d2.RetNextBox()))
                {
                    toRet[1] = true;
                }
               
            }
            return toRet;

        }

        private void _MoveAndCheckVelo(float rt)
        {
            bool[] ColiHV = new bool[] { false, false };
            bool coliOccured = false;
            float tr = rt;
            float timeStep;
            Vector2 tempRawOffset = new Vector2(rawOffSet.X, rawOffSet.Y);
            Enums.Navigation.Compass dirHeading;
            Vector2 scaledVelo = velo * (rt / 1000);
            tempRawOffset += scaledVelo;
            Vector2 turnVelo = new Vector2((int)(tempRawOffset.X - offset.x),(int)(tempRawOffset.Y-offset.y));
            timeStep = tr*(1/_RetHighest(turnVelo));
            if (turnVelo.X != 0 || turnVelo.Y != 0)
            {
                //so movement is occuring
                ColiSys.DiagMotion diagMot = new ColiSys.DiagMotion((int)turnVelo.X, (int)turnVelo.Y, this.coliBox);
                S_Box checkHere = diagMot.RetNextBox();

                while (tr > 0 && (turnVelo.X != 0 || turnVelo.Y != 0) && checkHere != null) 
                { 
                    coliOccured = false;
                    tr -= timeStep;

                    VagueObject connecter = new VagueObject(); //need to assign?
                    Collidables.ResetIT();                   
                    while (Collidables.GetNext(connecter))
                    {                        

                        if (connecter.Coli(checkHere)) //find type of coli that occured w/ x,y respects
                        {
                             //if coli occur, find out direction of the coli                            
                            ColiHV = _GetDirCol(turnVelo, connecter);  //i should be snaped to location, so i just need to check around me
                            coliOccured = true;     

                            switch (connecter.type) //switch->type object bodymech has colided with, and call their reactions based on that
                            {
                                case objType.Explosion:
                                    break;

                                case objType.Ground:
                                    AE.TriggerEvent(this, connecter.getObj<Ground>());
                                    break;


                                default:
                                    Console.Out.WriteLine("You hit default case in checking connector type in colision!");
                                    break;
                            }
                        }
                        //remember at end to snap to location


                    }
                    if (coliOccured)  //since a coli occured, need to reset tv,velo,timestep,"temprawoff",
                    {
                        //some forces have been applied, need to apply those forces to velo, returns velo the was added
                        
                        Vector2 addedVelo = _ApplyForceToVelo();
                        if (tr > 0)
                        {
                            //raw velo not updated cause its not spouse to move, only update rawoffset when move is possible
                            turnVelo += addedVelo * (1 / tr);
                            timeStep = (1 / _RetHighest(turnVelo));
                           // _SnapToColiSpot(diagMot);
                            diagMot = new ColiSys.DiagMotion((int)turnVelo.X, (int)turnVelo.Y, this.coliBox);                             
                            checkHere = diagMot.RetNextBox();
                        }
                        else //no time left, end the loop
                        {
                           // _SnapToColiSpot(diagMot);
                            checkHere = null;
                        }
                    }
                    else
                    {
                        checkHere = diagMot.RetNextBox();
                        rawOffSet += scaledVelo; //increase rawOffset
                        S_XY temps = new S_XY(diagMot.RetLast().loc.x, diagMot.RetLast().loc.y) ;
                        if (Math.Abs(offset.y - temps.y) != 1)
                             Console.Out.WriteLine("see, thats a lil odd");
                        offset = temps; //move offset foward
                       // _SnapToColiSpot(diagMot); //Sets offset then resets rawOffset

                    }


                }


            }
            else
            {
                rawOffSet += scaledVelo;

            }
            
        }


        


        public void newColiAndMoveFunc(float rt)
        {
            int expectedNumOfCycles;
            int NumOfCyclesCounter = 0;
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
                        while (Collidables.GetNext(connecter))
                        { //checking against each Collidable in bodyMechs list
                            bool thisCollided = connecter.Coli(checkHere);
                            List<BodyPart> collidedParts = new List<BodyPart>();
                            foreach ( BodyPart bp in bodyParts)
                                bp.CheckColi(checkHere.loc - offset,connecter,collidedParts);
                            if(thisCollided || collidedParts.Count != 0)
                            {
                                ColiHappend = true;
                                ApplyForce(ForceTypes.Internal, new Vector2(0, -(float)Math.Abs(velo.Y * mass * 2)));
                                //debug reaction, bounce up
                                //Multiple coli dectected on same turn can stack odly, example the -,
                                //In here, the action events will be called

                            } else {//has not collided with anything 
                                ColiHappend = false; //allready dont, just ensuring atm
                           
                            }
                        }
                    if (ColiHappend)
                    {
                        //cause a coli happened, we need to reapply the forces that happened above
                        

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
                            if (offset.y != dMot.RetLast().loc.y + 1 && dMot.RetLast().size.y != 15 && dMot.RetLast().size.x != 5)
                                Console.Out.WriteLine("breakpoint");
                            if (checkHere != null)
                            {
                                S_XY movePartsBy = _diffToColiSpot(checkHere);
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