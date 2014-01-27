using System;
using Enums.Node;
using Structs;

namespace ColiSys
{
    public class DiagMotion
    { //diagonal motion
        private int x, y;
        public S_Box E;
        int counter = 0;
        int Rem;
        double dR;
        double CdR;
        int diagSteps;
        int RemainderCompleted = 0;

        //constructors
        public DiagMotion(int a, int b, S_Box tE)
        {
            x = a;
            y = b;
            E = new S_Box(tE);
            priCalc();            
        }


        public S_Box NodetoBox(Node tN)
        {
            E = new S_Box(tN.Ret(Bounds.l), tN.Ret(Bounds.u), tN.Dwn().Ret(Bounds.l), tN.Dwn().Ret(Bounds.u), true);
            return E;
        }

        public void priCalc(){
            if (Math.Abs(x) > Math.Abs(y))
            {
                Rem = Math.Abs(x) - Math.Abs(y);
                dR = (double)Math.Abs(y) / (double)Rem;
            }
            else if (Math.Abs(x) < Math.Abs(y))
            {
                Rem = Math.Abs(y) - Math.Abs(x);
                dR = (double)Math.Abs(x) / (double)Rem;
            }
            else
            {
                Rem = 0;
                dR = 0;
            }
            CdR = dR / 2;
            if (x == 0 || y == 0)
            {
                diagSteps = (Math.Abs(x) > Math.Abs(y)) ? Math.Abs(x) : Math.Abs(y);
            }
            else
            {
                diagSteps = (Math.Abs(x) > Math.Abs(y)) ? Math.Abs(y) : Math.Abs(x);
            }
        }


        public DiagMotion(int a, int b, Node tN)
        {
            E = new S_Box(tN.Ret(Bounds.l), tN.Ret(Bounds.u), tN.Dwn().Ret(Bounds.l), tN.Dwn().Ret(Bounds.u), true);
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
            diagSteps = 1;
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
            E.loc.x = E.loc.x + tx;
            E.loc.y = E.loc.y + ty;
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
            if ((counter) >= diagSteps)
            {
                return null;
            }

            S_Box i = VelToBox();

            if ((double)counter >= CdR && Rem != 0 && CdR != 0 && RemainderCompleted != Rem)
            {
                if (x == diagSteps)
                {
                    if (y > 0)
                    {
                        i.loc.y += 1;
                        i.size.y += 1;
                        PMove(0, 1);
                    }
                    else
                    {
                        i.size.y += 1;
                        PMove(0, -1);
                    }
                }
                else
                {
                    if (x > 0)
                    {
                        i.size.x += 1;
                        PMove(1, 0);
                    }
                    else
                    {
                        i.loc.x += 1;
                        i.size.x += 1;
                        PMove(-1, 0);
                    }
                }
                //Console.Out.WriteLine("E has been moved to: " + E.GenString());
                CdR += dR;
                RemainderCompleted++;
            }
        
            PMove();
            counter += 1;
            return i;
        }

    }
}