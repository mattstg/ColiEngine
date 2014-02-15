using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntSys;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
//addition of all mass!
//When creating new bodyParts, please ensure it has an Update

namespace BodyParts
{
    public enum BodyPartType { Wings };
    public struct BodyPulse
    {
        long energy;
        int trigger;        

        public BodyPulse split(int splitBy)
        {
            BodyPulse bp = new BodyPulse();
            bp.energy = energy / splitBy;
            bp.trigger = trigger;
            return bp;
        }
    }

    abstract class BodyPart : Sprite
    {
        
        List<BodyPartConnection> connecters;
        public BodyPartType partType;
        public BodyMechanics Master;
        public BodyPart()
        {
            
            
        }

        public void ForceCnstr(DNA d1, DNA d2)
        {
            connecters = new List<BodyPartConnection>();
            base.ForceCnstr(d1, d2);
        }
        protected void Update(float rt)
        {
            //This should upgrade physical things on body part
            foreach (BodyPartConnection bpc in connecters)
                bpc.Update(this, rt);



        }
        //TakeDamage(float, DamageType, dir)
        //
        public void UnlockAllConnections()
        {
            foreach (BodyPartConnection bpc in connecters)            
                bpc.Unlock(this);          

        }


        public void MovePartBy(Structs.S_XY moveBy)
        {
            offset += moveBy;
            foreach (BodyPartConnection bpc in connecters)
                bpc.MovePartBy(this, moveBy);
        }

        public void CheckColi(Structs.S_XY byOffset, VagueObject coliObj, List<BodyPart> coliParts)
        {
            //check if coli has happened if move by this ammount, if yes, add itself to the list of parts that have colided this tick
            if (coliObj.Coli(nami.MoveTableByOffset(coliBox, byOffset)))
                coliParts.Add(this);
            foreach (BodyPartConnection bpc in connecters)
                bpc.CheckColi(this,byOffset, coliObj, coliParts);
        }

        public void SutureBodyPart()
        {
           // BodyPartConnection bp = new BodyPartConnection();
           // bp.SealConnection(

        }

        public void Recieve(BodyPulse bp)
        {
            bool hasChild = false;
            DecodePulse(bp);
            foreach (BodyPartConnection bpc in connecters)
            {
                hasChild = true;
                bpc.Send(this, bp.split(connecters.Count));                
            }
            if (!hasChild)
            {
                //Need to reverse the pulse? or pulse just ends here... hmm

            }

        }

        public int getTotalMass()
        {
            int toRet = this.mass;
            foreach (BodyPartConnection bpc in connecters)
                toRet += bpc.getTotalMass(this);
            return toRet;

        }

        public void Input(KeyboardState ks)
        {
            AE.TriggerEvent(ks);
            foreach (BodyPartConnection bpc in connecters)
                bpc.Input(this,ks);

        }

        public abstract void DecodePulse(BodyPulse bp);
       
    }
}
