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
        string[] _stopWords;
        string _name;
        Dictionary<string, int> _wordInformation;
        int _documentsUsed;
        /// <summary>
        /// set or sets the amount of documents used
        /// </summary>
        public int DocumentsUsed { get { return _documentsUsed; } set { _documentsUsed = value; } }
        /// <summary>
        /// get or set the name of the category
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }
        /// <summary>
        /// get or set the dictionary of words and the frequency in which they appear.
        /// </summary>
        public Dictionary<string, int> WordInformation { get {return _wordInformation; } set { _wordInformation = value; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stopWords">the array of stopWords</param>
        /// <param name="suffixes">the array of suffixes</param>
        public CategoryObj(string[] stopWords, string[] suffixes)
        {
            _documentsUsed = 0;
            _stopWords = stopWords;
            _suffixes = suffixes;
        }

        /// <summary>
        /// removes all of the stop words matches from the category text.
        /// </summary>
        /// <param name="text">the text which prior to the removal of strop words</param>
        /// <returns>the text without stop words</returns>
        string[] RemoveStopWords(string text)
        {
            List<string> removedStopgWords = text.Split().ToArray().ToList();
            List<string> revisedText = new List<string>();
            if (_stopWords != null)
            {
                int i= removedStopgWords.Count-1;
                //checks if the word needs removed
                while (i != -1) {
                    if (_stopWords.Contains(removedStopgWords[i].ToLower()))
                    {
                        removedStopgWords.Remove(removedStopgWords[i]);
                    }
                    else
                    {
                        i--;
                    }
                }
            }
            //removes suffixes from the words
            foreach(string s in removedStopgWords)
            {
                string update = s;
                foreach(string suffix in _suffixes)
                {
                    //checks that it ends with the suffix and that that word itself isn't a suffix
                    if (s.EndsWith(suffix) && s.Length >suffix.Length)
                    {
                        update =s.Remove(s.Length - suffix.Length, suffix.Length);
                        break;
                    }
                }
                //updates the word if needed
                if(update!= "")
                revisedText.Add(update);
            }
            return revisedText.ToArray();
        }
        /// <summary>
        /// Adds new text to the category
        /// </summary>
        /// <param name="text">the text to be added to the category</param>
        public void AddText(string text)
        {
            _documentsUsed++;
            String[] revisedText = RemoveStopWords(text);
            //checks if there are any words in the category
            if(_wordInformation == null)
            {
                Dictionary<string, int> kvp = new Dictionary<string, int>();
                _wordInformation = kvp;
            }
            //adds the words to the known information
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
        /// <summary>
        /// gets the total words
        /// </summary>
        /// <returns>returns the word count for this category</returns>
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
