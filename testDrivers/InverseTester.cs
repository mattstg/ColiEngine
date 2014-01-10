using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColiSys
{
    public class InverseTester
    {

        public static void main(string[] args) {
		// TODO Auto-generated method stub
		
		ShapeGenerator shapeGen = new ShapeGenerator();
		NodeManipulator nami = new NodeManipulator();
		
		
		Node hashNode = shapeGen.GenShape(Shape.Square, new S_XY(0,0), new S_XY(1,1));		
		Node a1 = shapeGen.GenShape(Shape.Square, new S_XY(2,0), new S_XY(1,26));
		//Node a2 = shapeGen.GenShape(Shape.Square, new S_XY(14,2), new S_XY(2,3));	
		//Node a3 = shapeGen.GenShape(Shape.Square, new S_XY(2,19), new S_XY(4,1));	
		
		Hashtable table = new Hashtable(hashNode);
		table.HashAdder(a1);
		//table.HashAdder(a2);
		//table.HashAdder(a3);
		Console.Out.WriteLine("World OverlapType.Before Inverse:" + '\n' + table.GenString());
	
			
		Node inversed = nami.Inverser(table.RetMainNode());
		table = new Hashtable(inversed);
        Console.Out.WriteLine("World OverlapType.After Inverse:" + '\n' + table.GenString());
	}

    }
}
