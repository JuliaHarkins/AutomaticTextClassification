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
        Dictionary<string, int> _wordAndCount;
        public string Name { get { return _name; } set { _name = value; } }
        public Dictionary<string, int> WordAndCount { get {return _wordAndCount; } set { _wordAndCount = value; } }

        public CategoryObj(string[] lemmatizingWords)
        {
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
            String[] revisedText = RemoveLemmatizingWords(text);
            if(_wordAndCount == null)
            {
                Dictionary<string, int> kvp = new Dictionary<string, int>();
                _wordAndCount = kvp;
            }
            foreach(string word in revisedText)
            {
                if (_wordAndCount.ContainsKey(word.ToLower()))
                {
                    _wordAndCount[word.ToLower()]++;
                }
                else
                {
                    _wordAndCount.Add(word.ToLower(), 1);
                }
            }
        }

        public int GetTotalWords()
        {
            int currentTotal = 0;
            foreach (int count in _wordAndCount.Values)
            {
                currentTotal += count;
            }
            return currentTotal;
        }

    }
}
