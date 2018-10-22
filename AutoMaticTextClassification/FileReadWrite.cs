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
        FileObj[] GetSavedBayesingNetworks()
        {
            List<FileObj> BayesingNetwork = new List<FileObj>();
            foreach (string file in Directory.EnumerateFiles(_TestDataFolder, "*.txt"))
            {
                FileObj f = new FileObj();
                f.FileName = file;
                f.FileContent = File.ReadAllText(file);
                BayesingNetwork.Add(f);
            }

            return BayesingNetwork.ToArray();
        }

    }
}
