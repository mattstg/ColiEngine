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

           
            //NEXT STEP IS TO INCOPORATE OFFSET  PROPERLY!!
            //nami check
        }

        private void _CheckAllColi()
        {
            ColiSys.DiagMotion mov1 = new ColiSys.DiagMotion((int)velo.X, (int)velo.Y, RetSizeLocCopyBox());
            //ColiSys.DiagMotion mov1 = new ColiSys.DiagMotion(0, 1, RetSizeLocCopyBox());
            Structs.S_XY newOffset = new Structs.S_XY();
            Structs.S_Box movingEnt = new Structs.S_Box(2,1,3,4);
            bool quit = false;


            Enums.ColiObjTypes.ColiTypes coliOccur = Enums.ColiObjTypes.ColiTypes.None;

            movingEnt = mov1.RetNextBox();
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
                                    _ColiWithGround(Statics.Converter.OverlapToCompass(otype));
                                    break;


                                default:
                                    Console.Out.WriteLine("You hit default case in checking connector type in colision!");
                                    break;


                            }
                            //coliOccur = Enums.ColiObjTypes.ColiTypes.None; //a coli has occured, stop checking for more
                        }
                        else //No coli has occured, move the Ent
                        {
                            this.offset = movingEnt.loc;

                        }

                        
                    }
                movingEnt = mov1.RetNextBox();

                }

        }


        private bool _ColiWithGround(Enums.Navigation.Compass coliDir) //recieves x&y type colision
        {
            //spouse to destroy the ground, then alter the Y velocity by a certain amount, would like to use force, but unsure how atm
            //Remember coliDir is MY direction from the object im colliding with
            bool createReaction = false;// (Math.Abs(this.momentum.X) > Consts.Ground.DIRT_HP || Math.Abs(this.momentum.Y) > Consts.Ground.DIRT_HP);
            //has the ent broke ground ^
            switch (coliDir)
            {
                case Enums.Navigation.Compass.N:
                    if (this.momentum.Y > Consts.Ground.DIRT_HP)
                        createReaction = true;
                    this.ApplyForce(Enums.Force.ForceTypes.Dirt, new Vector2(0, -Consts.Ground.DirtColiForce(this.momentum.Y)));
                    break;
                case Enums.Navigation.Compass.S:
                    if (-1 * this.momentum.Y > Consts.Ground.DIRT_HP)
                        createReaction = true;
                    this.ApplyForce(Enums.Force.ForceTypes.Dirt,new Vector2(0,Consts.Ground.DirtColiForce(this.momentum.Y)));
                    break;
                   
                case Enums.Navigation.Compass.W:
                    if (this.momentum.X > Consts.Ground.DIRT_HP)
                        createReaction = true;
                    this.ApplyForce(Enums.Force.ForceTypes.Dirt,new Vector2(0,Consts.Ground.DirtColiForce(this.momentum.X)));
                    break;
                case Enums.Navigation.Compass.E:
                    if (-1 * this.momentum.X > Consts.Ground.DIRT_HP)
                        createReaction = true;
                    this.ApplyForce(Enums.Force.ForceTypes.Dirt,new Vector2(0,-Consts.Ground.DirtColiForce(this.momentum.X)));
                    break;
                case Enums.Navigation.Compass.Center: //intresting case when has traversed into a pixel
                default:
                   //this case its kinda weird, so lets try "Stabalize" trying to make velo go closer to 0 regardless
                    float f = Math.Abs(velo.X)/velo.X;
                    float f2 = -1*f;
                    this.ApplyForce(Enums.Force.ForceTypes.Dirt,new Vector2(f2*Consts.Ground.DirtColiForce(this.momentum.X),f2*Consts.Ground.DirtColiForce(this.momentum.Y)));
                    break;
                    

            }


            if (createReaction)
            {
                ColiSys.Node nodex = new ColiSys.Node(this.trueEntShape);
                ColiSys.Node nodey = new ColiSys.Node(this.trueEntShape.Dwn());
                nodex += new S_XY(-5, 5);
                nodey += new S_XY(-5, 5);

                nodex.Dwn(nodey);
                ColiSys.Hashtable testMe = new ColiSys.Hashtable(nodex);
                bus.LoadPassenger(testMe, Enums.Global.VoidableTypes.Explosion,this.loc);
                Console.Out.WriteLine("EXPLOSION!");
            }
                

            
           // if(velo.Y >= 0)
          //      velo.Y = 0;
            return true; //shortcut trick
        }

    }
}