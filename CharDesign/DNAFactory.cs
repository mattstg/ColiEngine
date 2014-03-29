using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntSys;
using BodyParts;
using Microsoft.Xna.Framework;

namespace FactSys
{
    /// <summary>
    /// What the dna is going to be used to create
    /// </summary>
    public enum DNAType
    {
        Human,BodyPart,Material
    }


    class DNAFactory
    {
        MaterialFactory materialFact = MaterialFactory.Instance;
        EntityFactory entFact = EntityFactory.Instance;
        BodyPartFactory bpFact = BodyPartFactory.Instance;
        
        ColiSys.TestContent tc = ColiSys.TestContent.Instance;


         private static DNAFactory instance;
         private DNAFactory() { }
         public static DNAFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DNAFactory();
                }
                return instance;
            }
        }

        /// <summary>
        /// For human, loadout 0-1 correspond to player
        /// For Material loadout 0-1 correspond to Dirt-Border
        /// </summary>
        /// <param name="type"></param>
        /// <param name="loadout"></param>
        /// <returns></returns>
        public DNA GenerateDNA(DNAType type, int loadout)
        {
            switch(type)
            {
                case DNAType.Human:
                    return _GenerateHumanDNA(loadout);
                    break;

                case DNAType.Material:
                    return _GenerateMaterialDNA(loadout);
                    break;

                case DNAType.BodyPart:
                    return _GenerateBodyPartDNA(loadout);
                    break;
            }

            return null;
        }



        public DNA _GenerateHumanDNA(int loadout)
        {
            //human dna requires ec,sc,mc,bpc,hc

            EntCnstr ec;
            SprCnstr sc;
            MatCnstr mc;
            BpConstructor bpc;
            HumanCnstr hc;
            DNA dna;

            switch (loadout)
            {
                case 0: //Create DNA for player 1
                    
                    ec = entFact.GenerateEntC(0);
                    sc = new SprCnstr();
                    sc.texture = tc.dirt;
                    sc.color = Color.Red;
                    mc = materialFact.CreateMaterial(0);
                    bpc = bpFact.GenerateBodyPartC(0);
                    hc = new HumanCnstr(0);                   

                    dna = new DNA(ec, sc, mc, bpc, hc);
                    return dna;
                    break;

                case 1: //Create DNA for player 2
                    ec = entFact.GenerateEntC(1);
                    sc = new SprCnstr();
                    sc.texture = tc.dirt;
                    sc.color = Color.Red;
                    mc = materialFact.CreateMaterial(0);
                    bpc = bpFact.GenerateBodyPartC(0);
                    hc = new HumanCnstr(1);                   

                    dna = new DNA(ec, sc, mc, bpc, hc);
                    return dna;
                    break;



            }
            return null;
        }

        public DNA _GenerateMaterialDNA(int loadout)
        {
            //to build material DNA, need Ent,Sprite,Material DNA levels
            //ent requires ent factory for shapes

            switch (loadout)
            {
                case 0: //Create DNA Material for main ground
                    EntCnstr ec = entFact.GenerateEntC(2);
                    SprCnstr sc = new SprCnstr();
                    sc.texture = tc.sqr;
                    sc.color = Color.BlanchedAlmond;
                    MatCnstr mc = materialFact.CreateMaterial(0);
                    mc.MaterialIsTopScope = true;
                    DNA dna = new DNA(ec, sc, mc, null, null);
                    return dna;
                    break;

                case 1: //create indestructible border
              
                    EntCnstr ec2 = entFact.GenerateEntC(3);
                    SprCnstr sc2 = new SprCnstr();
                    sc2.texture = tc.sqr;
                    sc2.color = Color.Silver;
                    MatCnstr mc2 = materialFact.CreateMaterial(1);
                    mc2.MaterialIsTopScope = true;
                    DNA dna2 = new DNA(ec2, sc2, mc2, null, null);
                    return dna2;
                    break;



            }
            return null;
        }

        public DNA _GenerateBodyPartDNA(int loadout)
        {
            //Body part DNA

            EntCnstr ec;
            SprCnstr sc;
            MatCnstr mc;
            BpConstructor bpc;
            DNA dna;

            switch (loadout)
            {
                case 0: //plain looking 10by10

                    ec = entFact.GenerateEntC(0);
                    sc = new SprCnstr();
                    sc.texture = tc.sqr;
                    sc.color = Color.Silver;
                    mc = materialFact.CreateMaterial(0);
                    bpc = bpFact.GenerateBodyPartC(0);
                    bpc.isTopLevel = true;
                    dna = new DNA(ec, sc, mc, bpc, null);
                    return dna;
                    break;

                case 1: //fancy looking 10by10
                    ec = entFact.GenerateEntC(1);
                    sc = new SprCnstr();
                    sc.texture = tc.dirt;
                    sc.color = Color.Red;
                    mc = materialFact.CreateMaterial(0);
                    
                    bpc = bpFact.GenerateBodyPartC(0);
                    bpc.isTopLevel = true;
                    dna = new DNA(ec, sc, mc, bpc, null);
                    return dna;
                    break;

                case 2: //fancy looking 10by10
                    ec = entFact.GenerateEntC(1);
                    sc = new SprCnstr();
                    sc.texture = tc.dirt;
                    sc.color = Color.Salmon;
                    mc = materialFact.CreateMaterial(0);
                    bpc = bpFact.GenerateBodyPartC(0);
                    bpc.isTopLevel = true;
                    dna = new DNA(ec, sc, mc, bpc, null);
                    return dna;
                    break;

                case 3: //Special body part with _TestGrowPart as timer
                    ec = entFact.GenerateEntC(1);
                    sc = new SprCnstr();
                    sc.texture = tc.dirt;
                    sc.color = Color.Salmon;
                    mc = materialFact.CreateMaterial(0);
                    bpc = bpFact.GenerateBodyPartC(1);
                    bpc.isTopLevel = true;
                    dna = new DNA(ec, sc, mc, bpc, null);
                    return dna;
                    break;



            }
            return null;
        }




    }
}
