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
        protected List<BodyPart> bodyParts;
        //body vars
        protected int moveForce;
        protected int totalMass;
        


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

        public void AddBodyPart(BodyPart bpToAdd)
        {
            
            //not sure how to go about this yet since dont have sutures, but in the
            //mean time, get the total mass
            _UpdateBodyPartRelatedInfo();
        }

        private void _UpdateBodyPartRelatedInfo()
        {
            totalMass = this.mass;
            foreach (BodyPart bp in bodyParts)
            {
                bp.UnlockAllConnections();
                totalMass += bp.getTotalMass();
            }
            EI.totalMass = totalMass;
        }

        protected void ForceCnstr(DNA dna)
        {
            bodyParts = new List<BodyPart>();
            
            base.ForceCnstr(dna);
            _DNADecoder(dna);
            _UpdateBodyPartRelatedInfo();
        }

        public void Update(float rt)
        {
            _UpdateBodyParts(rt);
            base.Update(rt);
        }


        private void _UpdateBodyParts(float rt)
        {

            foreach (BodyPart bp in bodyParts)
            {
                bp.UnlockAllConnections();
                bp.Update(rt);
            }

        }

        public void Draw()
        {
            foreach (BodyPart bp in bodyParts)
            {
                bp.UnlockAllConnections();
                bp.Draw();
            }
            base.Draw();//pass draw down to sprite class
        }


        //This func prob wont be needed since abilities will be done inside the bodyparts now
        public bool HasBodyPart(BodyPartType type)
        {
            foreach (BodyPart bp in bodyParts)            
                if (bp.partType == type)
                    return true;
            
            return false;
        }

        


        public List<BodyPart> GetAllCollidingParts(S_XY moveBy,VagueObject checkAgainst)
        {
            
            List<BodyPart> allColiParts = new List<BodyPart>();
            foreach (BodyPart bp in bodyParts)
            {
                bp.UnlockAllConnections(); //VERY IMPORTANT, else you wont get proper data
                bp.CheckColi(moveBy, checkAgainst, allColiParts);
            }
            return allColiParts;
        }

        public void MoveAllBy(S_XY modOffset)
        {
            offset += modOffset;
            foreach (BodyPart bp in bodyParts)
            {
                bp.MovePartBy(modOffset); 
            }
        }

    }
}
