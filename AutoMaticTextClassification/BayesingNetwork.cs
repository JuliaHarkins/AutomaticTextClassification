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
        CategoryObj _analisingText;
        public List<CategoryObj> KnownInfomation { get { return _knownInformation; } }
        public string Name { get; set; }
        public BayesingNetwork()
        {

        }
        public BayesingNetwork(List<CategoryObj> knownInformation)
        {
            _knownInformation = knownInformation;
        }

        public void Train()
        {
            FileReadWrite frw = new FileReadWrite();

            //get the trianing files
            FileObj[] files = frw.GetTrainingData();
            List<CategoryObj> categories = new List<CategoryObj>();
            bool exists = false;
            //classifies the categories using the files
            foreach(FileObj f in files)
            {
                string category = "";
                foreach(char c in f.FileName)
                    if (c != '0' && c != '1' && c != '2' && c != '3' && c != '4' && c != '5' && c != '6' && c != '7' && c != '8' && c != '9' && c != '.') {
                        category += c;
                    }
                    else
                    {
                        break;
                    }
                exists = false;
                foreach(CategoryObj cat in categories)
                {
                    if(cat.Name == category)
                    {
                        exists = true;
                        cat.AddText( f.FileContent);
                    }
                }
                if (!exists)
                {
                    CategoryObj newCategory = new CategoryObj(frw.GetLemmatizingWords());
                    newCategory.Name = category;
                    newCategory.AddText(f.FileContent);

                    categories.Add(newCategory);
                }
            }
            _knownInformation = categories;
        }
        public void GetAnalizedText(FileObj file)
        {
            FileReadWrite frw = new FileReadWrite();
            CategoryObj analisingText = new CategoryObj(frw.GetLemmatizingWords());
            analisingText.Name = file.FileName;

            analisingText.AddText(file.FileContent);
            _analisingText = analisingText;
        }

        public List<string> GetAnalisedResult()
        {
            int totalWords = 0;
            int totalCatDoc = 0;
            List<string> ResultsTable = new List<string>();
            foreach (CategoryObj cat in _knownInformation)
            {
                totalCatDoc += cat.DocumentsUsed;
                totalWords += cat.GetTotalWords();
            }

            foreach (CategoryObj cat in _knownInformation)
            {
                double chance=0;
                string catName = cat.Name;
                foreach (KeyValuePair<string, int> kvp in cat.WordInformation)
                {

                    if (_analisingText.WordInformation.ContainsKey(kvp.Key))
                    {
                        if (chance == 0)
                        {
                            chance = (double)(cat.WordInformation[kvp.Key] + 1) / (cat.GetTotalWords() + totalWords);
                        }
                        else
                        {
                            chance *= (double)(cat.WordInformation[kvp.Key] + 1) / (cat.GetTotalWords() + totalWords);
                        }
                    }
                }
                if (chance != 0)
                {
                    chance *= cat.DocumentsUsed / totalCatDoc;
                }

                string catResult = cat.Name + String.Format(" %1$,.2f ", chance.ToString());
                ResultsTable.Add(catResult);

                
            }
            return ResultsTable;
        }
    }
}
