using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structs;

namespace EntSys
{
    class HumanPlayer : BodyMechanics
    {
        public HumanPlayer() { }
        public HumanPlayer(DNA EntDNA, DNA SprDNA, DNA BodDNA, DNA MekDNA, DNA dna)
        {
            //set the instructor to be prepped as if being fed a string of values
            //some things only affected by full ints, so passing doubles can make a good scope/buffer level
            ForceCnstr(EntDNA,SprDNA,BodDNA,MekDNA,dna);
            
        }

        private void _DNACopier(DNA dna)
        {
            //Need DNA copier

        }

        protected void ForceCnstr(DNA EntDNA, DNA SprDNA, DNA BodDNA, DNA MekDNA, DNA dna)
        {


            base.ForceCnstr(EntDNA,SprDNA,BodDNA,MekDNA);
            _DNACopier(dna);
        }



    }
}
