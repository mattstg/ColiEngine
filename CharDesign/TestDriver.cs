using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FactSys;
using ColiSys;
using EntSys;
using BodyParts;


namespace ColiSys
{
    class TestDriver
    {
        BodyPartFactory bpFact;


        public TestDriver()
        {
            //like a program main
            bpFact = BodyPartFactory.Instance;

            if(false) //To active, set to true
                 MattDriver();


            if (false) ; //to deactivate, set to false
                  MikeDriver();

            
            int i = 5;

            






        }

        public void MikeDriver()
        {
            /////////
            //O//O///
            // _  ///
            /////////
         /*   ////
         /////////
             /////
             /////
          */// ///
            
        }


        public void MattDriver()
        {
            int alphagetti;
            BodyPart bp = bpFact.CreateBodyPart(0);
            List<BodyPart> bpTest = new List<BodyPart>();
            bpTest.Add(bp);
            bpTest.Add(null);
            bpTest.Add(bpFact.CreateBodyPart(0));
            if(bpTest[1] == null)
                alphagetti = 9;

            int i = 6;

        }





    }
}
