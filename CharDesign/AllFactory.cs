using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FactSys
{
    class AllFactory
    {
        public AEManagerFactory AeMangFact;
        public AEPackFactory AePackFact;
        public BodyPartFactory BodyPartFact;
        public DNAFactory DnaFact;
        public EntityFactory EntFact;
        public HumanFactory HumanFact;
        public MaterialFactory MatFact;



        private static AllFactory instance;
        private AllFactory()
         {
             AEManagerFactory AeMangFact = AEManagerFactory.Instance;
             AEPackFactory AePackFact = AEPackFactory.Instance;
             BodyPartFactory BodyPartFact = BodyPartFactory.Instance;
             DNAFactory DnaFact = DNAFactory.Instance;
             EntityFactory EntFact = EntityFactory.Instance;
             HumanFactory HumanFact = HumanFactory.Instance;
             MaterialFactory MatFact = MaterialFactory.Instance;
             
         }
        public static AllFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AllFactory();
                }
                return instance;
            }
        }





    }
}
