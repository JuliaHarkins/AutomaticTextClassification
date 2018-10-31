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
        Dictionary<string, int> _wordInformation;
        int _documentsUsed;
        public int DocumentsUsed { get { return _documentsUsed; } set { _documentsUsed = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public Dictionary<string, int> WordInformation { get {return _wordInformation; } set { _wordInformation = value; } }

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
                Dictionary<string, int> kvp = new Dictionary<string, int>();
                _wordInformation = kvp;
            }
            foreach(string word in revisedText)
            {
                int AmountOfWords=0;
                if (_wordInformation.ContainsKey(word.ToLower()))
                {
                    _wordInformation[word.ToLower()]++;
                }
                else
                {
                    AmountOfWords = 1;
                    _wordInformation.Add(word.ToLower(), AmountOfWords);
                }
            }
        }

        public int GetTotalWords()
        {
            int currentTotal = 0;
            foreach (KeyValuePair<string, int> kvp in _wordInformation)
            {
                currentTotal += kvp.Value;
            }
            return currentTotal;
        }

    }
}
