using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structs;

namespace EntSys
{
    class BodyMechanics:Body
    {
        protected S_XY curForce = new S_XY(); //0,0

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

        public void ModForce(S_XY mag)
        {
            curForce += mag;

        }

        protected void ForceCnstr(DNA EntDNA, DNA SprDNA, DNA BodDNA, DNA dna)
        {
            base.ForceCnstr(EntDNA,SprDNA,BodDNA);
            _DNACopier(dna);
        }

        public void Update(float rt)
        {

            //do all calcs with force
            curForce = new S_XY(); //reset 0,0

        }


    }
}