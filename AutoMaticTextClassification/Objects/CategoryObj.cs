using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticTextClassification
{
    class CategoryObj
    {
        string[] _lemmatizingWords;
        string _name;
        Dictionary<string, WordCalulationsObj> _wordInformation;
        int _documentsUsed;
        public string Name { get { return _name; } set { _name = value; } }
        public Dictionary<string, WordCalulationsObj> WordInformation { get {return _wordInformation; } set { _wordInformation = value; } }

        public CategoryObj(string[] lemmatizingWords)
        {
            _documentsUsed = 0;
            _lemmatizingWords = lemmatizingWords;
        }

        string[] RemoveLemmatizingWords(string text)
        {
            List<string> revisedText = text.Split().ToArray().ToList() ;
            if (_lemmatizingWords != null)
            {
                int i= revisedText.Count-1;
                while (i != -1) {
                    if (_lemmatizingWords.Contains(revisedText[i].ToLower()))
                    {
                        revisedText.Remove(revisedText[i]);
                    }
                    else
                    {
                        i--;
                    }
                }
            }
            return revisedText.ToArray();
        }

        public void AddText(string text)
        {
            _documentsUsed++;
            String[] revisedText = RemoveLemmatizingWords(text);
            if(_wordInformation == null)
            {
                Dictionary<string, WordCalulationsObj> kvp = new Dictionary<string, WordCalulationsObj>();
                _wordInformation = kvp;
            }
            foreach(string word in revisedText)
            {
                if (_wordInformation.ContainsKey(word.ToLower()))
                {
                    _wordInformation[word.ToLower()].AmountOfWords++;
                }
                else
                {
                    WordCalulationsObj wc = new WordCalulationsObj();
                    wc.AmountOfWords = 1;
                    _wordInformation.Add(word.ToLower(), wc);
                }
            }
            foreach(KeyValuePair<string, WordCalulationsObj> wc in _wordInformation)
            {
                wc.Value.Percentage = (double)(wc.Value.AmountOfWords+1) / GetTotalWords();
            }
        }

        public int GetTotalWords()
        {
            int currentTotal = 0;
            foreach (WordCalulationsObj wc in _wordInformation.Values)
            {
                currentTotal += wc.AmountOfWords;
            }
            return currentTotal;
        }
        public double WordPercenageForCategory(string key)
        {
            double percent = 0;
            if(_wordInformation.ContainsKey(key))
            {
                WordInformation.TryGetValue(key,out WordCalulationsObj count);
            }

            return percent;
        }

    }
}
