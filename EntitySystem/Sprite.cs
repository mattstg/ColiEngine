using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Enums.Node;



namespace EntSys
{
    public struct SprCnstr
    {
        public Texture2D texture;
        public Color color;

    }


    public class Sprite : Entity
    {

        
        protected Texture2D t;
        protected Color color;

        public Sprite() { }

        public Sprite(DNA dna)
        {
            ForceCnstr(dna);
            
        }

        private void _DNADecoder(DNA dna)
        {
            t = dna.sprC.texture;
            color = dna.sprC.color;
            //Need DNA copier
            
        }
              

        public void LoadTexture(Texture2D tt, Color col)
        {
            t = tt;            
            color = col;
        }
        
        protected void ForceCnstr(DNA dna)
        {
            //load vars
            base.ForceCnstr(dna);
            _DNADecoder(dna);
        }

        public void Update(float rt)
        {
            base.Update(rt);
        }

        public void Draw()
        {
            SpriteBatch sb = ColiSys.Game1.spriteBatch;

            if (!this.ifBodyEmpty() && t != null)
            {
                
                ColiSys.Node htx = trueEntShapeOffset;
                ColiSys.Node hty;

                while (htx != null)
                {
                    hty = htx.Dwn();
                    while (hty != null)
                    {
                        Rectangle rect = new Rectangle(htx.Ret(Bounds.l) * Consts.TopScope.GAME_SCALE.x, hty.Ret(Bounds.l) * Consts.TopScope.GAME_SCALE.y, (htx.Ret(Bounds.u) - htx.Ret(Bounds.l) + 1) * Consts.TopScope.GAME_SCALE.x, (hty.Ret(Bounds.u) - hty.Ret(Bounds.l) + 1) * Consts.TopScope.GAME_SCALE.y);
                        sb.Draw(t, rect, color);
                        //sb.Draw(t, new Rectangle(0,0,100,100), Color.White);
                        hty = hty.Adj();
                    }
                    htx = htx.Adj();
                }




            }
            else
            {
                Console.Out.WriteLine("Sprite not drawn, no body or texture detected");

            }

        }

        public void Draw(ColiSys.Hashtable graphicSkin)
        {
            graphicSkin.Draw();

        }



    }
}
