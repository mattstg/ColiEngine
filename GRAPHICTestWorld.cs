using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;   //   for Texture2D
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
 
using Structs;
using EntSys;
using Enums.Node;
using BodyParts;
using FactSys;
//Next version of graphic world needs to take into account zooming in and out and the not drawing of things outside
namespace ColiSys
{
    class GRAPHICTestWorld
    {
        Global.Bus bus = Global.Bus.Instance;
        int boxSize;
        Material theGround;
        List<VagueObject> masterList;
        VOContainer bodyPartList;
        MaterialFactory forge;
        List<HumanPlayer> humanList;
        Hashtable toAdd;
        NodeManipulator nami = NodeManipulator.Instance;
        public static List<BodyMechanics> Update2List; //BodyMechanics access during update, add themselves if update was not completed
        float inputTimer;
        const float inputRefreshTimer = 50;
        ShapeGenerator shapeGen;
        HumanFactory humanFact = HumanFactory.Instance;

        public GRAPHICTestWorld()
        {
            Update2List = new List<BodyMechanics>();
            forge = MaterialFactory.Instance;
            shapeGen = ShapeGenerator.Instance;
            humanList = new List<HumanPlayer>();
            bodyPartList = new VOContainer(this);
            masterList = new List<VagueObject>();
            theGround = forge.CreateMaterial(0); //Reason created seperatly is for the mouse adding to specificlly this hash table
            masterList.Add(new VagueObject(theGround));
            masterList.Add(new VagueObject(forge.CreateMaterial(1)));
            HumanPlayer h1 = humanFact.CreateHuman(0);
            //HumanPlayer h2 = humanFact.CreateHuman(1);

            masterList.Add(new VagueObject(h1));
           // masterList.Add(new VagueObject(h2));
            LinkColiLists(h1);
          //  LinkColiLists(h2);
            humanList.Add(h1);
           // humanList.Add(h2);

            inputTimer = 0;
            boxSize = 50;
                       
            toAdd = new Hashtable();
	    }
	
	public void ResetWorld()
	{
        
      //  theGround.htable = new Hashtable(shapeGen.GenShape(Shape.Square, new S_XY(0, Consts.TopScope.WORLD_SIZE_Y),));
	}

    public void LoadWorldTexture(Texture2D texture)
    {
       
        toAdd.LoadTexture(texture, Color.GreenYellow);
    }

    public void CheckForNewBodyParts()
    {
        bool atLeastOneAdded = false;
        foreach (VagueObject vo in masterList)
        {
            if (vo.type == objType.Body)
            {
                MasterBody b = vo.getObj<MasterBody>();
                if (b.RegisterNewParts)
                {
                    //Moot
                    bodyPartList.Add(b.GetAllParts());
                    //Get List from body of all body parts, 
                    //add list of VOs to VOContatiner BodyPartList
                    b.RegisterNewParts = false;
                    atLeastOneAdded = true;
                }

            }
        }
        if(atLeastOneAdded)
            AddAllBpToMasterList(); //add the bodyPartList to all the objects that will accept it in the masterlist


    }


    public void AddAllBpToMasterList()
    {
        foreach (VagueObject vo in masterList)
            if (vo.baseType == objBaseType.Ent && vo.type == objType.Body) //is base type ent, and is not ground
                if(vo.getObj<EntSys.Entity>().acceptsColiType(objType.Body) || vo.getObj<EntSys.Entity>().acceptsColiType(objSpecificType.BodyPart)) //as an ent, accepts the types body or bodyparts
                    foreach (VagueObject bp in bodyPartList.ReturnList())
                        if (vo.getObj<EntSys.Entity>() != (EntSys.Entity)(bp.getObj<BodyPart>().Master)) //if the current object does not equal to the toAdd bodyParts master
                           vo.getObj<EntSys.Entity>().AddCollidables(bp);
    }
 

    public void Draw()
    {

        foreach (VagueObject vo in masterList)
            vo.Draw();

        

        toAdd.Draw();

    }

    public void Input(KeyboardState keys, MouseState mouse)
    {
        if (inputTimer <= 0)
        {
            foreach (HumanPlayer h in humanList)
                h.Input();


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
                /*Console.Out.WriteLine("///////////////////////////////CURRENT ADDITION theGround.htable/////////////////////");
                Console.Out.WriteLine(nami.GenString(toAdd));*/
                
            }
            if (mouse.RightButton == ButtonState.Pressed)
            {
                /*
                Console.Out.WriteLine("///////////////////////////////////////////////////////////////////");
                Console.Out.WriteLine("///////////////////////////////HASH SUBTRACTOR/////////////////////");
                Console.Out.WriteLine(nami.GenString(theGround.htable));

                Console.Out.WriteLine("////////Original ^////////////////////SUBTRACTION v  /////////////");
                Console.Out.WriteLine(nami.GenString(toAdd));
                */
                Hashtable ht = new Hashtable(theGround.trueEntShapeOffset);
                ht.HashSubtractor(toAdd);
                theGround.SetEntShape(ht);
                toAdd.EmptyTable();               
                
               /* Console.Out.WriteLine("////////////////////////// vvv Result vvv//////////////////////////");
                Console.Out.WriteLine(nami.GenString(theGround.htable));*/
            }
            if (mouse.MiddleButton == ButtonState.Pressed || keys.IsKeyDown(Keys.A))
            {
                /*
                Console.Out.WriteLine("///////////////////////////////////////////////////////////////////");
                Console.Out.WriteLine("///////////////////////////////HASH ADDER//////////////////////////");
                Console.Out.WriteLine(nami.GenString(theGround.htable));

                Console.Out.WriteLine("////////Original ^////////////////////ADDITION v  /////////////////");
                Console.Out.WriteLine(nami.GenString(toAdd));
                */
                Hashtable ht = new Hashtable(theGround.trueEntShapeOffset);
                ht.HashAdder(toAdd);
                theGround.SetEntShape(ht);
                toAdd.EmptyTable();   
                /*
                Console.Out.WriteLine("////////////////////////// vvv Result vvv//////////////////////////");
                Console.Out.WriteLine(nami.GenString(theGround.htable));*/

            }
            if (keys.IsKeyDown(Keys.I))
            {
                /*
                Console.Out.WriteLine("////////////////////////// vvv Before Inverse vvv//////////////////////////");
                Console.Out.WriteLine(nami.GenString(theGround.htable));
                Node temp = nami.Inverser(theGround.htable.RetMainNode());
                theGround.htable.ResetMainNode(temp);
                Console.Out.WriteLine("////////////////////////// vv After Inverse vvv//////////////////////////");
                Console.Out.WriteLine(nami.GenString(theGround.htable));
                
                */
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
        CheckForNewBodyParts();
        //Spliting into 3 checks and allowing for a backup check creates extra fine reactions
        foreach (VagueObject vo in masterList)
            vo.Update(rt/3);
        foreach (BodyMechanics bm in Update2List)
            bm.Update2();
        foreach (VagueObject vo in masterList)
            vo.Update(rt/3);
        foreach (BodyMechanics bm in Update2List)
            bm.Update2();
        foreach (VagueObject vo in masterList)
            vo.Update(rt/3);
        foreach (BodyMechanics bm in Update2List)
            bm.Update2();

       /* int safetyNet = 500;
       // while (Update2List.Count > 0)
       // {
          //  safetyNet--;
            for(int i = Update2List.Count - 1; i >= 0; i--)
            {
                BodyMechanics tempBM = Update2List[i];
                Update2List.RemoveAt(i); //pop
                tempBM.Update2();
            }
            //call update2 on each
            if (safetyNet <= 0)
            {
                Console.Out.WriteLine("safety net for Update2 activated");
                Update2List.Clear();
            }
        }*/

        //Unload explosions and turn them back into them


        _DestroyEmptyLists();

    }

    private void _DestroyEmptyLists()
    {
        for (int i = masterList.Count - 1; i >= 0; i--)
            if (masterList[i].Destroy() || masterList[i] == null)
                masterList.RemoveAt(i);


    }


    private void _UnloadBus()
    {
        List<VagueObject> recentlyAdded = new List<VagueObject>();
    //unload explosion types and reset them into explosions
        //Unload explosions
        List<object> t = bus.Unload(Global.PassengerType.Explosion);
        if (t.Count > 0)
        {
            foreach (object o in t)
            {
                Explosion exp = (Explosion)o;
                LinkColiLists(exp);
                masterList.Add(new VagueObject(exp));
                recentlyAdded.Add(new VagueObject(exp));
            }

            addNewColiItemsToMasterListObjs(recentlyAdded);
        }

    }
        //if obj is itself, remove from linkColiList
        //if obj is dead or null, remove from linkColiList
        //update to check all list items should be removed or not
        
    public void addNewColiItemsToMasterListObjs(List<VagueObject> toAdd)
    {
        //dont add to none ent items        

        foreach (VagueObject vo in masterList)
        {
            if (vo.baseType == objBaseType.Ent)
            {
                EntSys.Entity t = vo.getObj<EntSys.Entity>();
                
                foreach (VagueObject ta in toAdd)
                {
                    if (t.acceptsColiType(ta.type) || t.acceptsColiType(ta.specificType))
                        t.AddCollidables(ta);
                }
            }

        }

    }


    public void LinkColiLists(EntSys.Entity ent)
    {
        foreach(VagueObject vo in masterList)
             if (ent.acceptsColiType(vo.type) || ent.acceptsColiType(vo.specificType))
                ent.AddCollidables(vo);

        foreach (VagueObject bp in bodyPartList.ReturnList())
            if (ent.acceptsColiType(objType.Body) || ent.acceptsColiType(objSpecificType.BodyPart))
                if (ent != (EntSys.Entity)(bp.getObj<BodyPart>().Master)) //if the entity does not equal to the master ofthe body part (casted)                    
                    ent.AddCollidables(bp);
                else
                    Console.WriteLine("proof that blocked adding own part");
    }

    

    }
}
