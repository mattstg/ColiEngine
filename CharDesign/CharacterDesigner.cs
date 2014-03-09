using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BodyParts;
using EntSys;
using Structs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
//_spriteBatch.DrawString(font, "Mute the background music?", new Vector2(200, 140) * displayScale, Color.Black);
                    

namespace CharDesign
{
    class CharacterDesigner
    {
        struct BodyPartWithSuture
        {
            BodyPart bp;
            ColiSys.Hashtable sutureUsed;
            
        }

        struct PlayerChanges
        {
            List<BodyPartWithSuture> bpChanges;


           

        }

        List<PlayerChanges> Levels;

        ColiSys.TestContent tc;
        SpriteBatch sb;
        HumanPlayer human = new HumanPlayer(null);
        

        
        private static CharacterDesigner instance;
        public static CharacterDesigner Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CharacterDesigner();
                }
                return instance;
            }
        }




        private CharacterDesigner()
        {
            tc = ColiSys.TestContent.Instance;
            sb = ColiSys.Game1.spriteBatch;
            Levels = new List<PlayerChanges>();

        }




        public void Update(float rt)
        {


        }

        public void Input()
        {

        }

        public void Draw()
        {
            
            _DrawCharacterLevelBar();
        }


        private void _DrawCharacterLevelBar()
        {
            Rectangle rect = new Rectangle((int)(Consts.TopScope.WORLD_SIZE_X * .8), 0, (int)(Consts.TopScope.WORLD_SIZE_X * .2), Consts.TopScope.WORLD_SIZE_Y);
            sb.Draw(tc.sqr, rect, Color.LightGray);

        }

        private void _AddLevel(PlayerChanges pc, int lvl)
        {
            if (lvl == -1)
                Levels.Add(pc);
            else
                Levels.Insert(lvl, pc);

        }

    }
}
