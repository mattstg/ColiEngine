﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace EntSys
{/*
    public struct MaterialResistances
    {
        //should name other kinds of materials
        public float dirt;
        public float steel;
        public float Indestructable;


    }
    //////Please update vBOTH^ when you update ONE!!
    public enum MaterialTypes
    {
        dirt,steel,Indestructible
    }*/

    public class Ground
    {
        ColiSys.NodeManipulator nami = ColiSys.NodeManipulator.Instance;
        public bool destroy { get { return (htable.RetMainNode() == null); } }
        //so single object with these vars, a type, energy, 
        float hp = 1000;
        float bounceThreshold = .33f; //bounce within -100%
        float bounceForceMultLB = .2f;
        float bounceForceMultUB = 1f;
        float absorb = 1; //times it own health worth of force repelling        \
        MaterialResistances matRez;
        MaterialTypes matType;
        float friction;
        float thornDmg = 0;  //0-inf percent dmg back
        float stickyness = 0; //not sure yet
        public ColiSys.Hashtable htable;
        public ActionEvent AE; //public because other objects will call Grounds event since ground does not update
        //I imagine a ground factory object that creates types of ground
        public Ground(float hp, float bounceForceMultLB, float bounceForceMultUB, float bounceThreshold, float absorb, float thornDmg, float stickyness, MaterialResistances matRez, ColiSys.Hashtable htable, float friction, MaterialTypes type)
        {
            this.hp = hp; this.bounceForceMultLB = bounceForceMultLB; this.bounceForceMultUB = bounceForceMultUB; this.bounceThreshold = bounceThreshold;
            this.absorb = absorb; this.thornDmg = thornDmg; this.stickyness = stickyness; //armor struct
            this.matRez = matRez; this.htable = htable; this.friction = friction; this.matType = type;
            AE = new ActionEvent(new VagueObject(this));
        }

        public Ground() { }//AE = new ActionEvent(new VagueObject(this)); }

        public bool ColiWithGround(ColiSys.Hashtable coliBox)
        {
            return htable.Coli(coliBox.RetMainNode());

        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            htable.Draw();
        }

        public float GetBounceForce(float tforce,  ColiSys.Node coliBox)
        {
            coliBox = nami.StretchSquareTableByXY(coliBox, new Structs.S_XY(-1, 1));   
            float fMult = 1;
            float force = Math.Abs(tforce);
            int mag = (int)(Math.Abs(tforce) / tforce);

            if (force < hp * bounceThreshold)
                return -1*mag*force + -1*mag; //not enough force to trigger break or bounce, return his force as ground Normal balancing out
            else
            {
                if (force < hp)
                {           //doesnt break it but bounces    
                    fMult = (bounceForceMultUB - bounceForceMultLB) * (force / hp - bounceThreshold) + bounceForceMultLB;
                    return -1*mag*(force + force * fMult);
                }
                else  //breaks it
                {
                    htable.HashSubtractor(coliBox); //subtract colibox from ground

                    if (force > hp * absorb)
                        return (hp * absorb)*-1*mag; //max force returnable
                    else
                        return -1 * mag * force + -1 * mag;   //less than max, returns current force since all absorbed --future if dirt is object, return energy abosrded to the Material/Metals/Earth object
                //break the ground using vo colibox                  
                
                }

            }

        }
    }
}
