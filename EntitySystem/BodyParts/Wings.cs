﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntSys;

namespace BodyParts
{
    class Wings : BodyPart
    {
        ColiSys.TestContent tc;
        public Wings( BodyMechanics master, EntSys.DNA dna)
        {
            tc = ColiSys.TestContent.Instance;            
            partType = BodyPartType.Wings;
            LoadBodyPart(DefaultShapeGen());           
            AE = new ActionEvent(new VagueObject(this));
            AE.RegAbilityPack(10);
            base.ForceCnstr(dna);
            Master = master;
        }

        public ColiSys.Hashtable DefaultShapeGen()
        {
          
            ColiSys.Node nodey2 = new ColiSys.Node(57, 57);
            ColiSys.Node nodey1 = new ColiSys.Node(54, 55,nodey2,null);
            ColiSys.Node nodex2 = new ColiSys.Node(55, 62, null, nodey1);
            ColiSys.Node nodex1 = new ColiSys.Node(43, 50, nodex2, nodey1);
            

            LoadTexture(tc.dirt,Microsoft.Xna.Framework.Color.GhostWhite);

            return new ColiSys.Hashtable(nodex1);
        }

        public void LoadBodyPart(ColiSys.Hashtable ht)
        {
            SetEntShape(ht);           
        }

        public void Update(float rt)
        {
            base.Update(rt);

        }

        public override void DecodePulse(BodyPulse bp)
        {


        }



    }
}
