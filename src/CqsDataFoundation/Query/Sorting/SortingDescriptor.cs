using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;

namespace CqsDataFoundation.Query.Sorting
{
    [Serializable]
    public struct SortingDescriptor : IComparable<SortingDescriptor>, IComparable, IEquatable<SortingDescriptor>
    {
        [DataMember(Order = 1)]
        public string Field;

        [DataMember(Order = 2)]
        [DefaultValue(false)]
        public bool Desc;

        public void GetDescription(StringBuilder descriptionBuilder)
        {
            descriptionBuilder.Append(Field);
            if (Desc)
                descriptionBuilder.Append(" DSC");            
        }        

        public static SortingDescriptor Get<TEntity>(Expression<Func<TEntity, object>> memberSelector, bool desc)
        {
            return new SortingDescriptor
            {
                Field = LinqExpressionHelper.GetMembersChain(memberSelector),
                Desc = desc
            };
        }

        internal void Accept(IQueryVisitor visitor)
        {
            visitor.Visit(this);
        }

        #region Object
        public override int GetHashCode()
        {
            return 381 ^ (Field ?? string.Empty).GetHashCode() ^ Desc.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return obj is SortingDescriptor && Equals((SortingDescriptor)obj);
        }
        public override string ToString()
        {
            var bld = new StringBuilder();
            GetDescription(bld);
            return bld.ToString();
        }        
        #endregion Object

        #region Operators
        public static bool operator ==(SortingDescriptor n1, SortingDescriptor n2)
        {
            return n1.Equals(n2);
        }
        public static bool operator !=(SortingDescriptor n1, SortingDescriptor n2)
        {
            return !n1.Equals(n1);
        }
        public static bool operator <(SortingDescriptor n1, SortingDescriptor n2)
        {
            return n1.CompareTo(n2) < 0;
        }
        public static bool operator >(SortingDescriptor n1, SortingDescriptor n2)
        {
            return n1.CompareTo(n2) > 0;
        }
        #endregion Operators

        #region IComparable<T>
        public int CompareTo(SortingDescriptor other)
        {
            if (Equals(other))
                return 0;

            int res = Comparer<string>.Default.Compare(Field, other.Field);
            if (res == 0)
                res = Desc.CompareTo(other.Desc);            

            return res;
        }
        #endregion IComparable<T>

        #region IComparable
        int IComparable.CompareTo(object other)
        {
            if (!(other is SortingDescriptor))
                throw new InvalidOperationException("CompareTo: Not a SortingDescriptor");
            return CompareTo((SortingDescriptor)other);
        }
        #endregion IComparable

        #region Equatable<T>
        public bool Equals(SortingDescriptor val)
        {
            return Field == val.Field && Desc == val.Desc;
        }
        #endregion Equatable<T> 
    }
}