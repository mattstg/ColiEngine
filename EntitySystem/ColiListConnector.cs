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
        public object obj;



        public ColiListConnector(Ground g)
        {
            obj = g;
            hashTable = g.htable;
            type = ColiTypes.Dirt;

        }

        public ColiListConnector(Explosion e)
        {
            obj = e;
            hashTable = e.htable;
            type = ColiTypes.Explosion;

        }

        

    }
}
