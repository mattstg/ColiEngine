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
            _CheckAllColi();
            _ApplyForceToVelo();
            //things that mod velo
                  
            _MoveUpdate();


            PlaceInBounds();
            //do all calcs with force
            curForce = new Vector2(0,0); //reset 0,0
            
            base.Update(rt);
        }

        private void _ApplyForceToVelo()
        {
            //eventaully connect with weight and such
            //F=MA UP IN HERE
            velo += (curForce / mass);

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
            movingEnt = nami.NodetoBox(nami.StretchSquareTableByXY(this.coliBox, new S_XY(0, 1)));
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

                        
              //  movingEnt = mov1.RetNextBox();
                movingEnt = null;

        }

        }


        private bool _ColiWithGround(Enums.Navigation.Compass coliDir,S_Box coliSquare) //recieves x&y type colision
        {
            //So important, colisquare creates an area where coli occured, check for difference between you pos and its and you will find direction
            int[] dir = new int[2];
            if(coliSquare.loc.x > offset.x || coliSquare.size.x > this.size.x)
                dir[0] = 2;
                //was moving right
            else if (coliSquare.loc.x < offset.x)
                dir[0] = 0;
                //moving left
            else
                dir[0] = 1;
                //not moving vertical


            if (coliSquare.loc.y > offset.y || coliSquare.size.y > this.size.y)
                dir[1] = 2;
                //was moving down
            else if(coliSquare.loc.y < offset.y)
                dir[1] = 0;
                //moving up
            else
                dir[1] = 1;
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
                ColiSys.Node node = this.trueEntShapeOffset; //already copies
                node = nami.MoveTableByOffset(node, this.offset); //explosion takes shape of player and moves to player loc
                node = nami.StretchSquareTableByXY(node, new S_XY(-2, 2)); //increase size of explosion by   
                node = nami.MoveTableByOffset(node, this.offset*-1); //explosion takes shape of player and moves to player loc
                ColiSys.Hashtable testMe = new ColiSys.Hashtable(node);
                bus.LoadPassenger(testMe, Enums.Global.VoidableTypes.Explosion,this.offset);
                Console.Out.WriteLine("EXPLOSION!");
            }
                

            
           // if(velo.Y >= 0)
          //      velo.Y = 0;
            return true; //shortcut trick
        }

    }
}