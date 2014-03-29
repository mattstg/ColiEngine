using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structs;
using BodyParts;

namespace EntSys
{
    public class DNA
    {
        public EntCnstr entC;
        public SprCnstr sprC;
        public MatCnstr matC;
        public BpConstructor bpC;
        public HumanCnstr humanC;

        public DNA(EntCnstr ec, SprCnstr sc, MatCnstr mc, BpConstructor bc, HumanCnstr hc)
        {
            entC = ec;
            sprC = sc;
            matC = mc;
            bpC = bc;
            humanC = hc;
        }
    }
}
