using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using CqsDataFoundation.Query.Filtering;
using CqsDataFoundation.Query.Sorting;

namespace CqsDataFoundation.Query.Providers
{
    internal sealed class SqlProvider : IQueryVisitor
    {
        private const string EscapeSlqLike = "!";

        static readonly Regex ExScreeningSql = new Regex(@"\'",
            RegexOptions.ExplicitCapture | RegexOptions.Singleline | RegexOptions.CultureInvariant);

        static readonly Regex ExProhibitedLikeSymbols = new Regex(@"[%_\[\]\^]",
            RegexOptions.ExplicitCapture | RegexOptions.Singleline | RegexOptions.CultureInvariant);

        static readonly Regex ExProhibitedLikeSymbolsExtended = new Regex(@"[%_\[\]\^\!]",
            RegexOptions.ExplicitCapture | RegexOptions.Singleline | RegexOptions.CultureInvariant);

        readonly bool _unicode;
        StringBuilder _bld;        

        public SqlProvider(bool unicode)
        {
            _unicode = unicode;
        }

        public void BuildFilterExpression(StringBuilder bld, FilteringCollection coll)
        {
            _bld = bld;
            coll.Accept(this);
            _bld = null;
        }
        public void BuildSortExpression(StringBuilder bld, SortingCollection coll)
        {
            _bld = bld;
            coll.Accept(this);
            _bld = null;
        }

        #region IQueryVisitor
        void IQueryVisitor.Visit(SortingCollection sorting)
        {
            for (int i = 0; i < sorting.Count; ++i)
            {
                if (i > 0)
                    _bld.Append(", ");
                sorting[ i ].Accept(this);
            }            
        }
        void IQueryVisitor.Visit(SortingDescriptor fd)
        {
            _bld.AppendFormat("[{0}]{1}", fd.Field, fd.Desc ? " desc":string.Empty);
        }

        void IQueryVisitor.Visit(FilteringCollection filtering)
        {
            var combineWird = filtering.CombiningRule == FilteringCollection.CombiningFilters.And ? " and " : " or ";
            for (int i = 0; i < filtering.Count; ++i)
            {
                if (i > 0)
                    _bld.Append(combineWird);
                filtering[ i ].Accept(this);
            }            

        }
        void IQueryVisitor.Visit(FilteringDescriptor fd)
        {
            Tuple<string, bool> p = GetSqlPredicate(fd.Predicate);
            if (p.Item2)
                _bld.AppendFormat("[{0}] {1} {2}", fd.Field, p.Item1, Quote(fd.Criterion, fd.Predicate));
            else
                _bld.AppendFormat("[{0}] {1}", fd.Field, p.Item1);
        }
        #endregion IQueryVisitor

        #region Processing
        string Quote(object val, CriterionPredicate pred)
        {
            var res = new StringBuilder();
            bool needQuotes = false;
            bool escapedLike = false;
            bool sqlLike = pred == CriterionPredicate.StartsWith || pred == CriterionPredicate.Contains ||
                           pred == CriterionPredicate.EndsWith;
            if (val != null)
            {
                string valString = Convert.ToString(val, CultureInfo.InvariantCulture);
                if (IsNumber(val))
                    res.Append(valString);
                else
                {
                    Type objType = val.GetType();
                    objType = Nullable.GetUnderlyingType(objType) ?? objType;

                    if (typeof(bool) == objType)
                        res.Append((bool)val ? "1" : "0");
                    else
                    {
                        needQuotes = true;
                        res.Append(SqlCleanString(valString, sqlLike, ref escapedLike));
                    }
                }
            }

            if (pred == CriterionPredicate.StartsWith)
                res.Append('%');
            else if (pred == CriterionPredicate.Contains)
            {
                res.Append('%');
                res.Insert(0, '%');
            }
            else if (pred == CriterionPredicate.EndsWith)
                res.Insert(0, '%');

            if (needQuotes || sqlLike)
            {
                res.Append('\'');
                res.Insert(0, '\'');
                if (_unicode)
                    res.Insert(0, 'N');

                if (escapedLike)
                    res.AppendFormat(" escape '{0}'", EscapeSlqLike);
            }

            return res.ToString();
        }

        public static string SqlCleanString(string val, bool sqlLike, ref bool escaped)
        {
            if (string.IsNullOrEmpty(val))
                return val;

            val = ExScreeningSql.Replace(val, "''");
            if (sqlLike && ExProhibitedLikeSymbols.IsMatch(val))
            {
                val = ExProhibitedLikeSymbolsExtended.Replace(val, (Match m) => EscapeSlqLike + m.Value);
                escaped = true;
            }

            return val;
        }

        public static bool IsNumber(object obj)
        {
            if (Equals(obj, null))
            {
                return false;
            }

            Type objType = obj.GetType();
            objType = Nullable.GetUnderlyingType(objType) ?? objType;

            if (objType.IsPrimitive)
            {
                return objType != typeof(bool) &&
                    objType != typeof(char) &&
                    objType != typeof(IntPtr) &&
                    objType != typeof(UIntPtr);
            }

            return objType == typeof(decimal);
        }

        static Tuple<string, bool> GetSqlPredicate(CriterionPredicate pred)
        {
            string predSql = null;
            bool hasValue = true;
            switch (pred)
            {
                case CriterionPredicate.Eq:
                    predSql = "=";
                    break;

                case CriterionPredicate.Gt:
                    predSql = ">";
                    break;

                case CriterionPredicate.GtEq:
                    predSql = ">=";
                    break;

                case CriterionPredicate.Lt:
                    predSql = "<";
                    break;

                case CriterionPredicate.LtEq:
                    predSql = "<=";
                    break;

                case CriterionPredicate.Neq:
                    predSql = "<>";
                    break;

                case CriterionPredicate.Null:
                    predSql = "is null";
                    hasValue = false;
                    break;

                case CriterionPredicate.NotNull:
                    predSql = "is not null";
                    hasValue = false;
                    break;

                case CriterionPredicate.StartsWith:
                case CriterionPredicate.EndsWith:
                case CriterionPredicate.Contains:
                    predSql = "like";
                    break;

            }

            return new Tuple<string, bool>(predSql, hasValue);
        }
        #endregion Processing
    }    
}
