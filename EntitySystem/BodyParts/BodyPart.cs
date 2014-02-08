using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntSys;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
//addition of all mass!


namespace BodyParts
{
    public enum BodyPartType { Wings };


    class BodyPart : Sprite
    {
               
        public BodyPartType partType;
        public BodyPart()
        { }
        protected void Update(float rt)
        {
            //This should upgrade physical things on body part

        }
        //TakeDamage(float, DamageType, dir)
        //

        public void MovePartBy(Vector2 moveBy)
        {
            offset += new Structs.S_XY((int)moveBy.X, (int)moveBy.Y);
            rawOffSet += moveBy;
        }
        
       
    }
}
