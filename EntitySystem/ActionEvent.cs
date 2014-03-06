using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BodyParts;

namespace EntSys
{
    /*

     current order: BodyMech, Explosion, Material
    */
    public class ActionEvent
    {
        VagueObject master;
        bool forTest = false;
        objSpecificType specificType;
        Global.Bus bus = Global.Bus.Instance;
        AbilityStore AS = AbilityStore.Instance;
        //this class belongs to an Ent, it takes in triggers and commences proper events
        List<Func<VagueObject, BodyMechanics, Material, AERetType>> BmMatActions;
        List<Func<VagueObject, BodyMechanics, Explosion, AERetType>> BmExpActions;

        
        List<Func<VagueObject, Explosion, Material, AERetType>> ExpMatActions;
        List<Func<VagueObject, KeyboardState, AERetType>> KeyActions;

        List<Func<VagueObject, BodyPart, Material, AERetType>> BodypartMaterialActions;
        List<Func<VagueObject, BodyPart, Explosion, AERetType>> BodypartExpActions;

        public ActionEvent(VagueObject vo)
        {
            this.specificType = vo.specificType;
            master = vo;
            BmMatActions = new List<Func<VagueObject, BodyMechanics, Material, AERetType>>();
            BodypartMaterialActions = new List<Func<VagueObject, BodyPart, Material, AERetType>>();
            ExpMatActions = new List<Func<VagueObject, Explosion, Material, AERetType>>();
            BmExpActions = new List<Func<VagueObject, BodyMechanics, Explosion, AERetType>>();
            KeyActions = new List<Func<VagueObject, KeyboardState, AERetType>>();
            BodypartExpActions = new List<Func<VagueObject, BodyPart, Explosion, AERetType>>();
            AS.RegAbilityPack((int)specificType, BmMatActions, ExpMatActions, BmExpActions, KeyActions, BodypartMaterialActions, BodypartExpActions);
        }

        //atm unique checking is NOT a thing! ability to have some ability mutiple times
        public void RegAbilityPack(int num)
        {
            AS.RegAbilityPack(num, BmMatActions, ExpMatActions, BmExpActions, KeyActions, BodypartMaterialActions, BodypartExpActions);
        }

        

        public AERetType TriggerEvent(BodyMechanics bod,Material Material)
        {
            AERetType toRet = new AERetType();
            //cycle through events
            foreach (Func<VagueObject, BodyMechanics, Material, AERetType> func in BmMatActions)
            {
                toRet += func(master,bod, Material);
            }
            return toRet;
        }

        public bool TriggerEvent(BodyPart bod, Material Material)
        {
            //cycle through events
            
            foreach (Func<VagueObject, BodyPart, Material, AERetType> func in BodypartMaterialActions)
            {
                func(master, bod, Material);
            }
            return true;
        }

        public bool TriggerEvent(KeyboardState keyA)
        {
            //cycle through events
            foreach (Func<VagueObject, KeyboardState, AERetType> func in KeyActions)
            {
                func(master, keyA);
            }
            return true;
        }

        public bool TriggerEvent(Explosion exp, Material Material)
        {
            //cycle through events
            foreach (Func<VagueObject, Explosion, Material, AERetType> func in ExpMatActions)
            {
                func(master, exp, Material);
            }
            return true;
        }

        public bool TriggerEvent(BodyMechanics Bm, Explosion exp)
        {
            //cycle through events
            foreach (Func<VagueObject, BodyMechanics, Explosion, AERetType> func in BmExpActions)
            {
                func(master, Bm, exp);
            }
            return true;
        }

        public bool TriggerEvent(BodyPart bp, Explosion exp)
        {
            foreach (Func<VagueObject, BodyPart, Explosion, AERetType> func in BodypartExpActions)
            {
                func(master, bp, exp);
            }
            return true;

        }

        

    }
}
