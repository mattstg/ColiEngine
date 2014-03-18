using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntSys;

namespace FactSys
{
    class AEManagerFactory
    {
        AEPackFactory AEPFact = AEPackFactory.Instance;

        
         private static AEManagerFactory instance;
         private AEManagerFactory() { }
         public static AEManagerFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AEManagerFactory();
                }
                return instance;
            }
        }

         public AEManager CreateAEManager(int loadOut, Body requestingBody)
         {
             AEManager toRet = new AEManager();

             switch (loadOut)
             {
                 case 0:
                     toRet.channeledAbilities.Add(AEPFact.CreateAEPack(0, requestingBody));
                     toRet.initzAbilities.Add(AEPFact.CreateAEPack(1,requestingBody));
                     toRet.timerAbilities.Add(AEPFact.CreateAEPack(2,requestingBody));
                     return toRet;




             }

             return null;
         }



    }
}
