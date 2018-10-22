using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticTextClassification
{
    class BayesingNetwork
    {
        List<CategoryObj> _knownInformation;
        string _analisingText;

        public BayesingNetwork()
        {

        }
        public BayesingNetwork(List<CategoryObj> knownInfomation)
        {
            _knownInformation = knownInfomation;
        }
        public void Train()
        {
            FileReadWrite frw = new FileReadWrite();

            //get the trianing files
            FileObj[] files = frw.GetTrainingData();
            List<CategoryObj> categories = new List<CategoryObj>();





        }
        


    }
}
