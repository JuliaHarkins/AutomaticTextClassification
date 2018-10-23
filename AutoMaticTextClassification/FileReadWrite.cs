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

        /// <summary>
        /// gets all the training to train the AI
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// takes the test data for the AI to analyse 
        /// </summary>
        /// <returns></returns>
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

        
        /// <summary>
        /// takes the lemmatizing words from the debug folder
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// creates a folder to put the network into
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="bayesingNetwork"></param>
        /// <returns></returns>
        public bool SaveBayesingToFile(string folderName, List<CategoryObj> bayesingNetwork)
        {
            bool fileCreated = false;

            //find out if the folder already exists
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
        /// <summary>
        /// retrieves the networks from the folder.
        /// </summary>
        /// <returns></returns>
        public BayesingNetwork[] GetSavedBayesingNetworks()
        {
            //used to read the information from the file
            string line = "";
            
            //list of networks
            List<BayesingNetwork> bayesingNetworks = new List<BayesingNetwork>();

            //d is the directory which holds the networks infomation
            foreach (string d in Directory.GetDirectories(_BayesingNetworkFolder))
            {
                //contains the categories for the networks
                List<CategoryObj> cat = new List<CategoryObj>();
                
                //file is the networks categories
                foreach (string file in Directory.EnumerateFiles(_BayesingNetworkFolder + "\\" + d, "*.txt"))
                {
                    CategoryObj c = new CategoryObj(GetLemmatizingWords())
                    {
                        Name = file
                    };
                    //collects the dictionary information for the categories
                    using (StreamReader sr = new StreamReader(_BayesingNetworkFolder + "\\" + d + "\\" + file)){

                        //new dictionary entry to be added to the category
                        Dictionary<string, int> kvp = new Dictionary<string, int>();
                        while((line = sr.ReadLine()) != null)
                        {
                            //splits the key from the value it holds
                            string[] WordAndCountSplit = line.Split(',');
                            kvp.Add(WordAndCountSplit[0], int.Parse(WordAndCountSplit[1]));
                        }
                        c.WordAndCount = kvp;
                    }
                    cat.Add(c);
                }
                BayesingNetwork bn = new BayesingNetwork(cat)
                {
                    Name = d
                };
                bayesingNetworks.Add(bn);
            }
            return bayesingNetworks.ToArray();
        }
    }
}
