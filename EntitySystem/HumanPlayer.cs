using System;
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
    public class HumanPlayer : BodyMechanics
    {
        ColiSys.TestContent tc = ColiSys.TestContent.Instance;
        ColiSys.ShapeGenerator sgen = ColiSys.ShapeGenerator.Instance;
        //Remove Later^^
        ColiSys.NodeManipulator nami = ColiSys.NodeManipulator.Instance;
        Global.Timers UniResponseT = null;
        public keyMap keymap;
        bool inputShutDown = false;
           


        public HumanPlayer() { }
        public HumanPlayer(DNA dna)
        {
            //set the instructor to be prepped as if being fed a string of values
            //some things only affected by full ints, so passing doubles can make a good scope/buffer level
            ForceCnstr(dna);
            
        }

        private void _DNADecoder(DNA dna)
        {
            //Hardcoded DNA for now
            UniResponseT = new Global.Timers(0, 250, 251);

            //This should be in DNA passed down to Body//
            
            /////////////////////////////////////////////
        }

        

        protected void ForceCnstr(DNA dna)
        {
            AE = new ActionEvent(new VagueObject(this));

            
            base.ForceCnstr(dna);
            acceptedSColi = new List<objSpecificType>();
            acceptedColi = new List<objType>();

            acceptedColi.Add(objType.Ground);
            acceptedColi.Add(objType.Explosion);
            acceptedColi.Add(objType.Body);
            acceptedSColi.Add(objSpecificType.Human);
            

            

           
            _DNADecoder(dna);

            
        }

        public void Input()
        {
            if (!inputShutDown)
            {
                KeyboardState ks = Keyboard.GetState();
                if (Keyboard.GetState().GetPressedKeys().Length > 0)
                    AE.TriggerEvent(ks);

                foreach (BodyPart bp in bodyParts)
                    bp.Input(ks);
            }

            
        }

        public void Update(float rt)
        {
            _UpdateAllTimers(rt);
            base.Update(rt);


        }

          public void DebugLoadHuman(int defaultPack) //eventaully will be in dna
        {

            switch (defaultPack)
            {
                case 1:
                    LoadTexture(tc.dirt,Color.Blue);            
                    SetEntShape(new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.Human,new S_XY(50,50))));           
                    S_XY tOff = new S_XY(250, 50);
                    offset = tOff;
                    rawOffSet = new Vector2(tOff.x, tOff.y);
                    BodyPart wing = new BodyPart(this, null); //default creation, only used temp, add BpC later
                   // BodyPart wing2 = new BodyPart(this, null);
                    BodyPart arm1 = new BodyPart(this, null);
                   // BodyPart arm2 = new BodyPart(this, null);
                   // BodyPart leg1 = new BodyPart(this, null);
                    // leg2 = new BodyPart(this, null);

                  //  BodyPart feet1 = new BodyPart(this, null);
                  //  BodyPart feet2 = new BodyPart(this, null);

                    BodyPart claws1 = new BodyPart(this, null);
                   // BodyPart claws2 = new BodyPart(this, null);

                    arm1.AddBodyPart(claws1);


                   

                    bodyParts.Add(wing);
                    bodyParts.Add(arm1);
                    RegisterNewParts = true;
                    break;

                case 2:
                    LoadTexture(tc.dirt,Color.Red);            
                    SetEntShape(new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.Human,new S_XY(50,50))));           
                    S_XY ttOff = new S_XY(50, 50);
                    offset = ttOff;
                    rawOffSet = new Vector2(ttOff.x, ttOff.y);
                    BodyPart wing2 = new BodyPart(this, null); //default creation, only used temp, add BpC later
                    bodyParts.Add(wing2);
                    RegisterNewParts = true;
                    break;
                case 0:
                    LoadTexture(tc.dirt, Color.Teal);
                    SetEntShape(new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.Human, new S_XY(25, 25))));
                    S_XY tttOff = new S_XY(Consts.TopScope.WORLD_SIZE_X / 2, Consts.TopScope.WORLD_SIZE_Y / 2);
                    offset = tttOff;
                    rawOffSet = new Vector2(tttOff.x, tttOff.y);
                    inputShutDown = true;
                    break;


            }
            specType = objSpecificType.Human;
            _DebugSetKeyMap(defaultPack);
            

        }
        

        private void _UpdateAllTimers(float rt)
        {
            UniResponseT.Tick(rt);

        }

        private void _DebugSetKeyMap(int playerNum)
        {
            switch (playerNum)
            {
                case 1:
                    keymap = new keyMap();
                    keymap.jump = Keys.Space;
                    keymap.left = Keys.Left;
                    keymap.right = Keys.Right;
                    keymap.down = Keys.Down;
                    keymap.up = Keys.Up;
                    break;

                case 2:
                    keymap = new keyMap();
                    keymap.jump = Keys.W;
                    keymap.left = Keys.A;
                    keymap.right = Keys.D;
                    keymap.down = Keys.S;
                    keymap.up = Keys.Q;
                    break;

                default:
                    break;

            }


        }


        

    }
}
