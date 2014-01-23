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
        protected Vector2 curForce;
        protected Vector2 velo;
        private PhysSys.Physics phys = PhysSys.Physics.Instance;

        public BodyMechanics() { }
        public BodyMechanics(DNA EntDNA, DNA SprDNA, DNA BodDNA, DNA dna)
        {
            //set the instructor to be prepped as if being fed a string of values
            //some things only affected by full ints, so passing doubles can make a good scope/buffer level
            ForceCnstr(EntDNA,SprDNA,BodDNA,dna);
            
        }

        private void _DNACopier(DNA dna)
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
            _DNACopier(dna);
        }

        public void Update(float rt)
        {
            //things that apply force
            phys.applyNaturalLaws(this); //applys force
            _ApplyForceToVelo();

            //things that mod velo
            _CheckAllColi(); //check last cause ground coli can affect differntly.. dounoo still working on how to handle that           
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
            Structs.S_Box movingEnt = new Structs.S_Box(2,1,3,4);
            bool quit = false;


            Enums.ColiObjTypes.ColiTypes coliOccur = Enums.ColiObjTypes.ColiTypes.None;

                foreach (Structs.ColiListConnector connecter in Collidables)
                {
                    movingEnt = mov1.RetNextBox();
                    while (movingEnt != null && coliOccur == Enums.ColiObjTypes.ColiTypes.None)
                    {//since do while a 0,0 ret box or first null box can occur!
                        //possibly have some sort of connector, including the type your comparing..
                        //this just sorts through all of them, removes the checking by the need for check by cat then.. unless given type?
                        if (connecter.hashTable.Coli(movingEnt))
                        {
                            coliOccur = connecter.coliType;
                            switch (coliOccur)
                            {
                                case Enums.ColiObjTypes.ColiTypes.Magic:
                                    break;

                                case Enums.ColiObjTypes.ColiTypes.Dirt:
                                    _ColiWithGround();
                                    break;

                                default:
                                    Console.Out.WriteLine("You hit default case in checking connector type in colision!");
                                    break;


                            }
                        }

                        
                        Console.Out.WriteLine(movingEnt.GenString());
                        movingEnt = mov1.RetNextBox();
                        
                    }
                   

                }

        }


        private bool _ColiWithGround() //maybe can be overwritten by human or such for special reactions? or by body type makes more sense..ya
        {
            //spouse to destroy the ground, then alter the Y velocity by a certain amount, would like to use force, but unsure how atm
            Console.Out.WriteLine("COLI!");
            if(velo.Y >= 0)
                velo.Y = 0;
            return true; //shortcut trick
        }

    }
}