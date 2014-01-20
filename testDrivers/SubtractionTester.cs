using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NodeEnum;
using Structs;


namespace ColiSys
{
    public class SubtractionTester
    {

        public SubtractionTester(){
		ASCIIWorldGen World = new ASCIIWorldGen();
		World.ResetWorld();
        ShapeGenerator shapeGen = ShapeGenerator.Instance;
		
		
		Node hashNode = shapeGen.GenShape(Shape.Square, new S_XY(0,0), new S_XY(15,15));		
		Node s1 = shapeGen.GenShape(Shape.Square, new S_XY(10,10), new S_XY(7,7));
		//Node a2 = shapeGen.GenShape(Shape.Square, new S_XY(14,2), new S_XY(2,3));	
		//Node a3 = shapeGen.GenShape(Shape.Square, new S_XY(2,19), new S_XY(4,1));	
		
		Hashtable table = new Hashtable(hashNode);	
		World.LoadNodeIntoWorld(table);
		Console.Out.WriteLine("World OverlapType.Before Subtraction:" + '\n' + World.DrawWorld() + '\n' + table.GenString());			
	
		
		table.HashSubtractor(s1);
		World.LoadNodeIntoWorld(table);
        Console.Out.WriteLine("World Afer Subtraction:" + '\n' + World.DrawWorld() + '\n' + table.GenString());
		
		}
				
		
		
		
	

    }
}
