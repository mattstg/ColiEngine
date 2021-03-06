﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
 *  how to use in while loops
 *  ex) voContainer named voc
 * 
 * VagueObject vo = new VagueObject();
 * voc.ResetIT();
 * While(voc.GetNext(vo))
 * {
 *      //use vo how you want
 * 
 * }
 * 
 */

namespace EntSys
{
    public class VOContainer
    {
        List<VagueObject> voList;
        Object ntAllowed;
        int it;
        public int Count { get { return voList.Count; } }

        //literally type "this", its for checking unique objects
        public VOContainer(object typeThis)
        {
            ntAllowed = typeThis;
            voList = new List<VagueObject>();   
        }

        /// <summary>
        /// Returns List to be used, DO NOT USE TO ADD, only use for retrieval purposes, returned actaul list, not a copy, using add directly from list will overwrite safety checks such as uniqueness, massive problem
        /// </summary>
        /// <returns></returns>
        public List<VagueObject> ReturnList()
        {
            return voList;
        }

        public void Add(VagueObject vo)
        {
            bool isUnique = true;
            if (vo.obj != null && vo.obj != ntAllowed)
            {
                foreach (VagueObject v in voList)
                    if (vo == v)
                        isUnique = false;
                if (isUnique)
                    voList.Add(vo);
            }
            
          
        }

        public void Add(List<VagueObject> vol)
        {
            foreach (VagueObject vo in vol)
                Add(vo);           

        }

        public void Add(VOContainer voc)
        {
            
            VagueObject tvo = new VagueObject();
            voc.ResetIT();
            while (voc.GetNext(ref tvo))
            {
                Add(tvo);
            }

        }

        private void _PruneList()
        {
            for (int i = voList.Count - 1; i >= 0; i--)
            {
                if (voList[i] == null || voList[i].Destroy())
                    voList.RemoveAt(i);

            }

        }

        public void ResetIT()
        {
            if (voList.Count > 0)
                it = voList.Count - 1;

        }

        

        public bool GetNext(ref VagueObject o)
        {
            if (it < 0)
                return false;
            if (voList[it] == null || voList[it].Destroy())
            {
                voList.RemoveAt(it);
                it--;
                return GetNext(ref o);
            }
            else
            {
                o = voList[it];/*
                o.type = voList[it].type;
                o.specificType = voList[it].specificType;
                o.baseType = voList[it].baseType;
                o.obj = voList[it].obj; //oddly, it loses internal ptrs*/
                it--;
                return true;
            }           
        }

        
        




    }
}
