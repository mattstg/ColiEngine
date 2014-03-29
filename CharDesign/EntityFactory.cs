using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntSys;


namespace FactSys
{
    class EntityFactory
    {
        ColiSys.ShapeGenerator sgen = ColiSys.ShapeGenerator.Instance;



        private static EntityFactory instance;
        private EntityFactory() { }
        public static EntityFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityFactory();
                }
                return instance;
            }
        }


        /// <summary>
        /// calls for EntCnstr, 
        /// 0 - p1,  1 - p2, 2 - dirtOnFloor, 3 - mapBorder
        /// </summary>
        /// <param name="loadout"></param>
        /// <returns></returns>
        public EntCnstr GenerateEntC(int loadout)
        {
            EntCnstr ec = new EntCnstr();
            //ec.mass = 10;
            //ec.entShape
            //ec.startOffset

            switch (loadout)
            {
                case 0:
                    ec.startOffset = new Structs.S_XY(50, 50);
                    ec.entShape = new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(10, 10)));

                    break;

                case 1:
                    ec.startOffset = new Structs.S_XY(250, 50);
                    ec.entShape = new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(10, 10)));


                    break;

                case 2:
                    ec.startOffset = new Structs.S_XY(0, Consts.TopScope.WORLD_SIZE_Y/2);
                    ec.entShape = new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.Square, new Structs.S_XY(Consts.TopScope.WORLD_SIZE_X, Consts.TopScope.WORLD_SIZE_Y/2)));

                    break;

                case 3:
                    ec.startOffset = new Structs.S_XY(0, 0);
                    ColiSys.AdditionalInfo ai = new ColiSys.AdditionalInfo();
                    ai.width = 5;
                    ec.entShape = new ColiSys.Hashtable(sgen.GenShape(ColiSys.Shape.HollowSqaure, new Structs.S_XY(Consts.TopScope.WORLD_SIZE_X, Consts.TopScope.WORLD_SIZE_Y),ai));
                    break;

            }

            return ec;

        }




    }
}
