using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FactSys;
namespace BodyParts
{
    public struct bodyItem
    {
        public int id;
        public BpConstructor bp;

        public bodyItem(int id, BpConstructor bp)
        {
            this.id = id;
            this.bp = bp;
        }
    }

    public class BodyPartStore
    {
        
        ColiSys.NodeManipulator nami = ColiSys.NodeManipulator.Instance;
        List<bodyItem> bodyList;
        BodyPartSaveLoader bpSaveLoader;
        
        private static BodyPartStore instance;
        private BodyPartStore()
        {
            bodyList = new List<bodyItem>();
            bpSaveLoader = BodyPartSaveLoader.Instance;
            LoadAllPartsFromFilesIntoStore();
        
        }
        public static BodyPartStore Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BodyPartStore();
                }
                return instance;
            }
        }

        public void LoadAllPartsFromFilesIntoStore()
        {
            List<int> allIDsFromFile = bpSaveLoader.RetrieveAllIDs();

            foreach (int id in allIDsFromFile)
            {
                bodyItem bi = new bodyItem(id, bpSaveLoader.LoadFileIntoBpc(id));
                bodyList.Add(bi);
            }




        }

        /// <summary>
        /// Store a bodypart in the store, returns false if ID is not unique, part does not get stored
        /// </summary>
        /// <param name="UniqueID">pass in unique id for retreival later</param>
        /// <param name="bp"></param>
        /// <returns>returns false if id not unique and does not get stored</returns>
        public bool StoreBodyPart(int uniqueID, BpConstructor bpC)
        {
            bool valid = true;
            foreach (bodyItem bi in bodyList)            
                if (uniqueID == bi.id)
                    return false;
            //double validation, why not
            if (bpSaveLoader.DoesIDExist(uniqueID))
                return false; //if this return false does occur, that is disconcering


            
                //seperate offset from table
           bodyList.Add(new bodyItem(uniqueID, bpC));            
           bpSaveLoader.SaveBodyPart(bpC, uniqueID);
                //body part added to store, needs to be saved to disk

           return true;
           

            
        }

        public BodyPart LoadBodyPart(int id)
        {
            foreach (bodyItem bi in bodyList)
                if (id == bi.id)
                    return new BodyPart(bi.bp);

            return null;

        }

        /// <summary>
        /// Due to lazyness, returns actauly BpC list, careful with it
        /// </summary>
        /// <returns></returns>
        public List<bodyItem> ReturnBpcList()
        {
            return bodyList;
        }





    }
}
