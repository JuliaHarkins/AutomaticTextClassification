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
        CategoryObj _analysingText;

        public List<CategoryObj> KnownInfomation { get { return _knownInformation; } }
        public string Name { get; set; }

        public BayesingNetwork(){}

        public BayesingNetwork(List<CategoryObj> knownInformation)
        {
            _knownInformation = knownInformation;
        }

        /// <summary>
        /// Trains the network
        /// </summary>
        public void Train()
        {
            FileReadWrite frw = new FileReadWrite();

            //get the training files
            FileObj[] files = frw.GetTrainingData();
            List<CategoryObj> categories = new List<CategoryObj>();
            bool exists = false;
            //classifies the categories using the files
            foreach(FileObj f in files)
            {
                string category = ""; //name of the new category

                //creates the category name
                foreach(char c in f.FileName)
                    if (c != '0' && c != '1' && c != '2' && c != '3' && c != '4' && c != '5' && c != '6' && c != '7' && c != '8' && c != '9' && c != '.') {
                        category += c;
                    }
                    else
                    {
                        break;
                    }
                exists = false;
                //checks if information can be added to an existing category
                foreach(CategoryObj cat in categories)
                {
                    if(cat.Name == category)
                    {
                        exists = true;
                        cat.AddText( f.FileContent);
                    }
                }
                //makes a new category if the category doesn't already exist
                if (!exists)
                {
                    CategoryObj newCategory = new CategoryObj(frw.GetLemmatizingWords(), frw.GetSuffixes());
                    newCategory.Name = category;
                    newCategory.AddText(f.FileContent);

                    categories.Add(newCategory);
                }
            }
            //adds the list of categorys tot he known information
            _knownInformation = categories;
        }
        /// <summary>
        /// Gets the information the user wishes to analyse and prepares it for the network to analyse
        /// </summary>
        /// <param name="file">the file the user wishes to analyse</param>
        public void GetAnalysedText(FileObj file)
        {
            FileReadWrite frw = new FileReadWrite();
            CategoryObj analysingText = new CategoryObj(frw.GetLemmatizingWords(), frw.GetSuffixes());
            analysingText.Name = file.FileName;
            analysingText.AddText(file.FileContent);
            _analysingText = analysingText;
        }
        /// <summary>
        /// analyse the Text
        /// </summary>
        /// <returns>Returns the percentage chance for each category analysed</returns>
        public List<string> GetAnalysedResult()
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
                    if (_analysingText.WordInformation.ContainsKey(kvp.Key))
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
                //calculates the final result.
                if (chance != 0 && totalCatDoc !=0)
                {
                    chance += Math.Log((double)cat.DocumentsUsed / totalCatDoc);
                }

                double r = chance * -1;
                catResultsTable.Add(cat.Name, r);
                totalValue += r;
            }
            //sorting the list to get the highest value at the top.
            var sortedResult = from entry in catResultsTable orderby entry.Value descending select entry;
            //gets the percentage of each of the categories
            foreach ( KeyValuePair<string, double> kvp in sortedResult)
            {
                double percent = 0;
                percent = (kvp.Value / totalValue) *100;
                string catResult = kvp.Key + " " + Math.Round(percent,2) +"%";
                finalResult.Add(catResult);
            }
            

            //returns a list of the results.
            return finalResult;
        }
    }
}
