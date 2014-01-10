

namespace ColiSys{
public class CollisionSpace {
	int[] from =  new int[2]; 
	int[] to = new int[2];
	
	public CollisionSpace(int a, int b, int c, int d){
		from[0] = a;
		from[1] = b;
		to[0] = c;
		to[1] = d;
	}
	
	public int[] getFrom(){
		return from;
	}
	
	public int[] getTo(){
		return to;
	}
	
	public string GenString(){
		string s = "(" + from[0]+ " , "+ from[1] + ") to (" + to[0]+ " , " + to[1] + ")";
		return s;
	}
}
}
