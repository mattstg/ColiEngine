using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NodeEnum;


namespace EntSys
{
    class Sprite : Entity
    {
        protected Texture2D t;
        protected Color color;

        public Sprite() { }

        public Sprite(DNA EntDNA,DNA dna)
        {
            ForceCnstr(EntDNA);
            
        }

        private void _DNACopier(DNA dna)
        {
            //Need DNA copier
            
        }

        public void DebugLoadSprite(Texture2D tt,ColiSys.Node tBodyShape,S_XY tOffset,Color col) //eventaully will be in dna
        {
            t = tt;            
            color = col;
            bodyShape = tBodyShape;
            setColiBox();
            offset = tOffset;

        }

        protected void ForceCnstr(DNA EntDNA, DNA dna)
        {
            //load vars
            base.ForceCnstr(EntDNA);
            _DNACopier(dna);
        }

        public void Update(float rt)
        {
            base.Update(rt);
        }

        public void Draw(SpriteBatch sb)
        {
            if (bodyShape != null)
            {
                S_Box range;

                ColiSys.Node htx = bodyShape.CopySelf(copyTypes.copyBoth);
                ColiSys.Node hty;

                while (htx != null)
                {
                    hty = htx.Dwn();
                    while (hty != null)
                    {
                        range = new S_Box((htx.Ret(Bounds.l) + offset.x) * ColiSys.Constants.GAME_SCALE.x, (hty.Ret(Bounds.l) + offset.y) * ColiSys.Constants.GAME_SCALE.y, (htx.Ret(Bounds.u) + 1 + offset.x) * ColiSys.Constants.GAME_SCALE.x, (hty.Ret(Bounds.u) + 1 + +offset.y) * ColiSys.Constants.GAME_SCALE.y, false); //guess i could just make it straigh to rectangle eh?
                        sb.Draw(t, ColiSys.Converter.SBox2Rect(range), color);
                        //sb.Draw(t, new Rectangle(0,0,100,100), Color.White);
                        hty = hty.Adj();
                    }
                    htx = htx.Adj();
                }

            }

        }

    }
}
