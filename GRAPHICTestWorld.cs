using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;   //   for Texture2D
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Enums.Node;
using Structs;
using EntSys;
//Next version of graphic world needs to take into account zooming in and out and the not drawing of things outside
namespace ColiSys
{
    class GRAPHICTestWorld
    {
        Global.Bus bus = Global.Bus.Instance;
        int boxSize;
        List<Explosion> explosions;
        Hashtable dirtTable;
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
            dirtTable = new Hashtable(shapeGen.GenShape(Shape.Square, new S_XY(0, Consts.TopScope.WORLD_SIZE_Y / 2), new S_XY(Consts.TopScope.WORLD_SIZE_X, Consts.TopScope.WORLD_SIZE_Y / 2)));
            toAdd = new Hashtable();
            explosions = new List<Explosion>();
	    }
	
	public void ResetWorld()
	{
        dirtTable = new Hashtable(shapeGen.GenShape(Shape.Square, new S_XY(0, Consts.TopScope.WORLD_SIZE_Y), new S_XY(Consts.TopScope.WORLD_SIZE_X, Consts.TopScope.WORLD_SIZE_Y)));
	}

    public void LoadWorldTexture(Texture2D texture)
    {
        dirtTable.LoadTexture(texture,Color.White);
        toAdd.LoadTexture(texture, Color.GreenYellow);
    }
	
	public void AddNodeToWorld(Hashtable hasht)
	{
        dirtTable.HashAdder(hasht.RetMainNode());		
	}

    public void SubNodeFromWorld(Hashtable hasht)
    {
        dirtTable.HashSubtractor(hasht.RetMainNode());
    }

    public void Draw(SpriteBatch sb)
    {
        dirtTable.Draw(sb);
        toAdd.Draw(sb);

    }

    public void Input()
    {
        if (inputTimer <= 0)
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState keys = Keyboard.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                int m = (mouse.X / Consts.TopScope.GAME_SCALE.x)-Consts.TopScope.BRUSH_SIZE;
               // S_XY loc = new S_XY(mouse.X / Consts.TopScope.GAME_SCALE, mouse.Y / Consts.TopScope.GAME_SCALE); RRUSH SIZE
                Node tempx = new Node((mouse.X / Consts.TopScope.GAME_SCALE.x) - Consts.TopScope.BRUSH_SIZE+1, (mouse.X / Consts.TopScope.GAME_SCALE.x) + Consts.TopScope.BRUSH_SIZE-1);
                //Node tempx = new Node(0, 100);
               // Node tempy = new Node(0, 100);
                Node tempy = new Node((mouse.Y / Consts.TopScope.GAME_SCALE.x) - Consts.TopScope.BRUSH_SIZE + 1, (mouse.Y / Consts.TopScope.GAME_SCALE.x) + Consts.TopScope.BRUSH_SIZE - 1);
                tempx.Dwn(tempy);
                if (tempx.Ret(Bounds.l) >= 0 && tempy.Ret(Bounds.l) >= 0 && tempx.Ret(Bounds.u) < Consts.TopScope.WORLD_SIZE_X && tempy.Ret(Bounds.u) < Consts.TopScope.WORLD_SIZE_Y)                    
                    toAdd.HashAdder(tempx); //Error that can occur, reason so far unknown
                else
                    Console.Out.WriteLine("Error clicking outside monogame");
                Console.Out.WriteLine("///////////////////////////////CURRENT ADDITION dirtTable/////////////////////");
                Console.Out.WriteLine(nami.GenString(toAdd));
                
            }
            if (mouse.RightButton == ButtonState.Pressed)
            {
                Console.Out.WriteLine("///////////////////////////////////////////////////////////////////");
                Console.Out.WriteLine("///////////////////////////////HASH SUBTRACTOR/////////////////////");
                Console.Out.WriteLine(nami.GenString(dirtTable));

                Console.Out.WriteLine("////////Original ^////////////////////SUBTRACTION v  /////////////");
                Console.Out.WriteLine(nami.GenString(toAdd));

                dirtTable.HashSubtractor(toAdd);
                toAdd.EmptyTable();                
                
                Console.Out.WriteLine("////////////////////////// vvv Result vvv//////////////////////////");
                Console.Out.WriteLine(nami.GenString(dirtTable));
            }
            if (mouse.MiddleButton == ButtonState.Pressed || keys.IsKeyDown(Keys.A))
            {
                Console.Out.WriteLine("///////////////////////////////////////////////////////////////////");
                Console.Out.WriteLine("///////////////////////////////HASH ADDER//////////////////////////");
                Console.Out.WriteLine(nami.GenString(dirtTable));

                Console.Out.WriteLine("////////Original ^////////////////////ADDITION v  /////////////////");
                Console.Out.WriteLine(nami.GenString(toAdd));

                dirtTable.HashAdder(toAdd);
                toAdd.EmptyTable();

                Console.Out.WriteLine("////////////////////////// vvv Result vvv//////////////////////////");
                Console.Out.WriteLine(nami.GenString(dirtTable));

            }
            if (keys.IsKeyDown(Keys.I))
            {
                Console.Out.WriteLine("////////////////////////// vvv Before Inverse vvv//////////////////////////");
                Console.Out.WriteLine(nami.GenString(dirtTable));
                Node temp = nami.Inverser(dirtTable.RetMainNode());
                dirtTable.ResetMainNode(temp);
                Console.Out.WriteLine("////////////////////////// vv After Inverse vvv//////////////////////////");
                Console.Out.WriteLine(nami.GenString(dirtTable));
                

            }
            inputTimer = inputRefreshTimer;
        }

    }

    public void Update(float rt)
    {
        //for mouse clicking input
        inputTimer -= rt;
        if (inputTimer < 0)
            inputTimer = 0;
        _UnloadBus();
        foreach (Explosion expl in explosions)
        {
            expl.Update(rt);
        }
        //Unload explosions and turn them back into them


        _DestroyEmptyLists();

    }

    private void _DestroyEmptyLists()
    {
        for (int i = explosions.Count - 1; i >= 0; i--)
            if (explosions[i].Destroy)
                explosions.RemoveAt(i);


    }


    private void _UnloadBus()
    {
    //unload explosion types and reset them into explosions
        List<Global.Passenger> tempList = bus.UnloadPassengersOfType(Enums.Global.VoidableTypes.Explosion);
        foreach (Global.Passenger expl in tempList)
        {
            Explosion temp = new Explosion(expl.Hashtable.RetMainNode(), expl.offset);

            LinkColiLists(temp); //link all colitables it needs
            explosions.Add(temp);           
        }

    }


    public void LinkColiLists(EntSys.HumanPlayer human)
    {
        //create head list of list ptr
        List<ColiListConnector> toRet = new List<ColiListConnector>();
        

         
        //add all kinds of table types
        ColiListConnector GroundList = new ColiListConnector(dirtTable,Enums.ColiObjTypes.ColiTypes.Dirt);
        



        //add all lists
        toRet.Add(GroundList);
        human.SetCollidables(toRet);

     }

    public void LinkColiLists(EntSys.Rock rock)
    {
        //create head list of list ptr
        List<ColiListConnector> toRet = new List<ColiListConnector>();



        //add all kinds of table types
        ColiListConnector GroundList = new ColiListConnector(dirtTable, Enums.ColiObjTypes.ColiTypes.Dirt);


        //add all lists
        toRet.Add(GroundList);
        rock.SetCollidables(toRet);
    }

    public void LinkColiLists(EntSys.Explosion expl)
    {
        //create head list of list ptr
        List<ColiListConnector> toRet = new List<ColiListConnector>();
        //add all kinds of table types
        ColiListConnector tempDirtTable = new ColiListConnector(dirtTable, Enums.ColiObjTypes.ColiTypes.Dirt);
        //should add human here too, but hes in game1 for some reason, the good version of world will accomdate this
        
        //add all lists
        toRet.Add(tempDirtTable);
        expl.SetCollidables(toRet);
    }



    }
}
