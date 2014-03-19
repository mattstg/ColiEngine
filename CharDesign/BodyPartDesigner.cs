using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColiSys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Global;
using Enums.Node;
using BodyParts;
using Structs;
using Global;
using BankSys;
using BodyParts;
using FactSys;


namespace FactSys
{
    enum BpStep { drawBp = 0, addSutures = 1, addAbilities = 2, finalize = 3 }
    

    

    class BodyPartDesigner
    {
        AEManagerFactory AEMangFact = AEManagerFactory.Instance;
        Bank bank;
        List<Button> buttons;
        BodyPartStore bpStore;
        BpConstructor curDesign;
        Timers inputTimer;
        Hashtable BpHt;
        Hashtable sa;
        Hashtable sb;
        bool bool_sa;
        Hashtable toAdd;
        List<Hashtable> SutureSpots;
        NodeManipulator nami = NodeManipulator.Instance;
        TestContent tc = TestContent.Instance;
        List<BodyPart> bpList;
        BpStep bpStep = BpStep.drawBp;
        Hashtable tempDrawTable;
        List<int> regPacks;
        BodyPartSaveLoader bpSaveLoader;



        private static BodyPartDesigner instance;
        public static BodyPartDesigner Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BodyPartDesigner();
                }
                return instance;
            }
        }



        private BodyPartDesigner()
        {
        
            bank = Bank.Instance;
            buttons = _createButtonList();
            bpSaveLoader = BodyPartSaveLoader.Instance;
            bpStore = BodyPartStore.Instance;
            sa = new Hashtable();
            sb = new Hashtable();
            regPacks = new List<int>();
            sa.LoadTexture(tc.dirt, Color.Black);
            sb.LoadTexture(tc.dirt, Color.Black);

            inputTimer = new Timers(250);
            BpHt = new Hashtable();
            toAdd = new Hashtable();
            SutureSpots = new List<Hashtable>();
             bpList = new List<BodyPart>();
            BpHt.LoadTexture(tc.dirt, Color.Chartreuse);
            toAdd.LoadTexture(tc.dirt, Color.Yellow);
            bool_sa = true;
            tempDrawTable = new Hashtable();
            tempDrawTable.LoadTexture(tc.dirt, Color.BlueViolet);

        }

        private List<Button> _createButtonList()
        {
            List<Button> toRet = new List<Button>();
            Hashtable b1 = new Hashtable(nami.CreateNodesFromBox(new S_Box(0,0,15,15)));
            b1.LoadTexture(tc.dirt,Color.LawnGreen);
            toRet.Add(new Button(b1, _clickEvent, 0));
            
            Hashtable b2 = new Hashtable(nami.CreateNodesFromBox(new S_Box(0,16,15,15)));
            b2.LoadTexture(tc.dirt,Color.LavenderBlush);
            toRet.Add(new Button(b2, _clickEvent, 1));

            Hashtable b3 = new Hashtable(nami.CreateNodesFromBox(new S_Box(0,31,15,15)));
            b3.LoadTexture(tc.dirt,Color.Blue);
            toRet.Add(new Button(b3, _clickEvent, 2));
            /*
            Hashtable b4 = new Hashtable(nami.CreateNodesFromBox(new S_Box(0, 46, 15, 15)));
            b4.LoadTexture(tc.dirt, Color.Black);
            toRet.Add(new Button(b4, _clickEvent, 3));*/


            return toRet;
        }

        private bool _clickEvent(int butID)
        {
            
            bpStep = (BpStep)butID;
            Console.Out.WriteLine(bpStep.ToString());
            return true;
        }


        public void BeginDesign()
        {
            bpStep = BpStep.drawBp;
            curDesign = new BpConstructor();
        }

        public void Update(float rt)
        {
            inputTimer.Tick(rt);
            if (inputTimer.ready)
            {
                
                MouseState mouse = Mouse.GetState();
                KeyboardState keys = Keyboard.GetState();
                _ClickButton(mouse);
                switch (bpStep)
                {
                    case BpStep.drawBp:
                        _DrawBP(mouse,keys);
                        break;
                    case BpStep.addSutures:
                        _AddSutures(mouse, keys);
                        break;
                    case BpStep.addAbilities:
                        _AddColiAbilities(keys);
                        break;
                    case BpStep.finalize:
                        _Finalize();
                        break;



                }
            }



        }

        private Node _CreateNodeFromClick(MouseState mouse)
        {
            
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                int m = (mouse.X / Consts.TopScope.GAME_SCALE.x) - Consts.TopScope.BRUSH_SIZE;
                Node tempx = new Node((mouse.X / Consts.TopScope.GAME_SCALE.x) - Consts.TopScope.BRUSH_SIZE + 1, (mouse.X / Consts.TopScope.GAME_SCALE.x) + Consts.TopScope.BRUSH_SIZE - 1);
                Node tempy = new Node((mouse.Y / Consts.TopScope.GAME_SCALE.x) - Consts.TopScope.BRUSH_SIZE + 1, (mouse.Y / Consts.TopScope.GAME_SCALE.x) + Consts.TopScope.BRUSH_SIZE - 1);
                tempx.Dwn(tempy);
                if (tempx.Ret(Bounds.l) >= 0 && tempy.Ret(Bounds.l) >= 0 && tempx.Ret(Bounds.u) < Consts.TopScope.WORLD_SIZE_X && tempy.Ret(Bounds.u) < Consts.TopScope.WORLD_SIZE_Y)
                    return tempx; //clicking outside of monogames^^
                else
                    Console.Out.WriteLine("Error clicking outside monogame");
            }
            
            return null;

            


        }

        private void _ClickButton(MouseState mouse)
        {
            if (bpStep != BpStep.addAbilities && bpStep != BpStep.finalize)
            {
                Node t = _CreateNodeFromClick(mouse);
                if (t != null)
                    foreach (Button but in buttons)
                        but.Click(mouse);
            }
            
        }

        private void _NextState()
        {
            bpStep = ((BpStep)(int)bpStep + 1); //think this iterates
            toAdd.EmptyTable();
        }

        public void Input(KeyboardState keys, MouseState mouses)
        {

        }


        private void _DrawBP(MouseState mouse, KeyboardState keys)
        {
            
               
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    Node t = _CreateNodeFromClick(mouse);
                    toAdd.HashAdder(t);
                }

                if (mouse.RightButton == ButtonState.Pressed)
                {
                    inputTimer.Dec(true);
                    BpHt.HashSubtractor(toAdd);
                    toAdd.EmptyTable();
                }
                if (mouse.MiddleButton == ButtonState.Pressed || keys.IsKeyDown(Keys.A))
                {
                    inputTimer.Dec(true);
                    BpHt.HashAdder(toAdd);
                    toAdd.EmptyTable();
                }
                                          

                if (keys.IsKeyDown(Keys.E))
                    toAdd.EmptyTable();

                if (keys.IsKeyDown(Keys.R))
                {
                    toAdd.EmptyTable();
                    BpHt.EmptyTable();
                    SutureSpots = new List<Hashtable>();

                }


        }

        private void _AddSutures(MouseState mouse, KeyboardState keys)
        {
            Consts.TopScope.BRUSH_SIZE = 1;

            if(keys.IsKeyDown(Keys.D1))              
                bool_sa = true;

            if(keys.IsKeyDown(Keys.D2))            
                bool_sa = false;


          

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                inputTimer.Dec(true);
                Node t = _CreateNodeFromClick(mouse);
                if (t != null)
                {
                    if (bool_sa)
                        sa.ResetMainNode(t);
                    else
                        sb.ResetMainNode(t);

                    bool_sa = !bool_sa;

                    if (sa.RetMainNode() != null && sb.RetMainNode() != null )
                    {
                        Node ttt = nami.DrawLineInsideTable(sa.RetMainNode(), sb.RetMainNode(), BpHt);
                        if (ttt != null)
                            tempDrawTable.ResetMainNode(ttt);
                        else
                            tempDrawTable.EmptyTable();
                    }
                    else
                        tempDrawTable.EmptyTable();
                }

            }

           
            if (mouse.MiddleButton == ButtonState.Pressed || keys.IsKeyDown(Keys.A))
            {
                inputTimer.Dec(true);
                if (sa.RetMainNode() != null && sb.RetMainNode() != null)
                {
                    Node t = nami.DrawLineInsideTable(sa.RetMainNode(), sb.RetMainNode(), BpHt);
                    if (t != null)
                    {
                        Hashtable tt = new Hashtable(t);

                        bool valid = true;
                        foreach (Hashtable ht in SutureSpots)
                            if(ht.Coli(tt.RetMainNode()))
                                valid = false;

                        if (valid)
                        {
                            tt.LoadTexture(tc.dirt, Color.Red);
                            SutureSpots.Add(tt);                    
                        }

                        
                    }
                    sa.EmptyTable();
                    sb.EmptyTable();
                }
            }

            

        }

        private void _AddColiAbilities(KeyboardState keys)
        {
            bool quit = false;
            while(!quit)
            {
                Console.Out.Write("Please enter the value of the ability pack to register for this part, -1 to quit: ");
                int intTemp = Convert.ToInt32(Console.ReadLine());

                if (intTemp >= 0)                
                    regPacks.Add(intTemp);                
                else
                    quit = true;
            }
            _NextState();


        }

        private void _AddAEManager(int id)
        {


        }


        private void _Finalize()
        {
        //check all body parts touch
        //check at least one horz or vertical suture exists
        //
            //for now, auto passes

            BpConstructor bpConstr = new BpConstructor();
            bpConstr.shape = new Hashtable(BpHt);
            bpConstr.regPacks = regPacks;
            bpConstr.sutureSpots = new List<Hashtable>();
            foreach (Hashtable ht in SutureSpots)
                bpConstr.sutureSpots.Add(new Hashtable(ht));
          
            //gotta take off the offset
            bool valid = false;

            while(!valid)
            {
                Console.Out.Write("Please enter a unique ID for this body part, -1 to cancel the part: ");
                int newID = Convert.ToInt32(Console.ReadLine());
                valid = bpStore.StoreBodyPart(newID, _FixOffsets(bpConstr));
                if (!valid)
                    Console.Out.WriteLine("  ID Not Unique!, other bodypart with same ID found in store");
            }

            Console.Out.WriteLine("BodyPart stored in the store! thank you for using the BpDesigner");
            Console.Out.WriteLine("What would you like to do next?");
            Console.Out.WriteLine("Make Another Part(0)?");
            Console.Out.WriteLine("Go to the Bp Store(1)");
            Console.Out.WriteLine("Go to the game(2)");
            int choice = Convert.ToInt32(Console.ReadLine());
            
                    bpStep = BpStep.drawBp;
                    toAdd.EmptyTable();
                    BpHt.EmptyTable();
                    SutureSpots = new List<Hashtable>();

                   
                



            
        }

        private BpConstructor _FixOffsets(BpConstructor bpc)
        {

            TableAndOffset htNOffset = nami.SeperateOffsetFromHt(bpc.shape.RetMainNode());
            
            bpc.shape = new Hashtable(htNOffset.ht);
            List<Hashtable> t = bpc.sutureSpots;

            bpc.sutureSpots = new List<Hashtable>();

            foreach (Hashtable hasht in t)
                bpc.sutureSpots.Add(new Hashtable(nami.MoveTableByOffset(hasht.RetMainNode(), htNOffset.offset * -1)));

            return bpc;

        }

        public void Draw()
        {
            BpHt.Draw();
            toAdd.Draw();
            sa.Draw();
            sb.Draw();
            tempDrawTable.Draw();
            if (BpHt.RetMainNode() != null)
            {
                Hashtable scale2 = nami.Scale(BpHt, .5f);
                scale2.ResetMainNode(nami.MoveTableByOffset(scale2.RetMainNode(), new S_XY(50, 50)));
                Hashtable scale4 = nami.Scale(BpHt, .25f);
                scale4.ResetMainNode(nami.MoveTableByOffset(scale4.RetMainNode(), new S_XY(150, 150)));
                scale2.LoadTexture(tc.sqr, Color.Green);
                scale4.LoadTexture(tc.sqr, Color.Gold);
                scale2.Draw();
                scale4.Draw();
            }


            foreach (Button button in buttons)
                button.ht.Draw();
            
            foreach (Hashtable ht in SutureSpots)
                ht.Draw();
            

        }


    }
}
