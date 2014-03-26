﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BodyParts;
using Microsoft.Xna.Framework;

namespace EntSys
{
    /// <summary>
    /// The MasterBody Layer lies above the BM, it contains multiple body's and is in charge of managing them
    /// </summary>
    public class MasterBody : Body
    {
        /// <summary>
        /// alerts system new parts were added, does calculations appropriatly
        /// </summary>
        public bool RegisterNewParts;
        /// <summary>
        /// List of body parts, total of 4 for each direction (NESW), Possible values are NULL!
        /// </summary>
        protected List<BodyPart> bodyPartList;
        Dictionary<int, List<AEManager>> MasterChannelList;
        protected List<AEManager> MasterTransferList;


        public void AddBodyPart(BodyPart bpToAdd, BpDirection dir)
        {
            //shouldnt this function be setting master and etc?
            int index = (int)dir;
            _ReplaceBodyPart(dir);
            bodyPartList[index] = bpToAdd;
            
            RegisterNewParts = true;
            _UpdateBodyPartRelatedInfo();
        }

       

       




        public MasterBody()
        {

            ForceCnstr(null);
        }

        public MasterBody(DNA dna)
        {
            ForceCnstr(dna);

        }

        public void _UpdateBodyPartRelatedInfo()
        {
            _GetCombinedWeight();
        }

        public void _GetCombinedWeight()
        {
            totalMass = this.mass;
            foreach (BodyPart bp in bodyPartList)
            {
                if (bp != null)
                {
                    FuncPulse fp = new FuncPulse();

                    FeedbackPulse result = bp.SendFuncPulse(FuncPulseType.getTotalMass, fp);
                    totalMass += result.TotalWeight;
                    //bp.UnlockAllConnections();
                    //totalMass += bp.getTotalMass();
                }
            }
            //totalMass = this.mass;
            this.CombinedMass = totalMass;
            EI.totalMass = totalMass;


        }

        protected void ResetAllBodyPartCWM()
        {
            foreach (BodyPart bp in bodyPartList)
            {
                if (bp != null)
                {
                    //bp.UnlockAllConnections();
                    //bp.Update(rt);
                    FuncPulse fp = new FuncPulse();

                    bp.SendFuncPulse(FuncPulseType.ResetCVM, fp);
                }
            }

        }



        /// <summary>
        /// CHANGE THIS TO USE ONLY TRIGGERS! Human or AI layer will alter triggers
        /// </summary>
        public void Input()
        {
            /*
            if (!inputShutDown)
            {
                KeyboardState ks = Keyboard.GetState();
                if (Keyboard.GetState().GetPressedKeys().Length > 0)
                    AE.TriggerEvent(ks);

                foreach (BodyPart bp in bodyParts)
                    bp.Input(ks);
            }*/


        }

       
        public void MoveAllBy(Structs.S_XY modOffset)
        {
            offset += modOffset;
            foreach (BodyPart bp in bodyPartList)
            {
                if (bp != null)
                {
                    FuncPulse fp = new FuncPulse();
                    fp.byOffset = modOffset;
                    bp.SendFuncPulse(FuncPulseType.MovePartsBy, fp);
                    // bp.MovePartBy(modOffset); 
                }
            }
        }


        
        protected void UpdateBodyParts(float rt)
        {

            foreach (BodyPart bp in bodyPartList)
            {
                if (bp != null)
                {
                    //bp.UnlockAllConnections();
                    //bp.Update(rt);
                    FuncPulse fp = new FuncPulse();
                    fp.rt = rt;
                    bp.SendFuncPulse(FuncPulseType.Update, fp);
                }
            }

        }

        public void ForceCnstr(DNA dna)
        {

            RegisterNewParts = false;
            bodyPartList = new List<BodyPart>(){null,null,null,null};
            MasterTransferList = new List<AEManager>();
            MasterChannelList = new Dictionary<int, List<AEManager>>();
            base.ForceCnstr(null);
        }


        public void Update(float rt)
        {
            ChannelAbilities(rt);
            UpdateBodyParts(rt);


        }


        /// <summary>
        /// Retrieve all body parts and store as Vague Object list
        /// </summary>
        /// <returns></returns>
        public List<VagueObject> GetAllParts()
        {
            List<VagueObject> toRet = new List<VagueObject>();
            FuncPulse fp = new FuncPulse();
            fp.coliParts = new List<BodyPart>();
            foreach (BodyPart b in bodyPartList)
                if (b != null)                
                  b.SendFuncPulse(FuncPulseType.CollectAllParts, fp);


            //the return coliParts can be used as a list of all parts
            foreach (BodyPart b in fp.coliParts)
            {
                VagueObject vo = new VagueObject(b);
                toRet.Add(vo);
            }

            return toRet;
        }
        

        /// <summary>
        /// Fills master transfer list, should be done externally, not in the function, 
        /// especailly since its called "Get"
        /// </summary>
        /// <param name="id"></param>
        protected List<AEManager> _GetAbilityManagerListsWithID(int id)
        {
            List<AEManager> toRet = new List<AEManager>();
            foreach (BodyPart bp in bodyPartList)
            {
                if (bp != null)
                {
                    FuncPulse fp = new FuncPulse();
                    fp.AbilityManagerList = new List<AEManager>();
                    fp.Int = id;
                    fp.Eff = new List<float>();
                    bp.SendFuncPulse(FuncPulseType.PingForAbilityIDs, fp);
                    foreach (AEManager ae in fp.AbilityManagerList)
                        toRet.Add(ae);
                    //MasterTransferList now contains all the ones by id it needs
                }
            }
            return toRet;
        }


        protected void Input(List<int> triggers, Vector2 aimer )
        {
            List<int> keysToRemove = new List<int>();

            foreach(int i in triggers)
            {
                if (!MasterChannelList.ContainsKey(i))
                {
                    List<AEManager> t = _GetAbilityManagerListsWithID(i);
                    if(t != null && t.Count > 0)
                        MasterChannelList.Add(i, t);

                }
            }

            foreach (KeyValuePair<int, List<AEManager>> entry in MasterChannelList)
            {
                if (!triggers.Contains(entry.Key))
                {
                    foreach (AEManager ae in entry.Value)
                        ae.TriggerChannelAbility(entry.Key,aimer);

                    keysToRemove.Add(entry.Key);

                }
            }

            foreach(int i in keysToRemove)
                MasterChannelList.Remove(i); //careful may cause errors
    


        }

        protected void ChannelAbilities(float rt)
        {
            if (MasterChannelList == null || MasterChannelList.Count == 0)
                return;

            int totalChannelRate = 10;
            int totalEnergy = 100;
            int chnlRate;

            chnlRate = (int)((totalChannelRate * rt) / MasterChannelList.Count);

            List<AEManager> toChnl = new List<AEManager>();
            foreach (List<AEManager> aeL in MasterChannelList.Values)
               foreach (AEManager ae in aeL)
                    if (!toChnl.Contains(ae))
                        toChnl.Add(ae);
                    else
                        Console.Out.WriteLine("Testing123, should nt happen but could? (multiple triggers-1 ability)");

            chnlRate = totalChannelRate / toChnl.Count;
            //WARNING MORE ABILITIES THAN CHANNEL RAtE WILL MEAN NO TRANSFER AT ALL
            //WARNING MULTIPLE TRIGGERS TO ONE SPELL WILL...WELL EXTRA CHANNEL IT? same total tho... 
            foreach (AEManager ae in toChnl)
                foreach (int i in MasterChannelList.Keys)
                    ae.ChannelAbility(i, chnlRate);


        }


        public void Draw()
        {
            foreach (BodyPart bp in bodyPartList)
            {
                if (bp != null)
                {
                    FuncPulse fp = new FuncPulse();
                    bp.SendFuncPulse(FuncPulseType.Draw, fp);
                    //bp.UnlockAllConnections();
                }
            }
            base.Draw();//pass draw down to sprite class
        }

        public void _ReplaceBodyPart(BpDirection dir)
        {
            //Part internally replaced? maybe bool here
            //&*&
            //ReplacementNegatesDestroyEffect
            _DestoryBodyPart(dir);
        }

        public void _DestoryBodyPart(BpDirection dir)
        {
            //&*&
            //DestroyEffectsNegated
            //Part goes boom!
        }




    }
}
