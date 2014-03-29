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
        MaterialFactory forge = MaterialFactory.Instance;
        DNAFactory dnaFact = DNAFactory.Instance;

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
            //DNA matDNA = forge.CreateMaterial(0); //creates incomplete dna
            BpConstructor bpc = new BpConstructor();
            bpc.isTopLevel = false;
            bpc.shape = new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.Human, new Structs.S_XY(20, 20)));
            bpc.sutureSpots = new List<Hashtable>(){null,null,null,null};
            ColiSys.Hashtable toAdd =  new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(1,1)));
            toAdd.MoveTableByOffset(new Structs.S_XY(0, 10));

            toRet.SetEntShape(new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.Human, new Structs.S_XY(20, 20))));
            //BodyPart arm1 = bpFact.CreateBodyPart(0);
            //BodyPart claws1 = bpFact.CreateBodyPart(0);
            toRet.ForceNewOffset(new Vector2(50,50));
            //toRet._DebugSetKeyMap(1);
            toRet.specType = objSpecificType.Human;
            //arm1.SetMasterFromMaster(toRet);
            //arm1.SutureBodyPart(claws1, BpDirection.East);
           // toRet.AddBodyPart(arm1, BpDirection.East);
            
            return toRet;

        }

        private HumanPlayer _GenHuman1()
        {
            HumanPlayer toRet = new HumanPlayer(dnaFact.GenerateDNA(DNAType.Human,0));

            BodyPart arml1 = new BodyPart(dnaFact.GenerateDNA(DNAType.BodyPart, 3));
            BodyPart arml2 = new BodyPart(dnaFact.GenerateDNA(DNAType.BodyPart, 3));
            BodyPart arml3 = new BodyPart(dnaFact.GenerateDNA(DNAType.BodyPart, 3));
            BodyPart armr1 = new BodyPart(dnaFact.GenerateDNA(DNAType.BodyPart, 3));
            BodyPart armr2 = new BodyPart(dnaFact.GenerateDNA(DNAType.BodyPart, 3));
            BodyPart armr3 = new BodyPart(dnaFact.GenerateDNA(DNAType.BodyPart, 3));
            toRet.GrowBodyPart(arml1, BpDirection.West);
            arml1.GrowBodyPart(arml2,BpDirection.West);
            arml2.GrowBodyPart(arml3, BpDirection.South);

            toRet.GrowBodyPart(armr1, BpDirection.East);
            armr1.GrowBodyPart(armr2, BpDirection.East);
            armr2.GrowBodyPart(armr3, BpDirection.South);

            toRet.specType = objSpecificType.Human;
           

            return toRet;
        }
    }
}
