using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace ColiSys
{
    public class TestDiagMotionDriver
    {
        public static void main(string[] args) {
		
		//diagonal motion test going @ velocity [1,1] with object of size 2,2 @ origion (0,0)
		S_Box E = new S_Box(0,0,2,2); //default constructor Entity
		DiagMotion s = new DiagMotion(1,1,E); //creating a new motion, of velocity [1,1] on object E
		S_Box ColBox;
		Console.Out.WriteLine("E's initial attrivutes: " + s.E.GenString());
		Console.Out.WriteLine(s.GenString());
		S_Box w = new S_Box();
		while(w != null){
			Console.Out.WriteLine("********************************");
			w = s.RetNextBox();
			if(w != null){
                Console.Out.WriteLine("box to be checked for collision " + w.GenString());
			}else{
				Console.Out.WriteLine("s.RetNextBox() returned null. While loop should close...");
			}
		}
        Console.Out.WriteLine("While loop terminated.");
	}
    }
}

