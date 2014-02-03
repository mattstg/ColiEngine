using System;
using Enums.Node;
using Structs;

namespace ColiSys
{
    public class DiagMotion
    { //diagonal motion
        private int x, y;
        public S_Box E;
        int TotalSteps;
        int CurrentStep = 0;

        //these two variables are used to calculate when we should be doing diagonal movements and non diagonal moveements
        int DM;  //total diagonal motions
        int nDM; //total non diag motions
        
        int Rem; //remainder to add after all our most frequent motion
        double dR; //ideal distance (in blocks) between each time we are adding a remainder
        double CdR; //counter which keeps track of the location we are at in our motion, and increments by dR.
        int RemainderCompleted = 0; //remainder completed
        
        new S_Box lastLoc;

        enum MainMotion{ Diago, notDiag, Mixed };
        MainMotion thisMotion;
       
        //constructors
        public DiagMotion(int a, int b, S_Box tE)
        {
            x = a;
            y = b;
            E = new S_Box(tE);
            lastLoc = new S_Box(tE);
            priCalc();            
        }

        public S_Box NodetoBox(Node tN)
        {
            E = new S_Box(tN.Ret(Bounds.l), tN.Dwn().Ret(Bounds.u), tN.Ret(Bounds.u) - tN.Ret(Bounds.l) + 1, tN.Dwn().Ret(Bounds.u) - tN.Dwn().Ret(Bounds.l) + 1, true);
            return E;
        }

        private void priCalc(){
            TotalSteps = (Math.Abs(x) > Math.Abs(y))? Math.Abs(x): Math.Abs(y);
            if(Math.Abs(x) > Math.Abs(y)){ //if x velocity is largest 
                nDM = Math.Abs(x) - Math.Abs(y); // non diagonal motions
                DM = nDM - TotalSteps; //diag motions being deduces
            }else{
                nDM = Math.Abs(y) - Math.Abs(x); // non diagonal motions
                DM = nDM - TotalSteps; //diag motions being deduces
            }

            if (DM != nDM)
            {
                thisMotion = (DM > nDM) ? MainMotion.Diago : MainMotion.notDiag;
            }
            else
            {
                thisMotion = MainMotion.Mixed;
            }


            switch (thisMotion)
            {
                case MainMotion.Mixed: //so in the case where there is equal number of DM and nDM we need to arbitrarily choose one... So I made it the same as Equal.
                case MainMotion.Diago:
                    //remainder will be the nDM
                    Rem = nDM;
                    //dR should be the spacing between the nDM
                    dR = TotalSteps / DM;
                    //CdR should be counting up until the next moment where a nDM should be added.
                    break;

                case MainMotion.notDiag:
                    //remainder will be the DM
                    Rem = DM;
                    //dR should be the spacing between the DM
                    dR = TotalSteps / nDM;
                    //CdR should be counting up until the next moment where a DM should be added.
                    break;
                default:
                    break;
            };
            CdR = dR / 2;
        }

        public DiagMotion(int a, int b, Node tN)
        {
            E = NodetoBox(tN); 
            lastLoc = new S_Box(E);
            x = a;
            y = b;
            priCalc();
        }
        public DiagMotion()
        {
            x = 1;
            y = 1;
            E = new S_Box(); //see default constructor in Entity Class
            Rem = 0;
            dR = 0;
            CdR = 0;
            TotalSteps = 1;
        }

        public string GenString()
        {
            return "velocity: [" + x + " , " + y + "]";
        }

        //this returns a Box object which tell us the next location which we need to check
        private S_Box VelToBox()
        {
            int a = 0, b = 0, c = 0, d = 0;
            if (x < 0)
            {
                a = -1;
            }
            else if (x == 0)
            {
                c = -1;
            }

            if (y > 0)
            {
                b = 1;
            }
            else if (y == 0)
            {
                d = -1;
            }

            S_Box i = new S_Box(E.loc.x + a, E.loc.y + b, E.size.x + c + 1, E.size.y + d + 1);
            return i;
        }
        private S_Box VelToBox(int xx, int yy)
        {
            int a = 0, b = 0, c = 0, d = 0;
            if (xx < 0)
            {
                a = -1;
            }
            else if (xx == 0)
            {
                c = -1;
            }

            if (yy > 0)
            {
                b = 1;
            }
            else if (yy == 0)
            {
                d = -1;
            }

            S_Box i = new S_Box(E.loc.x + a, E.loc.y + b, E.size.x + c + 1, E.size.y + d + 1);
            return i;
        }

        //this method moves the copy of the object you are collision testing forward one step in its motion.
        private void PMove()
        {
            int tx = 0, ty = 0;
            if (x != 0)
            {
                if (x > 0)
                    tx = 1;
                else
                    tx = -1;
            }
            if (y != 0)
            {
                if (y > 0)
                    ty = 1;
                else
                    ty = -1;
            }
            E.loc.x =+ tx;
            E.loc.y =+ ty;
        }
        private void PMove(int xx, int yy)
        {
            int tx = 0, ty = 0;
            if (xx != 0)
            {
                if (x > 0)
                    tx = 1;
                else
                    tx = -1;
            }
            if (yy != 0)
            {
                if (y > 0)
                    ty = 1;
                else
                    ty = -1;
            }
            E.loc.x = E.loc.x + tx;
            E.loc.y = E.loc.y + ty;
        }

        public S_Box RetNextBox()
        {
            lastLoc = (CurrentStep != 0) ? E : lastLoc;
            if ((CurrentStep) >= TotalSteps) { return null; };
            S_Box i = null; //initializing our colision box
            if (((double)CurrentStep >= CdR) && (RemainderCompleted != Rem) && (Rem != 0) && (CdR != 0)) //this is the special case we need to be adding periodically 
            {
                switch (thisMotion)
                {
                    case MainMotion.Mixed:
                    case MainMotion.Diago: //if there are more diagonal movements than horizontal/virtical ones
                        if (Math.Abs(x) > Math.Abs(y))
                        {  //moving in the x
                            i = VelToBox((Math.Abs(x)/x), 0); //generating coli box for x only motion
                            PMove((Math.Abs(x)/x),0); //moving phantom 
                        }
                        else
                        { //moving in the y
                            i = VelToBox(0, (Math.Abs(y)/y)); //generating coli box for y only motion
                            PMove(0, (Math.Abs(y)/y)); //moving phantom 
                        }
                        break;
                    case MainMotion.notDiag: //if there are more horizontal/virtical movements than diagonal ones
                        i = VelToBox();
                        PMove();
                        break;
                    default:
                        break;
                };
                CdR += dR;
                RemainderCompleted++;
            }
            else //so this is the normal case... be doing the opposite of the special case.
            {
                switch (thisMotion)
                {
                    case MainMotion.Mixed:
                    case MainMotion.Diago: //if there are more diagonal movements than horizontal/virtical ones
                        i = VelToBox(); //doing a typical diagonal motion
                        PMove(); //appropriately moving phantom
                        break;
                    case MainMotion.notDiag: //if there are more horizontal/virtical movements than diagonal ones
                       //we then need to be doing a not Diagonal movement 
                        if (Math.Abs(x) > Math.Abs(y))
                        {  //moving in the x only
                            i = VelToBox((Math.Abs(x) / x), 0);
                            PMove((Math.Abs(x) / x), 0); //appropriately moving phantom
                        }
                        else
                        { //moving in the y only
                            i = VelToBox(0, (Math.Abs(y) / y));
                            PMove(0, (Math.Abs(y) / y)); //appropriately moving phantom
                        }
                        break;
                    default:
                        break;
                }
            }
            CurrentStep += 1;
            return i; //returning colibox
        }

        public S_Box RetLast()
        {
            return lastLoc; //returns the last location of the phantom (E)
        }

    }
}