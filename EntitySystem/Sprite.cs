﻿using System;
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
            TrueEntShape = new ColiSys.Hashtable(tBodyShape);
            setColiBox();
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
                        //range = new S_Box((htx.Ret(Bounds.l) + offset.x) * Consts.TopScope.GAME_SCALE.x, (hty.Ret(Bounds.l) + offset.y) * Consts.TopScope.GAME_SCALE.y, (htx.Ret(Bounds.u) + 1 + offset.x) * Consts.TopScope.GAME_SCALE.x, (hty.Ret(Bounds.u) + 1 + +offset.y) * Consts.TopScope.GAME_SCALE.y, false); //guess i could just make it straigh to rectangle eh?
                        Rectangle rect = new Rectangle((htx.Ret(Bounds.l) + offset.x) * Consts.TopScope.GAME_SCALE.x, (hty.Ret(Bounds.l) + offset.y) * Consts.TopScope.GAME_SCALE.y, (htx.Ret(Bounds.u) - htx.Ret(Bounds.l) + 1) * Consts.TopScope.GAME_SCALE.x, (hty.Ret(Bounds.u) - hty.Ret(Bounds.l) + 1) * Consts.TopScope.GAME_SCALE.y);
                        sb.Draw(t, rect, Color.Yellow);
                        
                        setColiBox();
                        ColiSys.Node test = RetSizeLocCopy();
                        rect = new Rectangle(test.Ret(Bounds.l),test.Dwn().Ret(Bounds.l),test.Ret(Bounds.u)-test.Ret(Bounds.l)+1*Consts.TopScope.GAME_SCALE.x,test.Dwn().Ret(Bounds.u)-test.Dwn().Ret(Bounds.l)+1*Consts.TopScope.GAME_SCALE.y);
                        sb.Draw(t, rect, color);
                        //sb.Draw(t, new Rectangle(0,0,100,100), Color.White);
                        hty = hty.Adj();
                    }
                    htx = htx.Adj();
                }

            }

        }

    }
}
