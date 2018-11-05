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
                f.FileName = Path.GetFileName(file);
                string content = File.ReadAllText(file);
                f.FileContent = RemovePunctuation(content);
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
                f.FileName = Path.GetFileName(file);
                string content = File.ReadAllText(file);
                f.FileContent = RemovePunctuation(content);
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
            bool fileFailed = true;
            string folderPath = _BayesingNetworkFolder + "\\" + folderName;
            //find out if the folder already exists
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
                //create a file for each category known to the network
                foreach (CategoryObj cat in bayesingNetwork)
                {
                    string filePath = folderPath+"\\" + cat.Name + ".txt";
                    
                    List<string> infomation = new List<string>();
                    
                    infomation.Add(cat.DocumentsUsed.ToString());

                    foreach (KeyValuePair<string, int> kvp in cat.WordInformation)
                    {
                        infomation.Add(kvp.Key + "+" + kvp.Value);
                    }
                    File.WriteAllLines(filePath, infomation);
                }
                fileFailed = false;
            }
            return fileFailed;
        }
        /// <summary>
        /// retrieves the networks from the folder.
        /// </summary>
        /// <returns></returns>
        public BayesingNetwork[] GetSavedBayesingNetworks()
        {            
            //list of networks
            List<BayesingNetwork> bayesingNetworks = new List<BayesingNetwork>();

            //d is the directory which holds the networks infomation
            foreach (string d in Directory.GetDirectories(_BayesingNetworkFolder))
            {
                //contains the categories for the networks
                List<CategoryObj> cat = new List<CategoryObj>();
                //file is the networks categories
                foreach (string file in Directory.EnumerateFiles( d, "*.txt"))
                {
                    CategoryObj c = new CategoryObj(GetLemmatizingWords())
                    {
                        Name = Path.GetFileName(file)
                    };
                    //collects the dictionary information for the categories

                    string[] information = File.ReadAllLines(file);

                    c.DocumentsUsed = int.Parse(information[0]);
                    Dictionary<string, int> kvp = new Dictionary<string, int>();
                    for (int i = 1; i<information.Length; i++)
                    {
                        //splits the key from the value it holds
                        string[] WordAndCountSplit = information[i].Split('+');

                        int amountOfWords = int.Parse(WordAndCountSplit[1]);

                        kvp.Add(WordAndCountSplit[0], amountOfWords);

                    }
                    //new dictionary entry to be added to the category
                    c.WordInformation = kvp; 
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

        string RemovePunctuation(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Char c in s)
            {
                if (!char.IsPunctuation(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
