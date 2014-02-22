using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColiSys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Global;
using Enums.Node;
namespace BPDesign
{
    class BodyPartDesigner
    {
        Timers inputTimer;
        Hashtable BpHt;
        Hashtable toAdd;
        List<Hashtable> SutureSpots;
        NodeManipulator nami = NodeManipulator.Instance;

        public BodyPartDesigner()
        {
            inputTimer = new Timers(250);
            BpHt = new Hashtable();
            toAdd = new Hashtable();
            SutureSpots = new List<Hashtable>();

        }


        public void Update(float rt)
        {
            inputTimer.Tick(rt);
            if (inputTimer.ready)
            {
                inputTimer.Dec(true);
                MouseState mouse = Mouse.GetState();
                KeyboardState keys = Keyboard.GetState();
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    int m = (mouse.X / Consts.TopScope.GAME_SCALE.x) - Consts.TopScope.BRUSH_SIZE;
                    Node tempx = new Node((mouse.X / Consts.TopScope.GAME_SCALE.x) - Consts.TopScope.BRUSH_SIZE + 1, (mouse.X / Consts.TopScope.GAME_SCALE.x) + Consts.TopScope.BRUSH_SIZE - 1);
                    Node tempy = new Node((mouse.Y / Consts.TopScope.GAME_SCALE.x) - Consts.TopScope.BRUSH_SIZE + 1, (mouse.Y / Consts.TopScope.GAME_SCALE.x) + Consts.TopScope.BRUSH_SIZE - 1);
                    tempx.Dwn(tempy);
                    if (tempx.Ret(Bounds.l) >= 0 && tempy.Ret(Bounds.l) >= 0 && tempx.Ret(Bounds.u) < Consts.TopScope.WORLD_SIZE_X && tempy.Ret(Bounds.u) < Consts.TopScope.WORLD_SIZE_Y)
                        toAdd.HashAdder(tempx); //Error that can occur, reason so far unknown
                    else
                        Console.Out.WriteLine("Error clicking outside monogame");


                }
                if (mouse.RightButton == ButtonState.Pressed)
                {

                    BpHt.HashSubtractor(toAdd);
                    toAdd.EmptyTable();
                }
                if (mouse.MiddleButton == ButtonState.Pressed || keys.IsKeyDown(Keys.A))
                {
                    BpHt.HashAdder(toAdd);
                    toAdd.EmptyTable();
                }
                if (keys.IsKeyDown(Keys.Enter))
                    ValidateAndFinalizeBodyPart();



            }
        }

         



        

    public void ValidateAndFinalizeBodyPart()
    {




    }

        /*
    public bool CreateSutureSpot()
    {
        Node spots = toAdd.RetMainNode();
        Node toMake;
        //check only two points exists
        if (spots != null && spots.Adj() != null && spots.Adj().Adj() == null && spots.Dwn() != null && spots.Dwn().Dwn() == null && spots.Adj().Dwn() != null && spots.Adj().Dwn().Dwn() == null)
        {
            toMake = nami.DrawANodeLine2(spots, spots.Adj());
            if (toMake == null)
                return false;

            //check if line FULLY exists in hashtable
            //check if suture is NOT touching any other sutures
            //add the suture
        }
        else        
            return false;

        



        return true;
    }*/


        public void Draw()
        {
            BpHt.Draw();
            toAdd.Draw();
            foreach (Hashtable ht in SutureSpots)
                ht.Draw();

        }


    }
}
