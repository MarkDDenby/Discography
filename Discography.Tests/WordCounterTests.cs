using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Discography.Tests
{
    [TestClass]
    public class WordCounterTests
    {
        [TestMethod]
        public void WordCounterReturnCorrectResultWhenGivenNoWords()
        {
            var expected = 0;
            var words = String.Empty;

            var sut = new WordCounter();
            var actual = sut.Count(words);

            Assert.AreEqual(expected, actual);
        }

        public void WordCounterReturnCorrectResultWhenGivenNullString()
        {
            var expected = 0;

            var sut = new WordCounter();
            var actual = sut.Count(null);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WordCounterReturnCorrectResultWhenGivenSingleWord()
        {
            var expected = 1;
            var words = "Test";

            var sut = new WordCounter();
            var actual = sut.Count(words);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WordCounterReturnCorrectResultWhenGivenMultipleWords()
        {
            var expected = 5;
            var words = "This is a simple test";

            var sut = new WordCounter();
            var actual = sut.Count(words);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WordCounterReturnCorrectResultWhenGivenMultipleWordsWithMultipleSpaces()
        {
            var expected = 5;
            var words = "This  is  a  simple  test";

            var sut = new WordCounter();
            var actual = sut.Count(words);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WordCounterReturnCorrectResultWhenGivenMultipleWordsWithSlashes()
        {
            var expected = 5;
            var words = "We need a test/tests";

            var sut = new WordCounter();
            var actual = sut.Count(words);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WordCounterReturnCorrectResultWhenGivenMultipleWordsWithSlashesSpaces()
        {
            var expected = 5;
            var words = "We need a test / tests";

            var sut = new WordCounter();
            var actual = sut.Count(words);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WordCounterReturnCorrectResultWhenGivenMultipleWordsWithCommas()
        {
            var expected = 10;
            var words = "This is a simple test,to see if this works.";

            var sut = new WordCounter();
            var actual = sut.Count(words);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WordCounterReturnCorrectResultWhenGivenMultipleWordsWithCommasSpace()
        {
            var expected = 10;
            var words = "This is a simple test, to see if this works.";

            var sut = new WordCounter();
            var actual = sut.Count(words);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WordCounterReturnCorrectResultWhenGivenMultipleWordsWithSemiColons()
        {
            var expected = 13;
            var words = "This is a simple test,to see if this works;Hopefully it will.";

            var sut = new WordCounter();
            var actual = sut.Count(words);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WordCounterReturnCorrectResultWhenGivenMultipleWordsWithSemiColonsSpace()
        {
            var expected = 13;
            var words = "This is a simple test, to see if this works; Hopefully it will.";

            var sut = new WordCounter();
            var actual = sut.Count(words);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WordCounterReturnCorrectResultWhenGivenMultipleWordsWithColons()
        {
            var expected = 13;
            var words = "This is a simple test,to see if this works:Hopefully it will.";

            var sut = new WordCounter();
            var actual = sut.Count(words);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WordCounterReturnCorrectResultWhenGivenMultipleWordsWithColonsSpace()
        {
            var expected = 13;
            var words = "This is a simple test, to see if this works: Hopefully it will.";

            var sut = new WordCounter();
            var actual = sut.Count(words);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WordCounterReturnCorrectResultWhenGivenMultipleWordsWithApostrophe()
        {
            var expected = 3;
            var words = "There's a thousand";

            var sut = new WordCounter();
            var actual = sut.Count(words);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WordCounterReturnCorrectResultWhenGivenMultipleWordsWithExclamation()
        {
            var expected = 9;
            var words = "My heart went boom!When I crossed that room";

            var sut = new WordCounter();
            var actual = sut.Count(words);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WordCounterReturnCorrectResultWhenGivenMultipleWordsWithExclamationSpace()
        {
            var expected = 9;
            var words = "My heart went boom! when I crossed that room";

            var sut = new WordCounter();
            var actual = sut.Count(words);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WordCounterReturnCorrectResultWhenGivenMultipleWordsWithQuestionMark()
        {
            var expected = 12;
            var words = "Ooh, when I saw her standing there?Well, she looked at me";

            var sut = new WordCounter();
            var actual = sut.Count(words);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WordCounterReturnCorrectResultWhenGivenMultipleWordsWithQuestionMarkSpace()
        {
            var expected = 12;
            var words = "Ooh, when I saw her standing there? Well, she looked at me";

            var sut = new WordCounter();
            var actual = sut.Count(words);

            Assert.AreEqual(expected, actual);
        }
    }
}
