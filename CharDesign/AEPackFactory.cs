using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntSys;
using Microsoft.Xna.Framework;

namespace FactSys
{
    class AEPackFactory
    {
         private static AEPackFactory instance;
         private AEPackFactory() { }
         public static AEPackFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AEPackFactory();
                }
                return instance;
            }
        }


        /// <summary>
        /// 0 - channel, 1 - init, 2 - timer
        /// </summary>
        /// <param name="loadoutID"></param>
        /// <param name="requestingBody"></param>
        /// <returns></returns>
         public AEPack CreateAEPack(int loadoutID, Body requestingBody)
         {
             AEPack toRet = new AEPack();


             switch (loadoutID)
             {
                 case 0:
                     toRet = new AEPack(AEPackType.channel, new int[] { 0, 0 }, new Global.Timers(5000), 0, 500, requestingBody, _testFunc);
                     break;
                 case 1:
                     toRet = new AEPack(AEPackType.init, new int[] { 0, 0 }, new Global.Timers(5000), 0, 500, requestingBody, _testFunc);
                     break;
                 case 2:
                     toRet = new AEPack(AEPackType.timer, new int[] { 0, 0 }, new Global.Timers(5000), 0, 500, requestingBody, _testFunc);
                     break;
                 default:
                     break;
                    

             }
             return toRet;
         }

         private AERetType _testFunc(Body summoner, Vector2 aimerMag, AEPack thisPack)
         {
             Console.Out.WriteLine("Ability Summoned!");
             return new AERetType();
         }

         
        



    }
}
