using Enums.Node;
using Structs;


namespace ColiSys
{
    public class NodeManipulator
    {
        OverlapType[,] overlapTable = { { OverlapType.Right, OverlapType.AEO, OverlapType.AEO }, { OverlapType.OEA, OverlapType.Equals, OverlapType.AEO }, { OverlapType.OEA, OverlapType.OEA, OverlapType.Left } };
       

        private static NodeManipulator instance;
        private NodeManipulator() { }
        public static NodeManipulator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NodeManipulator();
                }
                return instance;
            }
        }

        public S_Box NodetoBox(Node tN)
        {
            tN = ComplexNodeToSquareNode(tN);
            S_Box E = new S_Box(tN.Ret(Bounds.l), tN.Dwn().Ret(Bounds.l), tN.Ret(Bounds.u), tN.Dwn().Ret(Bounds.u), false);
            return E;
        }


        public Node Inverser(Node n)
        {
            //Node O = new Node(n,true); //Copy of the Orginal, 
            //Node toRetX = new Node(O,false);
            Node OHead = n.CopySelf(copyTypes.copyBoth);  //oit points to copy of full node to be inersed
            Node ox = OHead;
            Node oy;

            Node TRHead;
            Node TRx = null;



            //Node TRit;
            //Node Oit = O.Dwn();

            //do first case
            if (ox.Ret(Bounds.l) > 0)
            {
                TRHead = new Node(0, ox.Ret(Bounds.l) - 1, null, null);
                TRHead.Dwn(new Node(0, Consts.TopScope.WORLD_SIZE_Y));
                TRx = TRHead;

            }
            else
            {
                TRHead = new Node(ox);
                TRx = TRHead; ////////////////////ADDED THIS WHEN REALLY TIRED,NT SURE IF CORRECT
                TRHead.Dwn(_InverseAdjList(ox.Dwn()));
                if (ox.Adj() != null)
                {
                    ox = ox.Adj();
                    if (_GapExists(TRHead, ox))
                    {
                        TRHead.Adj(_Gap(TRHead, ox));
                        TRx = TRHead.Adj();
                        TRx.Dwn(new Node(0, Consts.TopScope.WORLD_SIZE_Y, null, null));
                    }
                }
                else
                {
                    if (TRHead.Ret(Bounds.u) < Consts.TopScope.WORLD_SIZE_X) //case that list is one element big, and does not strecth to end
                    {
                        TRHead.Adj(new Node(TRHead.Ret(Bounds.u), Consts.TopScope.WORLD_SIZE_X, null, null));
                        TRx = TRHead.Adj();
                        TRx.Dwn(new Node(0, Consts.TopScope.WORLD_SIZE_Y, null, null));
                    } //if it is one element big starting from 0 and strecthing to end, then the inverse above already took care of it

                }

            }


            while (ox != null)
            {
                //inverse the node
                TRx.Adj(new Node(ox));
                TRx = TRx.Adj();
                TRx.Dwn(_InverseAdjList(ox.Dwn()));

                //now gap the holes
                ox = ox.Adj();
                if (ox != null && _GapExists(TRx, ox))
                {
                    TRx.Adj(_Gap(TRx, ox));
                    TRx = TRx.Adj();
                    TRx.Dwn(new Node(0, Consts.TopScope.WORLD_SIZE_Y, null, null));
                }


            }

            //done, check for upper bound to end

            if (TRx.Ret(Bounds.u) < Consts.TopScope.WORLD_SIZE_X)
            {

                TRx.Adj(new Node(TRx.Ret(Bounds.u) + 1, Consts.TopScope.WORLD_SIZE_X, null, null));
                TRx = TRx.Adj();
                TRx.Dwn(new Node(0, Consts.TopScope.WORLD_SIZE_Y, null, null));
            }

            //Check through for nodes to be deleted//////////////////////////////////
            //Possible TRHead.adj being null? if possible, error can occur, please look into this, although its very unlikely, or TRHead being null will cause fatal crash
            Node tx = TRHead;

            while (TRHead.Dwn().Ret(Bounds.l) == -1)
            {

                TRHead = TRHead.Adj();
                tx = TRHead;
            }

            while (tx.Adj() != null)
            {


                if (tx.Adj().Dwn() == null || tx.Adj().Dwn().Ret(Bounds.l) == -1)
                    tx.Adj(tx.Adj().Adj());
                else
                    tx = tx.Adj();
            }


            return TRHead;
        }

        public Node CleanNode(Node a)
        {
            //Node toRet = a.CopySelf(copyTypes.copyBoth);
            //toRet = _MergeAllX(toRet);
            Node toRet = _DeleteAllEmptyX(a);
            toRet = _FinalXMerger(toRet);

            return toRet;
        }


        public OverlapType RetOverlap(Node O, Node A, bool tbool)
	{
		int boolean = (tbool) ? 1:0;
		
		if(Compare(O.Ret(Bounds.l)-boolean,A.Ret(Bounds.u)) == 2)
			return OverlapType.Before;
		if(Compare(A.Ret(Bounds.l)-boolean,O.Ret(Bounds.u)) == 2)
			return OverlapType.After;
		
				
		return overlapTable[Compare(O.Ret(Bounds.l),A.Ret(Bounds.l)),Compare(O.Ret(Bounds.u),A.Ret(Bounds.u))];	
				
	}

        public int Compare(int a, int b)
        {
            if (a < b)
                return 0;
            if (a > b)
                return 2;
            if (a == b)
                return 1;

            return -1;
        }




        public Node SubtractNodes(Node O, Node Ax)
        {          

            Node Oit = O.CopySelf(copyTypes.copyBoth);
            Node toRet = Oit;

            while (Oit != null && Ax != null)
            {


                OverlapType compared = RetOverlap(Oit, Ax, false);


                switch (compared)
                {
                    case OverlapType.Equals:

                        ////Console.Out.WriteLine(GenString());					
                        //good case, do Y ADDER
                        Oit.Dwn(YSubtractor(Oit.Dwn(), Ax.Dwn())); //merge the Ys of A into Oit

                        Oit = Oit.Adj();
                        Ax = Ax.Adj();

                        break;

                    case OverlapType.Before:

                        //If it comes OverlapType.Before, no need to subtract it
                        Ax = Ax.Adj();

                        break;

                    case OverlapType.After:

                        Oit = Oit.Adj();
                        break;

                    case OverlapType.Right:
                    case OverlapType.Left:
                        _OverlapSplitter(Oit, Ax);
                        break;

                    case OverlapType.OEA:
                        _SubsetSplitter(Ax, Oit);
                        break;

                    case OverlapType.AEO:
                        _SubsetSplitter(Oit, Ax);
                        break;

                }

            }
            //if A!= null, then there is sitll some A OverlapType.Left, since nothing to subtract, a becomes null
            Ax = null;

            return CleanNode(toRet);
        }
        
        private Node _FinalXMerger(Node mainNode)
        {
            //iterates through the list and checks if adj X are identical and merges them
            Node it = mainNode;
            if (it == null)
                return it;
            Node itt = it.Adj();
            if (itt == null)
                return it;

            bool again = false;
            Node xit;
            Node xitt;



            while (it != null && itt != null)
            {
                bool valid = true;
                if (it.Ret(Bounds.u) == itt.Ret(Bounds.l) - 1)
                {
                    //if connected side by side 3-4 ex
                    xit = it.Dwn();  //should not be null
                    xitt = itt.Dwn();//should not be null


                    while (valid)
                    {
                        if (xit == null || xitt == null)
                        {
                            valid = false;
                        }
                        else
                        {
                            if (xit.EqualBounds(xitt))
                            {
                                //good, iterate to the enxt
                                xit = xit.Adj();
                                xitt = xitt.Adj();
                            }
                            else
                            {
                                valid = false;
                            }

                        }

                    }

                    if (xit == null && xitt == null) //this should only occur when they exit simotanouesly through all
                    {

                        it.Set(itt.Ret(Bounds.u), Bounds.u);
                        it.Adj(itt.Adj()); //overwrite the one in front since its identical
                        itt.ClearLink();
                        itt = it.Adj();
                        again = true;
                    }



                }
                if (!again)
                {
                    it = it.Adj();
                    itt = itt.Adj();
                }
                else
                    again = false;
            }



            return mainNode;

        }

        







        public bool AlreadyExists(Node tO, Node tA)
        {
            //Checks if b already exists in the node a. Both nodes given at scope up
            Node o = tO.CopySelf(copyTypes.copyDwn);
            Node a = tA.CopySelf(copyTypes.copyDwn);
            o = o.Dwn();
            a = a.Dwn();

            
            while (a != null && o != null)
            {

                switch (RetOverlap(o, a, false))
                {
                    case OverlapType.AEO:
                    case OverlapType.Equals:
                        a = a.Adj(); //good case, they are equal, inc a
                        break;
                    case OverlapType.After:
                        o = o.Adj(); //acceptable case, need to increment o
                        break;

                    case OverlapType.OEA:                    
                    case OverlapType.Before:
                    case OverlapType.Left:
                    case OverlapType.Right:
                        o = null;
                        break;
                }
            }
            
            if(a == null)//a has reached its end, awesome, so this means a exists in o,                 
                return true;
            
            //else it exited because o has become null, meaning there is nothing left for a to check against, so obvouisly a does not exist in o
            return false;

            
                
           





        }



        public string GenString(Hashtable a)
        {
            return GenString(a.RetMainNode());
        }

        public string GenString(Node a)
        {
            
                string toRet = "";
                Node it = a;
                while (it != null)
                {

                    Node yit = it.Dwn();
                    toRet += it.GenString();
                    toRet += '\n';

                    while (yit != null)
                    {
                        toRet += "   " + yit.GenString() + '\n';
                        yit = yit.Adj();

                    }
                    it = it.Adj();

                }
                if (a == null)
                {
                    toRet = "Empty Hashtable";
                }

            return toRet;
        }


        private Node _MergeAllX(Node a)
        {
            return null;
        }

        private Node _DeleteAllEmptyX(Node a)
        {
            Node headNode = a;
            Node tx = null;

            while (headNode.Dwn() == null || headNode.Dwn().Ret(Bounds.l) == -1)
                headNode = headNode.Adj();


            tx = headNode;

            if (tx != null)
                while (tx.Adj() != null)
                {
                    if (tx.Adj().Dwn() == null || tx.Adj().Dwn().Ret(Bounds.l) == -1)
                        tx.Adj(tx.Adj().Adj());
                    else
                        tx = tx.Adj();
                }

            return headNode;

        }


        public Node YSubtractor(Node O, Node a)
        {
            Node TRHead = O.CopySelf(copyTypes.copyAdj);
            //Node TRit = TRHead;
           // Node TRTrail = TRHead;
            NodeSpider TRns = new NodeSpider(TRHead);


            //Loop not entered for Y Sub
            while (a != null && TRHead != null && TRns.cur != null)
            {


                switch (RetOverlap(TRns.cur, a, false))
                {
                    case OverlapType.Equals:

                        if (TRns.prev == null)
                        {
                            TRHead = TRns.next;
                            TRns.It();
                            TRns.prev = null;
                        }
                        else
                        {
                            TRns.prev.Adj(TRns.next);
                            TRns.cur.ClearLink();
                            TRns.cur = TRns.prev.Adj();
                            if(TRns.cur != null)
                               TRns.next = TRns.cur.Adj();
                        }
                             
                        a = a.Adj();
                        break;

                    case OverlapType.Before:
                        //if happens OverlapType.Before, no need to delete
                        a = a.Adj();
                        break;
                    case OverlapType.After:
                        if (TRns.next == null) //at end of list, just null
                            a = null;
                        else
                            TRns.It();
                        

                        break;

                    case OverlapType.Right:
                    case OverlapType.Left:
                        _OverlapSplitter(TRns.cur, a);
                        TRns.next = TRns.cur.Adj(); //splitter will keep cur at frontmost split
                        break;

                    case OverlapType.OEA:
                        _SubsetSplitter(a, TRns.cur);
                        break;

                    case OverlapType.AEO:
                        _SubsetSplitter(TRns.cur, a);
                        TRns.next = TRns.cur.Adj(); //splitter will keep cur at frontmost split
                        break;



                }
            }

            return TRHead;
        }



        //Old YSub without spider
        /*
        public Node YSubtractor2(Node O, Node a)
        {
            Node TRHead = O.CopySelf(copyTypes.copyAdj);
            Node TRit = TRHead;
            Node TRTrail = TRHead;


            //Loop not entered for Y Sub
            while (a != null && TRHead != null && TRit != null)
            {


                switch (RetOverlap(TRit, a, false))
                {
                    case OverlapType.Equals:

                        if (TRit == TRHead)
                            TRHead = TRHead.Adj();
                        else
                            TRTrail.Adj(TRTrail.Adj().Adj());


                        a = a.Adj();
                        break;

                    case OverlapType.Before:
                        //if happens OverlapType.Before, no need to delete
                        a = a.Adj();
                        break;
                    case OverlapType.After:

                        TRit = TRit.Adj();
                        if (TRTrail.Adj() != TRit)
                            TRTrail = TRTrail.Adj();

                        break;

                    case OverlapType.Right:
                    case OverlapType.Left:
                        _OverlapSplitter(TRit, a);
                        break;

                    case OverlapType.OEA:
                        _SubsetSplitter(a, TRit);
                        break;

                    case OverlapType.AEO:
                        _SubsetSplitter(TRit, a);
                        break;



                }
            }

            return TRHead;
        }
        */

        public Node ComplexNodeToSquareNode(Node O)
        {

            ColiSys.Node x = O;
            ColiSys.Node y = x.Dwn();
            S_XY yRange = new S_XY(int.MaxValue, 0);
            S_XY xRange = new S_XY(x.Ret(Bounds.l), 0);


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

            Node tr = new Node(xRange);
            tr.Dwn(new Node(yRange));
            
            return tr;
        }

        private void _MergeNodes(Node O, Node A)
        {
            //given two nodes, will merge A into node O, O will be modded, not A
            int lb;
            int ub;

            if (O.Ret(Bounds.l) <= A.Ret(Bounds.l))
                lb = O.Ret(Bounds.l);
            else
                lb = A.Ret(Bounds.l);

            if (O.Ret(Bounds.u) >= A.Ret(Bounds.u))
                ub = O.Ret(Bounds.u);
            else
                ub = A.Ret(Bounds.u);

            O.Set(lb, ub);


        }

        public void _OverlapSplitter(Node center, Node overlap)
        {


            //center node will not be split, overlap one will be
            if (overlap.Ret(Bounds.l) < center.Ret(Bounds.l))
            {
                //overlap is to the OverlapType.Left, break apart overlap
                Node oldAdjPtr = overlap.Adj();
                //Node newNode = new Node(center.Ret(Bounds.l),overlap.Ret(Bounds.u),overlap);
                Node newNode = overlap.CopySelf(copyTypes.copyDwn);
                newNode.Set(center.Ret(Bounds.l), overlap.Ret(Bounds.u));

                overlap.Adj(newNode);
                newNode.Adj(oldAdjPtr);
                overlap.Set(center.Ret(Bounds.l) - 1, Bounds.u);
            }
            else
            {

                Node oldAdjPtr = overlap.Adj();
                //Node newNode = new Node(center.Ret(Bounds.u)+1,overlap.Ret(Bounds.u),overlap);
                Node newNode = overlap.CopySelf(copyTypes.copyDwn);
                newNode.Set(center.Ret(Bounds.u) + 1, overlap.Ret(Bounds.u));

                overlap.Adj(newNode);
                newNode.Adj(oldAdjPtr);
                overlap.Set(center.Ret(Bounds.u), Bounds.u);

            }
        }

        private void _SubsetSplitter(Node a, Node b)
        {

            //Node B is a subset of node A. A will be the one that always splits
            //Node newNode()
            if (a.Ret(Bounds.l) == b.Ret(Bounds.l))
            { //lb are equal, splits into two

                Node oldAdjPtr = a.Adj();
                //Node newNode = new Node(b.Ret(Bounds.u)+1,a.Ret(Bounds.u),a);
                Node newNode = a.CopySelf(copyTypes.copyDwn);
                newNode.Set(b.Ret(Bounds.u) + 1, a.Ret(Bounds.u));
                a.Adj(newNode);
                newNode.Adj(oldAdjPtr);
                a.Set(b.Ret(Bounds.u), Bounds.u);

            }
            else if (a.Ret(Bounds.u) == b.Ret(Bounds.u))
            {

                Node oldAdjPtr = a.Adj();
                //Node newNode = new Node(b.Ret(Bounds.l),a.Ret(Bounds.u),a);
                Node newNode = a.CopySelf(copyTypes.copyDwn);
                newNode.Set(b.Ret(Bounds.l), a.Ret(Bounds.u));
                a.Adj(newNode);
                newNode.Adj(oldAdjPtr);
                a.Set(b.Ret(Bounds.l) - 1, Bounds.u);

            }
            else
            {

                //split into 3
                Node oldAdjPtr = a.Adj();
                //Node newNode = new Node(b.Ret(Bounds.l),a.Ret(Bounds.u),a);
                Node newNode = a.CopySelf(copyTypes.copyDwn);
                newNode.Set(b.Ret(Bounds.l), a.Ret(Bounds.u));
                a.Adj(newNode);
                newNode.Adj(oldAdjPtr);
                a.Set(b.Ret(Bounds.l) - 1, Bounds.u);
                _SubsetSplitter(a.Adj(), b);
            }


        }


        private Node _Gap(Node a, Node b)
        {
            Node toRet = new Node(a.Ret(Bounds.u) + 1, b.Ret(Bounds.l) - 1, null, null);
            return toRet;
        }

        private bool _GapExists(Node a, Node b)
        {
            if (a.Ret(Bounds.u) < b.Ret(Bounds.l) - 1)
                return true;
            return false;
        }

        public Node _InverseAdjList(Node a)
        {
            //Given an node, copies all adj of it 
            Node O = a.CopySelf(copyTypes.copyAdj);

            //Node toRetX = new Node(O,false);
            Node TRHead;// = new Node(O); //CopySelf(copyTypes.copyNode);		
            Node TRit;
            Node Oit = O;

            //Dont forget full case!!!
            if (O.Ret(Bounds.l) == 0 && O.Ret(Bounds.u) == Consts.TopScope.WORLD_SIZE_Y)
            {
                //this means the inverse should cause the scope above (x) to be deleted. 
                TRHead = new Node(-1, 0, null, null);
                return TRHead;
            }


            if (Oit.Ret(Bounds.l) > 0)
            {
                TRHead = new Node(0, Oit.Ret(Bounds.l) - 1, null, null);

            }
            else
            {
                if (Oit.Adj() != null)
                    TRHead = _Gap(Oit, Oit.Adj());
                else
                    TRHead = new Node(Oit.Ret(Bounds.u) + 1, Consts.TopScope.WORLD_SIZE_Y, null, null);
                Oit = Oit.Adj();
            }

            TRit = TRHead;
            //toRetX.Dwn(newDwn);
            //TRit = toRetX.Dwn();


            //Now down node has been taken care of, start with adj

            if (Oit != null) //case its null already, which means its all taken care of above
            {
                while (Oit.Adj() != null)
                {
                    Node temp = _Gap(Oit, Oit.Adj());
                    TRit.Adj(temp);
                    Oit = Oit.Adj();
                    if (Oit != null)
                        TRit = TRit.Adj();

                }
                //check if last one has reached end

                if (Oit.Ret(Bounds.u) < Consts.TopScope.WORLD_SIZE_Y) //then there is a gap
                {
                    Node temp = new Node(Oit.Ret(Bounds.u) + 1, Consts.TopScope.WORLD_SIZE_Y, null, null);
                    TRit.Adj(temp);

                }
            }


            //Y List should be inverted by here	


            return TRHead;

        }


        public Node MoveTableByOffset(Node a, S_XY offset)
        {

            Node toRet = a.CopySelf(copyTypes.copyBoth);
            Node itx = toRet;
            Node ity;
            while(itx != null)
            {
                itx.Mod(new S_XY(offset.x,offset.x));
                ity = itx.Dwn();
                while (ity != null)
                {
                    ity.Mod(new S_XY(offset.y,offset.y));
                    ity = ity.Adj();
                }
                itx = itx.Adj();
            }
            return toRet;
        }

        //I feel function could be so unsafe for any complex non square shape, only use for sqaure
        public Node StretchSquareTableByXY(Node a, S_XY offset)
       {

            Node toRet = a.CopySelf(copyTypes.copyBoth);
            Node itx = toRet;
            Node ity;
            while(itx != null)
            {
                itx.Mod(offset);
                ity = itx.Dwn();
                while (ity != null)
                {
                    ity.Mod(offset);
                    ity = ity.Adj();
                }
                itx = itx.Adj();
            }
            return toRet;
        }


    }
}