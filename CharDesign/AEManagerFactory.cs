using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntSys;

namespace FactSys
{
    class AEManagerFactory
    {
        AEPackFactory AEPFact;

        
         private static AEManagerFactory instance;
         private AEManagerFactory()
         {
            
         }
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
             if(AEPFact == null)
                 AEPFact = AEPackFactory.Instance;

             AEManager toRet = new AEManager();

             switch (loadOut)
             {
                 case 0:
                     //toRet.channeledAbilities.Add(AEPFact.CreateAEPack(3));
                    // toRet.channeledAbilities.Add(AEPFact.CreateAEPack(5));
                     toRet.channeledAbilities.Add(AEPFact.CreateAEPack(0));
                    // toRet.channeledAbilities.Add(AEPFact.CreateAEPack(4));
                    // toRet.initzAbilities.Add(AEPFact.CreateAEPack(1));
                    // toRet.timerAbilities.Add(AEPFact.CreateAEPack(2));
                     
                     //toRet.channeledAbilities.Add(AEPFact.CreateAEPack(0));
                     return toRet;


                 case 1:
                    // toRet.channeledAbilities.Add(AEPFact.CreateAEPack(5));
                     toRet.channeledAbilities.Add(AEPFact.CreateAEPack(0));
                    // toRet.initzAbilities.Add(AEPFact.CreateAEPack(1));
                    // toRet.timerAbilities.Add(AEPFact.CreateAEPack(2));
                     toRet.channeledAbilities.Add(AEPFact.CreateAEPack(3));
                     return toRet;




             }

             return null;
         }



    }
}
