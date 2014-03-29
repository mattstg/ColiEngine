using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntSys
{
    public struct unTargetables
    {
        VagueObject obj;
        Global.Timers timer;

    }


    public class Explosion : Sprite
    {
        Global.Timers lifeSpan;
        ColiSys.NodeManipulator nami = ColiSys.NodeManipulator.Instance;
        ActionEvent AE;
        float force;
        List<unTargetables> untargetableList;
        public ColiSys.Hashtable htable; //made public for connector to access

        public Explosion() { AE = new ActionEvent(new VagueObject(this)); }
        
        public Explosion(ColiSys.Node newHeadNode,Structs.S_XY offset,float force,List<unTargetables> untargetableList)
        {
            Console.Out.WriteLine("EXP Created");
            AE = new ActionEvent(new VagueObject(this));
            acceptedColi = new List<objType>();
            acceptedSColi = new List<objSpecificType>();
            acceptedColi.Add(objType.Ground);
            acceptedColi.Add(objType.Explosion);
            acceptedColi.Add(objType.Body);
            EntHashtable = new ColiSys.Hashtable(newHeadNode);
            htable = EntHashtable;
            this.offset = offset;
            lifeSpan = new Global.Timers(32);
            this.force = force;
            this.untargetableList = untargetableList;
            //base.ForceCnstr(null, null);

        }

       // virtual void ApplyDmg(); //unsure about this atm..

        public void Update(float rt)
        {
            lifeSpan.Tick(rt);
            if (!lifeSpan.ready)
                Console.Out.Write("BOOM!!!");
            else
            {
                Console.Out.WriteLine("EXP Destroyed");
                destroy = true;
            }
            

            _CheckAllColi();

        }

        public void _CheckAllColi()
        {
            /*objType coliOccur = objType.None;
            VagueObject connecter = new VagueObject(); //need to assign?
            Collidables.ResetIT();
            while (Collidables.GetNext(connecter))
            {         
       
               if (connecter.Coli(this.coliBox))  //so a coli has occured
                {
                   coliOccur = connecter.type;
                   switch (coliOccur) //switch->type object bodymech has colided with, and call their reactions based on that
                        {
                            case objType.Explosion:
                            break;

                            case objType.Ground:
                          // AE.TriggerEvent(this,(Ground)connecter.getObj));
                            _ColiWithGround(connecter.getObj<Ground>());
                            break;

                        default:
                            Console.Out.WriteLine("You hit default case in checking connector type in colision!");
                            break;
                        }
                    }            
            }*/
        }
        
        /*private void _ColiWithGround(Material g)
        {
            
            //explosion hits ground, tears through it, add feature to lessen explosion later
            AE.TriggerEvent(this, g);
            g.htable.HashSubtractor(trueEntShapeOffset); //eventaually just call Ground.DmgArea() or sumtin

        }*/
        

    }
}
