using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace BodyParts
{
    public class BodyPartConnection
    {
        BodyPart p1;
        BodyPart p2;
        private bool Lock = false;
        int str; //Strength of connection
        


        public BodyPartConnection(BodyPart part1, BodyPart part2)
        {
            p1 = part1;
            p2 = part2;



        }

        public void Update(BodyPart typeThis, float rt)
        {
            if (!Lock)
            {
                if (typeThis == p1)
                    p2.Update(rt);
                else
                    p1.Update(rt);
                Lock = true;
            }
        }

        public void MovePartBy(BodyPart typeThis, Structs.S_XY moveBy)
        {
            if (!Lock)
            {
                if (typeThis == p1)
                    p2.MovePartBy(moveBy);
                else
                    p1.MovePartBy(moveBy);
                Lock = true;
            }

        }

        public void Input(BodyPart typeThis, KeyboardState ks)
        {
            if (!Lock)
            {
                if (typeThis == p1)
                    p2.Input(ks);
                else
                    p1.Input(ks);
                Lock = true;
            }
        }

        public bool SealConnection(BodyPart p1, ColiSys.Node c1, BodyPart p2, ColiSys.Node c2)
        {
            //Attempt to seal two body parts

            return false;
        }

        public void Send(BodyPart typeThis,BodyPulse bp)
        {
            if (!Lock)
            {
                if (typeThis == p1)
                    p2.Recieve(bp);
                else
                    p1.Recieve(bp);
                Lock = true;
            }


        }

        public void CheckColi(BodyPart typeThis, Structs.S_XY byOffset, EntSys.VagueObject coliObj, List<BodyPart> coliParts)
        {
            if (!Lock)
            {
                if (typeThis == p1)
                    p2.CheckColi(byOffset, coliObj, coliParts);
                else
                    p1.CheckColi(byOffset, coliObj, coliParts);
                Lock = true;
            }
        }

        public void Unlock(BodyPart typeThis)
        {
            if (Lock)
            {
                if (typeThis == p1)
                    p2.UnlockAllConnections();
                else
                    p1.UnlockAllConnections();
                Lock = false;
            }
        }

        public int getTotalMass(BodyPart typeThis)
        {
            if (!Lock)
            {
                if (typeThis == p1)
                    return p2.getTotalMass();
                else
                    return p1.getTotalMass();
                Lock = true;
            }
            return 0;
        }

        public FeedbackPulse SendPulse(BodyPart typeThis, FuncPulseType funcPulseType, FuncPulse funcPulse)
        {

                if (typeThis == p1)
                    return p2.SendFuncPulse(funcPulseType, funcPulse);
                else
                    return p1.SendFuncPulse(funcPulseType, funcPulse);
             
        }

    }
}
