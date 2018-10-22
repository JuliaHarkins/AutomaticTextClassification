﻿using System;
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
            List<string> revisedText = text.Split().ToList() ;
            if (_lemmatizingWords != null)
            {
                foreach(string word in revisedText)
                {
                    if (_lemmatizingWords.Contains(word))
                    {
                        revisedText.Remove(word);
                    }
                }
            }
            return revisedText.ToArray();
        }

        public void AddText(string text)
        {
            String[] revisedText = RemoveLemmatizingWords(text);
            foreach(string word in revisedText)
            {
                if (_wordAndCount.ContainsKey(word))
                {
                    _wordAndCount[word]++;
                }
                else
                {
                    _wordAndCount.Add(word, 1);
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
