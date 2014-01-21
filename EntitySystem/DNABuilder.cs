using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structs;

namespace EntSys
{
    class DNABuilder
    {
        
        public DNABuilder(){}

        public DNA buildEntDNA(S_XY loc, S_XY size)
        {
            DNA toBuild = new DNA();
            toBuild.dDNA = new S_XY[] { loc,size };
            return toBuild;
        }

        public DNA createEmptyDNA()
        {
            return null;
        }
    }
}
