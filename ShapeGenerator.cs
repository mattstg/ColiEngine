namespace ColiSys
{
    public class ShapeGenerator
    {


        public ShapeGenerator()
        {


        }



        public Node GenShape(Shape shape, S_XY loc, S_XY size)
        {
            Node toRet = new Node(); //Dud node to make compiler happy..

            switch (shape)
            {
                case Shape.Circle:
                    toRet = _MakeCircle(loc, size);  //Not functional atm
                    break;
                case Shape.Square:
                    toRet = _MakeSquare(loc, size);
                    break;
                default:
                    break;


            }




            return toRet;
        }




        //Need to complete 
        private Node _MakeCircle(S_XY loc, S_XY size)
        {
            Node toRet = new Node(1, 1, null, null);


            return toRet;
        }

        private Node _MakeSquare(S_XY loc, S_XY size)
        {

            Node yCompenent = new Node(loc.y, loc.y + size.y - 1, null, null);
            Node toRet = new Node(loc.x, loc.x + size.x - 1, null, yCompenent);
            return toRet;

        }

    }
}