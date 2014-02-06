using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structs;
using Microsoft.Xna.Framework;


namespace EntSys
{
    class BodyMechanics:Body
    {
        ActionEvent AE = new ActionEvent(objType.Body);
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
            _MoveAndCheckVelo(rt);
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
      
        
        private Enums.Navigation.Compass getTravelingDir(Vector2 velo)
        {
            //So important, colisquare creates an area where coli occured, check for difference between you pos and its and you will find direction
            int[] dir = new int[2];
            if (velo.X > 0 )
                dir[1] = 2;
            //was moving right
            else if (velo.X < 0)
                dir[1] = 0;
            //moving left
            else
                dir[1] = 1;
            //not moving vertical


            if (velo.Y > 0)
                dir[0] = 2;
            //was moving down
            else if (velo.Y < 0)
                dir[0] = 0;
            //moving up
            else
                dir[0] = 1;
            //not moving Horz
            Structs.Navigation.Compass comp = new Structs.Navigation.Compass();
            return comp.SetCompass(dir);

        }

        private float _RetHighest(Vector2 a)
        {
            if (a.X > a.Y)
                return a.X;
            else 
                return a.Y;
        }

        private void _SnapToColiSpot(ColiSys.DiagMotion diag)
        {
            S_Box tloc = diag.RetLast();
            offset = tloc.loc;
            //rawOffSet = new Vector2(rawOffSet.X - (int)rawOffSet.X + offset.x, rawOffSet.Y - (int)rawOffSet.Y + offset.y);
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


                    foreach (VagueObject connecter in Collidables)
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
                                    Console.Out.WriteLine("COLLISION WITH GROUND!");
                                    //_ColiWithGround(Statics.Converter.OverlapToCompass(otype), movingEnt);
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
                            turnVelo += addedVelo * (1 / tr);
                            timeStep = (1 / _RetHighest(turnVelo));
                            _SnapToColiSpot(diagMot);
                            diagMot = new ColiSys.DiagMotion((int)turnVelo.X, (int)turnVelo.Y, this.coliBox);                             
                            checkHere = diagMot.RetNextBox();
                        }
                        else
                        {
                            _SnapToColiSpot(diagMot);
                            checkHere = null;
                        }
                    }
                    else
                    {
                        checkHere = diagMot.RetNextBox();
                        rawOffSet += scaledVelo; //increase rawOffset
                        offset = new S_XY(diagMot.RetLast().loc.x, diagMot.RetLast().loc.y) ; //move offset foward
                       // _SnapToColiSpot(diagMot); //Sets offset then resets rawOffset

                    }


                }


            }
            else
            {
                rawOffSet += scaledVelo;

            }
            
        }

    }
}