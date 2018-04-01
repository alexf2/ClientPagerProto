using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace CqsDataFoundation.Query.Filtering
{
    [Serializable]
    public sealed class FilteringCollection : List<FilteringDescriptor>
    {
        public enum CombiningFilters
        {
            And = 0,
            Or = 1
        }

        public FilteringCollection()
        {            
        }

        public FilteringCollection(IEnumerable<FilteringDescriptor> lst)
            : base(lst)
        {            
        }

        [DataMember(Order = 10)]
        [DefaultValue(CombiningFilters.And)]
        public CombiningFilters CombiningRule
        {
            get; set;
        }        

        public void GetDescription(StringBuilder bld)
        {
            if (Count > 0)
            {
                var joinOperation = string.Format("_{0}_", CombiningRule);
                for (int i = 0; i < Count; ++i)
                {
                    if (i > 0)
                        bld.Append(joinOperation);
                    this[i].GetDescription(bld);
                }
            }
        }

        public override string ToString()
        {
            var bld = new StringBuilder();
            GetDescription(bld);
            return bld.ToString();
        }

        internal void Accept(IQueryVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
