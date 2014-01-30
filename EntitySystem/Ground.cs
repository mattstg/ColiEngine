using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structs.EntStructs;

namespace MSquared.ColiEngine.EntitySystem
{
    class Ground
    {
        //so single object with these vars, a type, energy, 
        float hp = 15;
        float bounceThreshold = .33f; //bounce within 90-100%
        float bounceForceMultLB = .2f;
        float bounceForceMultUB = 1f;
        float absorb = 3; //times it own health worth of force repelling        \
        ArmorResistance armor;
        float friction;
        float thornDmg = 0;  //0-inf percent dmg back
        float stickyness = 0; //not sure yet
        public ColiSys.Hashtable htable;

        //I imagine a ground factory object that creates types of ground
        public Ground(float hp, float bounceForceMultLB, float bounceForceMultUB, float bounceThreshold, float absorb, float thornDmg, float stickyness, ArmorResistance armor, ColiSys.Hashtable htable, float friction)
        {
            this.hp = hp; this.bounceForceMultLB = bounceForceMultLB; this.bounceForceMultUB = bounceForceMultUB; this.bounceThreshold = bounceThreshold;
            this.absorb = absorb; this.thornDmg = thornDmg; this.stickyness = stickyness; //armor struct
            this.armor = armor; this.htable = htable; this.friction = friction;
        }


        public bool ColiWithGround(ColiSys.Hashtable coliBox)
        {
            return htable.Coli(coliBox.RetMainNode());

        }

        public float GetBounceForce(float force)
        {
            float fMult = 1;

            if (force < hp * bounceThreshold)
                return force + 1; //not enough force to trigger break or bounce, return his force as ground Normal balancing out
            else
            {
                if (force < hp)
                {           //doesnt break it but bounces    
                    fMult = (bounceForceMultUB - bounceForceMultLB) * (force / hp - bounceThreshold) + bounceForceMultLB;
                    return force + force * fMult;
                }
                else  //breaks it
                    if (force > hp * absorb)
                        return hp * absorb; //max force returnable
                    else
                        return force + 1;  //less than max, returns current force since all absorbed --future if dirt is object, return energy abosrded to the Material/Metals/Earth object
            }

        }
    }
}
