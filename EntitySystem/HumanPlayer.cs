﻿using System;
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
        
        ColiSys.NodeManipulator nami = ColiSys.NodeManipulator.Instance;
        Global.Timers UniResponseT = null;
        keyMap keymap;
           


        public HumanPlayer() { }
        public HumanPlayer(DNA EntDNA, DNA SprDNA, DNA BodDNA, DNA MekDNA, DNA dna)
        {
            //set the instructor to be prepped as if being fed a string of values
            //some things only affected by full ints, so passing doubles can make a good scope/buffer level
            ForceCnstr(EntDNA,SprDNA,BodDNA,MekDNA,dna);
            
        }

        private void _DNADecoder(DNA dna)
        {
            //Hardcoded DNA for now
            UniResponseT = new Global.Timers(0, 250, 251);
        }

        

        protected void ForceCnstr(DNA EntDNA, DNA SprDNA, DNA BodDNA, DNA MekDNA, DNA dna)
        {

            acceptedColi = new AcceptedCollidables(true,true,true);
            base.ForceCnstr(EntDNA,SprDNA,BodDNA,MekDNA);
            _DNADecoder(dna);
            _DebugSetKeyMap();
            

        }

        public void Input()
        {
            KeyboardState keys = Keyboard.GetState();
            if (UniResponseT.ready && keys.GetPressedKeys().Length != 0)
            {
                
                if (keys.IsKeyDown(keymap.left))
                    ApplyForce(Enums.Force.ForceTypes.Internal,new Vector2(-moveForce, 0));
                if (keys.IsKeyDown(keymap.right))
                    ApplyForce(Enums.Force.ForceTypes.Internal, new Vector2(moveForce, 0));
                if (keys.IsKeyDown(keymap.up))
                    ApplyForce(Enums.Force.ForceTypes.Internal, new Vector2(0, -moveForce));
                if (keys.IsKeyDown(keymap.down))
                    ApplyForce(Enums.Force.ForceTypes.Internal, new Vector2(0, moveForce));


                UniResponseT.Dec(true);
                //setColiBox();
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
