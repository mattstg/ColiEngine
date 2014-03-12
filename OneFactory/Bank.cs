using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BodyParts;
using EntSys;
using ColiSys;
using FactSys;

namespace BankSys
{
    class Bank
    {
        BodyPartSaveLoader bpSaveLoader;


        private static Bank instance;
        private Bank()
        {
            bpSaveLoader = BodyPartSaveLoader.Instance;
            bpList = new List<bodyItem>();
            matList = new List<Material>();
        }
        public static Bank Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Bank();
                }
                return instance;
            }
        }


        List<bodyItem> bpList;
        List<Material> matList;






        public void FillBpListFromFileSystem()
        {
            List<int> ids = bpSaveLoader.RetrieveAllIDs();
            foreach (int id in ids)
            {
                bool validEntry = true;
                bodyItem bi = new bodyItem(id, bpSaveLoader.LoadFileIntoBpc(id));
                foreach (bodyItem comp in bpList)
                    if (comp.id == id)
                        validEntry = false;

                if (validEntry)
                    bpList.Add(bi);

            }
        }

        public bool AddBodyItem(bodyItem newItem)
        {
            bool validEntry = true;
            foreach (bodyItem comp in bpList)
                if (comp.id == newItem.id)
                    validEntry = false;

            if (validEntry)
                bpList.Add(newItem);

            return validEntry;

        }


    }
}
