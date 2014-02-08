using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EntSys
{
    /*

     current order: BodyMech, Explosion, Ground
    */
    class ActionEvent
    {
        VagueObject master;
        bool forTest = false;
        objType type;
        Global.Bus bus = Global.Bus.Instance;
        AbilityStore AS = AbilityStore.Instance;
        //this class belongs to an Ent, it takes in triggers and commences proper events
        List<Func<VagueObject, BodyMechanics, Ground, bool>> BmGActions;
        List<Func<VagueObject, Explosion, Ground, bool>> ExpGActions;
        List<Func<VagueObject, BodyMechanics, Explosion, bool>> BmExpActions;
        List<Func<VagueObject, KeyboardState,bool>> KeyActions;


        public ActionEvent(VagueObject vo)
        {
            this.type = vo.type;
            master = vo;
            BmGActions = new List<Func<VagueObject, BodyMechanics, Ground, bool>>();
            ExpGActions = new List<Func<VagueObject, Explosion, Ground, bool>>();
            BmExpActions = new List<Func<VagueObject, BodyMechanics, Explosion, bool>>();
            KeyActions = new List<Func<VagueObject, KeyboardState, bool>>();
            AS.RegAbilityPack((int)type, BmGActions, ExpGActions, BmExpActions,KeyActions);
        }

        //atm unique checking is NOT a thing! ability to have some ability mutiple times
        public void RegAbilityPack(int num)
        {
            AS.RegAbilityPack((int)type, BmGActions, ExpGActions, BmExpActions, KeyActions);
        }



        public bool TriggerEvent(BodyMechanics bod,Ground ground)
        {
            //cycle through events
            foreach (Func<VagueObject, BodyMechanics, Ground, bool> func in BmGActions)
            {
                func(master,bod, ground);
            }
            return true;
        }

        public bool TriggerEvent(KeyboardState keyA)
        {
            //cycle through events
            foreach (Func<VagueObject, KeyboardState,bool> func in KeyActions)
            {
                func(master, keyA);
            }
            return true;
        }

        public bool TriggerEvent(Explosion exp, Ground ground)
        {
            //cycle through events
            foreach (Func<VagueObject, Explosion, Ground, bool> func in ExpGActions)
            {
                func(master, exp, ground);
            }
            return true;
        }

        public bool TriggerEvent(BodyMechanics Bm, Explosion exp)
        {
            //cycle through events
            foreach (Func<VagueObject, BodyMechanics, Explosion, bool> func in BmExpActions)
            {
                func(master, Bm, exp);
            }
            return true;
        }
        

        

    }
}
