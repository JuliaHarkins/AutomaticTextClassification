using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticTextClassification
{
    class FileReadWrite
    {
        string _BayesingNetworkFolder = "BayesingNetwork";
        string _TestDataFolder = "TestData";
        string _TrainingDataFolder = "TrainingData";

        /// <summary>
        /// Ensures the folders are created
        /// </summary>
        public FileReadWrite()
        {
            if (!System.IO.Directory.Exists(_BayesingNetworkFolder))
            {
                System.IO.Directory.CreateDirectory(_BayesingNetworkFolder);
            }
            if (!System.IO.Directory.Exists(_TestDataFolder))
            {
                System.IO.Directory.CreateDirectory(_TestDataFolder);
            }
            if (!System.IO.Directory.Exists(_TrainingDataFolder))
            {
                System.IO.Directory.CreateDirectory(_TrainingDataFolder);
            }
        }

        public FileObj[] GetTrainingData()
        {
            List<FileObj> trainingData = new List<FileObj>();
            foreach (string file in Directory.EnumerateFiles(_TrainingDataFolder, "*.txt"))
            {
                FileObj f = new FileObj();
                f.FileName = file;
                f.FileContent = File.ReadAllText(file);
                trainingData.Add(f);
            }

            return trainingData.ToArray();
        }

        public FileObj[] GetTestData()
        {
            List<FileObj> testData = new List<FileObj>();
            foreach (string file in Directory.EnumerateFiles(_TestDataFolder, "*.txt"))
            {
                FileObj f = new FileObj();
                f.FileName = file;
                f.FileContent = File.ReadAllText(file);
                testData.Add(f);
            }

            return testData.ToArray();
        }

        

        public string[] GetLemmatizingWords()
        {
            List<string> lemmatizingWords = new List<string>();
            using (StreamReader sr = new StreamReader("LemmatizingWords.txt"))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    lemmatizingWords.Add(line);
                }
            }

            return lemmatizingWords.ToArray();
        }

        public bool SaveBayesingToFile(string folderName, List<CategoryObj> bayesingNetwork)
        {
            bool fileCreated = false;

            //find out if the file already exists
            if (!System.IO.Directory.Exists(_BayesingNetworkFolder+"\\"+folderName))
            {
                
                System.IO.Directory.CreateDirectory(folderName);
                //create a file for each category known to the network
                foreach (CategoryObj cat in bayesingNetwork)
                {
                    using (StreamWriter sr = new StreamWriter(_BayesingNetworkFolder + "\\" + folderName + "\\" + cat.Name + ".txt"))
                    {
                        //write the words and how offten they appear 
                        foreach (KeyValuePair<string, int> kvp in cat.WordAndCount)
                        {
                            sr.WriteLine(kvp.Key + "," + kvp.Value);
                        }
                    }
                }
                fileCreated = true;
            }
            return fileCreated;
        }

        BayesingNetwork[] GetSavedBayesingNetworks()
        {
            string line = "";
            Dictionary<string, int> kvp = new Dictionary<string, int>();
            List<BayesingNetwork> bayesingNetworks = new List<BayesingNetwork>();
            foreach (string d in Directory.GetDirectories(_BayesingNetworkFolder))
            {
                
                List<CategoryObj> cat = new List<CategoryObj>();
                foreach (string file in Directory.EnumerateFiles(_BayesingNetworkFolder + "\\" + d, "*.txt"))
                {
                    CategoryObj c = new CategoryObj(GetLemmatizingWords());
                    using (StreamReader sr = new StreamReader(_BayesingNetworkFolder + "\\" + d + "\\" + file)){
                        
                        c.Name = file;
                        while((line = sr.ReadLine()) != null)
                        {
                            string[] WordAndCountSplit = line.Split(',');
                            kvp.Add(WordAndCountSplit[0], int.Parse(WordAndCountSplit[1]));
                        }
                        c.WordAndCount = kvp;
                    }
                    cat.Add(c);

                }
                BayesingNetwork bn = new BayesingNetwork(cat);
                bayesingNetworks.Add(bn);
            }

            return bayesingNetworks.ToArray();
        }
    }
}
