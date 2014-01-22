using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enums.Node;
using Structs;

namespace ColiSys
{

    public class squareVsHashTable
    {

        public static void main(string[] args) {
		// TODO Auto-generated method stub

		Node Oy21 = new Node(4,4,null,null);
		Node Oy20 = new Node (1,2,Oy21,null);
		Node Ox2 = new Node(3,3,null,Oy20);
		
		Node Oy10 = new Node(3,4,null,null);
		Node Ox1 = new Node(2,2,Ox2,Oy10);
		
		Node Oy00 = new Node(4,4,null,null);
		Node Ox0 = new Node(0,0,Ox1,Oy00);
		
		
		S_Box box = new S_Box(0,1,2,2);
		
		Hashtable table = new Hashtable(Ox0);
		
		//Console.Out.Write(table.Coli(box));
		//coli test of static?
		
		
		//Console.Out.Write(table.Coli(box));
		
		
		DiagMotion mov1 = new DiagMotion(2,0,box);
		
		S_Box s = new S_Box();
		int counter = 0;
		while(s != null)
		{
			
			s = mov1.RetNextBox();
			//Console.Out.WriteLine(mov1.E.GenString());
		
			
			if(s != null)
				{
					Console.Out.WriteLine("s:" + s.GenString());
                    Console.Out.WriteLine(table.Coli(s));		
				}
			//Console.Out.Write("COUNTER{" + counter + "}");
			//counter++;
		};
		
		//Always read A is *** of O
		//Console.Out.Write(table.RetOverlap(OX,AX,false).GenString());

		
		
		
	}

    }
}
