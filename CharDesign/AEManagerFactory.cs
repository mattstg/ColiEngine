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


         

         public AEManager CreateAEManager(int loadOut)
         {
             AEManager toRet = new AEManager();

             switch (loadOut)
             {
                 case 0:
                     toRet.channeledAbilities.Add(AEPFact.CreateAEPack(0));
                     toRet.initzAbilities.Add(AEPFact.CreateAEPack(1));
                     toRet.timerAbilities.Add(AEPFact.CreateAEPack(2));
                     return toRet;




             }

             return null;
         }



    }
}
