using CqsDataFoundation.Query.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CqsDataFoundation.Tests.Query
{
    [TestClass]
    public class FullTextSearchUtilsTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            
        }

        [TestMethod]
        public void TestSimple()
        {            
            string badChars, removedWords;
            var s1 = FullTextSearchUtils.PrepareSearchCriteriaStep1(" word1 OR w2 word3 NOT  a about test - evaluate //x1", out badChars, out removedWords);
            var s2 = FullTextSearchUtils.BuildSearchTextExpressionSimple(s1);

            Assert.AreEqual("/, /,a,-", badChars);
            Assert.AreEqual("OR, NOT, about", removedWords);

            Assert.AreEqual("word1 w2 word3 test evaluate x1", s1);
            Assert.AreEqual("word1 AND w2 AND word3 AND test AND evaluate AND x1", s2);

            Assert.AreEqual(null, FullTextSearchUtils.PrepareSearchCriteriaStep1(null, out badChars, out removedWords));
            Assert.AreEqual("", FullTextSearchUtils.PrepareSearchCriteriaStep1("", out badChars, out removedWords));

            Assert.AreEqual(null, FullTextSearchUtils.BuildSearchTextExpressionSimple(null));
            Assert.AreEqual("", FullTextSearchUtils.BuildSearchTextExpressionSimple(""));
            Assert.AreEqual("", FullTextSearchUtils.BuildSearchTextExpressionSimple("  "));

            Assert.AreEqual("word1 AND word2", FullTextSearchUtils.BuildSearchTextExpressionSimple("word1,word2"));
            Assert.AreEqual("word1 AND word2", FullTextSearchUtils.BuildSearchTextExpressionSimple("word1;;word2"));
            Assert.AreEqual("word1 AND word2", FullTextSearchUtils.BuildSearchTextExpressionSimple("word1.word2"));

            Assert.AreEqual("\"word1,word2\"", FullTextSearchUtils.BuildSearchTextExpressionSimple("  \"word1,word2\""));
            Assert.AreEqual("\"word1;word2\"", FullTextSearchUtils.BuildSearchTextExpressionSimple("\"word1;;word2\"  "));
            Assert.AreEqual("\"wor d1.word2\"", FullTextSearchUtils.BuildSearchTextExpressionSimple("\"wor  d1.word2\" "));

            Assert.AreEqual("\"some phraze\" AND word% AND (1 AND 2)", FullTextSearchUtils.BuildSearchTextExpressionSimple("\"some phraze\"  word* (1 2)"));
            Assert.AreEqual("\"a;\"", FullTextSearchUtils.BuildSearchTextExpressionSimple("\"a;;\""));
            Assert.AreEqual("ab%%", FullTextSearchUtils.BuildSearchTextExpressionSimple("ab**"));
        }

        [TestMethod]
        public void TestValidating()
        {
            Assert.AreEqual(FullTextSearchUtils.ValidationResult.OK, FullTextSearchUtils.ValidateSearchCriteria("a"));
            Assert.AreEqual(FullTextSearchUtils.ValidationResult.TooShortPattern, FullTextSearchUtils.ValidateSearchCriteria("a*"));
            Assert.AreEqual(FullTextSearchUtils.ValidationResult.TooShortPattern, FullTextSearchUtils.ValidateSearchCriteria("*"));
            Assert.AreEqual(FullTextSearchUtils.ValidationResult.Empty, FullTextSearchUtils.ValidateSearchCriteria("   "));
            Assert.AreEqual(FullTextSearchUtils.ValidationResult.OK, FullTextSearchUtils.ValidateSearchCriteria("  abc "));
            Assert.AreEqual(FullTextSearchUtils.ValidationResult.OK, FullTextSearchUtils.ValidateSearchCriteria("abc \"a phraze\""));
            Assert.AreEqual(FullTextSearchUtils.ValidationResult.OK, FullTextSearchUtils.ValidateSearchCriteria("abc \"a phraze\" word"));
            Assert.AreEqual(FullTextSearchUtils.ValidationResult.NotPairedQuotes, FullTextSearchUtils.ValidateSearchCriteria("abc \"a phraze"));
            Assert.AreEqual(FullTextSearchUtils.ValidationResult.NotPairedQuotes, FullTextSearchUtils.ValidateSearchCriteria("abc \"a phraze \"word\""));
        }

        [TestMethod]
        public void TestStep1()
        {
            string badChars, removedWords;
            var s1 = FullTextSearchUtils.PrepareSearchCriteriaStep1("word", out badChars, out removedWords);

            Assert.AreEqual("word", s1);
            Assert.AreEqual(null, badChars);
            Assert.AreEqual(null, removedWords);

            s1 = FullTextSearchUtils.PrepareSearchCriteriaStep1("word word2  ", out badChars, out removedWords);
            Assert.AreEqual("word word2", s1);
            Assert.AreEqual(null, badChars);
            Assert.AreEqual(null, removedWords);

            s1 = FullTextSearchUtils.PrepareSearchCriteriaStep1("word a word2", out badChars, out removedWords);
            Assert.AreEqual("word word2", s1);
            Assert.AreEqual("a", badChars);
            Assert.AreEqual(null, removedWords);

            s1 = FullTextSearchUtils.PrepareSearchCriteriaStep1("word \"a\" word2", out badChars, out removedWords);
            Assert.AreEqual("word \"a\" word2", s1);
            Assert.AreEqual(null, badChars);
            Assert.AreEqual(null, removedWords);

            s1 = FullTextSearchUtils.PrepareSearchCriteriaStep1("not  word or word2 about near", out badChars, out removedWords);
            Assert.AreEqual("word word2", s1);
            Assert.AreEqual(null, badChars);
            Assert.AreEqual("not, or, about, near", removedWords);
        }

        [TestMethod]
        public void TestBuildSearchTextExpressionExt()
        {
            var res = FullTextSearchUtils.BuildSearchTextExpressionExt("word1 \"some phraze 1\" word2", " word3  word4 \"some phraze 2\"", null);
            Assert.AreEqual(false, res.Item1);
            Assert.AreEqual("word1 AND \"some phraze 1\" AND word2 AND (word3 OR word4 OR \"some phraze 2\")", res.Item2);

            res = FullTextSearchUtils.BuildSearchTextExpressionExt("word1 word2", "word2 word3  a  ", null);
            Assert.AreEqual(false, res.Item1);
            Assert.AreEqual("word1 AND word2 AND (word2 OR word3 OR a)", res.Item2);

            res = FullTextSearchUtils.BuildSearchTextExpressionExt("www", "yyy", null);
            Assert.AreEqual(false, res.Item1);
            Assert.AreEqual("www AND yyy", res.Item2);

            res = FullTextSearchUtils.BuildSearchTextExpressionExt(null, "yyy   mmm not", null);
            Assert.AreEqual(false, res.Item1);
            Assert.AreEqual("(yyy OR mmm OR not)", res.Item2);

            res = FullTextSearchUtils.BuildSearchTextExpressionExt(null, "", "word1, word2");
            Assert.AreEqual(true, res.Item1);
            Assert.AreEqual("(word1 OR word2)", res.Item2);

            res = FullTextSearchUtils.BuildSearchTextExpressionExt(null, "xxx", "word1, word2");
            Assert.AreEqual(false, res.Item1);
            Assert.AreEqual("xxx NOT (word1 OR word2)", res.Item2);

            res = FullTextSearchUtils.BuildSearchTextExpressionExt("yyy", "xxx", "word1, word2");
            Assert.AreEqual(false, res.Item1);
            Assert.AreEqual("yyy AND xxx NOT (word1 OR word2)", res.Item2);

            res = FullTextSearchUtils.BuildSearchTextExpressionExt("", "", "");
            Assert.AreEqual(false, res.Item1);
            Assert.AreEqual("", res.Item2);

            res = FullTextSearchUtils.BuildSearchTextExpressionExt(null, null, null);
            Assert.AreEqual(false, res.Item1);
            Assert.AreEqual("", res.Item2);

            res = FullTextSearchUtils.BuildSearchTextExpressionExt("yyy", "xxx \"zzz 22\" tt", "word1, word2");
            Assert.AreEqual(false, res.Item1);
            Assert.AreEqual("yyy AND (xxx OR \"zzz 22\" OR tt) NOT (word1 OR word2)", res.Item2);
        }
    }
}
