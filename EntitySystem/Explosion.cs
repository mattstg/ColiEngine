using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntSys
{
    struct unTargetables
    {
        VagueObject obj;
        Global.Timers timer;

    }


    class Explosion : Sprite
    {
        Global.Timers lifeSpan;
        ColiSys.NodeManipulator nami = ColiSys.NodeManipulator.Instance;
        ActionEvent AE = new ActionEvent(objType.Explosion);
        float force;
        List<unTargetables> untargetableList;
        public ColiSys.Hashtable htable; //made public for connector to access
        
        
        public Explosion(ColiSys.Node newHeadNode,Structs.S_XY offset,float force,List<unTargetables> untargetableList)
        {
            acceptedColi = new List<objType>();
            acceptedColi.Add(objType.Ground);
            acceptedColi.Add(objType.Explosion);
            acceptedColi.Add(objType.Body);
            HashTrueEntShape = new ColiSys.Hashtable(newHeadNode);
            htable = HashTrueEntShape;
            this.offset = offset;
            lifeSpan = new Global.Timers(250);
            this.force = force;
            this.untargetableList = untargetableList;
            base.ForceCnstr(null, null);

        }

       // virtual void ApplyDmg(); //unsure about this atm..

        public void Update(float rt)
        {
            lifeSpan.Tick(rt);
            if (!lifeSpan.ready)            
                Console.Out.Write("BOOM!!!");            
            else            
                destroy = true;
            

            _CheckAllColi();

        }

        public void _CheckAllColi()
        {
            objType coliOccur = objType.None;
            if (Collidables != null)
            foreach (VagueObject connecter in Collidables)
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
            }
        }
        
        private void _ColiWithGround(Ground g)
        {
            
            //explosion hits ground, tears through it, add feature to lessen explosion later
            AE.TriggerEvent(this, g);
            g.htable.HashSubtractor(trueEntShapeOffset); //eventaually just call Ground.DmgArea() or sumtin

        }
        

    }
}
