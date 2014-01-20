using System;
using NodeEnum;
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
        int t;

        //constructors
        public DiagMotion(int a, int b, S_Box tE)
        {
            x = a;
            y = b;
            E = new S_Box(tE);
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
                t = (Math.Abs(x) > Math.Abs(y)) ? Math.Abs(x) : Math.Abs(y);
            }
            else
            {
                t = (Math.Abs(x) > Math.Abs(y)) ? Math.Abs(x) : Math.Abs(y);
            }
        }
        public DiagMotion()
        {
            x = 1;
            y = 1;
            E = new S_Box(); //see default constructor in Entity Class
            Rem = 0;
            dR = 0;
            CdR = 0;
            t = 1;
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
            else if (x == 0)
            {
                c = -1;
            }

            if (yy > 0)
            {
                b = 1;
            }
            else if (y == 0)
            {
                d = -1;
            }

            S_Box i = new S_Box(new S_XY(E.loc.x + a, E.loc.y + b), new S_XY(E.size.x + c, E.size.y + d));
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
            //Console.Out.WriteLine("doing while(counter >= t)     counter :" + counter + " t: " + t);
            if ((counter - 1) >= t && counter != 0)
            {
                return null;
            }
            if (counter != 0)
            {
                PMove();
                //Console.Out.WriteLine("E has been moved to: " + E.GenString());
            }
            else
            {
                //Console.Out.WriteLine("counter == 0. Not moving E.");
            }

            S_Box i = VelToBox();

            if ((double)counter >= CdR && Rem != 0 && CdR != 0)
            {
                //Console.Out.WriteLine("Special case has occured. Moving E additionally...");
                if (x == t)
                {
                    if (y > 0)
                    {
                        i.loc.y += 1;
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
                        PMove(-1, 0);
                    }
                }
                //Console.Out.WriteLine("E has been moved to: " + E.GenString());
                CdR += dR;
            }
            counter += 1;
            return i;
        }

    }
}