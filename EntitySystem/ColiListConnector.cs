using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enums.ColiObjTypes;

namespace EntSys
{


    class ColiListConnector
    {
        public ColiSys.Hashtable hashTable;
        public ColiTypes type;


        private object obj;

        public ColiListConnector(Ground g)
        {
            _ground = g;
            hashTable = g.htable;
            type = ColiTypes.Dirt;

        }

        public ColiListConnector(Explosion e)
        {
            _exp = e;
            hashTable = e.htable;
            type = ColiTypes.Explosion;

        }

        public object getObj()
        {
            switch (type)
            {
                case ColiTypes.Dirt:
                    return _ground;
                    break;

                case ColiTypes.Explosion:
                    return _exp;
                    break;

                default:
                    return null;
                    break;
            }
        }

    }
}
