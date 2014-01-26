using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using EntStructEnum;
using Structs;

namespace EntSys
{
    class Rock : BodyMechanics
    {
        ColiSys.NodeManipulator nami = ColiSys.NodeManipulator.Instance;
        
        
        public Rock() { }
        public Rock(DNA EntDNA, DNA SprDNA, DNA BodDNA, DNA MekDNA, DNA dna)
        {
            ForceCnstr(EntDNA, SprDNA, BodDNA, MekDNA, dna);
        }

        private void _DNADecoder(DNA dna)
        {            
            UniResponseT = new Global.Timers(0, 250, 251);
        }



        protected void ForceCnstr(DNA EntDNA, DNA SprDNA, DNA BodDNA, DNA MekDNA, DNA dna)
        {

            base.ForceCnstr(EntDNA, SprDNA, BodDNA, MekDNA);
            _DNADecoder(dna);            
        }
           
        public void Update(float rt)
        {
           
            base.Update(rt);

        }
        
        public void SetCollidables(List<ColiListConnector> tCollidables)
        {
            Collidables = tCollidables;
        }

    }
}

