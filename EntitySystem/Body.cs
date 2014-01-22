using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structs;
using EntStructEnum;

namespace EntSys
{
    class Body : Sprite
    {
        protected Timers UniResponseT = new Timers();//this way until dna
        
        //body vars
        protected int moveForce;
        protected int mass;


        public Body() { }
        public Body(DNA EntDNA,DNA SprDNA, DNA dna)
        {
            //set the instructor to be prepped as if being fed a string of values
            //some things only affected by full ints, so passing doubles can make a good scope/buffer level
            ForceCnstr(EntDNA,SprDNA,dna);
            
        }

        private void _DNACopier(DNA dna)
        {
            //Need DNA copier
            moveForce = 5;
            mass = 1;
        }

        protected void ForceCnstr(DNA EntDNA, DNA SprDNA, DNA dna)
        {


            base.ForceCnstr(EntDNA,SprDNA);
            _DNACopier(dna);
        }

        public void Update(float rt)
        {

            base.Update(rt);
        }



    }
}
