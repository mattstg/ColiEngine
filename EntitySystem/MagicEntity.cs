using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntSys
{
    class MagicEntity : BodyMechanics
    {

        public MagicEntity()
        {
            ForceCnstr(null);
        }

        public MagicEntity(DNA dna)
        {

            ForceCnstr(dna);
        }

        public void ForceCnstr(DNA dna)
        {


        }

        public void Update(float rt)
        {}

        



    }
}
