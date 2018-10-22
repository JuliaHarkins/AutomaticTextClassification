using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMaticTextClassification
{
    class FileReader
    {
        string _BayesingNetworkFolder = "BayesingNetwork";
        string _TestDataFolder = "TestData";

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
        string GetTestData()
        {
            return "b";
        }
        string LoadSavedNetwork()
        {
            return "c";
        }
    }
}
