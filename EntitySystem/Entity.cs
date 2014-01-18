using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColiSys;  

namespace EntitySys
{
    class Entity
    {
        S_XY size;
        S_XY loc;
        Node sizeLoc;


        public Entity(S_XY tloc,S_XY tsize) { ForceCnstr(tloc,tsize); }


        public void ForceCnstr(S_XY tloc,S_XY tsize)
        {
            size = tsize;
            loc = tloc;
            _SetSizeInNodeForm();
        }

        private void _SetSizeInNodeForm()
        {
            Node temp = new Node(loc.x, loc.x + size.x);
            temp.Dwn(new Node(loc.y - size.y, loc.y));
            sizeLoc = temp;
        }

        public Node RetSizeLocCopy()
        {
            return sizeLoc.CopySelf(copyTypes.copyDwn);
        }

    }
}
