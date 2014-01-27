using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structs;
using Enums.Node;
using Microsoft.Xna.Framework;

namespace EntSys
{
    class Entity
    {
        ColiSys.NodeManipulator nami = ColiSys.NodeManipulator.Instance;
        ColiSys.Hashtable TrueEntShape = new ColiSys.Hashtable();

        protected S_XY size;
        protected S_XY loc;
        private S_XY _offset = new S_XY();
        protected Vector2 curForce;
        protected Vector2 velo;
        protected int mass;

        
        protected List<Structs.ColiListConnector> Collidables;


        public Vector2 momentum { get { return velo * mass; } }

        //This boundary trick no longer works, was worth a shot tho
        protected S_XY offset { set { _offset.x = (_offset.x < 0) ? 0 : value.x; _offset.y = (_offset.y < 0) ? 0 : value.y; } get { return new S_XY((int)_offset.x,(int)_offset.y); } }
        protected Vector2 rawOffSet = new Vector2(0,0);
        //protected S_XY offset = new S_XY();
        protected ColiSys.Node sizeLocSquare; //offset included
        protected ColiSys.Node bodyShape;
        protected ColiSys.Node realBodyShapeLoc { get { return nami.IncreaseTableByOffset(bodyShape,offset); } }
        //I feel this thing recalcating everytime you call it cause offset has changed might be taxing for large tables.. :/
        public Entity() { }
        public Entity(DNA dna) { ForceCnstr(dna); }


        protected void ForceCnstr(DNA dna)
        {
            //size = tsize;
            //loc = tloc;
            _DNADecoder(dna);
            _SetSizeInNodeForm();
        }

        private void _DNADecoder(DNA dna)
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
            mass = 10;
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

        protected void PlaceInBounds()
        {
            if (offset.x < 0)
            {
                offset.x = 0;
                rawOffSet.X = 0;
                velo.X = 0;
            }
            
            if(offset.y < 0)
            { 
                offset.y = 0;
                rawOffSet.Y = 0;
                velo.Y = 0;
            }


            if (offset.x > Consts.TopScope.WORLD_SIZE_X)
            {
                offset.x = Consts.TopScope.WORLD_SIZE_X;
                rawOffSet.X = Consts.TopScope.WORLD_SIZE_X;
            }

            if (offset.y > Consts.TopScope.WORLD_SIZE_Y)
            {
                offset.y = Consts.TopScope.WORLD_SIZE_Y;
                rawOffSet.Y = Consts.TopScope.WORLD_SIZE_Y;
            }

        }

        public Structs.S_Box RetSizeLocCopyBox()
        {
            setColiBox(); //to preset the sizeLoc
            return new S_Box(offset.x, offset.y, sizeLocSquare.Ret(Bounds.u) - sizeLocSquare.Ret(Bounds.l), sizeLocSquare.Dwn().Ret(Bounds.u) - sizeLocSquare.Dwn().Ret(Bounds.l));
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

        }

        public void SetCollidables(List<ColiListConnector> tCollidables)
        {
            Collidables = tCollidables;
        }
        
    }
}
