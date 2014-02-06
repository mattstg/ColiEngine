using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enums.ColiObjTypes;

namespace EntSys
{


    class VagueObject
    {
       // public ColiSys.Hashtable hashTable;
        public objBaseType baseType;
        public objType type;
        public objSpecificType specificType;

        public object obj;



        public VagueObject(Ground g)
        {
            obj = g;
            baseType = objBaseType.ground;
            type = objType.Ground;
            specificType = objSpecificType.ground;

        }

        public VagueObject(Explosion e)
        {
            obj = e;
          
            baseType = objBaseType.ent;
            type = objType.Explosion;
            specificType = objSpecificType.exp;

        }

        public VagueObject(HumanPlayer h)
        {
            obj = h;
            type = objType.Body;
            baseType = objBaseType.ent;
            specificType = objSpecificType.human;

        }


        public T getObj<T>()
        {
            return (T)obj;

        }

        //This can only be called 
        public bool Coli(Structs.S_Box sbox)
        {
            switch (type)
            {
                case objType.Ground:
                    Ground g = (Ground)obj;
                    return g.htable.Coli(sbox);


                case objType.Explosion:
                case objType.Body:
                    Entity t = (Entity)obj;
                    return t.Coli(sbox);

                default:
                    return false;
            }
        }

        public bool Coli(ColiSys.Node n)
        {
            ColiSys.NodeManipulator nami = ColiSys.NodeManipulator.Instance;
            Structs.S_Box sbox = nami.NodetoBox(n);

            return Coli(sbox);
        }

        //Needs a .draw and a .update and .destroy
        public void Draw()
        {
            if (baseType == objBaseType.ent)
            {
                Sprite t = (Sprite)obj;
                t.Draw(ColiSys.Game1.spriteBatch);

            }
            else if (baseType == objBaseType.ground)
            {
                Ground g = (Ground)obj;
                g.Draw(ColiSys.Game1.spriteBatch);
            }
            else
            {
                Console.Out.WriteLine("Error: Vague object Draw called Misc");

            }


        }
        public void Update(float rt)
        {
            switch (specificType)
            {
                case objSpecificType.bm:
                    BodyMechanics bm = (BodyMechanics)obj;
                    bm.Update(rt);
                    
                    break;
                case objSpecificType.body:
                    Body b = (Body)obj;
                    b.Update(rt);
                    break;
                case objSpecificType.ent:
                    Entity ent = (Entity)obj;
                    ent.Update(rt);
                    break;
                case objSpecificType.human:
                    HumanPlayer h = (HumanPlayer)obj;
                    h.Update(rt);
                    break;
                case objSpecificType.sprite:
                    Sprite s = (Sprite)obj;
                    s.Update(rt);
                    break;
                case objSpecificType.exp:
                    Explosion exp = (Explosion)obj;
                    exp.Update(rt);
                    break;
                default:
                    //kinds like ground and such dont get updated
                    break;



            }
            


        }
        public bool Destroy()
        {
            if (baseType == objBaseType.ent)
            {
                Entity t = (Entity)obj;
                return t.destroy;

            }
            else if (baseType == objBaseType.ground)
            {
                Ground g = (Ground)obj;
                return g.destroy;
            }
            else
            {
                Console.Out.WriteLine("Error: Vague object Destroy called Misc");
                return false;
            }


        }
    }
}
