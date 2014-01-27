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
    class Sprite : Entity
    {


        protected Texture2D t;
        protected Color color;

        public Sprite() { }

        public Sprite(DNA EntDNA,DNA dna)
        {
            ForceCnstr(EntDNA);
            
        }

        private void _DNADecoder(DNA dna)
        {
            //Need DNA copier
            
        }

        public void DebugLoadSprite(Texture2D tt,ColiSys.Node tBodyShape,S_XY tOffset,Color col) //eventaully will be in dna
        {
            t = tt;            
            color = col;
            HashTrueEntShape = new ColiSys.Hashtable(tBodyShape);
            //setColiBox();
            offset = tOffset;
            rawOffSet = new Vector2(tOffset.x,tOffset.y);

        }

        protected void ForceCnstr(DNA EntDNA, DNA dna)
        {
            //load vars
            base.ForceCnstr(EntDNA);
            _DNADecoder(dna);
        }

        public void Update(float rt)
        {
            base.Update(rt);
        }

        public void Draw(SpriteBatch sb)
        {
            if (!this.ifBodyEmpty())
            {
                S_Box range;
                ColiSys.Node htx = trueEntShapeOffset;
                ColiSys.Node hty;



                ///DRAW COLI BOX
                htx = coliBox;
                htx = nami.StretchSquareTableByXY(htx, new S_XY(0, 1)); //increase size of explosion by  
                while (htx != null)
                {
                    hty = htx.Dwn();
                    while (hty != null)
                    {
                        //Rectangle rect = new Rectangle(htx.Ret(Bounds.l), hty.Dwn().Ret(Bounds.l), htx.Ret(Bounds.u) - htx.Ret(Bounds.l) + 1 * Consts.TopScope.GAME_SCALE.x, hty.Dwn().Ret(Bounds.u) - hty.Dwn().Ret(Bounds.l) + 1 * Consts.TopScope.GAME_SCALE.y);
                        Rectangle rect = new Rectangle(htx.Ret(Bounds.l) * Consts.TopScope.GAME_SCALE.x, hty.Ret(Bounds.l) * Consts.TopScope.GAME_SCALE.y, (htx.Ret(Bounds.u) - htx.Ret(Bounds.l)) * Consts.TopScope.GAME_SCALE.x, (hty.Ret(Bounds.u) - hty.Ret(Bounds.l)) * Consts.TopScope.GAME_SCALE.y);
                        //Rectangle rect = new Rectangle(100, 100, 50, 50);
                        sb.Draw(t, rect, Color.DeepPink);
                        //sb.Draw(t, new Rectangle(0,0,100,100), Color.White);
                        hty = hty.Adj();
                    }
                    htx = htx.Adj();
                }


                ///////////Draw actaul Sprite///////////
                htx = trueEntShapeOffset;               

                while (htx != null)
                {
                    hty = htx.Dwn();
                    while (hty != null)
                    {
                        Rectangle rect = new Rectangle(htx.Ret(Bounds.l) * Consts.TopScope.GAME_SCALE.x, hty.Ret(Bounds.l) * Consts.TopScope.GAME_SCALE.y, (htx.Ret(Bounds.u) - htx.Ret(Bounds.l)) * Consts.TopScope.GAME_SCALE.x, (hty.Ret(Bounds.u) - hty.Ret(Bounds.l)) * Consts.TopScope.GAME_SCALE.y);
                        sb.Draw(t, rect, Color.Black);
                        //sb.Draw(t, new Rectangle(0,0,100,100), Color.White);
                        hty = hty.Adj();
                    }
                    htx = htx.Adj();
                }


               

            }

        }

    }
}
