using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntSys
{
    enum objType { Body = 0, Explosion = 1, Ground = 2 }
    class ActionEvent
    {
        bool forTest = false;
        objType type;
        Global.Bus bus = Global.Bus.Instance;
        AbilityStore AS = AbilityStore.Instance;
        //this class belongs to an Ent, it takes in triggers and commences proper events
        List<Func<Object,BodyMechanics,Ground,bool>> BmGActions;
        List<Func<Object, Explosion, Ground, bool>> ExpGActions;
        List<Func<Object, BodyMechanics, Explosion, bool>> BmExpActions;


        public ActionEvent(objType type)
        {
            this.type = type;
            BmGActions = new List<Func<Object,BodyMechanics, Ground, bool>>();
            ExpGActions = new List<Func<Object,Explosion, Ground, bool>>();
            BmExpActions = new List<Func<Object, BodyMechanics, Explosion, bool>>();
            AS.RegAbilityPack((int)type, BmGActions, ExpGActions, BmExpActions);
        }

        //atm unique checking is NOT a thing! ability to have some ability mutiple times
        public void RegAbilityPack(int num)
        {
            AS.RegAbilityPack((int)type, BmGActions, ExpGActions, BmExpActions);
        }



        public bool TriggerEvent(BodyMechanics bod,Ground ground)
        {
            //cycle through events
            foreach(Func<Object,BodyMechanics,Ground,bool> func in BmGActions)
            {
                func(this,bod, ground);
            }
            return true;
        }

        public bool TriggerEvent(Explosion exp, Ground ground)
        {
            //cycle through events
            foreach (Func<Object, Explosion, Ground, bool> func in ExpGActions)
            {
                func(this,exp, ground);
            }
            return true;
        }

        public bool TriggerEvent(BodyMechanics Bm, Explosion exp)
        {
            //cycle through events
            foreach (Func<Object, BodyMechanics, Explosion, bool> func in BmExpActions)
            {
                func(this, Bm, exp);
            }
            return true;
        }
        

        

    }
}
