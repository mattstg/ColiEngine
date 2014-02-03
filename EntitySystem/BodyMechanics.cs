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
        
        Global.Bus bus = Global.Bus.Instance;
        private PhysSys.Physics phys = PhysSys.Physics.Instance;
        private ColiSys.NodeManipulator nami = ColiSys.NodeManipulator.Instance;

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
            phys.applyNaturalLaws(this, mass); //applys force
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

        private void _CheckAllColi()
        {
            //ColiSys.DiagMotion mov1 = new ColiSys.DiagMotion((int)velo.X, (int)velo.Y, this.coliBox);
           // ColiSys.DiagMotion mov1 = new ColiSys.DiagMotion(0, 1, this.coliBox);
            Structs.S_XY newOffset = new Structs.S_XY();
            Structs.S_Box movingEnt = new Structs.S_Box();//2,1,3,4);
            bool quit = false;


            Enums.ColiObjTypes.ColiTypes coliOccur = Enums.ColiObjTypes.ColiTypes.None;

           // movingEnt = mov1.RetNextBox();
            movingEnt = nami.NodetoBox(nami.MoveTableByOffset(this.coliBox, new S_XY(0, 1)));
            S_Box lastValidBox = null;
            while (movingEnt != null && coliOccur == Enums.ColiObjTypes.ColiTypes.None)
            {//since do while a 0,0 ret box or first null box can occur!
                //possibly have some sort of connector, including the type your comparing..
                //this just sorts through all of them, removes the checking by the need for check by cat then.. unless given type?

                
                foreach (Structs.ColiListConnector connecter in Collidables)
                {
                   
                   

                        Enums.Node.OverlapType[] otype = connecter.hashTable.ColiType(movingEnt); //find type of coli that occured w/ x,y respects
                        if (otype[0] != Enums.Node.OverlapType.Before)  //so a coli has occured
                        {
                            coliOccur = connecter.coliType;
                            switch (coliOccur) //switch->type object bodymech has colided with, and call their reactions based on that
                            {
                                case Enums.ColiObjTypes.ColiTypes.Explosion:
                                    break;

                                case Enums.ColiObjTypes.ColiTypes.Dirt:
                                    _ColiWithGround(Statics.Converter.OverlapToCompass(otype),movingEnt);
                                    break;


                                default:
                                    Console.Out.WriteLine("You hit default case in checking connector type in colision!");
                                    break;


                            }
                            //coliOccur = Enums.ColiObjTypes.ColiTypes.None; //a coli has occured, stop checking for more
                        }
                        

                        
                }//foreach
                //if(coliOccur == Enums.ColiObjTypes.ColiTypes.None) //if no coli happen in all tables, move 
                 //  this.offset = movingEnt.loc;  
                //Cant move foward, the update function takes care of that! but we can return location of colision!

                        
              
                //lastValidBox = movingEnt;
                //  movingEnt = mov1.RetNextBox();
                movingEnt = null;

        }

        }


        private bool _ColiWithGround(Enums.Navigation.Compass coliDir,S_Box coliSquare) //recieves x&y type colision
        {
            //So important, colisquare creates an area where coli occured, check for difference between you pos and its and you will find direction
            int[] dir = new int[2];
            if(coliSquare.loc.x > offset.x || coliSquare.size.x > this.size.x)
                dir[1] = 2;
                //was moving right
            else if (coliSquare.loc.x < offset.x)
                dir[1] = 0;
                //moving left
            else
                dir[1] = 1;
                //not moving vertical


            if (coliSquare.loc.y > offset.y || coliSquare.size.y > this.size.y)
                dir[0] = 2;
                //was moving down
            else if(coliSquare.loc.y < offset.y)
                dir[0] = 0;
                //moving up
            else
                dir[0] = 1;
                //not moving Horz

            Structs.Navigation.Compass compass = new Structs.Navigation.Compass();
            Enums.Navigation.Compass _coliDir = compass.SetCompass(dir); //Coli dir as seen from thing colided with. Ent being above dirt for ex is North


            //Direction it is from this ent
            bool createReaction = false;// (Math.Abs(this.momentum.X) > Consts.Ground.DIRT_HP || Math.Abs(this.momentum.Y) > Consts.Ground.DIRT_HP);
            //has the ent broke ground ^
            if(Enums.Navigation.Compass.S == _coliDir || Enums.Navigation.Compass.SE == _coliDir || Enums.Navigation.Compass.SW == _coliDir)
            {
                    if (this.momentum.Y > Consts.Ground.DIRT_HP)
                        createReaction = true;
                    this.ApplyForce(Enums.Force.ForceTypes.Dirt, new Vector2(0, -Consts.Ground.DirtColiForce(this.momentum.Y)));
            }
                    
            if(Enums.Navigation.Compass.N == _coliDir || Enums.Navigation.Compass.NE == _coliDir || Enums.Navigation.Compass.NW == _coliDir)
            {
                    if (-1 * this.momentum.Y > Consts.Ground.DIRT_HP)
                        createReaction = true;
                    this.ApplyForce(Enums.Force.ForceTypes.Dirt,new Vector2(0,Consts.Ground.DirtColiForce(this.momentum.Y)));                    
            }

            if (Enums.Navigation.Compass.E == _coliDir || Enums.Navigation.Compass.NE == _coliDir || Enums.Navigation.Compass.SE == _coliDir)
            {
                    if (this.momentum.X > Consts.Ground.DIRT_HP)
                        createReaction = true;
                    this.ApplyForce(Enums.Force.ForceTypes.Dirt,new Vector2(-Consts.Ground.DirtColiForce(this.momentum.X),0));
                    
            }

            if(Enums.Navigation.Compass.W == _coliDir || Enums.Navigation.Compass.NW == _coliDir || Enums.Navigation.Compass.SW == _coliDir)
            {
                    if (-1 * this.momentum.X > Consts.Ground.DIRT_HP)
                        createReaction = true;
                    this.ApplyForce(Enums.Force.ForceTypes.Dirt,new Vector2(Consts.Ground.DirtColiForce(this.momentum.X),0));
                    
            }

            if(Enums.Navigation.Compass.Center == _coliDir)
            {//intresting case when has traversed into a pixel
                
                   //this case its kinda weird, so lets try "Stabalize" trying to make velo go closer to 0 regardless
                    float f = Math.Abs(velo.X)/velo.X;
                    float f2 = -1*f;
                    this.ApplyForce(Enums.Force.ForceTypes.Dirt,new Vector2(f2*Consts.Ground.DirtColiForce(this.momentum.X),f2*Consts.Ground.DirtColiForce(this.momentum.Y)));
                    
            }
                    

            


            if (createReaction)
            {
                ColiSys.Node node = this.trueEntShape; //already copies
                
                node = nami.MoveTableByOffset(node, new S_XY(0, 5)); //increase size of explosion by   
              
                ColiSys.Hashtable testMe = new ColiSys.Hashtable(nami.MoveTableByOffset(node,(this.offset)));
                bus.LoadPassenger(testMe, Enums.Global.VoidableTypes.Explosion,this.offset);
                Console.Out.WriteLine("EXPLOSION!");
            }
                

            
           // if(velo.Y >= 0)
          //      velo.Y = 0;
            return true; //shortcut trick
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
            rawOffSet = new Vector2(rawOffSet.X - (int)rawOffSet.X + offset.x, rawOffSet.Y - (int)rawOffSet.Y + offset.y);
        }

        private bool[] _GetDirCol(Vector2 turnVelo,ColiSys.Hashtable ht)
        {
            bool[] toRet = new bool[]{false,false};
            ColiSys.DiagMotion d = new ColiSys.DiagMotion((int)(Math.Abs(turnVelo.X) / turnVelo.X),0, this.coliBox);
            if(ht.Coli(d.RetNextBox()))
            {
                toRet[0] = true;
            }
            ColiSys.DiagMotion d2 = new ColiSys.DiagMotion(0,(int)(Math.Abs(turnVelo.Y) / turnVelo.Y), this.coliBox);
            if (ht.Coli(d.RetNextBox()))
            {
                toRet[1] = true;
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
            tempRawOffset += velo * (rt/1000) ;
            Vector2 turnVelo = new Vector2((int)(tempRawOffset.X - offset.x),(int)(tempRawOffset.Y-offset.y));
            timeStep = tr*(1/_RetHighest(turnVelo));
            if (turnVelo.X != 0 || turnVelo.Y != 0)
            {
                //so movement is occuring
                ColiSys.DiagMotion diagMot = new ColiSys.DiagMotion((int)turnVelo.X, (int)turnVelo.Y,this.coliBox);
                S_Box checkHere = diagMot.RetNextBox();

                while (tr > 0 && (turnVelo.X >= 1 || turnVelo.Y >= 1) && checkHere != null)
                {
                    coliOccured = false;
                    _SnapToColiSpot(diagMot);
                    tr -= timeStep;


                    foreach (Structs.ColiListConnector connecter in Collidables)
                    {


                        if (connecter.hashTable.Coli(checkHere)) //find type of coli that occured w/ x,y respects
                        {
                            //if coli occur, find out direction of the coli
                            // dirHeading = getTravelingDir(turnVelo);
                           // _SnapToColiSpot(diagMot);
                            ColiHV = _GetDirCol(turnVelo, connecter.hashTable);  //i should be snaped to location, so i just need to check around me
                            coliOccured = true;



                            switch (connecter.coliType) //switch->type object bodymech has colided with, and call their reactions based on that
                            {
                                case Enums.ColiObjTypes.ColiTypes.Explosion:
                                    break;

                                case Enums.ColiObjTypes.ColiTypes.Dirt:
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
                        turnVelo += addedVelo * (1 / tr);
                        timeStep = (1 / _RetHighest(turnVelo));
                        diagMot = new ColiSys.DiagMotion((int)turnVelo.X, (int)turnVelo.Y, this.coliBox); //should have snapped to location from above

                    }

                    checkHere = diagMot.RetNextBox();
                }


            }
            
        }

    }
}