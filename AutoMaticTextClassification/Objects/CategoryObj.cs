using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticTextClassification
{
    class CategoryObj
    {
        string[] _suffixes;
        string[] _lemmatizingWords;
        string _name;
        Dictionary<string, int> _wordInformation;
        int _documentsUsed;
        public int DocumentsUsed { get { return _documentsUsed; } set { _documentsUsed = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public Dictionary<string, int> WordInformation { get {return _wordInformation; } set { _wordInformation = value; } }

        public CategoryObj(string[] lemmatizingWords, string[] suffixes)
        {
            _documentsUsed = 0;
            _lemmatizingWords = lemmatizingWords;
            _suffixes = suffixes;
        }

        string[] RemoveLemmatizingWords(string text)
        {
            List<string> removedLemmatizingWords = text.Split().ToArray().ToList();
            List<string> revisedText = new List<string>();
            if (_lemmatizingWords != null)
            {
                int i= removedLemmatizingWords.Count-1;
                while (i != -1) {
                    if (_lemmatizingWords.Contains(removedLemmatizingWords[i].ToLower()))
                    {
                        removedLemmatizingWords.Remove(removedLemmatizingWords[i]);
                    }
                    else
                    {
                        i--;
                    }
                }
            }
            foreach(string s in removedLemmatizingWords)
            {
                string update = s;
                foreach(string suffix in _suffixes)
                {
                    if (s.EndsWith(suffix) && s.Length >suffix.Length)
                    {
                        update =s.Remove(s.Length - suffix.Length, suffix.Length);
                        break;
                    }
                }
                if(update!= "")
                revisedText.Add(update);
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
