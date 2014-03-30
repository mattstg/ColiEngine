using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntSys;
using Microsoft.Xna.Framework;
using BodyParts;


namespace FactSys
{
    class AEPackFactory
    {
        DNAFactory dnaFact;


         private static AEPackFactory instance;
         private AEPackFactory()
         {
             dnaFact = DNAFactory.Instance;
         }
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
        /// 0 - channel, 1 - init, 2 - timer, 3 grow part south, 4 basic dir flight, 5 breakpoint
        /// </summary>
        /// <param name="loadoutID"></param>
        /// <param name="requestingBody"></param>
        /// <returns></returns>
         public AEPack CreateAEPack(int loadoutID)
         {
             AEPack toRet;// = new AEPack();


             switch (loadoutID)
             {
                 case 0:
                     toRet = new AEPack(AEPackType.channel, new int[] { 0, 0 }, new Global.Timers(5000), 0, 500, true, _testFunc);
                     break;
                 case 1:
                     toRet = new AEPack(AEPackType.init, new int[] { 0, 0 }, new Global.Timers(5000), 0, 500, true, _testFunc);
                     break;
                 case 2:
                     toRet = new AEPack(AEPackType.timer, new int[] { 0, 0 }, new Global.Timers(5000), 0, 500, true, _testFunc);
                     break;
                 case 3: //grow body part south
                     toRet = new AEPack(AEPackType.channel, new int[] { 0, 0 }, new Global.Timers(500), 0, 500,true, _TestGrowPartS);
                     break;
                 case 4: //basic directional flight
                     toRet = new AEPack(AEPackType.channel, new int[] { 0, 0 }, new Global.Timers(500), 0, 500,true, _TestFlight);
                     break;
                 case 5: //Adds the BREAKPOINT Trigger, set trigger input to 1
                     toRet = new AEPack(AEPackType.channel, new int[] { 1, 1 }, new Global.Timers(200), 0, 1, true, _BreakpointFunc);
                     break;
                 default:
                     return null;
                     break;
                    

             }
             return toRet;
         }

         private AERetType _testFunc(BodyPart summoner, Vector2 aimerMag, AEPack thisPack)
         {
             //Console.Out.WriteLine("Ability Summoned!");
             return new AERetType();
         }

         private AERetType _BreakpointFunc(BodyPart summoner, Vector2 aimerMag, AEPack thisPack)
         {
             Console.Out.WriteLine("Breakpoint in code forced, Line 84 in AEPackFactory.cs");
             return new AERetType();
         }

         private AERetType _TestFlight(BodyPart summoner, Vector2 aimerMag, AEPack thisPack)
         {
             Console.Out.WriteLine("FLIGHT!");
             return new AERetType();
         }

         private AERetType _TestGrowPartS(BodyPart summoner, Vector2 aimerMag, AEPack thisPack)
         {
             Console.Out.WriteLine("GROW!");
             DNA dna = dnaFact.GenerateDNA(DNAType.BodyPart,3);
             BodyPart bp = new BodyPart(dna);
             summoner.GrowBodyPart(bp,BpDirection.South);
             
                      
            
             return new AERetType();
         }

 

         
        



    }
}
