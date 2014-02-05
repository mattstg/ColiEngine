using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntSys
{
    struct unTargetables
    {
        ColiListConnector obj;
        Global.Timers timer;

    }


    class Explosion : Sprite
    {
        Global.Timers lifeSpan;
        ColiSys.NodeManipulator nami = ColiSys.NodeManipulator.Instance;
        public bool Destroy = false;
        ActionEvent AE = new ActionEvent(objType.Explosion);
        float force;
        List<unTargetables> untargetableList;
        public ColiSys.Hashtable htable; //made public for connector to access
        
        
        public Explosion(ColiSys.Node newHeadNode,Structs.S_XY offset,float force,List<unTargetables> untargetableList)
        {
            acceptedColi = new AcceptedCollidables(true, true, false);
            HashTrueEntShape = new ColiSys.Hashtable(newHeadNode);
            htable = HashTrueEntShape;
            this.offset = offset;
            lifeSpan = new Global.Timers(250);
            this.force = force;
            this.untargetableList = untargetableList;

        }

       // virtual void ApplyDmg(); //unsure about this atm..

        public void Update(float rt)
        {
            lifeSpan.Tick(rt);
            if (!lifeSpan.ready)            
                Console.Out.Write("BOOM!!!");            
            else            
                Destroy = true;
            

            _CheckAllColi();

        }

        public void _CheckAllColi()
        {
            Enums.ColiObjTypes.ColiTypes coliOccur = Enums.ColiObjTypes.ColiTypes.None;
            if (Collidables != null)
            foreach (ColiListConnector connecter in Collidables)
            {                
               if (connecter.hashTable.Coli(this.coliBox))  //so a coli has occured
                {
                   coliOccur = connecter.type;
                   switch (coliOccur) //switch->type object bodymech has colided with, and call their reactions based on that
                        {
                        case Enums.ColiObjTypes.ColiTypes.Magic:
                            break;

                        case Enums.ColiObjTypes.ColiTypes.Dirt:
                          // AE.TriggerEvent(this,(Ground)connecter.getObj));
                            _ColiWithGround((Ground)connecter.obj);
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
