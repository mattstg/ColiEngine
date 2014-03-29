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

        /// <summary>
        /// 0 for normal dirt material, 1 for indestructible
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public MatCnstr CreateMaterial(int pattern)
        {
            MatCnstr tr = new MatCnstr();
        //so single object with these vars, a type, energy, 
        
        ColiSys.Hashtable htable = new ColiSys.Hashtable();
        AdditionalInfo addInfo = new AdditionalInfo();



            switch (pattern)
            {
                case 0: //normal dirt
                    
                    tr.hp = 300;
                    tr.bounceThreshold = .60f; //bounce within -100%
                    tr.bounceForceMultLB = .2f;
                    tr.bounceForceMultUB = 1f;
                    tr.absorb = 1; //times it own health worth of force repelling        \
                    tr.matRez = new MaterialResistances();
                    tr.matRez.Dirt = 1;
                    tr.matRez.Steel = 1;
                    tr.matType = MaterialTypes.Dirt;
                    tr.friction = .5f;
                   // tr.loc = new S_XY(0, 0);
                    tr.thornDmg = 0;  //0-inf percent dmg back
                    tr.stickyness = 0; //not sure yet
                    //addInfo = new AdditionalInfo();


                    htable = new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(Consts.TopScope.WORLD_SIZE_X, Consts.TopScope.WORLD_SIZE_Y / 2), addInfo));
                    htable.ResetMainNode(nami.MoveTableByOffset(htable.RetMainNode(), new S_XY(0, Consts.TopScope.WORLD_SIZE_Y / 2)));
                   // htable.LoadTexture(tc.dirt, Color.White);
                    //tr = new Material(hp, bounceForceMultLB, bounceForceMultUB, bounceThreshold, absorb, thornDmg, stickyness, armor, htable, friction,matType,true,null,loc);
                    //tr.LoadTexture(tc.sqr, Color.Brown);
                    /*SprCnstr sc = new SprCnstr();
                    sc.color = Color.Brown;
                    sc.texture = tc.sqr;
                    EntCnstr ec = new EntCnstr();
                    ec.mass = 10;*/

                    return tr;
                    break;

                case 1: //Indestructible wall
                    tr.hp = 1000000;
                    tr.bounceThreshold = .9f; //bounce within -100%
                    tr.bounceForceMultLB = .2f;
                    tr.bounceForceMultUB = 1f;
                    tr.absorb = 100; //times it own health worth of force repelling        \
                    tr.matRez = new MaterialResistances();
                    tr.matRez.Dirt = .001f;
                    tr.matRez.Steel = .001f;
                    tr.matType = MaterialTypes.Indestructible;
                    tr.friction = .5f;
                    //tr.addInfo.width = 5;
                    //tr.loc = new S_XY(0, 0);
                    tr.thornDmg = 0;  //0-inf percent dmg back
                    tr.stickyness = 0; //not sure yet
                    
                   
                    htable = new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.HollowSqaure, new Structs.S_XY(Consts.TopScope.WORLD_SIZE_X, Consts.TopScope.WORLD_SIZE_Y),addInfo));
                    

                    //tr = new Material(hp, bounceForceMultLB, bounceForceMultUB, bounceThreshold, absorb, thornDmg, stickyness, armor, htable, friction, matType,true,null,loc);
                   // tr.LoadTexture(tc.dirt, Color.SteelBlue);
                    /*SprCnstr sc2 = new SprCnstr();
                    sc.color = Color.Brown;
                    sc.texture = tc.sqr;
                    EntCnstr ec2 = new EntCnstr();
                    ec.mass = 10;*/

                    return tr;
                    break;

                default:
                    return CreateMaterial(0);
                    break;


            }
            

        }
        //Decoder, translates input into proper constructor feed


        //Creator, creates object





    }
}
