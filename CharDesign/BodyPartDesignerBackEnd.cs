using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FactSys
{
    class BodyPartDesignerBackEnd
    {

        
        private static BodyPartDesignerBackEnd instance;
        public static BodyPartDesignerBackEnd Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BodyPartDesignerBackEnd();
                }
                return instance;
            }
        }
        private BodyPartDesignerBackEnd()
        {
        

        }






    }
}
