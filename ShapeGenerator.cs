using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enums.Node;
using Structs;



namespace ColiSys
{
    public class ShapeGenerator
    {
        NodeManipulator nami;
        private static ShapeGenerator instance;
                
        private ShapeGenerator()
        {
             nami = NodeManipulator.Instance;
        }


        public static ShapeGenerator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ShapeGenerator();
                }
                return instance;
            }
        }
        public Node GenShape(Shape shape)
        {
            if (shape != Shape.ConsoleIn)
                return GenShape(shape, new S_XY(10, 10), new S_XY(5, 5));
            else
                return _MakeConsoleIn();           



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
                case Shape.ConsoleIn:
                    toRet = _MakeConsoleIn();
                    break;
                case Shape.Human:
                    toRet = _MakeHuman(size);
                    break;

                default:
                    break;


            }




            return toRet;
        }

        private Node _MakeHuman(S_XY size)
        {
            Node toRet = _MakeSquare(new S_XY(0,0),new S_XY(size.x,size.y /2));
            Node EndNode = null;

            for (Node x = toRet; x != null; x = x.Adj())
            {
                if (x.Adj() == null) //last x
                {
                    EndNode = x;
                }
            }

            EndNode.Adj(_MakeSquare(new S_XY((int)((size.x/2)+.5), 0), new S_XY(1, size.y)));
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

        private Node _MakeConsoleIn()
        {
            Node toRet = new Node();
            Node itx = toRet;
            Node ity;
            bool addAdj = false;
            bool XScope = true;
            Console.Out.WriteLine("Please create a shape by entering the l,u bounds. After entering an X, you will be prompted to enter a Y. enter -1 to upscope, upscoping in the X plane will end the shape" + '\n');
            int l = 0;
            int u = 0;
            
            Console.Out.Write("L: ");
            String input = Console.In.ReadLine();
            l = Convert.ToInt16(input);

            Console.Out.Write("U: ");
            input = Console.In.ReadLine();
            u = Convert.ToInt16(input);

            toRet = new Node(l, u, null, null);
            itx = toRet;
            ity = null;

            bool quit = false;
            XScope = false;


            while(!quit)
            {
              if(!XScope)
                  Console.Out.Write("  ");
              Console.Out.Write("L: ");
              input = Console.In.ReadLine();
              l = Convert.ToInt16(input);

              if(!XScope)
                 Console.Out.Write("  ");
              Console.Out.Write("U: ");
              input = Console.In.ReadLine();
              u = Convert.ToInt16(input);

              if (l > -1)
              {
                  Node temp = new Node(l, u, null, null);

                  if (addAdj == true)
                  {
                      ity.Adj(temp);
                      ity = ity.Adj();
                      if (XScope == true)
                      {
                          itx = ity;
                          addAdj = false;
                      }

                  }
                  else
                  {
                      itx.Dwn(temp);
                      addAdj = true;
                      XScope = false;
                      ity = itx.Dwn();

                  }
              }
              else
              {
                  if (!XScope)
                  {
                      Console.Out.WriteLine("Returning back to X scope");
                      XScope = true;
                      ity = itx;

                  }
                  else
                  {
                      Console.Out.WriteLine("Finished, leaving shapeGen");
                      quit = true;
                  }


              }
              
              Console.Out.WriteLine(nami.GenString(toRet));
            }

            return toRet;

        }

    }
}