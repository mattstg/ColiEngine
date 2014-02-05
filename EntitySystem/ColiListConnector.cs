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


        private EntSys.Ground _ground;

        public ColiListConnector(Ground g)
        {
            _ground = g;
            hashTable = g.htable;
            type = ColiTypes.Dirt;

        }

        public object getObj()
        {
            switch (type)
            {
                case ColiTypes.Dirt:
                    return _ground;
                    break;

                default:
                    return null;
                    break;
            }
        }

    }
}
