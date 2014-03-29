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
    /// <summary>
    /// Body part direction, NESW 0,1,2,3
    /// </summary>
    public enum BpDirection{
        North = 0,
        East = 1,
        South= 2,
        West = 3


    }


    public class BpConstructor
    {
       // public S_XY offsetToMaster;
        public ColiSys.Hashtable shape;
        /// <summary>
        /// List of  4 Hashtables, North=0,East=1,S=South,W=West 
        /// </summary>
        public List<ColiSys.Hashtable> sutureSpots;
        public List<int> regPacks;
        public bool isTopLevel;
        public int SummonCost;
        public AEManager aeMang;

        public BpConstructor() { }
    }

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
       ///<summary>Returns a FeedBack pulse instead of using fp for now, old function </summary>
        getTotalMass,
        ///<summary>Move all parts by offset, Fill "byOffset" in fp </summary>
       MovePartsBy,
        ///<summary>Check all bp if a colision exists in direction, requires fp: "byOffset,coliObj,coliParts" where coliparts is the list to fill</summary>
        CheckColiInDir,
        ///<summary>Cast as anything Nullable (obj, int, w/e) </summary>
        PassBodyPulse,
        ///<summary>Cast as FeedbackPulse </summary>
        PassInput,
        ///<summary>Cast as Bool </summary>
        CollectAllParts,
        ///<summary>Cast as dont care </summary>
        Update,
        ///<summary>Resets all the CollidedWithMe lists, no fp needed </summary>
        ResetCVM,
        /// <summary>nomnomnomn calls draw for all parts </summary>
        Draw,
        /// <summary> ask to fill a given list-AEW- type by id, Fill fp with Eff=new,AbilityManagerList=new, Int=ID  </summary>
        PingForAbilityIDs

    }

    

    public struct FuncPulse //this could have totally been a generic class probably
    {
        public FuncPulseType funcCalling;
        public S_XY byOffset;
        public VagueObject coliObj;
        public List<BodyPart> coliParts;
        public KeyboardState keyState;
        public BodyPulse pulse;
        public float rt;
        public List<AEManager> AbilityManagerList;
        public int Int;
        public List<float> Eff;


    }
    public struct FeedbackPulse
    {
        public int TotalWeight;


        public static FeedbackPulse operator +(FeedbackPulse v1, FeedbackPulse v2)
        {
            FeedbackPulse fp = new FeedbackPulse();
            fp.TotalWeight = v1.TotalWeight + v2.TotalWeight;
            //combined the rest of the elements! VERY IMPORTANT, or make a list of fb!
           return fp;

        }
    }

    public class BodyPart : Body
    {
        
        List<BodyPartConnection> connecters;
        List<ColiSys.Hashtable> SutureHashtables;
        ColiSys.TestContent tc = ColiSys.TestContent.Instance;
        public BpConstructor bpDNA;
       // S_XY OffsetDifToMaster;
        AEManager AbilityManager;
        ColiSys.Hashtable graphicSkin;
        
       

        //given a bodypart to clone from
        public BodyPart(BpConstructor bpC)
        {
            ForceCnstr(null);
            bpDNA = bpC;            
            SetEntShape(bpC.shape);            
            foreach (int i in bpC.regPacks)            
                AE.RegAbilityPack(i);
           // OffsetDifToMaster = bpC.offsetToMaster;
            SutureHashtables = bpC.sutureSpots;
            LoadTexture(_LoadDefaultSkin());

        }

        public BodyPart(EntSys.DNA dna)
        {
            base.ForceCnstr(dna);
            ForceCnstr(dna);

        }

        public BodyPart()
        { }


        /// <summary>
        /// This is called directly when bodypart is attached to master, not when bodypart is
        /// attached to more parts
        /// </summary>
        /// <param name="master"></param>
        public void SetMasterFromMaster(BodyMechanics master)
        {
            
            Master = master;
            offset = master.offsetCopy;
           // offset += OffsetDifToMaster;
            rawOffSet.X = offset.x;
            rawOffSet.Y = offset.y;          
            
        }




        // default made without bodyPartDesigner, makes wings
        public BodyPart(BodyMechanics master, EntSys.DNA dna)
        {
            ForceCnstr(dna);
            SetEntShape(DefaultShapeGen());           
            AE.RegAbilityPack(10);           
            SetMasterFromMaster(master);
            
            
        }

        private ColiSys.Hashtable _LoadDefaultSkin()
        {

            ColiSys.Hashtable TgraphicSkin = new ColiSys.Hashtable(trueEntShape);
            TgraphicSkin.LoadTexture(tc.sqr, Color.Black);
            return TgraphicSkin;

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

        public void LoadTexture(ColiSys.Hashtable graphicSkin)
        {
            this.graphicSkin = graphicSkin;
        }

        /*
        private void Draw()
        {
            ColiSys.Hashtable toDraw = new ColiSys.Hashtable(graphicSkin);
            toDraw.MoveTableByOffset(offset);
            toDraw.LoadTexture(tc.sqr, Color.Black);
            base.Draw(toDraw);
            //graphicSkin.MoveTableByOffset(offset);
            //base.Draw(graphicSkin);
        }*/

        

        public void ForceCnstr(DNA dna)
        {
            base.ForceCnstr(dna);
            ///
            bpDNA = dna.bpC;
            //SetEntShape(bpDNA.shape);

            if (dna.bpC.isTopLevel)
            {

                AE = new ActionEvent(new VagueObject(this)); 

                foreach (int i in bpDNA.regPacks)
                    AE.RegAbilityPack(i);
                
                
            }
            // OffsetDifToMaster = bpC.offsetToMaster;
            SutureHashtables = bpDNA.sutureSpots;
           // LoadTexture(_LoadDefaultSkin());

            if (dna.bpC.aeMang != null)
                InsertAEManager(dna.bpC.aeMang);

           
            //

            specType = objSpecificType.BodyPart;
            //OffsetDifToMaster = new S_XY();
            connecters = new List<BodyPartConnection>(){null,null,null,null};
            

        }
        protected void Update(float rt)
        {
            //This should upgrade physical things on body part
            Master.ApplyForce(Enums.Force.ForceTypes.Coli, curForce);
            Master.ApplyForceToVelo(); //any forces applied to bodypart should be moved up to master
            curForce = new Vector2(0,0);
            AbilityManager.Update(rt, Master.aimer);

        }
        //TakeDamage(float, DamageType, dir)
        //
        public void UnlockAllConnections()
        {
            foreach (BodyPartConnection bpc in connecters) 
                if(bpc != null)
                    bpc.Unlock(this);          

        }

        
        public FeedbackPulse SendFuncPulse(FuncPulseType funcPulseType,FuncPulse funcPulse)
        {
            FeedbackPulse toRet = new FeedbackPulse();


            switch (funcPulseType)
            {
                case FuncPulseType.CheckColiInDir:
                    if (funcPulse.coliObj.Coli(nami.MoveTableByOffset(coliBox, funcPulse.byOffset)))
                        funcPulse.coliParts.Add(this);
                    break;
                case FuncPulseType.getTotalMass:
                    toRet.TotalWeight = mass;
                    break;
                case FuncPulseType.MovePartsBy:
                    offset += funcPulse.byOffset;
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
                case FuncPulseType.ResetCVM:
                    ResetCVM();
                    break;
                case FuncPulseType.Draw:
                    Draw();
                    break;
                case FuncPulseType.PingForAbilityIDs:
                    AddEfficencyRatingToList(funcPulse.Eff);
                    _RespondToAbilityManagerPing(funcPulse.Int, funcPulse.AbilityManagerList, funcPulse.Eff);
                    break;
                default:
                    Console.Out.WriteLine("error, unhandled funcPulseType");
                    break;
            }


            toRet += SendPulseToEachBp(funcPulseType, funcPulse);

            return toRet;
        }

        /// <summary>
        /// Insert new Ability Manager into bp. Returns old one if one exisits
        /// </summary>
        /// <param name="newManager"></param>
        /// <returns>Returns old AbilityManager or Null</returns>
        public AEManager InsertAEManager(AEManager newManager)
        {
            
            if (AbilityManager != null)
            {
                AEManager toRet = AbilityManager;
                AbilityManager = newManager;
                AbilityManager.LinkBody(this);
                return toRet;
            }
            else
            {
                AbilityManager = newManager;
                AbilityManager.LinkBody(this);
                return null;
            }


        }

        private void AddEfficencyRatingToList(List<float> eff)
        {
            eff.Add(1);

        }

        public void _RespondToAbilityManagerPing(int id, List<AEManager> abList, List<float> eff)
        {

            if (this.AbilityManager.Ping(id))
            {
                if (!abList.Contains(this.AbilityManager))
                {
                    float effRating = 0;
                    foreach (float f in eff)
                        effRating += f;
                    if (eff.Count != 0)
                        effRating /= eff.Count;
                    else
                        effRating = 1; //shouldnt happen if built properly

                    this.AbilityManager.SetEffRating(effRating);
                    abList.Add(this.AbilityManager);
                }
            }

        }

        /// <summary>
        /// Resets the CollidedWithMe list, called when its masters turn to do ColiAndMove
        /// </summary>
        private void ResetCVM()
        {
            hasCollidedWithMe = new List<Material>();
        }


        
       
        

        public void SutureBodyPart(BodyPart otherPart,BpDirection bpDir)
        {
            if ((connecters[(int)bpDir] == null && otherPart.connecters[((int)bpDir + 2) % 4] == null) //spots arnt used
                && (SutureHashtables[(int)bpDir] != null && otherPart.SutureHashtables[((int)bpDir + 2) % 4] != null))
            { //if connect in dir and connector of new part in opp dir are both valid suture spots

                int yMod = 0;
                int xMod = 0;

               if((int)bpDir % 2 == 0)
                   if((int)bpDir == 0)
                       yMod = -1;
                   else
                       yMod = 1;
               else
                   if((int)bpDir == 3)
                       xMod = -1;
                   else
                       xMod = 1;

 

               BodyPartConnection bpc = new BodyPartConnection(this,otherPart);
               otherPart.Master = Master; 

               S_Box thisConnectorBox = nami.NodetoBox(this.SutureHashtables[(int)bpDir].RetMainNode()); //Then a place by my side               
               S_Box otherConnectorBox = nami.NodetoBox(otherPart.SutureHashtables[((int)bpDir + 2)%4].RetMainNode());
               S_XY diffOff = new S_XY();
            
                
               switch(bpDir)
                {
                    case BpDirection.North:
                       diffOff.y = thisConnectorBox.loc.y -  otherConnectorBox.loc.y + 1; //how much it is moved by
                       diffOff.x = 0;
                        
                        break;
                    case BpDirection.East:
                       diffOff.x = thisConnectorBox.loc.x -  otherConnectorBox.loc.x - 1; //how much it is moved by
                       diffOff.y = 0;
                       break;
                    case BpDirection.South:
                       diffOff.y = thisConnectorBox.loc.y -  otherConnectorBox.loc.y - 1; //how much it is moved by
                       diffOff.x = 0;
                        break;
                    case BpDirection.West:
                        diffOff.x = thisConnectorBox.loc.x -  otherConnectorBox.loc.x + 1; //how much it is moved by
                        diffOff.y = 0;
                        break;
                }

               otherPart.offset = this.offset + diffOff ;
               //otherPart.offset = new S_XY(10, 10);
              // otherPart.offset = new S_XY(70, 32);
              
               otherPart.rawOffSet.X = otherPart.offset.x; //rel to master
               otherPart.rawOffSet.Y = otherPart.offset.y;  //rel to master
               int index = (int)bpDir;
               connecters[index] = bpc;
               Master._UpdateBodyPartRelatedInfo();
           } else {
                //missing specific connectors in each direction, fails

            }

            //otherPart.AddBPConnecter(bp); needed if want two way list (in future, needs fixing)
        }

        public void GrowBodyPart(BodyPart BP, BpDirection bpDir)
        {
            if (connecters[(int)bpDir] == null && BP.connecters[((int)bpDir + 2) % 4] == null)
            { //if connect in dir and connector of new part in opp dir are both valid suture spots

                if (true) //if enough energy and no coli
                {
                    
                    SutureBodyPart(BP, bpDir);


                }
            }

        }

        protected FeedbackPulse SendPulseToEachBp(FuncPulseType funcPulseType, FuncPulse funcPulse)
        {
            FeedbackPulse fp = new FeedbackPulse();
            for (int c = 0; c < 4; c++)
                if (connecters[c] != null) 
                try
                {

                    fp += connecters[c].SendPulse(this, funcPulseType, funcPulse);
                }
                catch (Exception e)
                {
                   // Console.Out.WriteLine("Exception: " + e.Message);
                }
            return fp;

        }

       /*
        public int getTotalMass()
        {
            int toRet = this.mass;
            foreach (BodyPartConnection bpc in connecters)
                toRet += bpc.getTotalMass(this);
            return toRet;

        }*/

        public void Input(KeyboardState ks)
        {
            AE.TriggerEvent(ks);
            foreach (BodyPartConnection bpc in connecters)
                if(bpc != null)
                    bpc.Input(this,ks);

        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////should be done
        public void CheckColi(Structs.S_XY byOffset, VagueObject coliObj, List<BodyPart> coliParts)
        {
            //check if coli has happened if move by this ammount, if yes, add itself to the list of parts that have colided this tick
            if (coliObj.Coli(nami.MoveTableByOffset(coliBox, byOffset)))
                coliParts.Add(this);
            foreach (BodyPartConnection bpc in connecters)
                if(bpc != null)
                    bpc.CheckColi(this, byOffset, coliObj, coliParts);
        }
        /*
        public void MovePartBy(Structs.S_XY moveBy)
        {
            offset += moveBy;
            foreach (BodyPartConnection bpc in connecters)
                bpc.MovePartBy(this, moveBy);
        }*/

        public void Recieve(BodyPulse bp)
        {
            _SplitEvenlyAndSendPulse(bp);

        }

        /*public void GrowPart(BodyPart bpToGrow)
        {
            float growthCost = bpToGrow.bpDNA.SummonCost;

            //Now we have to channel growing it, or have it be a burst cost.


        }*/

        private void _SplitEvenlyAndSendPulse(BodyPulse bp)
        {
            bool hasChild = false;
            DecodePulse(bp);
            foreach (BodyPartConnection bpc in connecters)
            {
                if (bpc != null)
                {
                    hasChild = true;
                    bpc.Send(this, bp.split(connecters.Count));
                }
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
