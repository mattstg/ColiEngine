using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structs;
using EntStructEnum;
using BodyParts;
using Microsoft.Xna.Framework;

namespace EntSys
{



    public class Body : Material
    {
        protected Global.Timers UniResponseT = new Global.Timers();//this way until dna
        
        //body vars
        protected int moveForce;
        protected int totalMass;
        ///public bool RegisterNewParts; //New parts have been added and need to be registered with the world
        //protected List<BodyPart> bodyParts; //main branching bodyparts from master
        //protected List<AEManager> MasterTransferList;


        public Body() { }
        public Body(DNA dna)
        {
            //set the instructor to be prepped as if being fed a string of values
            //some things only affected by full ints, so passing doubles can make a good scope/buffer level
            ForceCnstr(dna);
            
        }

        private void _DNADecoder(DNA dna)
        {
            //Need DNA copier
            moveForce = 60;
            
        }

        

        protected void ForceCnstr(DNA dna)
        {
           // MasterTransferList = new List<AEManager>();
            
            //bodyParts = new List<BodyPart>();
            base.ForceCnstr(dna);
            _DNADecoder(dna);
           // _UpdateBodyPartRelatedInfo();
        }

        public void Update(float rt)
        {
            
            base.Update(rt);
        }
       

    }
}
