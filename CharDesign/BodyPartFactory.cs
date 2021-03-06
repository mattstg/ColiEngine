﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BodyParts;
using EntSys;

namespace FactSys
{
    class BodyPartFactory
    {
        AEManagerFactory AEMangFact;
        ColiSys.ShapeGenerator sgen;
        ColiSys.NodeManipulator Nami;
        MaterialFactory forge;


         private static BodyPartFactory instance;
         private BodyPartFactory()
         {
             AEMangFact = AEManagerFactory.Instance;
             sgen = ColiSys.ShapeGenerator.Instance;
             Nami = ColiSys.NodeManipulator.Instance;
             forge = MaterialFactory.Instance;
         
         }
         public static BodyPartFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BodyPartFactory();
                }
                return instance;
            }
        }

        /// <summary>
        /// Currently only 0 exists
        /// </summary>
        /// <param name="loadOut"></param>
        /// <returns></returns>
         public BodyPart CreateBodyPart(int loadOut)
         {
             BodyPart toRet;
             BpConstructor bpc = new BpConstructor();
             bpc.regPacks = new List<int>();
             bpc.sutureSpots = new List<ColiSys.Hashtable>();

             switch (loadOut)
             {/*
                 case 0:
                     bpc.regPacks.Add(10);//wings!
                     bpc.shape = new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.Square,new Structs.S_XY(10,10)));
                    // bpc.offsetToMaster = new Structs.S_XY(-10, 0);
                     bpc.sutureSpots = new List<ColiSys.Hashtable>(){null,null,null,null};
                     bpc.sutureSpots[3] = (new ColiSys.Hashtable(Nami.MoveTableByOffset(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(1, 1)),new Structs.S_XY(0,5))));
                     bpc.sutureSpots[1] = (new ColiSys.Hashtable(Nami.MoveTableByOffset(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(1, 1)), new Structs.S_XY(10, 5))));
                     
                     bpc.sutureSpots[0] = (new ColiSys.Hashtable(Nami.MoveTableByOffset(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(1, 1)),new Structs.S_XY(5,0))));
                     bpc.sutureSpots[2] = (new ColiSys.Hashtable(Nami.MoveTableByOffset(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(1, 1)), new Structs.S_XY(5, 10))));
                     DNA dna = forge.CreateMaterial(0);
                     dna.bpC = bpc;                     
                     toRet = new BodyPart(dna);
                     AEManager abMang = AEMangFact.CreateAEManager(0);                     
                     toRet.InsertAEManager(abMang);
                     return toRet;

                     */
                 default:
                     return null;

                     break;

             }

             return null;
         }


        /// <summary>
        /// 0 - normal 10by10 wings
        /// 1 - channeled down growth
        /// </summary>
        /// <param name="loadOut"></param>
        /// <returns></returns>
         public BpConstructor GenerateBodyPartC(int loadOut)
         {
             
             BpConstructor bpc = new BpConstructor();
             bpc.regPacks = new List<int>();
             bpc.sutureSpots = new List<ColiSys.Hashtable>();

             switch (loadOut)
             {
                 case 0:
                     bpc.regPacks.Add(10);//wings!
                     bpc.shape = new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.Square,new Structs.S_XY(10,10)));
                    // bpc.offsetToMaster = new Structs.S_XY(-10, 0);
                     bpc.sutureSpots = new List<ColiSys.Hashtable>(){null,null,null,null};
                     bpc.sutureSpots[3] = (new ColiSys.Hashtable(Nami.MoveTableByOffset(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(1, 1)),new Structs.S_XY(0,5))));
                     bpc.sutureSpots[1] = (new ColiSys.Hashtable(Nami.MoveTableByOffset(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(1, 1)), new Structs.S_XY(10, 5))));
                     
                     bpc.sutureSpots[0] = (new ColiSys.Hashtable(Nami.MoveTableByOffset(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(1, 1)),new Structs.S_XY(5,0))));
                     bpc.sutureSpots[2] = (new ColiSys.Hashtable(Nami.MoveTableByOffset(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(1, 1)), new Structs.S_XY(5, 10))));
                     bpc.aeMang = AEMangFact.CreateAEManager(0);                     

                     
                     return bpc;

                 case 1:
                     //bpc.regPacks.Add(10);//wings!
                     bpc.shape = new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(20, 10)));
                     // bpc.offsetToMaster = new Structs.S_XY(-10, 0);
                     bpc.sutureSpots = new List<ColiSys.Hashtable>() { null, null, null, null };
                     bpc.sutureSpots[3] = (new ColiSys.Hashtable(Nami.MoveTableByOffset(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(1, 1)), new Structs.S_XY(0, 5))));
                     bpc.sutureSpots[1] = (new ColiSys.Hashtable(Nami.MoveTableByOffset(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(1, 1)), new Structs.S_XY(10, 5))));

                     bpc.sutureSpots[0] = (new ColiSys.Hashtable(Nami.MoveTableByOffset(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(1, 1)), new Structs.S_XY(10, 0))));
                     bpc.sutureSpots[2] = (new ColiSys.Hashtable(Nami.MoveTableByOffset(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(1, 1)), new Structs.S_XY(10, 10))));
                     bpc.aeMang = AEMangFact.CreateAEManager(1);


                     return bpc;

                     
                 default:
                     return null;

                     break;

             }

             return null;
         }
        /*
        /// <summary>
        /// Calls a mix hashtable from bodyshape maker and loadout from AEManager and merges them into a new bp
        /// </summary>
        /// <param name="hashTableLoadOut"></param>
        /// <param name="AEManagerLoadout"></param>
        /// <returns></returns>
         public BodyPart CreateBodyPart(int hashTableLoadOut, int AEManagerLoadout)
         {
             BodyPart toRet;
             BpConstructor bpc = new BpConstructor();
             bpc.regPacks = new List<int>();
             bpc.sutureSpots = new List<ColiSys.Hashtable>();

             switch (loadOut)
             {
                 case 0:
                    // bpc.regPacks.Add(10);//wings!
                     bpc.shape = new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(30, 12)));
                     bpc.offsetToMaster = new Structs.S_XY(0, 0);
                     toRet = new BodyPart(bpc);
                     AEManager abMang = AEMangFact.CreateAEManager(AEManagerLoadout);
                     toRet.InsertAEManager(abMang);
                     return toRet;


                 default:


                     break;

             }

             return null;
         }*/


    }
}
