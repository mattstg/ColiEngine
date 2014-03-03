using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BodyParts
{
    public class BodyPartSaveLoader
    {
        List<int> bodyIDs;
        ColiSys.NodeManipulator nami = ColiSys.NodeManipulator.Instance;

        private static BodyPartSaveLoader instance;
        private BodyPartSaveLoader()
        {
            bodyIDs = LoadAllIDs();
            LoadFileIntoBodyPart(2);
        
        }
        public static BodyPartSaveLoader Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BodyPartSaveLoader();
                }
                return instance;
            }
        }



        public List<int> LoadAllIDs()
        {
            List<int> toRet = new List<int>();
            string startupPath = Environment.CurrentDirectory;

            Directory.CreateDirectory(startupPath + "/SavedBodyParts");
            string[] files = Directory.GetFiles(startupPath + "/SavedBodyParts");
            List<string> fileNames = new List<string>();
            foreach (string file in files)
                try
                {
                    fileNames.Add(Path.GetFileName(file));
                }
                catch { }


            foreach (string file in fileNames)
            {
                try
                {
                    int t = Convert.ToInt32(file);
                    toRet.Add(t);
                }
                catch
                {
                    Console.Out.WriteLine("File: " + file + " could not be loaded, invalid name");
                }
               
            }


            return toRet;

        }


        public BodyPart LoadFileIntoBodyPart(int fileID)
        {
            if (Directory.Exists(Environment.CurrentDirectory + "/SavedBodyParts"))
            {
                try
                {
                    
                    //final versions
                    
                    ColiSys.Hashtable ht = new ColiSys.Hashtable();
                    List<ColiSys.Hashtable> sutureSpots = new List<ColiSys.Hashtable>();

                    //read everything
                    string[] allString = System.IO.File.ReadAllLines(Environment.CurrentDirectory + "/SavedBodyParts" + "/" + fileID);
                   int i = 0;

                    //load regpack
                   string s_regPack;
                   s_regPack = allString[0]; //first line is regpack info
                   List<int> regPack = new List<int>();
                   string[] s_regPackSplit = s_regPack.Split(',');

                   foreach(string s in s_regPackSplit)
                   try
                   {
                       int t = Convert.ToInt32(s);
                       regPack.Add(t);
                   }
                   catch
                   {
                       //if operation fails, do nothing
                   }


                   i++;


                    //load ht
                   i = _FillHtFromAllString(ht, allString, i);


                    //load all suture spots
                   while (i != -1)
                   {
                       ColiSys.Hashtable h = new ColiSys.Hashtable();
                       i = _FillHtFromAllString(h, allString, i);
                       sutureSpots.Add(h);
                   }



                    //at this point, i have all reg, ht, and suturespots
                   BpConstructor bpC = new BpConstructor();
                   bpC.regPacks = regPack;
                   bpC.shape = ht;
                   bpC.sutureSpots = sutureSpots;
                   BodyPart toRet = new BodyPart(bpC);
                   return toRet;


                    
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }
               



            }

            return null;
        }


        private int _FillHtFromAllString(ColiSys.Hashtable ht, string[] allString, int i)
        {
            bool Xmode = true;
            bool lastNodeWasX = true;
            ColiSys.Node lx = null;
            ColiSys.Node ly = null;
            ColiSys.Node head = null;


            for (; allString[i] != "&&"; i++)
            {
                if (allString[i] != "Y" && allString[i] != "X")
                {
                    string[] s_lub = allString[i].Split('-');
                    int lb = Convert.ToInt32(s_lub[0]);
                    int ub = Convert.ToInt32(s_lub[1]);

                    if (lx == null)
                    {
                        lx = new ColiSys.Node(lb, ub); //its also assumed an x node of course
                        head = lx;
                        lastNodeWasX = true;
                    }
                    else if (Xmode)
                    {
                        lx.Adj(new ColiSys.Node(lb, ub));
                        lx = lx.Adj();
                        lastNodeWasX = true;
                    }
                    else
                    { //y mode
                        if (lastNodeWasX)
                        {
                            lx.Dwn(new ColiSys.Node(lb, ub));
                            ly = lx.Dwn();
                        }
                        else
                        {
                            ly.Adj(new ColiSys.Node(lb, ub));
                            ly = ly.Adj();
                        }
                        lastNodeWasX = false;
                    }
                }
                else if (allString[i] == "Y")
                {
                    Xmode = false;
                }
                else if (allString[i] == "X")
                {
                    Xmode = true;
                }
            }

            ht.ResetMainNode(head);
            i++; //go past the &&
            if (allString[i] == "")
                i = -1; //if there is no more, return  -1 to break the loop

            return i;


        }


        public bool DoesIDExist(int id)
        {
            foreach (int i in bodyIDs)
                if (i == id)
                    return true;

            return false;

        }


        public void SaveBodyPart(BodyPart bp, int id)
        {
            SaveBodyPart(bp.bpDNA,id);



        }

        public void SaveBodyPart(BpConstructor bp, int id)
        {
            if (!DoesIDExist(id))
            {
                // Compose a string that consists of three lines.
                List<string> s_regpack = new List<string>();
                foreach (int i in bp.regPacks)
                    s_regpack.Add(i + ",");
                s_regpack.Add(" &&");

                string s_ht = nami.TurnHtIntoString(bp.shape.RetMainNode());
                List<string> s_sutures = new List<string>();

                foreach (ColiSys.Hashtable ht in bp.sutureSpots)
                    s_sutures.Add(nami.TurnHtIntoString(ht.RetMainNode()));



                // Write the string to a file.
                try
                {
                    System.IO.StreamWriter file = new System.IO.StreamWriter(Environment.CurrentDirectory + "/SavedBodyParts/" + id);
                    foreach (string s in s_regpack)
                        file.Write(s);
                    file.WriteLine("");
                    file.WriteLine(s_ht);
                    foreach (string s in s_sutures)
                        file.WriteLine(s);
                    file.WriteLine("");
                    file.Close();
                    bodyIDs.Add(id);
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("File could not be written or created");
                }

            }
            else
            {
                Console.Out.WriteLine("failed to save body part, ID not unique");
            }


        }


    }
}
