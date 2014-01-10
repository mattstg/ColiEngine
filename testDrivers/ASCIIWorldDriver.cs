using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColiSys
{

    public class ASCIIWorldDriver
    {
        public static void main(string[] args) {
		// TODO Auto-generated method stub
			
		ASCIIWorldGen World = new ASCIIWorldGen();
		World.ResetWorld();
		ShapeGenerator shapeGen = new ShapeGenerator();
		//NodeManipulator nami = new NodeManipulator();
		
		
		Node hashNode = shapeGen.GenShape(Shape.Square, new S_XY(7,7), new S_XY(7,7));
		Node addNode = shapeGen.GenShape(Shape.Square, new S_XY(5,5), new S_XY(3,3));
		
		
		Console.Out.WriteLine("World New:" + '\n' + World.DrawWorld());
		Hashtable table = new Hashtable(hashNode);
		
		World.LoadNodeIntoWorld(table);
		Console.Out.WriteLine("World with hashtable:" + '\n' + World.DrawWorld());
		
		//Console.Out.WriteLine(table.RetOverlap(Oy00, a3, true));
		//Console.Out.Write(table.RetOverlap(Ox0, added, false));
		
		
		 table.HashAdder(addNode);
		 
		 World.LoadNodeIntoWorld(table);
		 Console.Out.WriteLine("World OverlapType.After Dirt-splosion:" + '\n' + World.DrawWorld());
		/*
		 Node y3 = new Node(18,20,null,null);
		 Node y2 = new Node(12,12,null,null);
		 Node y1 = new Node(6,10,null,null);
		 Node y0 = new Node(0,4,null,null);
			Node x0 = new Node(0,0,null,y0);
		 
			
		 table = new Hashtable(x0);
		 World.LoadNodeIntoWorld(table);
		 Console.Out.WriteLine("World OverlapType.Before the Inverse:" + '\n' + World.DrawWorld());
			
			
			
		Node inversed = nami._InverseYList(x0);
		table = new Hashtable(inversed);
		World.LoadNodeIntoWorld(table);
		Console.Out.WriteLine("World OverlapType.After Inverse:" + '\n' + World.DrawWorld());
		*/
         Console.Out.WriteLine(table.GenString());
		
		
}
    }
}