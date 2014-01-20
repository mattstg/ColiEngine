using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structs;

namespace EntSys
{
    class Sprite : Entity
    {
        public Sprite() { }

        public Sprite(DNA EntDNA,DNA dna)
        {
            ForceCnstr(EntDNA);

        }

        private void _DNACopier(DNA dna)
        {
            //Need DNA copier

        }

        protected void ForceCnstr(DNA EntDNA, DNA dna)
        {
            //load vars
            base.ForceCnstr(EntDNA);
            _DNACopier(dna);
        }





    }
}
