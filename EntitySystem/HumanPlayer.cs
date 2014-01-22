using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structs;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using EntStructEnum;


namespace EntSys
{
    class HumanPlayer : BodyMechanics
    {
        Timers UniResponseT = null;
        keyMap keymap;

        


        public HumanPlayer() { }
        public HumanPlayer(DNA EntDNA, DNA SprDNA, DNA BodDNA, DNA MekDNA, DNA dna)
        {
            //set the instructor to be prepped as if being fed a string of values
            //some things only affected by full ints, so passing doubles can make a good scope/buffer level
            ForceCnstr(EntDNA,SprDNA,BodDNA,MekDNA,dna);
            
        }

        private void _DNACopier(DNA dna)
        {
            //Hardcoded DNA for now
            UniResponseT = new Timers(0, 2500, 3000);
        }

        protected void ForceCnstr(DNA EntDNA, DNA SprDNA, DNA BodDNA, DNA MekDNA, DNA dna)
        {


            base.ForceCnstr(EntDNA,SprDNA,BodDNA,MekDNA);
            _DNACopier(dna);
            _DebugSetKeyMap();
            

        }

        public void Input()
        {
            
            if (UniResponseT.ready)
            {
                KeyboardState keys = Keyboard.GetState();
                if (keys.IsKeyDown(keymap.left))
                    ApplyForce(new S_XY(-moveForce, 0));
                if (keys.IsKeyDown(keymap.right))
                    ApplyForce(new S_XY(moveForce, 0));
                if (keys.IsKeyDown(keymap.up))
                    ApplyForce(new S_XY(0,moveForce));
                if (keys.IsKeyDown(keymap.down))
                    ApplyForce(new S_XY(0,moveForce));


                UniResponseT.Dec(true);
            }
        }

        public void Update(float rt)
        {
            _UpdateAllTimers(rt);
            base.Update(rt);


        }

        private void _UpdateAllTimers(float rt)
        {
            UniResponseT.Tick(rt);

        }

        private void _DebugSetKeyMap()
        {
            keymap = new keyMap();
            keymap.jump = Keys.Space;
            keymap.left = Keys.Left;
            keymap.right = Keys.Right;
            keymap.down = Keys.Down;
            keymap.up = Keys.Up;
        }


    }
}
