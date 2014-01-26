using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;   //   for Texture2D
using Microsoft.Xna.Framework;
using Enums.Node;
using Structs;

namespace ColiSys
{
    class SpriteHTable
    {
        Texture2D t;


        public SpriteHTable()
        { }


        public void LoadTexture(Texture2D tt)
        {
            t = tt;
        }


        public void Draw(SpriteBatch sb, Node ht,Color color)
        {
            if(ht != null)
            {
                S_Box range;
            
                Node htx = ht.CopySelf(copyTypes.copyBoth) ;
                Node hty;

                while (htx != null)
                {
                    hty = htx.Dwn();
                    while (hty != null)
                    {
                        range = new S_Box(htx.Ret(Bounds.l) * Consts.TopScope.GAME_SCALE.x, hty.Ret(Bounds.l) * Consts.TopScope.GAME_SCALE.y, (htx.Ret(Bounds.u)+1) * Consts.TopScope.GAME_SCALE.x, (hty.Ret(Bounds.u)+1) * Consts.TopScope.GAME_SCALE.y, false); //guess i could just make it straigh to rectangle eh?
                        sb.Draw(t, Statics.Converter.SBox2Rect(range), color);
                        //sb.Draw(t, new Rectangle(0,0,100,100), Color.White);
                        hty = hty.Adj();
                    }
                    htx = htx.Adj();
                }

            }

        }



    }
}
