using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntSys;

namespace BodyParts
{
    public enum BodyPartType { Wings };


    class BodyPart : Sprite
    {
        public BodyPartType partType;
        public BodyPart()
        { }

    }
}
