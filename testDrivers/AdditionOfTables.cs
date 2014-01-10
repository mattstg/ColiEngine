using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColiSys
{
    public class AdditionOfTables
    {


        //import TheGreatHashtable.enums.Bounds;
        //import TheGreatHashtable.enums.S_XY;

        public static void main(string[] args) {
			// TODO Auto-generated method stub
				
			
			
			//Addition "DirtSplosion"
			/*	Node Ay6 = new Node(3,3,null,null);
			Node Ax6 = new Node(6,6,null,Ay6);
				Node Ay5 = new Node(2,4,null,null);					
			Node Ax5 = new Node(5,5,Ax6,Ay5);
				Node Ay4 = new Node(1,5,null,null);
			Node Ax4 = new Node(4,4,Ax5,Ay4);
				Node Ay3 = new Node(0,6,null,null);
			Node Ax3 = new Node(3,3,Ax4,Ay3);
				Node Ay2 = new Node(1,5,null,null);
			Node Ax2 = new Node(2,2,Ax3,Ay2);*/
			//	Node Ay1 = new Node(2,4,null,null);	
			//Node Ax1 = new Node(1,1,null,Ay1);
				Node Ay0 = new Node(5,5,null,null);
			Node Ax0 = new Node(0,0,null,Ay0);
			
			
			
				//Original Node
			
			/*	Node Oy51 = new Node(4,4,null,null);	
				Node Oy5 = new Node(1,2,Oy51,null);					
			Node Ox5 = new Node(5,5,null,Oy5);
				Node Oy41 = new Node(3,3,null,null);
				Node Oy4 = new Node(1,1,Oy41,null);
			Node Ox4 = new Node(4,4,Ox5,Oy4);
				Node Oy31 = new Node(5,7,null,null);
				Node Oy3 = new Node(2,2,Oy31,null);
			Node Ox3 = new Node(3,3,Ox4,Oy3);
				Node Oy22 = new Node(5,5,null,null);
				Node Oy21 = new Node(3,3,Oy22,null);
				Node Oy2 = new Node(1,1,Oy21,null);
			Node Ox2 = new Node(2,2,Ox3,Oy2);*/
			//	Node Oy11 = new Node(2,2,null,null);	
			//	Node Oy1 = new Node(1,1,Oy11,null);	
			//Node Ox1 = new Node(1,1,Ox2,Oy1);
			
				Node Oy1 = new Node(4,4,null,null);			
				Node Oy0 = new Node(1,2,Oy1,null);
			Node Ox0 = new Node(0,0,null,Oy0);
			
			S_Box box = new S_Box(2,2,1,1);
			
			Hashtable table = new Hashtable(Ox0);
			
			
			//Console.Out.WriteLine(table.RetOverlap(Oy00, a3, true));
			//Console.Out.Write(table.RetOverlap(Ox0, added, false));
			
			//Console.Out.Write(table.Coli(box));
			 table.HashAdder(Ax0);
			
				
			 
			Console.Out.WriteLine(table.GenString());
			 
			 
			
			 //Know problems. 11 is not equal to 11
			 
			 
			/* From example above
			 * is dirt from hashtable tree  
			 { box
			 
			  * - * * - - - - -
			  - - * - - - - - -
			  - - { *}- - - - -
			  - - { *}- - - - -
			  - - - - - - - - -
			  */
			
			
			//Always read A is *** of O
			//Console.Out.Write(table.RetOverlap(OX,AX,false).GenString());	
			
		}

    }


}