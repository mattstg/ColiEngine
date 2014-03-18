using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EntSys
{
    class AEManager
    {
        public List<AEPack> initzAbilities;
        public List<AEPack> channeledAbilities;
        public List<AEPack> timerAbilities;
        public List<AEPack> coliAbilities;
        float efficiency;


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

        public AEManager Ping(int triggerID,float efficiency)
        {
            
            foreach (AEPack aep in timerAbilities)
                if(aep.triggerID[0] == triggerID)
                {
                    this.efficiency = efficiency;
                    return this;
                }

            return null;

        }

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
                    energyLeftOver += aep.Channel(energyTransfered / count);

                return energyLeftOver;

            }
            
            Console.Out.WriteLine("WEIRD ERROR CHNL");
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
    



    class AEPack
    {
        AEPackType type;
        public int[] triggerID;
        Global.Timers timer;
        long energyStored;
        long energyStoreMax;
        Body Summoner;
        /// <summary>
        /// Link to ability that is called. Requires body that is calling it, Vector Magnitude location of aimer,and AEPack surronding it for the energystored
        /// </summary>
        public Func<Body, Vector2, AEPack, AERetType> Ability;


        public AEPack(AEPackType type, int[] triggerID, Global.Timers timer, long estore, long emax, Body thisBody, Func<Body, Vector2, AEPack, AERetType> ab)
        {
            this.type = type;
            this.triggerID = triggerID;
            this.timer = timer;
            this.energyStored = estore;
            this.energyStoreMax = emax;
            Summoner = thisBody;
            Ability = ab;

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
            Summoner = new Body();
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
            timer.Dec(true);
            _testFunc(Summoner, aimer, this);

        }

        

        public long Channel(long EnergyTransfered)
        {
            energyStored += EnergyTransfered;
            if (energyStoreMax < energyStored)
            {
                long toRet = energyStored - energyStoreMax;
                energyStored = energyStoreMax;
                return toRet;

            }
            return 0;
        }


    }
}
