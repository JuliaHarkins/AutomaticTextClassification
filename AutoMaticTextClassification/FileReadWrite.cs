using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AutomaticTextClassification
{
    class FileReadWrite
    {
        //the folder names
        string _BayesingNetworkFolder = "BayesingNetwork"; 
        string _TestDataFolder = "TestData";
        string _TrainingDataFolder = "TrainingData";

        /// <summary>
        /// Ensures the folders are created
        /// </summary>
        public FileReadWrite()
        {
            //created required folders if they don't exist already
            if (!Directory.Exists(_BayesingNetworkFolder))
            {
                Directory.CreateDirectory(_BayesingNetworkFolder);
            }
            if (!Directory.Exists(_TestDataFolder))
            {
                Directory.CreateDirectory(_TestDataFolder);
            }
            if (!Directory.Exists(_TrainingDataFolder))
            {
                Directory.CreateDirectory(_TrainingDataFolder);
            }
        }

        /// <summary>
        /// gets all the training to train the networks folder
        /// </summary>
        /// <returns>returns the training data</returns>
        public FileObj[] GetTrainingData()
        {
            List<FileObj> trainingData = new List<FileObj>();
            foreach (string file in Directory.EnumerateFiles(_TrainingDataFolder, "*.txt"))
            {
                FileObj f = new FileObj();
                f.FileName = Path.GetFileName(file).ToLower();
                string content = File.ReadAllText(file);
                f.FileContent = RemovePunctuation(content);
                trainingData.Add(f);
            }

            return trainingData.ToArray();
        }
        /// <summary>
        /// gets the test data for the network to analyse 
        /// </summary>
        /// <returns>returns the test data</returns>
        public FileObj[] GetTestData()
        {
            List<FileObj> testData = new List<FileObj>();
            foreach (string file in Directory.EnumerateFiles(_TestDataFolder, "*.txt"))
            {
                FileObj f = new FileObj();
                f.FileName = Path.GetFileName(file).ToLower();
                string content = File.ReadAllText(file);
                f.FileContent = RemovePunctuation(content);
                testData.Add(f);
            }
            return testData.ToArray();
        }

        
        /// <summary>
        /// takes the lemmatizing words from the debug folder
        /// </summary>
        /// <returns>returns the array of lemmatizingWords</returns>
        public string[] GetLemmatizingWords()
        {
            List<string> lemmatizingWords = new List<string>();
            using (StreamReader sr = new StreamReader("stopWords.txt"))
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
        /// Gets the list of suffixes from the debug file
        /// </summary>
        /// <returns>the array of suffixes</returns>
        public string[] GetSuffixes()
        {
            List<string> suffixes = new List<string>();
            using (StreamReader sr = new StreamReader("suffixes.txt"))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    suffixes.Add(line);
                }
            }
            return suffixes.ToArray();
        }

        /// <summary>
        /// creates a folder to put the network into
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="bayesingNetwork"></param>
        /// <returns>returns a true or a false based on the success of the method</returns>
        public bool SaveBayesingToFile(string folderName, List<CategoryObj> bayesingNetwork)
        {
            bool fileFailed = true;
            string folderPath = _BayesingNetworkFolder + "\\" + folderName;
            //find out if the folder already exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                //create a file for each category known to the network
                foreach (CategoryObj cat in bayesingNetwork)
                {
                    string filePath = folderPath+"\\" + cat.Name + ".txt";
                    
                    List<string> information = new List<string>();
                    
                    information.Add(cat.DocumentsUsed.ToString());

                    foreach (KeyValuePair<string, int> kvp in cat.WordInformation)
                    {
                        information.Add(kvp.Key + "+" + kvp.Value);
                    }
                    File.WriteAllLines(filePath, information);
                }
                fileFailed = false;
            }
            return fileFailed;
        }
        /// <summary>
        /// retrieves the networks from the folder.
        /// </summary>
        /// <returns>returns the bayesingNetwork array</returns>
        public BayesingNetwork[] GetSavedBayesingNetworks()
        {            
            //list of networks
            List<BayesingNetwork> bayesingNetworks = new List<BayesingNetwork>();
            
            try
            {
                //d is the directory which holds the networks infomation
                foreach (string d in Directory.GetDirectories(_BayesingNetworkFolder))
                {
                    //contains the categories for the networks
                    List<CategoryObj> cat = new List<CategoryObj>();
                    //file is the networks categories
                    foreach (string file in Directory.EnumerateFiles(d, "*.txt"))
                    {
                        CategoryObj c = new CategoryObj(GetLemmatizingWords(), GetSuffixes())
                        {
                            Name = Path.GetFileName(file)
                        };
                        //collects the dictionary information for the categories

                        string[] information = File.ReadAllLines(file);

                        c.DocumentsUsed = int.Parse(information[0]);
                        Dictionary<string, int> kvp = new Dictionary<string, int>();
                        for (int i = 1; i < information.Length; i++)
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
            }
            catch (Exception)
            {
                Console.WriteLine("File error, please check the files are input correctly");
            }
            return bayesingNetworks.ToArray();
        }

        /// <summary>
        /// removes punctuation from a string
        /// </summary>
        /// <param name="s">the information in the file</param>
        /// <returns>the files information without any punctuation </returns>
        string RemovePunctuation(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
            {
                if (!char.IsPunctuation(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
