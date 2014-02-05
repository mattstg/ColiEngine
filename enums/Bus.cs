using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Global
{
    public enum PassengerType { Ground, Explosion }

    
    public struct Passenger
    {
        public object pass;
        public PassengerType type;

        public Passenger(object o, PassengerType t) { pass = o; type = t; }
    }

    class Bus
    {

        List<Passenger> passList = new List<Passenger>();

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
        public void LoadPassenger(EntSys.Ground g)
        { passList.Add(new Passenger(g, PassengerType.Ground)); }

        public void LoadPassenger(EntSys.Explosion e)
        { passList.Add(new Passenger(e, PassengerType.Explosion)); }

        //Unload/////////////////////////////
        public List<object> Unload(PassengerType t)
        {
            List<object> toRet = new List<object>();
            for (int i = passList.Count - 1; i >= 0;i--)
            {
                if (passList[i].type == t)
                {
                    toRet.Add(passList[i].pass);
                    passList.RemoveAt(i);
                }
            }
            return toRet;
        }

       


    }
}
