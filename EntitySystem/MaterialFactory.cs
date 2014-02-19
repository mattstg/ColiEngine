using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ColiSys;

namespace EntSys
{
    class MaterialFactory
    {
        ColiSys.ShapeGenerator sgen;
        ColiSys.TestContent tc;

        private static MaterialFactory instance;
        private MaterialFactory() {
            sgen = ColiSys.ShapeGenerator.Instance;
            tc = ColiSys.TestContent.Instance;
        }
        public static MaterialFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MaterialFactory();
                }
                return instance;
            }
        }

        public Ground CreateMaterial(int pattern)
        {
           
        //so single object with these vars, a type, energy, 
        float hp;
        float bounceThreshold;; //bounce within -100%
        float bounceForceMultLB;
        float bounceForceMultUB;
        float absorb;
        MaterialResistances armor = new MaterialResistances();       
        MaterialTypes matType;
        float friction;
        float thornDmg;
        float stickyness;
        ColiSys.Hashtable htable = new ColiSys.Hashtable();
        AdditionalInfo addInfo = new AdditionalInfo();
        


            Ground tr = new Ground();
            switch (pattern)
            {
                case 0: //normal dirt
                    hp = 1000;
                    bounceThreshold = .33f; //bounce within -100%
                    bounceForceMultLB = .2f;
                    bounceForceMultUB = 1f;
                    absorb = 1; //times it own health worth of force repelling        \
                    armor = new MaterialResistances();
                    armor.dirt = 1;
                    armor.steel = 1;
                    matType  = MaterialTypes.dirt;
                    friction = .5f;
                    thornDmg = 0;  //0-inf percent dmg back
                    stickyness = 0; //not sure yet
                    //addInfo = new AdditionalInfo();


                    htable = new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(Consts.TopScope.WORLD_SIZE_X, Consts.TopScope.WORLD_SIZE_Y / 2), addInfo));
                    htable.LoadTexture(tc.dirt, Color.White);
                    tr = new Ground(hp, bounceForceMultLB, bounceForceMultUB, bounceThreshold, absorb, thornDmg, stickyness, armor, htable, friction,matType);
                    break;

                case 1: //Indestructible wall
                    hp = 1000000;
                    bounceThreshold = .9f; //bounce within -100%
                    bounceForceMultLB = .2f;
                    bounceForceMultUB = 1f;
                    absorb = 100; //times it own health worth of force repelling        \
                    armor = new MaterialResistances();
                    armor.dirt = .001f;
                    armor.steel = .001f;
                    matType  = MaterialTypes.Indestructible;
                    friction = .5f;
                    addInfo.width = 5;
                    thornDmg = 0;  //0-inf percent dmg back
                    stickyness = 0; //not sure yet
                    
                   
                    htable = new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.HollowSqaure, new Structs.S_XY(Consts.TopScope.WORLD_SIZE_X, Consts.TopScope.WORLD_SIZE_Y),addInfo));
                    htable.LoadTexture(tc.dirt, Color.SteelBlue);

                    tr = new Ground(hp, bounceForceMultLB, bounceForceMultUB, bounceThreshold, absorb, thornDmg, stickyness, armor, htable, friction,matType);
                    
                    break;

                default:
                    break;


            }
            return tr;

        }
        //Decoder, translates input into proper constructor feed


        //Creator, creates object





    }
}
