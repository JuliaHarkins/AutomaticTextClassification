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
            double totalValue=0; //the value of all results added together
            int totalWords = 0;   //the total words in all the documents      
            int totalCatDoc = 0;  //the amount of categories
            List<string> finalResult = new List<string>();  // the users results
            Dictionary<string, double> catResultsTable = new Dictionary<string, double>(); //holds all the results

            //gets total categories and total words
            foreach (CategoryObj cat in _knownInformation)
            {
                totalCatDoc += cat.DocumentsUsed;
                totalWords += cat.GetTotalWords();
            }

            foreach (CategoryObj cat in _knownInformation)
            {                
                double chance=0; //running total of category number
                foreach (KeyValuePair<string, int> kvp in cat.WordInformation)
                {

                    if (_analisingText.WordInformation.ContainsKey(kvp.Key))
                    {
                        //sets the first value
                        if (chance == 0)
                        {
                            double top = (cat.WordInformation[kvp.Key] + 1);
                            double bot = ( cat.WordInformation.Sum(x => x.Value) + totalWords);
                            chance = Math.Log(top / bot);
                        }
                        else//edits the value
                        {
                            double top = (cat.WordInformation[kvp.Key] + 1);
                            double bot = (cat.WordInformation.Sum(x => x.Value) + totalWords);
                            chance += Math.Log(top / bot);
                        }
                    }
                }
                if (chance != 0 && totalCatDoc !=0)
                {
                    chance += Math.Log((double)cat.DocumentsUsed / totalCatDoc);
                }

                double r = chance * -1;
                catResultsTable.Add(cat.Name, r);
                totalValue += r;
            }

            foreach ( KeyValuePair<string, double> kvp in catResultsTable)
            {
                double percent = 0;
                percent = Math.Log(kvp.Value / totalValue);
                 string catResult = kvp.Key + " " + Math.Round(percent,3);
                finalResult.Add(catResult);
            }
            


            return finalResult;
        }
    }
}
