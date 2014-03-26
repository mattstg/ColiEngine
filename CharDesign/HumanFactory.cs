using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColiSys;
using EntSys;
using Microsoft.Xna.Framework;
using BodyParts;

namespace FactSys
{
    class HumanFactory
    {
        TestContent tc = TestContent.Instance;
        ColiSys.ShapeGenerator sgen = ColiSys.ShapeGenerator.Instance;
        BodyPartFactory bpFact = BodyPartFactory.Instance;

         private static HumanFactory instance;
         private HumanFactory() { }
         public static HumanFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HumanFactory();
                }
                return instance;
            }
        }




        public HumanPlayer CreateHuman(int loadoutID)
        {

            switch(loadoutID)
            {
                case 0:
                    return _GenHuman0();
                    break;

                case 1:
                    return _GenHuman1();
                    break;

                default:
                    return _GenHuman0();
            }



        }


        private HumanPlayer _GenHuman0()
        {
            HumanPlayer toRet = new HumanPlayer(null);
            toRet.LoadTexture(tc.dirt, Color.Blue);
            toRet.SetEntShape(new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.Human, new Structs.S_XY(50, 50))));
            BodyPart arm1 = bpFact.CreateBodyPart(0);
            BodyPart claws1 = bpFact.CreateBodyPart(0);
            toRet.ForceNewOffset(new Vector2(50,50));
            toRet._DebugSetKeyMap(1);
            toRet.specType = objSpecificType.Human;
            arm1.SetMasterFromMaster(toRet);
            arm1.SutureBodyPart(claws1, BpDirection.East);
            toRet.AddBodyPart(arm1, BpDirection.East);
            
            return toRet;

        }

        private HumanPlayer _GenHuman1()
        {
            HumanPlayer toRet = new HumanPlayer(null);
            toRet.LoadTexture(tc.dirt, Color.Red);
            toRet.SetEntShape(new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.Human, new Structs.S_XY(50, 50))));
            BodyPart arm1 = bpFact.CreateBodyPart(0);
            BodyPart claws1 = bpFact.CreateBodyPart(0);
            toRet.ForceNewOffset(new Vector2(200, 50));
            toRet._DebugSetKeyMap(2);
            toRet.specType = objSpecificType.Human;
            arm1.SetMasterFromMaster(toRet);
            arm1.SutureBodyPart(claws1, BpDirection.West);
            toRet.AddBodyPart(arm1, BpDirection.North);

            return toRet;
        }
    }
}
