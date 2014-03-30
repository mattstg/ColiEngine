using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BodyParts;

namespace EntSys
{
    public class AEManager
    {
        public List<AEPack> initzAbilities;
        public List<AEPack> channeledAbilities;
        public List<AEPack> timerAbilities;
        public List<AEPack> coliAbilities;
        float efficiency;
        BodyPart master;


        public AEManager()
        {
            initzAbilities = new List<AEPack>();
            channeledAbilities = new List<AEPack>();
            timerAbilities = new List<AEPack>();
            coliAbilities = new List<AEPack>();
            efficiency = 1f;


        }



        public void Update(float rt, Vector2 aimer)
        {
            foreach (AEPack aep in timerAbilities)
                if (aep.UpdateTimer(rt))
                    aep.ActivateAbility(aimer);
        }

        public bool Ping(int triggerID)
        {
            
            foreach (AEPack aep in channeledAbilities)
                if(aep.triggerID[0] == triggerID)                
                    return true;
                

            return false;

        }

        public void LinkBody(BodyParts.BodyPart summonerMaster)
        {
            master = summonerMaster;
            foreach(AEPack ae in initzAbilities)
                ae.LinkBody(summonerMaster);
            foreach(AEPack ae in channeledAbilities)
                ae.LinkBody(summonerMaster);
            foreach(AEPack ae in timerAbilities)
                ae.LinkBody(summonerMaster);
            foreach(AEPack ae in coliAbilities)
                ae.LinkBody(summonerMaster);
        }

        public void SetEffRating(float eff)
        {
            this.efficiency = eff;
        }


        /// <summary>
        /// This stage calculates how much energy can be divided to each AEPack
        /// </summary>
        /// <param name="triggerID"></param>
        /// <param name="energyTransfered"></param>
        /// <returns></returns>
        public long ChannelAbility(int triggerID, long energyTransfered)
        {
            int count = 0;
            long taxesToRet = ((long)(energyTransfered*(1-efficiency)));
            energyTransfered -= taxesToRet;
            
            long energyLeftOver = 0;
            foreach (AEPack aep in channeledAbilities)
                if (aep.triggerID[0] == triggerID)
                    count++;

            //after recieving count, divide and transfer energy
            if (count != 0)
            {
                foreach (AEPack aep in channeledAbilities)
                    if (aep.triggerID[0] == triggerID)                                           
                        energyLeftOver += aep.Channel(energyTransfered / count);

                return energyLeftOver;

            }
            
            //Console.Out.WriteLine("WEIRD ERROR CHNL");
            return 0;
        }

        public void TriggerChannelAbility(int triggerID, Vector2 aimer)
        {
            foreach (AEPack aep in channeledAbilities)
                if (aep.triggerID[0] == triggerID)
                    aep.ActivateAbility(aimer);


        }
    }
}

namespace EntSys
{
    public enum AEPackType { channel, timer, init, coli }
    



    public class AEPack
    {
        AEPackType type;
        public int[] triggerID;
        Global.Timers timer;
        long energyStored;        
        long energyStoreMax;
        BodyParts.BodyPart Summoner;
        bool channelTriggerWhenFull;
        /// <summary>
        /// Link to ability that is called. Requires body that is calling it, Vector Magnitude location of aimer,and AEPack surronding it for the energystored
        /// </summary>
        public Func<BodyParts.BodyPart, Vector2, AEPack, AERetType> Ability;

        /// <summary>
        /// uses energy stored in the AEPack, returns amount up to amount stored, use 99999... if wanna use all
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public long UseEnergy(long amount)
        {
            long toRet;
            if (amount > energyStored)
            {
                toRet = energyStored;
                energyStored = 0;
            }
            else
            {
                toRet = amount;
                energyStored -= amount;

            }
            return toRet;

        }

        public AEPack(AEPackType type, int[] triggerID, Global.Timers timer, long estore, long emax, bool trigfull, Func<BodyParts.BodyPart, Vector2, AEPack, AERetType> ab)
        {
            this.type = type;
            this.triggerID = triggerID;
            this.timer = timer;
            this.energyStored = estore;
            this.energyStoreMax = emax;
            this.channelTriggerWhenFull = trigfull;
            
            Ability = ab;

        }

        public void LinkBody(BodyParts.BodyPart summonerMaster)
        {
            Summoner = summonerMaster;
        }
        
        /// <summary>
        /// Default test cnstr, never use legitly
        /// </summary>
        public AEPack() 
        {
            
            type = AEPackType.init;
            triggerID = new int[] { 0, 0 };
            timer = new Global.Timers(5000);
            energyStored = 0;
            energyStoreMax = 500;
            Ability = _testFunc;
            Summoner = new BodyParts.BodyPart();
        }

        private AERetType _testFunc(Body summoner, Vector2 aimerMag, AEPack thisPack)
        {
            Console.Out.WriteLine("Ability Summoned!");
            return new AERetType();
        }


        public bool UpdateTimer(float rt)
        {
            timer.Tick(rt);            
            return timer.ready;
        
        }
        public void ActivateAbility(Vector2 aimer)
        {
           
            Ability(Summoner, aimer,this);
            if (energyStored == energyStoreMax)
                Console.Out.WriteLine("Released fully charged ability!");
            else
                Console.Out.WriteLine("Released Ability at " + energyStored);

            timer.Dec(true);  //use all resources
            energyStored = 0;


        }

        

        public long Channel(long EnergyTransfered)
        {

            energyStored += EnergyTransfered;


            
            //Console.Out.Write(energyStored);

            if (energyStoreMax < energyStored)
            {
                long toRet = energyStored - energyStoreMax;
                energyStored = energyStoreMax;
                Console.Out.WriteLine("FULLY CHARGED!");
                if (channelTriggerWhenFull)
                {
                    Console.Out.WriteLine("and firing!!");
                    ActivateAbility(Summoner.aimer);
                }
                return toRet;

            }
            //Console.Out.WriteLine("CHARGING " + energyStored + " / " + energyStoreMax);
            return 0;
        }


    }
}
