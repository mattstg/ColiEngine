using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structs;

namespace EntSys
{
    class Body : Sprite
    {
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

        }

        protected void ForceCnstr(DNA EntDNA, DNA SprDNA, DNA dna)
        {


            base.ForceCnstr(EntDNA,SprDNA);
            _DNACopier(dna);
        }



    }
}
