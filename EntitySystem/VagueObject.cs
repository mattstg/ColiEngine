using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enums.ColiObjTypes;

namespace EntSys
{
    /*
     * Please remmeber, any changes to the vairables in VagueObject would
     * require changes to be made to GetNext in VOContainer because of the weird pointer problem
     */ 

    public class VagueObject
    {
       // public ColiSys.Hashtable hashTable;
        public objBaseType baseType;
        public objType type;
        public objSpecificType specificType;        
        public object obj;
        public ColiSys.Node coliBox { get { return _GetColiBox(); } }

        public VagueObject()
        { //calling this when using it as a pointer for GetNext

        }


        public VagueObject(Ground g)
        {
            obj = g;
            baseType = objBaseType.Ground;
            type = objType.Ground;
            specificType = objSpecificType.Ground;

        }

        public VagueObject(Explosion e)
        {
            obj = e;
          
            baseType = objBaseType.Ent;
            type = objType.Explosion;
            specificType = objSpecificType.Exp;

        }

        public VagueObject(HumanPlayer h)
        {
            obj = h;
            type = objType.Body;
            baseType = objBaseType.Ent;
            specificType = objSpecificType.Human;

        }

        public VagueObject(BodyParts.BodyPart bp)
        {
            obj = bp;
            type = objType.Body;
            baseType = objBaseType.Ent;
            specificType = objSpecificType.BodyPart;

        }

        public VagueObject(Material m)
        {
            obj = m;
            type = objType.Ground;
            baseType = objBaseType.Ent;
            specificType = objSpecificType.Material;

        }



        public T getObj<T>()
        {
            return (T)obj;

        }

        //This can only be called 
        public bool Coli(Structs.S_Box sbox)
        {
            switch (baseType)
            {
                case objBaseType.Ground:
                    Ground g = (Ground)obj;
                    return g.htable.Coli(sbox);


                case objBaseType.Ent:
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
            if (specificType == objSpecificType.Human)
            {
                HumanPlayer h = (HumanPlayer)obj;
                h.Draw();

            } else if (baseType == objBaseType.Ent)
            {
                Sprite t = (Sprite)obj;
                t.Draw();

            }
            else if (baseType == objBaseType.Ground)
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
                case objSpecificType.Bm:
                    BodyMechanics bm = (BodyMechanics)obj;
                    bm.Update(rt);
                    
                    break;
                case objSpecificType.Body:
                    Body b = (Body)obj;
                    b.Update(rt);
                    break;
                case objSpecificType.Ent:
                    Entity ent = (Entity)obj;
                    ent.Update(rt);
                    break;
                case objSpecificType.Human:
                    HumanPlayer h = (HumanPlayer)obj;
                    h.Update(rt);
                    break;
                case objSpecificType.Sprite:
                    Sprite s = (Sprite)obj;
                    s.Update(rt);
                    break;
                case objSpecificType.Exp:
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
            if (baseType == objBaseType.Ent)
            {
                Entity t = (Entity)obj;
                return t.destroy;

            }
            else if (baseType == objBaseType.Ground)
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
        private ColiSys.Node _GetColiBox()
        {
            switch (baseType)
            {
                case objBaseType.Ent:
                    Entity t = (Entity)obj;
                    return t.coliBox; 
                    
                case objBaseType.Ground:
                    Ground g = (Ground)obj;
                    return g.htable.RetMainNode();
                    
                default:
                    Console.Out.WriteLine("Default type asked for colibox");
                    break;
            }

                    return new ColiSys.Node(0, 0);

            


        }
    }
}
