using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structs;
using NodeEnum;

namespace EntSys
{
    class Entity
    {

        protected S_XY size;
        protected S_XY loc;
        private S_XY _offset = new S_XY();
        protected S_XY offset { set { _offset.x = (_offset.x < 0) ? 0 : value.x; _offset.y = (_offset.y < 0) ? 0 : value.y; } get { return _offset; } }
        //protected S_XY offset = new S_XY();
        protected ColiSys.Node sizeLocSquare;
        protected ColiSys.Node bodyShape;

        public Entity() { }
        public Entity(DNA dna) { ForceCnstr(dna); }


        protected void ForceCnstr(DNA dna)
        {
            //size = tsize;
            //loc = tloc;
            _DNACopier(dna);
            _SetSizeInNodeForm();
        }

        private void _DNACopier(DNA dna)
        {
            //Need DNA copier
            if (dna != null)
            {
                loc = dna.dDNA[0];
                size = dna.dDNA[1];
            }
            else
            {
                loc = new S_XY(0, 0);
                size = new S_XY(1, 1);
            }
        }

        private void _SetSizeInNodeForm()
        {
            ColiSys.Node temp = new ColiSys.Node(loc.x, loc.x + size.x);
            temp.Dwn(new ColiSys.Node(loc.y - size.y, loc.y));
            sizeLocSquare = temp;
        }

        public ColiSys.Node RetSizeLocCopy()
        {
            _SetSizeInNodeForm();
            return sizeLocSquare.CopySelf(copyTypes.copyDwn);
        }

        public void Update(float rt)
        {

        }

        protected void setColiBox()
        {
            ColiSys.Node x = bodyShape;
            ColiSys.Node y = x.Dwn();
            S_XY yRange = new S_XY(int.MaxValue,0);            
            S_XY xRange = new S_XY(bodyShape.Ret(Bounds.l),0);


            while (x != null)
            {
                y = x.Dwn();
                while (y != null)
                {
                    if (y.Ret(Bounds.l) < yRange.x)
                        yRange.x = y.Ret(Bounds.l);

                    if (y.Ret(Bounds.u) > yRange.y)
                        yRange.y = y.Ret(Bounds.u);

                    y = y.Adj();

                }
                if (x.Ret(Bounds.u) > xRange.y)
                    xRange.y = x.Ret(Bounds.u);
                x = x.Adj();
            }

            sizeLocSquare = new ColiSys.Node(xRange) + offset.x;
            sizeLocSquare.Dwn(new ColiSys.Node(yRange) + offset.y);

            Console.Out.WriteLine("Calculated Coli Box" +'\n' + sizeLocSquare.GenString() + '\n' + "   " + sizeLocSquare.Dwn().GenString());
        }

    }
}
