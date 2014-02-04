using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Global
{
    enum PassengerType { Ground, Explosion }

    /*
    public struct Passenger
    {
        public ColiSys.Hashtable Hashtable;
        PassengerType type;
        public Structs.S_XY offset;

    }*/

    class Bus
    {

        List<EntSys.Ground> GroundList = new List<EntSys.Ground>();

        private static Bus instance;
        private Bus() { }
        public static Bus Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Bus();
                }
                return instance;
            }
        }       
        
        //Load//////////////////////////
        public void LoadPassenger(EntSys.Ground pass)
        { GroundList.Add(pass); }





        //Unload/////////////////////////////
        public List<EntSys.Ground> UnloadGround(bool del)
        {
            if (del)
            {
                List<EntSys.Ground> toRet = new List<EntSys.Ground>();
                GroundList.Clear();
                return toRet;
            }
            else            
                return GroundList;
        }

       


    }
}
