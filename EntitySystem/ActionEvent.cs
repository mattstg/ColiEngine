using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntSys
{
    class ActionEvent
    {
        bool forTest = false;
        Global.Bus bus = Global.Bus.Instance;
        //this class belongs to an Ent, it takes in triggers and commences proper events
        List<Func<BodyMechanics,Ground,bool>> bodGroundActions;
        List<Func<Explosion, Ground, bool>> expGroundActions;
        
        
        public ActionEvent() {
        
            bodGroundActions = new List<Func<BodyMechanics,Ground,bool>>();
            expGroundActions = new List<Func<Explosion, Ground, bool>>();
            _DebugFillEventLists();
        
        }

        private void _DebugFillEventLists()
        {
            bodGroundActions.Add(bg1);
            expGroundActions.Add(eg1);

        }


        public bool TriggerEvent(BodyMechanics bod,Ground ground)
        {
            //cycle through events
            foreach(Func<BodyMechanics,Ground,bool> func in bodGroundActions)
            {
                func(bod, ground);
            }
            return true;
        }

        public bool TriggerEvent(Explosion exp, Ground ground)
        {
            //cycle through events
            foreach (Func<Explosion, Ground, bool> func in expGroundActions)
            {
                func(exp, ground);
            }
            return true;
        }

        

        /////For now, all Events will be created here, they should be created elsewhere i guess... this way for custom skill generation
        private bool bg1(BodyMechanics bod, Ground ground)
        {
            if (!forTest)
            {
                bus.LoadPassenger(new Explosion(bod.trueEntShape,new Structs.S_XY(bod.offsetCopy.x,bod.offsetCopy.y+14)));
                Console.Out.Write("YO SHIT BE HAPPENING");
                forTest = true;
            }
             
             return true;
        }

        private bool eg1(Explosion exp, Ground ground)
        {
            Console.Out.Write("EXPLOSION HIT GROUND!");
            return true;

        }

    }
}
