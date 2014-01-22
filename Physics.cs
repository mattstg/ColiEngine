using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PhysSys
{
    class Physics
    {
         private static Physics instance;
         private Physics() { }
         public static Physics Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Physics();
                }
                return instance;
            }
        }

         public void applyNaturalLaws(EntSys.BodyMechanics body)
         {
             body.ApplyForce(Enums.Force.ForceTypes.NaturalLaw, new Vector2(0, Consts.World.gravity));
         }
    }
}
