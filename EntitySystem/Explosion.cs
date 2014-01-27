using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntSys
{
    class Explosion : Sprite
    {
        Global.Timers lifeSpan;
        ColiSys.NodeManipulator nami = ColiSys.NodeManipulator.Instance;
        public bool Destroy = false;
        

        public Explosion(ColiSys.Node newHeadNode,Structs.S_XY offset)
        {
            HashTrueEntShape = new ColiSys.Hashtable(newHeadNode);
            this.offset = offset;
            lifeSpan = new Global.Timers(250);

        }


        public void Update(float rt)
        {
            lifeSpan.Tick(rt);
            if (!lifeSpan.ready)
            {
                Console.Out.Write("BOOM!!!");

            }
            else
            {
                Destroy = true;

            }

            _CheckAllColi();

        }

        public void _CheckAllColi()
        {
            Enums.ColiObjTypes.ColiTypes coliOccur = Enums.ColiObjTypes.ColiTypes.None;

            foreach (Structs.ColiListConnector connecter in Collidables)
            {                
               if (connecter.hashTable.Coli(this.coliBox))  //so a coli has occured
                {
                   coliOccur = connecter.coliType;
                   switch (coliOccur) //switch->type object bodymech has colided with, and call their reactions based on that
                        {
                        case Enums.ColiObjTypes.ColiTypes.Magic:
                            break;

                        case Enums.ColiObjTypes.ColiTypes.Dirt:
                            _ColiWithGround(connecter.hashTable);
                            break;

                        default:
                            Console.Out.WriteLine("You hit default case in checking connector type in colision!");
                            break;
                        }
                    }            
            }
        }
        
        private void _ColiWithGround(ColiSys.Hashtable ht)
        {
            //explosion hits ground, tears through it, add feature to lessen explosion later
            ht.HashSubtractor(this.trueEntShape); //eventaually just call Ground.DmgArea() or sumtin

        }
        

    }
}
