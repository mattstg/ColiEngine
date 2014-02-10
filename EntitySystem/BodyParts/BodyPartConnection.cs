﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BodyParts
{
    class BodyPartConnection
    {
        BodyPart p1;
        BodyPart p2;
        private bool Lock = false;
        int str; //Strength of connection



        public BodyPartConnection()
        {
            



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


    }
}
