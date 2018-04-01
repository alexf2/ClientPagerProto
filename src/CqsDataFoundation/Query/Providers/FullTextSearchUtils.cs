using System;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;

namespace CqsDataFoundation.Query.Providers
{
    /// <summary>
    /// Constructs a search expression for full text search SQL query. The expression is built on 'and, or, not' optional parts.
    /// Words, within each part, are separated by spaces or commas or semicolons.
    /// Phrazes and word combinations should be enclosed into quotes.
    /// Use * symbol to specify a mask for arbitrary symbols.
    /// Only letters, digits, apos, ampersand and minus are used in search. All other symbols are removed. Inside of quotes, in addition comma, dot and semicolon are used.
    /// The next keywords are removed and do not participate in the search: ABOUT, ACCUM, AND, BT, BTG, BTI, BTP, FUZZY, HASPATH, INPATH, MINUS, NEAR, NOT, NT, NTG, NTI, NTP, OR, PT, RT, SQE, SYN, TR, TRSYN, TT, WITHIN.
    /// </summary>
    public static class FullTextSearchUtils
    {
        public const string ExclusionKeywordPref = "NOT";
        private const int MinWordLength = 2;

        public enum PartsConnection
        {
            [Description(" AND ")]
            And,
            [Description(" OR ")]
            Or,
            [Description(" OR ")] //is used construction: NOT (x1 OR x2 ...OR xN)
            Not
        }

        public enum ValidationResult
        {
            [Description("Ok")]
            OK,
            [Description("Empty")]
            Empty, 
            [Description("There is a not paired quote")]
            NotPairedQuotes,			
            [Description("There is a too short search mask. When * is used, the word should be at least 3 symbols in size")]
            TooShortPattern
        }
        
        //Acceptable symbols in words are: \w * - & '
        //In addition, within quotes are valid: , ; .        

        private static readonly Regex _exNormSpaces = new Regex(@"[\s,;\.]{2,}", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
        private static readonly Regex _exTrimSpaces = new Regex(@"^[\s,;\.]+|[\s,;\.]+$", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
        private static readonly Regex _exNormAsterisks = new Regex(@"\*{2,}", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);

        private static readonly Regex _exReplaceChars = new Regex(@"[^\w\-&'*"",;\.\s]", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
        private static readonly Regex _exReplaceWords = new Regex(@"\b(ABOUT|ACCUM|AND|BT|BTG|BTI|BTP|FUZZY|HASPATH|INPATH|MINUS|NEAR|NOT|NT|NTG|NTI|NTP|OR|PT|RT|SQE|SYN|TR|TRSYN|TT|WITHIN)\b", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        

        private static readonly Regex _exShortWords = new Regex(@"(?'exact'""[^""]*"")|(?'word'[\w\*\-&']+)", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        /// <summary>                
        /// Finds critical errors in a search criterion: empty, non-paired quotes, too short masks with *.       
        /// </summary>
        /// <param name="val">A serach criterion.</param>
        /// <returns>The first found error.</returns>
        public static ValidationResult ValidateSearchCriteria(string val)
        {
            if (val == null)
            {
                return ValidationResult.Empty;
            }
            val = _exNormSpaces.Replace(val, m => m.Value.Substring(0, 1));
            val = _exTrimSpaces.Replace(val, string.Empty);
            if (val.Length == 0)
            {
                return ValidationResult.Empty;
            }
            int cnt = 0;
            foreach (char c in val)
            {
                if (c == '"')
                {
                    ++cnt;
                }
            }
            if (cnt % 2 != 0)
            {
                return ValidationResult.NotPairedQuotes;
            }
            val = val.Replace("\"", string.Empty);
            foreach (string word in val.Split(' ', ',', ';'))
            {
                string w = word.Trim();
                if (w.Length == 0)
                {
                    continue;
                }
                if (w.Contains("*") && CountNonAsterisk(w) < 3)
                {
                    return ValidationResult.TooShortPattern;
                }
            }
            return ValidationResult.OK;
        }

        public static string GetErrDescr(ValidationResult res)
        {
            return GetDescr(res);
        }

        private static int CountNonAsterisk(string val)
        {
            int res = 0;
            foreach (char c in val)
            {
                if (c != '*')
                {
                    ++res;
                }
            }
            return res;
        }

        /// <summary>        
        /// Does the first stage of preparing the arguments for a search: removing extra spaces, too short * masks, keywords, normalizing, trimming.
        /// </summary>
        /// <param name="criteria">A raw serach criterion.</param>
        /// <param name="badChars">Removed characters.</param>
        /// <param name="keywords">Removed keywords.</param>
        /// <returns></returns>
        public static string PrepareSearchCriteriaStep1(string criteria, out string badChars, out string keywords)
        {
            badChars = null;
            keywords = null;

            if (string.IsNullOrEmpty(criteria))
            {
                return criteria;
            }

            Regex[] regexes = { _exReplaceChars, _exReplaceWords };
            for (int i = 0; i < 2; ++i)
            {
                Regex rx = regexes[i];

                MatchCollection matches = rx.Matches(criteria);
                if (matches.Count > 0)
                {
                    StringBuilder bld = new StringBuilder();
                    foreach (Match m in matches)
                    {
                        if (bld.Length > 0)
                        {
                            bld.Append(", ");
                        }
                        bld.Append(m.Value);
                    }
                    if (i == 0)
                    {
                        badChars = bld.ToString();
                    }
                    else
                    {
                        keywords = bld.ToString();
                    }
                    criteria = rx.Replace(criteria, string.Empty);
                }
            }

            string removedWords = string.Empty;
            criteria = _exShortWords.Replace(criteria, delegate(Match m)
            {
                if (m.Groups["word"].Success)
                {
                    if (m.Value.Length < MinWordLength)
                    {
                        if (removedWords.Length > 0)
                        {
                            removedWords += ",";
                        }
                        removedWords += m.Groups["word"].Value;
                        return string.Empty;
                    }
                    else
                    {
                        return m.Value;
                    }
                }
                else
                {
                    return m.Value;
                }
            });

            if (removedWords.Length > 0)
            {
                if (string.IsNullOrEmpty(badChars))
                {
                    badChars = removedWords;
                }
                else
                {
                    badChars += "," + removedWords;
                }
            }

            criteria = _exNormSpaces.Replace(criteria.Trim(), m => m.Value.Substring(0, 1));
            criteria = _exTrimSpaces.Replace(criteria, string.Empty);
            return _exNormAsterisks.Replace(criteria, "*").Trim();
        }

        public static string MakeFinalCompositeSearchCriteria(params string[] criterias)
        {
            if (criterias.Length == 1)
            {
                return criterias[0];
            }
            else
            {
                StringBuilder bld = new StringBuilder();
                for (int i = 0; i < criterias.Length; ++i)
                {
                    string criteria = criterias[i];
                    if (string.IsNullOrEmpty(criteria))
                    {
                        continue;
                    }

                    if (bld.Length > 0)
                    {
                        bld.Append(criteria.StartsWith(ExclusionKeywordPref + " ") ? " " : " AND ");
                    }

                    bld.AppendFormat("{0}", criteria);
                }
                return bld.ToString();
            }
        }

        private static string GetDescr<T>(T val) where T : struct
        {
            return new DescribedEnumItem<T>(val).ToString();
        }

        /// <summary>
        /// Does the second stage of preparing: quotes are replaced by brackets, * is replaced by *, comma and semicolon is replaced by a space, spaces are normalized
        /// and words are connected by means of the specified logical operation word (AND, OR, NOT).        
        /// </summary>
        /// <param name="criteria">The search criterion.</param>
        /// <param name="connectionWord">The logical search operation.</param>
        /// <returns></returns>
        public static string PrepareSearchCriteriaStep2(string criteria, PartsConnection connectionWord)
        {
            if (string.IsNullOrEmpty(criteria))
            {
                return criteria;
            }

            criteria = _exNormSpaces.Replace(criteria, m => m.Value.Substring(0, 1)).Trim();

            string connectionW = GetDescr(connectionWord);

            StringBuilder bld = new StringBuilder();
            bool outOfPhrase = true;

            int countParts = 0;
            bool connectWordAdded = false;
            foreach (char c in criteria)
            {
                if (c == '"')
                {
                    bld.Append(c);
                    outOfPhrase = !outOfPhrase;
                    connectWordAdded = false;
                }
                else if (c == ' ' || c == ',' || c == ';' || c == '.')
                {
                    if (outOfPhrase)
                    {
                        if (connectWordAdded)
                        {
                            continue;
                        }
                        bld.Append(connectionW);
                        connectWordAdded = true;
                        ++countParts;
                    }
                    else
                    {                        
                        bld.Append(c);
                        connectWordAdded = false;
                    }
                }
                else
                {
                    bld.Append(c);
                    connectWordAdded = false;
                }
            }

            if (countParts > 0)
            {
                if (connectionWord != PartsConnection.And)
                {
                    bld.Insert(0, connectionWord == PartsConnection.Not ? ExclusionKeywordPref + " (" : "(");
                    bld.Append(")");
                }
            }
            else
            {
                if (connectionWord == PartsConnection.Not)
                {
                    bld.Insert(0, ExclusionKeywordPref + " ");
                }
            }
            criteria = bld.ToString();

            criteria = criteria.Replace('*', '%').Trim();

            return criteria;
        }

        /// <summary>
        /// Builds full text SQL-ready search expression based on three groups of words/phrazes: concatenated by And, Or, Not.
        /// </summary>
        /// <param name="andText">Words, separated by a space or comma. All of them should be prsent.</param>
        /// <param name="orText">Words, separated by a space or comma. At least one of them should be present.</param>
        /// <param name="notText">Words, separated by a space or comma. No one should be prsent.</param>
        /// <returns>Item1 - true if only notText was specified, otherwise is false. The flag is helpful for building fulltext serach SQL.
        /// It helps to handle a special case, when there is only notText part.
        /// </returns>
        public static Tuple<bool, string> BuildSearchTextExpressionExt(string andText, string orText, string notText)
        {
            bool searchExclusion = false;
            string res = FullTextSearchUtils.MakeFinalCompositeSearchCriteria(
                    FullTextSearchUtils.PrepareSearchCriteriaStep2(andText, FullTextSearchUtils.PartsConnection.And),
                    FullTextSearchUtils.PrepareSearchCriteriaStep2(orText, FullTextSearchUtils.PartsConnection.Or),
                    FullTextSearchUtils.PrepareSearchCriteriaStep2(notText, FullTextSearchUtils.PartsConnection.Not)
                );

            string pref = FullTextSearchUtils.ExclusionKeywordPref + " ";
            if (!string.IsNullOrEmpty(res) && res.StartsWith(pref))
            {
                res = res.Substring(pref.Length, res.Length - pref.Length);
                searchExclusion = true;
            }

            return new Tuple<bool, string>(searchExclusion, res);
        }

        /// <summary>
        /// Builds full text SQL-ready search expression based on 'And' concatenated words group.
        /// </summary>        
        public static string BuildSearchTextExpressionSimple(string andText)
        {
            return PrepareSearchCriteriaStep2(andText, FullTextSearchUtils.PartsConnection.And);
        }
    }

}
