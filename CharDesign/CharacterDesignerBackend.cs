using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FactSys
{
    class CharacterDesignerBackend
    {
        SpriteBatch sb;
        private static CharacterDesignerBackend instance;
        public static CharacterDesignerBackend Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CharacterDesignerBackend();
                }
                return instance;
            }
        }
        private CharacterDesignerBackend()
        {
            sb = ColiSys.Game1.spriteBatch;

        }






    }
}
