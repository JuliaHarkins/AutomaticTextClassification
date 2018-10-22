using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticTextClassification
{
    class FileReader
    {
        string _BayesingNetworkFolder = "BayesingNetwork";
        string _TestDataFolder = "TestData";

        /// <summary>
        /// Ensures the folders are created
        /// </summary>
        FileReader()
        {
            if (!System.IO.Directory.Exists(_BayesingNetworkFolder))
            {
                System.IO.Directory.CreateDirectory(_BayesingNetworkFolder);
            }
            if (!System.IO.Directory.Exists(_TestDataFolder))
            {
                System.IO.Directory.CreateDirectory(_TestDataFolder);
            }
        }
        FileObj[] GetTestData()
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
        FileObj[] GetSavedBayesingNetwork()
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
