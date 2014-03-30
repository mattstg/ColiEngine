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
        /// 0 - channel, 1 - init, 2 - timer, 3 grow part south, 4-7(triggers 4-7) basic dir flight, 8 breakpoint
        /// 
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
                     toRet = new AEPack(AEPackType.channel, new int[] { 4, 0 }, new Global.Timers(500), 0, 500, true, _TestFlightUp);
                     break;
                 case 5: //basic directional flight
                     toRet = new AEPack(AEPackType.channel, new int[] { 5, 1 }, new Global.Timers(200), 0, 500, true, _TestFlightRight);
                     break;
                 case 6: //basic directional flight
                     toRet = new AEPack(AEPackType.channel, new int[] { 6, 1 }, new Global.Timers(200), 0, 500, true, _TestFlightDown);
                     break;
                 case 7: //basic directional flight
                     toRet = new AEPack(AEPackType.channel, new int[] { 7, 1 }, new Global.Timers(200), 0, 500, true, _TestFlightLeft);
                     break;
                 case 8: //Adds the BREAKPOINT Trigger, set trigger input to 1
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

         

         private AERetType _TestGrowPartS(BodyPart summoner, Vector2 aimerMag, AEPack thisPack)
         {
             Console.Out.WriteLine("GROW!");
             DNA dna = dnaFact.GenerateDNA(DNAType.BodyPart,3);
             BodyPart bp = new BodyPart(dna);
             summoner.GrowBodyPart(bp,BpDirection.South);
             
                      
            
             return new AERetType();
         }

        /////////////////////////////////FLIGHT ABILITIES /////////////////////
      
         private AERetType _TestFlightUp(BodyPart summoner, Vector2 aimerMag, AEPack thisPack)
         {
             //summoner.ApplyForce(Enums.Force.ForceTypes.Internal, new Vector2(0, -50000));
             summoner.ApplyForce(Enums.Force.ForceTypes.Internal, new Vector2(0, -thisPack.UseEnergy(9999999)));
             return new AERetType();
         }
         private AERetType _TestFlightDown(BodyPart summoner, Vector2 aimerMag, AEPack thisPack)
         {
             
             summoner.ApplyForce(Enums.Force.ForceTypes.Internal, new Vector2(0, thisPack.UseEnergy(9999999)));
             return new AERetType();
         }
         private AERetType _TestFlightLeft(BodyPart summoner, Vector2 aimerMag, AEPack thisPack)
         {
             
             summoner.ApplyForce(Enums.Force.ForceTypes.Internal, new Vector2(-thisPack.UseEnergy(9999999),0));
             return new AERetType();
         }
         private AERetType _TestFlightRight(BodyPart summoner, Vector2 aimerMag, AEPack thisPack)
         {
             
             summoner.ApplyForce(Enums.Force.ForceTypes.Internal, new Vector2(thisPack.UseEnergy(9999999),0));
             return new AERetType();
         }

         

        

 

         
        



    }
}
