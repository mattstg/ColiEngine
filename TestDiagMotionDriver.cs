namespace ColiSys
{
    public class TestDiagMotionDriver
    {
        public static void main(string[] args) {
		
		//diagonal motion test going @ velocity [1,1] with object of size 2,2 @ origion (0,0)
		S_Box E = new S_Box(0,0,2,2); //default constructor Entity
		DiagMotion s = new DiagMotion(1,1,E); //creating a new motion, of velocity [1,1] on object E
		S_Box ColBox;
		System.out.println("E's initial attrivutes: " + s.E.toString());
		System.out.println(s.toString());
		S_Box w = new S_Box();
		while(w != null){
			System.out.println("********************************");
			w = s.RetNextBox();
			if(w != null){
			System.out.println("box to be checked for collision " + w.toString());
			}else{
				System.out.println("s.RetNextBox() returned null. While loop should close...");
			}
		}
		System.out.println("While loop terminated.");
	}
    }
}

