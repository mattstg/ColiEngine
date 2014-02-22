using Microsoft.Xna.Framework.Graphics;  
using Microsoft.Xna.Framework;
 using Structs;
using Enums.Node;
namespace ColiSys
{
    public class Hashtable
    {
        Node mainNode = null;
        NodeManipulator nami = NodeManipulator.Instance;
        SpriteHTable spriteTable;
        Color color;
        //The overlap table
        OverlapType[,] overlapTable = { { OverlapType.Right, OverlapType.AEO, OverlapType.AEO }, { OverlapType.OEA, OverlapType.Equals, OverlapType.AEO }, { OverlapType.OEA, OverlapType.OEA, OverlapType.Left } };
        /*  compareTable
        R   BeA  BeA 
        AeB Eq   BeA
        AeB Aeb  L

        */

        public Hashtable(Node hashTreeHead)
        {
            mainNode = hashTreeHead.CopySelf(copyTypes.copyBoth);
            spriteTable = new SpriteHTable();
        }

        public Hashtable()
        {
            mainNode = null;
            spriteTable = new SpriteHTable();
        }


        public Node RetMainNode()  //Delete this function later, mainNode should stay in hashtable to avoid security/consistency issues
        {
            if (mainNode != null)
                return mainNode.CopySelf(copyTypes.copyBoth);
            else
                return null;
        }

        public void Draw()
        {
            SpriteBatch sb = Game1.spriteBatch;
            spriteTable.Draw(sb,mainNode,color);
        }

        public void LoadTexture(Texture2D texture,Color color)
        {
            this.color = color;
            spriteTable.LoadTexture(texture);
        }

        public void EmptyTable()
        {
            mainNode = null;
        }

        public void ResetMainNode(Node n)
        {
            EmptyTable();
            mainNode = n.CopySelf(copyTypes.copyBoth);
        }


        public string GenString()
        {
            string toRet = "";
            Node it = mainNode;
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

            return toRet;
        }

        //true connects 5-6, false would return OverlapType.Before
        public OverlapType RetOverlap(Node O, Node A, bool tbool)
{
	int boolean = (tbool) ? 1:0;

    if (Compare(O.Ret(Bounds.l) - boolean, A.Ret(Bounds.u)) == 2)
		return OverlapType.Before;
    if (Compare(A.Ret(Bounds.l) - boolean, O.Ret(Bounds.u)) == 2)
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

        private Node _pop(Node node)
        {
            if (node == null)
                return null;

            Node toRet = node;
            node = node.Adj(); //points to next node
            toRet.ClearLink(); //isolate the toRet node

            return toRet;
        }

        private bool _GetColiBool(Node a, Node b)
        {
            if (RetOverlap(a, b, false) == OverlapType.After || RetOverlap(a, b, false) == OverlapType.Before)
            {
                return false;
            }
            return true;


        }

        private OverlapType _GetColiType(Node a, Node b)
        {           
                return RetOverlap(a, b, false) ;
        }

        public bool Coli(S_Box box)
        {
            //turn the passed box into a hashtable
            //Node x = new Node(l,u,adj,dwn);

            Node nodeY = new Node(box.loc.y, box.size.y + box.loc.y - 1, null, null);
            Node nodeX = new Node(box.loc.x, box.loc.x + box.size.x - 1, null, nodeY);
            //////Console.Out.WriteLine("The NodeY: " + '\n' + nodeY.GenString() + '\n' + "The NodeX: " + '\n' +  nodeX.Dwn().GenString());
            //now have a tree for box 


            return Coli(nodeX); //given the nodeX trees
        }

        public OverlapType[] ColiType(S_Box box)
        {
            //turn the passed box into a hashtable
            //Node x = new Node(l,u,adj,dwn);

            Node nodeY = new Node(box.loc.y, box.loc.y + box.size.y - 1, null, null); //-1 to cancel out the +1 that comes with size when size = 0 (ex lb == ub)
            Node nodeX = new Node(box.loc.x, box.loc.x + box.size.x - 1, null, nodeY);
            //////Console.Out.WriteLine("The NodeY: " + '\n' + nodeY.GenString() + '\n' + "The NodeX: " + '\n' +  nodeX.Dwn().GenString());
            //now have a tree for box 


            return ColiType(nodeX); //given the nodeX trees

        }


        public OverlapType[] ColiType(Node hashTree)
        {
            Node mainITX = mainNode;  //Iterator to main node in x plane
            OverlapType[] toRet = new OverlapType[2];

            while (mainITX != null)
            {
                //////Console.Out.WriteLine("main" + mainITX.GenString());
                //////Console.Out.WriteLine("hashTree" + hashTree.GenString());


                if (_GetColiBool(mainITX, hashTree)) //if coli in x
                {
                    toRet[0] = _GetColiType(mainITX, hashTree);
                    //an overlap of a kind has occured, now loop through for the y laps
                    Node mainITY = mainITX.Dwn(); //retrieve the dwn node

                    while (mainITY != null)
                    {
                        //////Console.Out.WriteLine("hashTree.dwn" + hashTree.Dwn().GenString());
                        //////Console.Out.WriteLine("main.y" + mainITY.GenString());

                        if (_GetColiBool(mainITY, hashTree.Dwn())) //if a coli occurs between the y from local hash and y from given tree			
                        {
                            toRet[1] = _GetColiType(mainITY, hashTree.Dwn());
                            return toRet; ;//A coli has occured, this means there is a colision between these two trees
                        }
                        else
                            mainITY = mainITY.Adj();//iterate to the next mainITY				
                    }
                    //no coli in y, go down one more x,

                }
                mainITX = mainITX.Adj(); //iterate to next x		

            }
            toRet[0] = OverlapType.Before; //no coli happened, put some impossible otherwise values in as garabage
            toRet[1] = OverlapType.Right;
            return toRet; //no coli occured
        }

        public bool Coli(Node hashTree)  //WORKS UNDER ASSUMPTION THAT hashTree CONTAINS 1 x and 1 y!!!!!, please grow algo if needed
        {
            OverlapType[] result = ColiType(hashTree);
            if (result[0] == OverlapType.Before)
                return false;
            return true;
            
        }



        private void YMerger(Node Ox, Node Ax)
        {
            //Given the two higher scopes to be combined
            Node O = Ox.Dwn();
            Node A = Ax.Dwn();
            Node OLast = null;
            NodeSpider Ons = new NodeSpider(O);
            NodeSpider Ans = new NodeSpider(A);
            //////Console.Out.WriteLine("Enter YMerger");

            while (Ons.cur != null && A != null)
            {

                OverlapType compared = RetOverlap(O, A, false);
                //////Console.Out.WriteLine("Start YMergerLoop");

                switch (compared)
                {
                    case OverlapType.Equals:
                        //If identical, no need to merge, increase A
                        //////Console.Out.WriteLine("YMerger: OverlapType.Equals");
                        A = A.Adj();
                        break;

                    case OverlapType.Before:
                        //////Console.Out.WriteLine("YMerger: OverlapType.Before");
                        //Should be taken care of already in OverlapType.After below, unless it is the first one
                        if (Ons.cur == Ox.Dwn())
                        {
                            Node newNode = new Node(A);
                            newNode.Adj(Ox.Dwn());  //new node sets its adj to the it's
                            Ox.Dwn(newNode);
                            Ons.SetCur(newNode);
                            A = A.Adj();

                        }
                        break;

                    case OverlapType.After:
                        ////Console.Out.WriteLine("YMerger: OverlapType.After");		
                        if (Ons.next != null)
                        {
                            if (RetOverlap(Ons.next, A, false) == OverlapType.Before)
                            {
                                ////Console.Out.WriteLine("YMerge: OverlapType.After-MergeNeghb");
                                //then insert
                                Node newNode = new Node(A); //non full copy of node (no dwn should exist)
                                newNode.Adj(Ons.next);  //new node sets its adj to the it's
                                Ons.cur.Adj(newNode);
                                Ons.next = newNode;
                                A = A.Adj(); //iterate the A
                                //MergeNeighbour(O);
                            }
                            else
                            {
                                Ons.It(); ///NEXT HERE
                            }
                        }
                        else
                        {
                            OLast = Ons.cur;
                            Ons.cur = null; //triggers the while to break and case below to add all leftofer As


                        }


                        break;

                    case OverlapType.Right:
                    case OverlapType.Left:
                    case OverlapType.AEO:
                    case OverlapType.OEA:

                        _MergeNodes(O, A);
                        A = A.Adj();
                        //MergeNeighbour(O); //merge with neighbours possibly
                        break;

                }

            }

            //while(A != null) //if its in here, its because Oit reached null, so just add the rest of the A in
            //{
            if (A != null)
            {
                Node newNode = A.CopySelf(copyTypes.copyAdj);	//this new node points to remaining list of A				
                OLast.Adj(newNode);
                //A = A.Adj();
                //MergeNeighbour(OLast);

                //O = OLast.Adj();	
            }

            //}

            _MergeTouchingYs(Ox);


        }

        /*  old ymerger, pre SpiderNode
        private void YMerger2t(Node Ox, Node Ax)
        {



            //Given the two higher scopes to be combined
            Node O = Ox.Dwn();
            Node A = Ax.Dwn();
            Node OLast = null;
            //////Console.Out.WriteLine("Enter YMerger");

            while (O != null && A != null)
            {

                OverlapType compared = RetOverlap(O, A, false);
                //////Console.Out.WriteLine("Start YMergerLoop");

                switch (compared)
                {
                    case OverlapType.Equals:
                        //If identical, no need to merge, increase A
                        //////Console.Out.WriteLine("YMerger: OverlapType.Equals");
                        A = A.Adj();
                        break;

                    case OverlapType.Before:
                        //////Console.Out.WriteLine("YMerger: OverlapType.Before");
                        //Should be taken care of already in OverlapType.After below, unless it is the first one
                        if (O == Ox.Dwn())
                        {
                            Node newNode = new Node(A);
                            newNode.Adj(Ox.Dwn());  //new node sets its adj to the it's
                            Ox.Dwn(newNode);
                            A = A.Adj();

                        }
                        break;

                    case OverlapType.After:
                        ////Console.Out.WriteLine("YMerger: OverlapType.After");		
                        if (O.Adj() != null)
                        {
                            if (RetOverlap(O.Adj(), A, false) == OverlapType.Before)
                            {
                                ////Console.Out.WriteLine("YMerge: OverlapType.After-MergeNeghb");
                                //then insert
                                Node newNode = new Node(A); //non full copy of node (no dwn should exist)
                                newNode.Adj(O.Adj());  //new node sets its adj to the it's
                                O.Adj(newNode);
                                A = A.Adj(); //iterate the A
                                //MergeNeighbour(O);
                            }
                            else
                            {
                                O = O.Adj(); ///NEXT HERE
                            }
                        }
                        else
                        {
                            OLast = O;
                            O = null; //triggers the while to break and case below to add all leftofer As

                           
                        }

                        
                        break;

                    case OverlapType.Right:
                    case OverlapType.Left:
                    case OverlapType.AEO:
                    case OverlapType.OEA:

                        _MergeNodes(O, A);
                        A = A.Adj();
                        //MergeNeighbour(O); //merge with neighbours possibly
                        break;

                }

            }

            //while(A != null) //if its in here, its because Oit reached null, so just add the rest of the A in
            //{
            if (A != null)
            {
                Node newNode = A.CopySelf(copyTypes.copyAdj);	//this new node points to remaining list of A				
                OLast.Adj(newNode);
                //A = A.Adj();
                //MergeNeighbour(OLast);

                //O = OLast.Adj();	
            }

            //}

            _MergeTouchingYs(Ox);
        }
        */
        private void _MergeTouchingYs(Node Ox)
        {
            Node o = Ox.Dwn();

            while (o != null && o.Adj() != null)
            {
                if (o.Adj() != null)
                {

                    OverlapType c = RetOverlap(o, o.Adj(), true);
                    //////Console.Out.WriteLine(O.GenString() + '\n' + O.Adj().GenString() + "  " + c);
                    if (c != OverlapType.Before && c != OverlapType.After) //should never be OverlapType.After, but just to be sure
                    {
                        _MergeNodes(o, o.Adj());
                        Node toDel = o.Adj();
                        o.Adj(o.Adj().Adj());
                        toDel.ClearLink(); //clear links so will be deleted				

                    }
                    else
                    {
                        o = o.Adj();
                    }
                }
                ////Console.Out.WriteLine("DEBUG TEST: " + '\n' + this.GenString() + '\n');
            }


        }


        private void MergeNeighbour(Node O)
        {
            //OverlapType.After insertion of LOWEST scope possible, check to merge with neighbours

            bool mergeNeighbour = true;
            while (mergeNeighbour)
            {
                mergeNeighbour = false;
                if (O.Adj() != null)
                {

                    OverlapType c = RetOverlap(O, O.Adj(), true);
                    //////Console.Out.WriteLine(O.GenString() + '\n' + O.Adj().GenString() + "  " + c);
                    if (c != OverlapType.Before && c != OverlapType.After) //should never be OverlapType.After, but just to be sure
                    {
                        //needs to be merged with neighbor
                        mergeNeighbour = true;
                        _MergeNodes(O, O.Adj());
                        Node toDel = O.Adj();
                        O.Adj(O.Adj().Adj());
                        toDel.ClearLink(); //clear links so will be deleted				

                    }
                }
                ////Console.Out.WriteLine("DEBUG TEST: " + '\n' + this.GenString() + '\n');
            }


        }


        public void HashAdder(Hashtable a)
        {
            HashAdder(a.RetMainNode());
        }
        public void HashAdder(Node A)
        {
            //given another hashTable head A, add to current hashtable
            if (mainNode == null)
            {
                mainNode = A.CopySelf(copyTypes.copyBoth);
                return;
            }

            if (A == null)
                return;


            Node Oit = this.mainNode;
            Node OLast = this.mainNode;



            //Scope 1 (X)

            while (Oit != null && A != null)
            {


                OverlapType compared = RetOverlap(Oit, A, false);

                //Console.Out.WriteLine(A.GenString());
                //Console.Out.WriteLine(compared);
                //try {Thread.sleep(4000);} catch (InterruptedException e) {e.printStackTrace();}



                switch (compared)
                {
                    case OverlapType.Equals:

                        ////Console.Out.WriteLine("OverlapType.Before Equal:");
                        ////Console.Out.WriteLine(GenString());					
                        //good case, do Y ADDER
                        YMerger(Oit, A); //merge the Ys of A into Oit


                        Oit = Oit.Adj();

                        //OLast is null for some reason
                        if (OLast.Adj() != Oit)
                            OLast = OLast.Adj(); //increase the trail it
                        A = A.Adj();

                        break;

                    case OverlapType.Before:

                        //Should be taken care of already in OverlapType.After below, unless is first case
                        if (Oit == this.mainNode)
                        {
                            Node newNode = A.CopySelf(copyTypes.copyDwn);
                            newNode.Adj(this.mainNode);  //new node sets its adj to the it's
                            this.mainNode = newNode;
                            Oit = this.mainNode;
                            OLast = this.mainNode;
                            A = A.Adj();
                        }
                        else
                        {
                            //an A was created to the OverlapType.Before via OverlapType.Left or OverlapType.AEO
                            ////Console.Out.WriteLine("Step1, whats coming in:" + A.GenString());try {Thread.sleep(2000);} catch (InterruptedException e) {e.printStackTrace();}

                            Node newNode = A.CopySelf(copyTypes.copyDwn);
                            newNode.Adj(Oit);
                            ////Console.Out.WriteLine("NewNode:" + newNode.GenString());try {Thread.sleep(2000);} catch (InterruptedException e) {e.printStackTrace();}
                            ////Console.Out.WriteLine("Its new ADJ:" + Oit.GenString());try {Thread.sleep(2000);} catch (InterruptedException e) {e.printStackTrace();}
                            OLast.Adj(newNode);
                            ////Console.Out.WriteLine("OLast Adj adding newNode:" + OLast.GenString());try {Thread.sleep(2000);} catch (InterruptedException e) {e.printStackTrace();}
                            A = A.Adj();
                            Oit = newNode;

                            /*////Console.Out.WriteLine("Adj: " + A.GenString());try {Thread.sleep(2000);} catch (InterruptedException e) {e.printStackTrace();}

                            ////Console.Out.WriteLine('\n' + "Final Test:");
                            Node t = mainNode;
                            while(t != null)
                            {
                                ////Console.Out.WriteLine('\n' + t.GenString());
                                t = t.Adj();						
                            }
                            ////Console.Out.WriteLine("END RESULT: " + '\n' + '\n');try {Thread.sleep(2000);} catch (InterruptedException e) {e.printStackTrace();}
        */
                        }

                        break;

                    case OverlapType.After:


                        if (Oit.Adj() != null)
                            if (RetOverlap(Oit.Adj(), A, false) == OverlapType.Before)
                            {

                                //then insert
                                Node newNode = A.CopySelf(copyTypes.copyDwn);
                                newNode.Adj(Oit.Adj());  //new node sets its adj to the it's
                                Oit.Adj(newNode);
                                A = A.Adj(); //iterate the A
                            }
                        Oit = Oit.Adj();
                        if (OLast.Adj() != Oit)
                            OLast = OLast.Adj(); //increase the trail it
                        break;

                    case OverlapType.Right:
                    case OverlapType.Left:
                        _OverlapSplitter(Oit, A);
                        break;

                    case OverlapType.OEA:
                        _SubsetSplitter(A, Oit);
                        break;

                    case OverlapType.AEO:
                          if (!nami.AlreadyExists(Oit, A))
                             _SubsetSplitter(Oit, A);
                          else
                              A = A.Adj();
                        break;

                }



            }


            if (A != null)
            {
                Node newNode = A.CopySelf(copyTypes.copyBoth);
                OLast.Adj(newNode);
            }

            /*while(A != null) //if its in here, its because Oit reached null, so just add the rest of the A in
            {
                Node newNode = new Node(A,true);
                OLast.Adj(newNode);
                A = A.Adj();
                OLast = newNode;
			
			
			
			
			
			
            }*/


           _FinalXMerger();


        }

        public void HashSubtractor(Hashtable a)
        {
            HashSubtractor(a.RetMainNode());
        }

        public void HashSubtractor(Node Ax)
        {
            //given another hashTable head A, add to current hashtable


            Node Oit = this.mainNode;

            while (Oit != null && Ax != null)
            {


                OverlapType compared = RetOverlap(Oit, Ax, false);


                switch (compared)
                {
                    case OverlapType.Equals:

                        ////Console.Out.WriteLine(GenString());					
                        //good case, do Y ADDER
                        Oit.Dwn(nami.YSubtractor(Oit.Dwn(), Ax.Dwn())); //merge the Ys of A into Oit

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




            _FinalXMerger();  //remove when fix nami
            mainNode = nami.CleanNode(mainNode);
            //_NodeDeletor();

        }





        private void _FinalXMerger()
        {
            //iterates through the list and checks if adj X are identical and merges them
            Node it = this.mainNode;
            if (it == null)
                return;
            Node itt = it.Adj();
            if (itt == null)
                return;

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





        }

        /*
        public Node yMerger(Node O, Node A){	
            Node n = null;
            switch(RetOverlap(O,A,true)){
            case OverlapType.Right:
                n.Set(O.Ret(Bounds.l),A.Ret(Bounds.u));
                return n;
            case OverlapType.Left:	
                n.Set(A.Ret(Bounds.l),O.Ret(Bounds.u));
                return n;
            case OverlapType.Equals: 
            case OverlapType.AEO:
                //n.Set(O.Ret(Bounds.l),O.Ret(Bounds.u));
                //return n;
                O.ClearLink();
                return O;
            case OverlapType.OEA:
                //n.Set(A.Ret(Bounds.l),A.Ret(Bounds.u));
                //return n;
                A.ClearLink();
                return A;
            default: 
                ////Console.Out.Write("Warning: Invalid input into ");
            }

            return O;
        }
        */


        

        //Possible Refactor
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


        //REFACTOR  I think the cases where the bounds match is useless since that is technically a subset, please refactor
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

        public void InverseTable()
        {
            nami.Inverser(mainNode);

        }




    }
}