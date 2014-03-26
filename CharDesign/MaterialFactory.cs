using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ColiSys;
using Structs;
using EntSys;


namespace FactSys
{
    class MaterialFactory
    {
        ShapeGenerator sgen;
        TestContent tc;
        NodeManipulator nami;

        private static MaterialFactory instance;
        private MaterialFactory() {
            sgen = ShapeGenerator.Instance;
            tc = TestContent.Instance;
            nami = NodeManipulator.Instance;

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

        public Material CreateMaterial(int pattern)
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
        S_XY loc;
        ColiSys.Hashtable htable = new ColiSys.Hashtable();
        AdditionalInfo addInfo = new AdditionalInfo();



        Material tr = new Material();
            switch (pattern)
            {
                case 0: //normal dirt
                    hp = 600;
                    bounceThreshold = .60f; //bounce within -100%
                    bounceForceMultLB = .2f;
                    bounceForceMultUB = 1f;
                    absorb = 1; //times it own health worth of force repelling        \
                    armor = new MaterialResistances();
                    armor.Dirt = 1;
                    armor.Steel = 1;
                    matType  = MaterialTypes.Dirt;
                    friction = .5f;
                    loc = new S_XY(0, 0);
                    thornDmg = 0;  //0-inf percent dmg back
                    stickyness = 0; //not sure yet
                    //addInfo = new AdditionalInfo();


                    htable = new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(Consts.TopScope.WORLD_SIZE_X, Consts.TopScope.WORLD_SIZE_Y / 2), addInfo));
                    htable.ResetMainNode(nami.MoveTableByOffset(htable.RetMainNode(), new S_XY(0, Consts.TopScope.WORLD_SIZE_Y / 2)));
                   // htable.LoadTexture(tc.dirt, Color.White);
                    tr = new Material(hp, bounceForceMultLB, bounceForceMultUB, bounceThreshold, absorb, thornDmg, stickyness, armor, htable, friction,matType,true,null,loc);
                    tr.LoadTexture(tc.sqr, Color.Brown);
                    break;

                case 1: //Indestructible wall
                    hp = 1000000;
                    bounceThreshold = .9f; //bounce within -100%
                    bounceForceMultLB = .2f;
                    bounceForceMultUB = 1f;
                    absorb = 100; //times it own health worth of force repelling        \
                    armor = new MaterialResistances();
                    armor.Dirt = .001f;
                    armor.Steel = .001f;
                    matType  = MaterialTypes.Indestructible;
                    friction = .5f;
                    addInfo.width = 5;
                    loc = new S_XY(0, 0);
                    thornDmg = 0;  //0-inf percent dmg back
                    stickyness = 0; //not sure yet
                    
                   
                    htable = new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.HollowSqaure, new Structs.S_XY(Consts.TopScope.WORLD_SIZE_X, Consts.TopScope.WORLD_SIZE_Y),addInfo));
                    

                    tr = new Material(hp, bounceForceMultLB, bounceForceMultUB, bounceThreshold, absorb, thornDmg, stickyness, armor, htable, friction, matType,true,null,loc);
                    tr.LoadTexture(tc.dirt, Color.SteelBlue);
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
