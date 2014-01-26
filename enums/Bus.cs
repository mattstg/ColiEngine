using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Global
{
    
    public struct Passenger
    {
        public ColiSys.Hashtable Hashtable;
        public Enums.Global.VoidableTypes type;
        public Structs.S_XY offset;

    }




    class Bus
    {

        List<Passenger> passengers = new List<Passenger>();

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

        
        public void LoadPassenger(ColiSys.Hashtable pass,Enums.Global.VoidableTypes type,Structs.S_XY offset)
        {
            Passenger newPass;
            newPass.Hashtable = pass;
            newPass.type = type;
            newPass.offset = offset;
            passengers.Add(newPass);

        }

        public List<Passenger> UnloadPassengersOfType(Enums.Global.VoidableTypes type)
        {
            List<Passenger> toRet = new List<Passenger>();
            for (int c = passengers.Count-1; c >= 0; c--)
            {
                if (passengers[c].type == type)
                {
                    toRet.Add(passengers[c]);
                    passengers.RemoveAt(c);
                }

            }
            
            return toRet;
        }

       


    }
}
