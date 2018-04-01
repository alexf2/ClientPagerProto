using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;

namespace CqsDataFoundation.Query.Filtering
{
    [Serializable]
    public struct FilteringDescriptor : IComparable<FilteringDescriptor>, IComparable, IEquatable<FilteringDescriptor>
    {
        [DataMember(Order = 1)]
        public string Field;

        [DataMember(Order = 2)]
        [DefaultValue(CriterionPredicate.Eq)]
        public CriterionPredicate Predicate;

        [DataMember(Order = 3)]
        public object Criterion;

        [DataMember(Order = 4)]
        [DefaultValue(false)]
        public bool IgnoreCase;

        public void GetDescription(StringBuilder descriptionBuilder)
        {
            descriptionBuilder.AppendFormat("{0}_{1}_{2}", Field, Predicate, Criterion ?? "EMPTY");
        }        

        public static FilteringDescriptor Get<TEntity>(
           Expression<Func<TEntity, object>> memberSelector,           
           object criterion,
           CriterionPredicate predicate = CriterionPredicate.Eq,
           bool ignoreCase = false)
        {
            return new FilteringDescriptor()
            {
                Field = LinqExpressionHelper.GetMembersChain(memberSelector),
                Predicate = predicate,
                Criterion = criterion,
                IgnoreCase = ignoreCase
            };
        }

        internal void Accept(IQueryVisitor visitor)
        {
            visitor.Visit(this);
        }

        #region Object
        public override int GetHashCode()
        {
            return 381 ^ (Field ?? string.Empty).GetHashCode() ^ (int)Predicate ^ (Criterion == null ? 0 : Criterion.GetHashCode());
        }
        public override bool Equals(object obj)
        {
            return obj is FilteringDescriptor && Equals((FilteringDescriptor)obj);
        }
        public override string ToString()
        {
            var bld = new StringBuilder();
            GetDescription(bld);
            return bld.ToString();
        }
        #endregion Object

        #region Operators
        public static bool operator ==(FilteringDescriptor n1, FilteringDescriptor n2)
        {
            return n1.Equals(n2);
        }
        public static bool operator !=(FilteringDescriptor n1, FilteringDescriptor n2)
        {
            return !n1.Equals(n1);
        }
        public static bool operator <(FilteringDescriptor n1, FilteringDescriptor n2)
        {
            return n1.CompareTo(n2) < 0;
        }
        public static bool operator >(FilteringDescriptor n1, FilteringDescriptor n2)
        {
            return n1.CompareTo(n2) > 0;
        }
        #endregion Operators

        #region IComparable<T>
        public int CompareTo(FilteringDescriptor other)
        {
            if (Equals(other))
                return 0;

            int res = Comparer<string>.Default.Compare(Field, other.Field);
            if (res == 0)
                res = Comparer<int>.Default.Compare((int)Predicate, (int)other.Predicate);
            if (res == 0)
                res = Comparer<object>.Default.Compare(Criterion, other.Criterion);
            
            return res;
        }
        #endregion IComparable<T>

        #region IComparable
        int IComparable.CompareTo(object other)
        {
            if (!(other is FilteringDescriptor))
                throw new InvalidOperationException("CompareTo: Not a FilteringDescriptor");
            return CompareTo((FilteringDescriptor)other);
        }
        #endregion IComparable

        #region Equatable<T>
        public bool Equals(FilteringDescriptor val)
        {
            return Field == val.Field && Predicate == val.Predicate && Equals(Criterion, val.Criterion);
        }
        #endregion Equatable<T> 
    }
}
