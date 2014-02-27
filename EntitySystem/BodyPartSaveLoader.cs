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


        private static BodyPartSaveLoader instance;
        private BodyPartSaveLoader()
        {
            bodyIDs = LoadAllIDs();

        
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


                string lines = "RegPack:\r\nSecond line.\r\nThird line.";

                // Write the string to a file.
                try
                {
                    System.IO.StreamWriter file = new System.IO.StreamWriter(Environment.CurrentDirectory + "/SavedBodyParts/" + id);
                    file.WriteLine(lines);
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
