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

        protected ColiSys.NodeManipulator nami = ColiSys.NodeManipulator.Instance;
        protected ColiSys.Hashtable HashTrueEntShape;
        public ColiSys.Node trueEntShape { get { return HashTrueEntShape.RetMainNode(); } }
        protected Global.Timers timerOncePerSec;
        public ColiSys.Node trueEntShapeOffset { get { return nami.MoveTableByOffset(trueEntShape, offset); } }
        public ColiSys.Node coliBox { get { return nami.MoveTableByOffset(nami.ComplexNodeToSquareNode(trueEntShape), offset); } }
        public bool destroy;


        protected S_XY size{
            get
            {
                ColiSys.Node t = coliBox;
                return new S_XY(t.Ret(Bounds.u) - t.Ret(Bounds.l) + 1, t.Dwn().Ret(Bounds.u) - t.Dwn().Ret(Bounds.l) + 1);
            }
        }


        protected S_XY loc;
       // private S_XY _offset = new S_XY();
        protected Vector2 curForce = new Vector2(0,0);
        protected Vector2 velo;
        protected int mass;

        
        protected List<VagueObject> Collidables;
        public List<objType> acceptedColi; //type of coli to load into collidables

        public Vector2 momentum { get { return velo * mass; } }

        //This boundary trick no longer works, was worth a shot tho
        protected S_XY offset = new S_XY();// { set { _offset.x = (_offset.x < 0) ? 0 : value.x; _offset.y = (_offset.y < 0) ? 0 : value.y; } get { return new S_XY((int)_offset.x,(int)_offset.y); } }
        public S_XY offsetCopy { get { return new S_XY(offset); } } //returns a copy of offset publically so others cant alter
        protected Vector2 rawOffSet = new Vector2(0,0);
        //protected S_XY offset = new S_XY();
       // protected ColiSys.Node sizeLocSquare; //offset included
       // protected ColiSys.Node bodyShape;
       // protected ColiSys.Node realBodyShapeLoc { get { return nami.IncreaseTableByOffset(bodyShape,offset); } }
        //I feel this thing recalcating everytime you call it cause offset has changed might be taxing for large tables.. :/
        public Entity() { }
        public Entity(DNA dna) { ForceCnstr(dna); }

        public void SetBody(ColiSys.Hashtable tempTrueEntShape)
        {


        }

        protected bool ifBodyEmpty()
        {
            return (HashTrueEntShape == null);

        }

        public bool Coli(S_Box sbox)
        {
            ColiSys.Hashtable ht = new ColiSys.Hashtable(trueEntShapeOffset);
            return ht.Coli(sbox);

        }

        protected void ForceCnstr(DNA dna)
        {
            Collidables = new List<VagueObject>();
            //size = tsize;
            //loc = tloc;
            destroy = false;
            _DNADecoder(dna);
            timerOncePerSec = new Global.Timers(1000, 1001);
            timerOncePerSec.curT = 1000; //start it at ready
            //_SetSizeInNodeForm();
        }

        private void _DNADecoder(DNA dna)
        {
            //Need DNA copier
            
            
            mass = 10;
        }

        /*private void _SetSizeInNodeForm()
        {
            ColiSys.Node temp = new ColiSys.Node(loc.x, loc.x + size.x);
            temp.Dwn(new ColiSys.Node(loc.y - size.y, loc.y));
            sizeLocSquare = temp;
        }

        public ColiSys.Node RetSizeLocCopy()
        {
            _SetSizeInNodeForm();
            return sizeLocSquare.CopySelf(copyTypes.copyDwn);
        }*/

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

        /*
        public Structs.S_Box RetSizeLocCopyBox()
        {
            setColiBox(); //to preset the sizeLoc
            return new S_Box(offset.x, offset.y, sizeLocSquare.Ret(Bounds.u) - sizeLocSquare.Ret(Bounds.l), sizeLocSquare.Dwn().Ret(Bounds.u) - sizeLocSquare.Dwn().Ret(Bounds.l));
        }*/

        public void Update(float rt)
        {







            //should be done last, less you dec before ur ready
            if (timerOncePerSec.ready)
                timerOncePerSec.Dec(true);
            else
                timerOncePerSec.Tick(rt);
        }
        /*
        protected void setColiBox()
        {
            ColiSys.Node x = bodyShape;
            ColiSys.Node y = x.Dwn();
            S_XY yRange = new S_XY(int.MaxValue,0);            
            S_XY xRange = new S_XY(x.Ret(Bounds.l),0);


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


        }*/

        public void SetCollidables(List<VagueObject> tCollidables)
        {
            Collidables = tCollidables;
        }
        public void AddCollidables(List<VagueObject> tCollidables)
        {
            Collidables.AddRange(tCollidables);
        }

        public void AddCollidables(VagueObject tCollidables)
        {
            Collidables.Add(tCollidables);
        }

        public bool acceptsColiType(objType ty)
        {
            bool toRet = false;
            foreach (objType ot in acceptedColi)
            {
                if (ty == ot)
                    toRet = true;

            }
            return toRet;
        }
    }
}
