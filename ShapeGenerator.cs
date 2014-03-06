using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
using Structs;



namespace ColiSys
{
    public enum Shape { HollowSqaure, Circle, Square, ConsoleIn, Human }
    public struct AdditionalInfo{
        
        public int width;
       
    };
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
       /* public Node GenShape(Shape shape)
        {
            if (shape != Shape.ConsoleIn)
                return GenShape(shape, new S_XY(10, 10), new S_XY(5, 5));
            else
                return _MakeConsoleIn();           



        }*/

        public Node GenShape(Shape shape, S_XY size, AdditionalInfo Ai)
        {
            Node toRet = new Node(); //Dud node to make compiler happy..

            switch (shape)
            {
                case Shape.Circle:
                    toRet = _MakeCircle(size);  //Not functional atm
                    break;
                case Shape.Square:
                    toRet = _MakeSquare(size);
                    break;
                case Shape.ConsoleIn:
                    toRet = _MakeConsoleIn();
                    break;
                case Shape.Human:
                    toRet = _MakeHuman(size);
                    break;
                case Shape.HollowSqaure:
                    toRet = _MakeHollowSquare(size, Ai);
                    break;

                default:
                    break;


            }
            return toRet;
        }

       public Node GenShape(Shape shape, S_XY size)
        {
            Node toRet = new Node(); //Dud node to make compiler happy..
            
            switch (shape)
            {
                case Shape.Circle:
                    toRet = _MakeCircle(size);  //Not functional atm
                    break;
                case Shape.Square:
                    toRet = _MakeSquare(size);
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
            Node toRet = _MakeSquare(size);
           // Node EndNode = null;

            
            
            //EndNode.Adj(_MakeSquare(new S_XY((int)((size.x/2)+.5), 0), new S_XY(1, size.y)));
            return toRet;
        }

        //Need to complete 
        private Node _MakeCircle(S_XY size)
        {
            Node toRet = new Node(1, 1, null, null);


            return toRet;
        }

        private Node _MakeSquare(S_XY size)
        {

            Node yCompenent = new Node(0, size.y-1, null, null);
            Node toRet = new Node(0,size.x - 1, null, yCompenent);
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

        private Node _MakeHollowSquare(S_XY size,AdditionalInfo Ai)
        {
            int width = 2;
            if (Ai.width > 0)                
               if(Ai.width < (size.x/2) && Ai.width < (size.y/2))
               {                            
                     width = Ai.width;
               } else {
                   //is one gaint node
                   size.x--; size.y--;
                   Node yy1 = new Node(0,size.y,null,null);
                   Node xx1 = new Node(0,size.x,null,yy1);
                   return xx1;
               }

            size.x--; size.y--;
            Node y3 = new Node(0, size.y);
            Node x3 = new Node(size.x - width + 1, size.x,null,y3);

            Node y22 = new Node(size.y - width - 1, size.y, null, null);
            Node y21 = new Node(0, width, y22, null);            
            Node x2 = new Node(width, size.x - width, x3, y21);

            Node y1 = new Node(0, size.y);
            Node x1 = new Node(0, width - 1, x2 , y1);


            return x1;
        }

    }
}