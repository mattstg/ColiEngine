using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structs;
using EntStructEnum;
using BodyParts;

namespace EntSys
{
    class Body : Sprite
    {
        protected Global.Timers UniResponseT = new Global.Timers();//this way until dna
        protected List<BodyPart> bodyParts;
        //body vars
        protected int moveForce;
        


        public Body() { }
        public Body(DNA EntDNA,DNA SprDNA, DNA dna)
        {
            //set the instructor to be prepped as if being fed a string of values
            //some things only affected by full ints, so passing doubles can make a good scope/buffer level
            ForceCnstr(EntDNA,SprDNA,dna);
            
        }

        private void _DNADecoder(DNA dna)
        {
            //Need DNA copier
            moveForce = 60;
            
        }

        protected void ForceCnstr(DNA EntDNA, DNA SprDNA, DNA dna)
        {
            bodyParts = new List<BodyPart>();

            base.ForceCnstr(EntDNA,SprDNA);
            _DNADecoder(dna);
        }

        public void Update(float rt)
        {
            _UpdateBodyParts(rt);
            base.Update(rt);
        }


        private void _UpdateBodyParts(float rt)
        {
            foreach (BodyPart bp in bodyParts)
                bp.Update(rt);

        }

        public void Draw()
        {
            foreach (BodyPart bp in bodyParts)
                bp.Draw();
            base.Draw();//pass draw down to sprite class
        }


        //Public Funcs
        public bool HasBodyPart(BodyPartType type)
        {
            foreach (BodyPart bp in bodyParts)            
                if (bp.partType == type)
                    return true;
            
            return false;
        }

        public void MoveBodyParts(Microsoft.Xna.Framework.Vector2 modOffset)
        {
            foreach (BodyPart bp in bodyParts)
            {
                bp.MovePartBy(modOffset); 
            }
        }

    }
}
