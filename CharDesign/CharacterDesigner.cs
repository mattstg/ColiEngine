using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BodyParts;
using EntSys;
using Structs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
//_spriteBatch.DrawString(font, "Mute the background music?", new Vector2(200, 140) * displayScale, Color.Black);
using BodyParts;       

namespace FactSys
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
        List<bodyItem> bpList;
        BodyPartStore bpStore;
        ColiSys.NodeManipulator nami;
        ColiSys.TestContent tc;
        SpriteBatch sb;
        HumanPlayer human;// = new HumanPlayer(null);
        

        
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
            nami = ColiSys.NodeManipulator.Instance;
            tc = ColiSys.TestContent.Instance;
            sb = ColiSys.Game1.spriteBatch;
            Levels = new List<PlayerChanges>();           
            bpStore = BodyPartStore.Instance;
            bpList = bpStore.ReturnBpcList();
            
        }




        public void Update(float rt)
        {


        }

        public void Input(KeyboardState keys, MouseState mouses)
        {

        }

        public void Draw()
        {
            
            _DrawCharacterLevelBar();
            _DrawBodyStoreBar();
            _DrawCatalogOfParts();
        }

        private void _DrawCatalogOfParts()
        {
            List<ColiSys.Hashtable> toDraw = new List<ColiSys.Hashtable>();
            int goalSize = 90;
            S_XY byOf = new S_XY(0, (int)(Consts.TopScope.WORLD_SIZE_Y*.9));            
            foreach (bodyItem b in bpList)
            {
                ColiSys.Hashtable ht = new ColiSys.Hashtable(b.bp.shape);
                while (ht.GetSize().x > goalSize)
                    ht = nami.Scale(ht, .5f);
                while (ht.GetSize().y > goalSize)
                    ht = nami.Scale(ht, .5f);
                ht.LoadTexture(tc.dirt, Color.Green);
                toDraw.Add(ht);
            }

            foreach (ColiSys.Hashtable h in toDraw)
            {
                h.ResetMainNode(nami.MoveTableByOffset(h.RetMainNode(),byOf));
                h.Draw();
                byOf.x += goalSize + 20;

            }

        }

        private void _DrawBodyStoreBar()
        {
            Rectangle rect = new Rectangle(0, (int)(Consts.TopScope.WORLD_SIZE_Y*.85), Consts.TopScope.WORLD_SIZE_X, (int)(Consts.TopScope.WORLD_SIZE_Y*.15));
            sb.Draw(tc.sqr, rect, Color.LightGray);
        }

        private void _DrawCharacterLevelBar()
        {
            Rectangle rect = new Rectangle((int)(Consts.TopScope.WORLD_SIZE_X * .9), 0, (int)(Consts.TopScope.WORLD_SIZE_X * .1), Consts.TopScope.WORLD_SIZE_Y);
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
