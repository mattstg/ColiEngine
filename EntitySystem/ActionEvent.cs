using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntSys
{
    enum objType { Body = 0, Explosion = 1 }
    class ActionEvent
    {
        bool forTest = false;
        objType type;
        Global.Bus bus = Global.Bus.Instance;
        AbilityStore AS = AbilityStore.Instance;
        //this class belongs to an Ent, it takes in triggers and commences proper events
        List<Func<Object,BodyMechanics,Ground,bool>> bodGroundActions;
        List<Func<Object, Explosion, Ground, bool>> expGroundActions;


        public ActionEvent(objType type)
        {
            this.type = type;
            bodGroundActions = new List<Func<Object,BodyMechanics, Ground, bool>>();
            expGroundActions = new List<Func<Object,Explosion, Ground, bool>>();
            AS.RegAbilityPack((int)type,bodGroundActions,expGroundActions);
        }

        //atm unique checking is NOT a thing! ability to have some ability mutiple times
        public void RegAbilityPack(int num)
        {
            AS.RegAbilityPack((int)type, bodGroundActions, expGroundActions);
        }



        public bool TriggerEvent(BodyMechanics bod,Ground ground)
        {
            //cycle through events
            foreach(Func<Object,BodyMechanics,Ground,bool> func in bodGroundActions)
            {
                func(this,bod, ground);
            }
            return true;
        }

        public bool TriggerEvent(Explosion exp, Ground ground)
        {
            //cycle through events
            foreach (Func<Object, Explosion, Ground, bool> func in expGroundActions)
            {
                func(this,exp, ground);
            }
            return true;
        }

        

        

    }
}
