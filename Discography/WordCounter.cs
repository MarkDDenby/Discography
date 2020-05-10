using System.Collections.Generic;
using Discography.Contracts;

namespace Discography
{
    public class WordCounter : IWordCounter
    {
        private readonly List<char> _wordSeparators = new List<char>() { ';', '.', ',', ':', ' ', '!', '?', '/' };

        public int Count(string words)
        {
            var wordCount = 0;

            if (!string.IsNullOrWhiteSpace(words))
            {
                for (var index = 1; index < words.Length; index++)
                {
                    if (_wordSeparators.Contains(words[index - 1]))
                    {
                        if (char.IsLetterOrDigit(words[index]))
                        {
                            wordCount++;
                        }
                    }
                }
                if (words.Length > 2)
                {
                    wordCount++;
                }
            }

            return wordCount;
        }
    }
}
