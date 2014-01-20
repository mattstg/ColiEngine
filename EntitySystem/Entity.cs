using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structs;
using NodeEnum;

namespace EntSys
{
    class Entity
    {
        S_XY size;
        S_XY loc;
        ColiSys.Node sizeLoc;

        public Entity() { }
        public Entity(DNA dna) { ForceCnstr(dna); }


        protected void ForceCnstr(DNA dna)
        {
            //size = tsize;
            //loc = tloc;
            _DNACopier(dna);
            _SetSizeInNodeForm();
        }

        private void _DNACopier(DNA dna)
        {
            //Need DNA copier

        }

        private void _SetSizeInNodeForm()
        {
            ColiSys.Node temp = new ColiSys.Node(loc.x, loc.x + size.x);
            temp.Dwn(new ColiSys.Node(loc.y - size.y, loc.y));
            sizeLoc = temp;
        }

        public ColiSys.Node RetSizeLocCopy()
        {
            _SetSizeInNodeForm();
            return sizeLoc.CopySelf(copyTypes.copyDwn);
        }

    }
}
