﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structs;
using Enums.Node;
using Microsoft.Xna.Framework;

namespace EntSys
{
    public struct EntCnstr
    {
        //public int mass;
        public ColiSys.Hashtable entShape;
        public S_XY startOffset;

    }


    public class Entity
    {
        public struct EventInfo //for body and bodyparts
        {
            //public ColiSys.Node GetColiBox { get { return this.coliBox; } }
            public bool[] coliHV;
            public int totalMass;
            public Vector2 momentum;
            public int PointsOfContact;

        }




        public ActionEvent AE;
        public EventInfo EI;
        protected ColiSys.NodeManipulator nami = ColiSys.NodeManipulator.Instance;
        protected ColiSys.Hashtable EntHashtable;
        public ColiSys.Node trueEntShape { get { return EntHashtable.RetMainNode(); } }
        protected Global.Timers timerOncePerSec;
        public ColiSys.Node trueEntShapeOffset { get { return nami.MoveTableByOffset(trueEntShape, offset); } }
        public ColiSys.Node coliBox { get { return nami.MoveTableByOffset(nami.ComplexNodeToSquareNode(trueEntShape), offset); } }
        public bool destroy;
        public objSpecificType specType; //place in ever concerete classes stand alone constructor
        public BodyMechanics Master; //another body can be a master
        public int CombinedMass;

        protected S_XY size{
            get
            {
                ColiSys.Node t = coliBox;
                return new S_XY(t.Ret(Bounds.u) - t.Ret(Bounds.l) + 1, t.Dwn().Ret(Bounds.u) - t.Dwn().Ret(Bounds.l) + 1);
            }
        }


       // private S_XY _offset = new S_XY();
        protected Vector2 curForce = new Vector2(0,0);
        protected Vector2 velo;
        protected int mass;

        
        protected VOContainer Collidables;
        public List<objType> acceptedColi; //type of coli to load into collidables
        public List<objSpecificType> acceptedSColi; //specific type
        public Vector2 momentum { get { return velo * CombinedMass; } }

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

        public void SetEntShape(ColiSys.Hashtable tempTrueEntShape)
        {            
            EntHashtable = tempTrueEntShape;
        }

        public void ApplyForce(Enums.Force.ForceTypes forceType, Vector2 mag)
        {
            //force type can be reacted differently depending on body stats or(both) states
            curForce += mag;


        }


        public void ForceNewOffset(Vector2 newOffset)
        {
            offset.x = (int)(newOffset.X);
            offset.y = (int)(newOffset.Y);
            rawOffSet = newOffset;

        }

        public Vector2 ApplyForceToVelo()
        {
            //eventaully connect with weight and such
            //F=MA UP IN HERE
            if (CombinedMass == 0)
                CombinedMass = 1;
            Vector2 toRet = curForce / CombinedMass;
            velo += toRet;
            curForce.X = 0; curForce.Y = 0; //all force used into velo
            return toRet;

        }

        protected bool ifBodyEmpty()
        {
            return (EntHashtable == null);

        }

        public bool Coli(S_Box sbox)
        {
            ColiSys.Hashtable ht = new ColiSys.Hashtable(trueEntShapeOffset);
            return ht.Coli(sbox);

        }

        protected void ForceCnstr(DNA dna)
        {
            SetEntShape(dna.entC.entShape);
            velo = new Vector2(0, 0);
            offset = dna.entC.startOffset;
            rawOffSet.X = offset.x;
            rawOffSet.Y = offset.y;
            Collidables = new VOContainer(this);
            EI = new EventInfo();           
            destroy = false;
            _DNADecoder(dna);
            
        }

        private void _DNADecoder(DNA dna)
        {
            
            CombinedMass = 10;
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

        public void SetCollidables(VOContainer tCollidables)
        {
            Collidables = tCollidables;
        }
        public void AddCollidables(VOContainer tCollidables)
        {
            Collidables.Add(tCollidables);
        }

        public void AddCollidables(VagueObject tCollidables)
        {
            Collidables.Add(tCollidables);
        }

        protected Enums.Navigation.Compass GetHeading(Vector2 tvelo)
        {
            //So important, colisquare creates an area where coli occured, check for difference between you pos and its and you will find direction
            int[] dir = new int[2];
            if (tvelo.X > 0)
                dir[1] = 2;
            //was moving right
            else if (tvelo.X < 0)
                dir[1] = 0;
            //moving left
            else
                dir[1] = 1;
            //not moving vertical


            if (tvelo.Y > 0)
                dir[0] = 2;
            //was moving down
            else if (tvelo.Y < 0)
                dir[0] = 0;
            //moving up
            else
                dir[0] = 1;
            //not moving Horz
            Structs.Navigation.Compass comp = new Structs.Navigation.Compass();
            return comp.SetCompass(dir);

        }

        protected Enums.Navigation.Compass GetHeading()
        {
            return GetHeading(velo);
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

        public bool acceptsColiType(objSpecificType ty)
        {
            bool toRet = false;
            foreach (objSpecificType ot in acceptedSColi)
            {
                if (ty == ot)
                    toRet = true;
            }
            return toRet;
        }
    }
}
