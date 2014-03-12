using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FactSys
{
    class MaterialDesigner
    {

        private static MaterialDesigner instance;
        private MaterialDesigner()
        {

        }
        public static MaterialDesigner Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MaterialDesigner();
                }
                return instance;
            }
        }
        


    }
}
