using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;   //   for Texture2D
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input; 
//Next version of graphic world needs to take into account zooming in and out and the not drawing of things outside
namespace ColiSys
{
    class GRAPHICTestWorld
    {

        int boxSize;
        Hashtable table;
        Hashtable toAdd;
        NodeManipulator nami = NodeManipulator.Instance;
        float inputTimer;
        const float inputRefreshTimer = 50;
        ShapeGenerator shapeGen;

        public GRAPHICTestWorld()
        {
            inputTimer = 0;
            boxSize = 50;
            shapeGen = ShapeGenerator.Instance;
            table = new Hashtable(shapeGen.GenShape(Shape.Square, new S_XY(0, Constants.WORLD_SIZE_Y / 2), new S_XY(Constants.WORLD_SIZE_X, Constants.WORLD_SIZE_Y / 2)));
            toAdd = new Hashtable();
	    }
	
	public void ResetWorld()
	{
        table = new Hashtable(shapeGen.GenShape(Shape.Square, new S_XY(0, Constants.WORLD_SIZE_Y), new S_XY(Constants.WORLD_SIZE_X, Constants.WORLD_SIZE_Y)));
	}

    public void LoadWorldTexture(Texture2D texture)
    {
        table.LoadTexture(texture,Color.White);
        toAdd.LoadTexture(texture, Color.GreenYellow);
    }
	
	public void AddNodeToWorld(Hashtable hasht)
	{
        table.HashAdder(hasht.RetMainNode());		
	}

    public void SubNodeFromWorld(Hashtable hasht)
    {
        table.HashSubtractor(hasht.RetMainNode());
    }

    public void Draw(SpriteBatch sb)
    {
        table.Draw(sb);
        toAdd.Draw(sb);

    }

    public void Input()
    {
        if (inputTimer <= 0)
        {
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                int m = (mouse.X / Constants.GAME_SCALE.x)-Constants.BRUSH_SIZE;
               // S_XY loc = new S_XY(mouse.X / Constants.GAME_SCALE, mouse.Y / Constants.GAME_SCALE); RRUSH SIZE
                Node tempx = new Node((mouse.X / Constants.GAME_SCALE.x) - Constants.BRUSH_SIZE+1, (mouse.X / Constants.GAME_SCALE.x) + Constants.BRUSH_SIZE-1);
                //Node tempx = new Node(0, 100);
               // Node tempy = new Node(0, 100);
                Node tempy = new Node((mouse.Y / Constants.GAME_SCALE.x) - Constants.BRUSH_SIZE + 1, (mouse.Y / Constants.GAME_SCALE.x) + Constants.BRUSH_SIZE - 1);
                tempx.Dwn(tempy);
                if (tempx.Ret(Bounds.l) >= 0 && tempy.Ret(Bounds.l) >= 0 && tempx.Ret(Bounds.u) < Constants.WORLD_SIZE_X && tempy.Ret(Bounds.u) < Constants.WORLD_SIZE_Y)                    
                    toAdd.HashAdder(tempx); //Error that can occur, reason so far unknown
                else
                    Console.Out.WriteLine("Error clicking outside monogame");
                Console.Out.WriteLine("///////////////////////////////CURRENT ADDITION TABLE/////////////////////");
                Console.Out.WriteLine(nami.GenString(toAdd));
                
            }
            if (mouse.RightButton == ButtonState.Pressed)
            {
                Console.Out.WriteLine("///////////////////////////////////////////////////////////////////");
                Console.Out.WriteLine("///////////////////////////////HASH SUBTRACTOR/////////////////////");
                Console.Out.WriteLine(nami.GenString(table));

                Console.Out.WriteLine("////////Original ^////////////////////SUBTRACTION v  /////////////");
                Console.Out.WriteLine(nami.GenString(toAdd));

                table.HashSubtractor(toAdd);
                toAdd.EmptyTable();                
                
                Console.Out.WriteLine("////////////////////////// vvv Result vvv//////////////////////////");
                Console.Out.WriteLine(nami.GenString(table));
            }
            if (mouse.MiddleButton == ButtonState.Pressed)
            {
                Console.Out.WriteLine("///////////////////////////////////////////////////////////////////");
                Console.Out.WriteLine("///////////////////////////////HASH ADDER//////////////////////////");
                Console.Out.WriteLine(nami.GenString(table));

                Console.Out.WriteLine("////////Original ^////////////////////ADDITION v  /////////////////");
                Console.Out.WriteLine(nami.GenString(toAdd));

                table.HashAdder(toAdd);
                toAdd.EmptyTable();

                Console.Out.WriteLine("////////////////////////// vvv Result vvv//////////////////////////");
                Console.Out.WriteLine(nami.GenString(table));

            }
            inputTimer = inputRefreshTimer;
        }

    }

    public void Update(float refreshTimer)
    {
        inputTimer -= refreshTimer;
        if (inputTimer < 0)
            inputTimer = 0;

    }





    }
}
