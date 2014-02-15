﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structs;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using EntStructEnum;
using BodyParts;

//remove later^^
namespace EntSys
{
    class HumanPlayer : BodyMechanics
    {
        ColiSys.TestContent tc = ColiSys.TestContent.Instance;
        ColiSys.ShapeGenerator sgen = ColiSys.ShapeGenerator.Instance;
        //Remove Later^^
        ColiSys.NodeManipulator nami = ColiSys.NodeManipulator.Instance;
        Global.Timers UniResponseT = null;
        public keyMap keymap;
           


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

            //This should be in DNA passed down to Body//
            
            /////////////////////////////////////////////
        }

        

        protected void ForceCnstr(DNA EntDNA, DNA SprDNA, DNA BodDNA, DNA MekDNA, DNA dna)
        {
            AE = new ActionEvent(new VagueObject(this));

            

            acceptedSColi = new List<objSpecificType>();
            acceptedColi = new List<objType>();

            acceptedColi.Add(objType.Ground);
            acceptedColi.Add(objType.Explosion);
            acceptedColi.Add(objType.Body);

            

            base.ForceCnstr(EntDNA,SprDNA,BodDNA,MekDNA);
            _DNADecoder(dna);
            _DebugSetKeyMap();

            Wings wing = new Wings(this, null, null);
            bodyParts.Add(wing);
        }

        public void Input()
        {
            KeyboardState ks = Keyboard.GetState();
            if(Keyboard.GetState().GetPressedKeys().Length > 0)
                 AE.TriggerEvent(ks);

            foreach (BodyPart bp in bodyParts)
                bp.Input(ks);

            /*
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
            }*/
        }

        public void Update(float rt)
        {
            _UpdateAllTimers(rt);
            base.Update(rt);


        }

          public void DebugLoadHuman() //eventaully will be in dna
        {
              
           
            LoadTexture(tc.dirt,Color.Blue);
            SetEntShape(new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.Human,new S_XY(), new S_XY(5,15))));           
            S_XY tOff = new S_XY(50, 50);
            offset = tOff;
            rawOffSet = new Vector2(tOff.x, tOff.y);

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
