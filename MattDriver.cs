using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NodeEnum;
using Structs;


namespace ColiSys
{
    public class MattDriver
    {

        public MattDriver() {
		// TODO Auto-generated method stub

        ShapeGenerator sg = ShapeGenerator.Instance;
       // Node o = sg.GenShape(Shape.ConsoleIn, new S_XY(), new S_XY());
        Node o = sg.GenShape(Shape.Square, new S_XY(100,100), new S_XY(1,1));	
		S_Box box = new S_Box(0,1,2,2);
		
		Hashtable table = new Hashtable(o);
		
		//Console.Out.Write(table.Coli(box));
		//coli test of static?
		
		
		//Console.Out.Write(table.Coli(box));
		
		
		DiagMotion mov1 = new DiagMotion(1,0,box);
		
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