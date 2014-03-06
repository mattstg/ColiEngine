using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntSys;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Structs;
//addition of all mass!
//When creating new bodyParts, please ensure it has an Update

namespace BodyParts
{
    public struct BodyPulse
    {
        long energy;
        int trigger;        

        public BodyPulse split(int splitBy)
        {
            BodyPulse bp = new BodyPulse();
            bp.energy = energy / splitBy;
            bp.trigger = trigger;
            return bp;
        }
    }
    public enum FuncPulseType
    {
       ///<summary>Cast Function as INT </summary>
        getTotalMass,
        ///<summary>Cast as anything Nullable (obj, int, w/e) </summary>
       MovePartsBy,
        ///<summary>Cast as Bool </summary>
        CheckColiInDir,
        ///<summary>Cast as anything Nullable (obj, int, w/e) </summary>
        PassBodyPulse,
        ///<summary>Cast as FeedbackPulse </summary>
        PassInput,
        ///<summary>Cast as Bool </summary>
        CollectAllParts,
        ///<summary>Cast as dont care </summary>
        Update

    }
    public struct FuncPulse
    {
        public FuncPulseType funcCalling;
        public S_XY byOffest;
        public VagueObject coliObj;
        public List<BodyPart> coliParts;
        public KeyboardState keyState;
        public BodyPulse pulse;
        public float rt;


    }
    public struct FeedbackPulse
    {
        public int TotalWeight;


        public static FeedbackPulse operator +(FeedbackPulse v1, FeedbackPulse v2)
        {
            FeedbackPulse fp = new FeedbackPulse();
            fp.TotalWeight = v1.TotalWeight + v2.TotalWeight;
           return fp;

        }
    }

    public class BodyPart : Body
    {
        
        List<BodyPartConnection> connecters;
        ColiSys.TestContent tc;
        public BpConstructor bpDNA;


        //given a bodypart to clone from
        public BodyPart(BpConstructor bpC)
        {
            bpDNA = bpC;
            tc = ColiSys.TestContent.Instance;
            SetEntShape(bpC.shape);
            AE = new ActionEvent(new VagueObject(this));
            foreach (int i in bpC.regPacks)            
                AE.RegAbilityPack(i);
            connecters = new List<BodyPartConnection>();
            base.ForceCnstr(null);
            specType = objSpecificType.BodyPart;

        }

        public void SetMaster(BodyMechanics master)
        {
            Master = master;
        }




        // default made without bodyPartDesigner, makes wings
        public BodyPart(BodyMechanics master, EntSys.DNA dna)
        {
            tc = ColiSys.TestContent.Instance;  
            SetEntShape(DefaultShapeGen());           
            AE = new ActionEvent(new VagueObject(this));
            AE.RegAbilityPack(10);
            connecters = new List<BodyPartConnection>();
            base.ForceCnstr(dna);
            Master = master;
            offset = master.offsetCopy;
            offset.x -= 15;
            rawOffSet.X = offset.x;
            rawOffSet.Y = offset.y;
            specType = objSpecificType.BodyPart;

            
        }

        public ColiSys.Hashtable DefaultShapeGen()
        {

            ColiSys.Node nodey2 = new ColiSys.Node(10, 16);
            ColiSys.Node nodey1 = new ColiSys.Node(0, 5, nodey2, null);
            ColiSys.Node nodex2 = new ColiSys.Node(65, 80, null, nodey1);
            ColiSys.Node nodex1 = new ColiSys.Node(0, 15, nodex2, nodey1);


            LoadTexture(tc.dirt, Microsoft.Xna.Framework.Color.GhostWhite);

            return new ColiSys.Hashtable(nodex1);
        }

        public void ForceCnstr(DNA dna)
        {
           
            connecters = new List<BodyPartConnection>();
            base.ForceCnstr(dna);
        }
        protected void Update(float rt)
        {
            //This should upgrade physical things on body part
            Master.ApplyForce(Enums.Force.ForceTypes.Coli, curForce);
            Master.ApplyForceToVelo(); //any forces applied to bodypart should be moved up to master
            curForce = new Vector2(0,0);

        }
        //TakeDamage(float, DamageType, dir)
        //
        public void UnlockAllConnections()
        {
            foreach (BodyPartConnection bpc in connecters)            
                bpc.Unlock(this);          

        }

        
        public FeedbackPulse SendFuncPulse(FuncPulseType funcPulseType,FuncPulse funcPulse)
        {
            FeedbackPulse toRet = new FeedbackPulse();


            switch (funcPulseType)
            {
                case FuncPulseType.CheckColiInDir:
                    if (funcPulse.coliObj.Coli(nami.MoveTableByOffset(coliBox, funcPulse.byOffest)))
                        funcPulse.coliParts.Add(this);
                    break;
                case FuncPulseType.getTotalMass:
                    toRet.TotalWeight = mass;
                    break;
                case FuncPulseType.MovePartsBy:
                    offset += funcPulse.byOffest;
                    break;
                case FuncPulseType.PassBodyPulse:
                    DecodePulse(funcPulse.pulse);
                    break;
                case FuncPulseType.Update:
                    Update(funcPulse.rt);
                    break;
                case FuncPulseType.PassInput:
                    AE.TriggerEvent(funcPulse.keyState);
                    break;
                case FuncPulseType.CollectAllParts:
                    funcPulse.coliParts.Add(this);
                    break;
                default:
                    Console.Out.WriteLine("error, unhandled funcPulseType");
                    break;
            }


            toRet += _SendPulseToEachBp(funcPulseType, funcPulse);

            return toRet;
        }







        

        

        public void SutureBodyPart()
        {
           // BodyPartConnection bp = new BodyPartConnection();
           // bp.SealConnection(

        }

        private FeedbackPulse _SendPulseToEachBp(FuncPulseType funcPulseType, FuncPulse funcPulse)
        {
            FeedbackPulse fp = new FeedbackPulse();
            foreach (BodyPartConnection bpc in connecters)
                fp += bpc.SendPulse(this, funcPulseType,funcPulse);
            return fp;

        }

        public int getTotalMass()
        {
            int toRet = this.mass;
            foreach (BodyPartConnection bpc in connecters)
                toRet += bpc.getTotalMass(this);
            return toRet;

        }

        public void Input(KeyboardState ks)
        {
            AE.TriggerEvent(ks);
            foreach (BodyPartConnection bpc in connecters)
                bpc.Input(this,ks);

        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////should be done
        public void CheckColi(Structs.S_XY byOffset, VagueObject coliObj, List<BodyPart> coliParts)
        {
            //check if coli has happened if move by this ammount, if yes, add itself to the list of parts that have colided this tick
            if (coliObj.Coli(nami.MoveTableByOffset(coliBox, byOffset)))
                coliParts.Add(this);
            foreach (BodyPartConnection bpc in connecters)
                bpc.CheckColi(this, byOffset, coliObj, coliParts);
        }

        public void MovePartBy(Structs.S_XY moveBy)
        {
            offset += moveBy;
            foreach (BodyPartConnection bpc in connecters)
                bpc.MovePartBy(this, moveBy);
        }

        public void Recieve(BodyPulse bp)
        {
            bool hasChild = false;
            DecodePulse(bp);
            foreach (BodyPartConnection bpc in connecters)
            {
                hasChild = true;
                bpc.Send(this, bp.split(connecters.Count));
            }
            if (!hasChild)
            {
                //Need to reverse the pulse? or pulse just ends here... hmm

            }

        }


        public void DecodePulse(BodyPulse bp)
        {


        }
       
    }
}
