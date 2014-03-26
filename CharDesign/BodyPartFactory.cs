using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BodyParts;
using EntSys;

namespace FactSys
{
    class BodyPartFactory
    {
        AEManagerFactory AEMangFact = AEManagerFactory.Instance;
        ColiSys.ShapeGenerator sgen = ColiSys.ShapeGenerator.Instance;
        
         private static BodyPartFactory instance;
         private BodyPartFactory() { }
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
             {
                 case 0:
                     bpc.regPacks.Add(10);//wings!
                     bpc.shape = new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.Square,new Structs.S_XY(10,10)));
                     bpc.offsetToMaster = new Structs.S_XY(-10, 0);
                     toRet = new BodyPart(bpc);
                     AEManager abMang = AEMangFact.CreateAEManager(0);                     
                     toRet.InsertAEManager(abMang);
                     return toRet;

                     
                 default:


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
