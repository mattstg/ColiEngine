using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace EntSys
{
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
    }

    public class Material : Sprite
    {
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
        public bool MaterialIsTopScope = false;
       // public ColiSys.Hashtable htable;
        //public ActionEvent AE; //public because other objects will call Grounds event since ground does not update
        //I imagine a ground factory object that creates types of ground
        public Material(float hp, float bounceForceMultLB, float bounceForceMultUB, float bounceThreshold, float absorb, float thornDmg, float stickyness, MaterialResistances matRez, ColiSys.Hashtable htable, float friction, MaterialTypes type, bool isTopScope, DNA dna, Structs.S_XY loc)
        {
            
            this.hp = hp; this.bounceForceMultLB = bounceForceMultLB; this.bounceForceMultUB = bounceForceMultUB; this.bounceThreshold = bounceThreshold;
            this.absorb = absorb; this.thornDmg = thornDmg; this.stickyness = stickyness; //armor struct
            this.matRez = matRez; SetEntShape(htable); this.friction = friction; this.matType = type;
            offset = new Structs.S_XY(loc);
            rawOffSet.X = offset.x;
            rawOffSet.Y = offset.y;

            MaterialIsTopScope = isTopScope;
            if (isTopScope) //if this is the highest scope, it creates the action event, otherise it gets created elsewhere
            {
                specType = objSpecificType.Material;
                AE = new ActionEvent(new VagueObject(this));
            }
            base.ForceCnstr(dna);
        }

        private void _ForceCnstr(DNA dna)
        {
            base.ForceCnstr(dna);
        }

        private void _DecodeDNA(DNA dna)
        {



        }
        public Material() { }
        public Material(DNA dna) { _ForceCnstr(dna); }//AE = new ActionEvent(new VagueObject(this)); }


        public float GetBounceForce(float tforce, ColiSys.Node colibox, Vector2 dirOfCollider )
        {
            if (MaterialIsTopScope)
                return GetBounceForceWithSubtract(tforce, colibox, dirOfCollider);
            else
                return GetBounceForceWithoutSubtract(tforce, colibox, dirOfCollider);

        }

        public float GetBounceForceWithSubtract(float tforce, ColiSys.Node coliBox, Vector2 dirOfCollider)
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
                    //Hashtable needs to be moved by offset!!!!
                    //EntHashtable.HashSubtractor(coliBox); //subtract colibox from ground
                    EntHashtable = new ColiSys.Hashtable(nami.MoveTableByOffset(nami.SubtractNodes(trueEntShapeOffset, coliBox),offset*-1));
                    SetEntShape(EntHashtable);

                    if (force > hp * absorb)
                        return (hp * absorb)*-1*mag; //max force returnable
                    else
                        return -1 * mag * force + -1 * mag;   //less than max, returns current force since all absorbed --future if dirt is object, return energy abosrded to the Material/Metals/Earth object
                //break the ground using vo colibox                  
                
                }

            }

        }


        public float GetBounceForceWithoutSubtract(float tforce, ColiSys.Node coliBox, Vector2 dir)
        {
            float fMult = 1;
            float force = Math.Abs(tforce);
            int mag = (int)(Math.Abs(tforce) / tforce);

          
            float momDif = 0;
            Vector2 thisObjVelo = momentum;
            if (specType == objSpecificType.BodyPart)
                thisObjVelo = Master.momentum;

            if (dir.X == 0)
                momDif = tforce - thisObjVelo.Y;
            else
                momDif = tforce - thisObjVelo.X;

            momDif /= 2;
            this.ApplyForce(Enums.Force.ForceTypes.Coli, new Vector2(4000, 0));
            return momDif * -1;
            
             /*
            if (force < hp)
            {           //doesnt break it but bounces    
                fMult = (bounceForceMultUB - bounceForceMultLB) * (force / hp - bounceThreshold) + bounceForceMultLB;
                float t = mag * (force + force * fMult);
                this.ApplyForce(Enums.Force.ForceTypes.Coli, -1 * mag * (force + force * fMult) * dir);
                return -1 * mag * (force + force * fMult);
               }
            else
            {
                float t = mag * (force + force * fMult);
                this.ApplyForce(Enums.Force.ForceTypes.Coli, -1 * mag * (force + force * fMult) * dir);
                return -1 * mag * force + -1 * mag; //not enough force to trigger break or bounce, return his force as ground Normal balancing out
            }*/
            
        }
    }
}
