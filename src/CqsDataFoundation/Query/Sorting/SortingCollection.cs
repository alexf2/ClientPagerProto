using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CqsDataFoundation.Query.Sorting
{
    [Serializable]
    public sealed class SortingCollection : List<SortingDescriptor>
    {
        public SortingCollection()
        {            
        }

        public SortingCollection(IEnumerable<SortingDescriptor> lst): base(lst)
        {            
        }

        public void GetDescription(StringBuilder bld)
        {
            if (Count > 0)
            {                
                for (int i = 0; i < Count; ++i)
                {
                    if (i > 0)
                        bld.Append(',');
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

        public static SortingCollection GetDefaultSorting<T>()
        {
            var itemType = typeof(T);

            var itemTypeProperties = itemType.GetProperties();

            var explicitKeyProperties = itemTypeProperties.Where(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Any()).ToArray();

            if (explicitKeyProperties.Any())
                return new SortingCollection(explicitKeyProperties.Select(p => new SortingDescriptor() { Field = p.Name }));

            var keyProperty = itemTypeProperties.FirstOrDefault(p => string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase));
            if (keyProperty != null)
                return new SortingCollection(new[]{new SortingDescriptor() { Field = keyProperty.Name }});

            keyProperty = itemTypeProperties.FirstOrDefault(p => string.Equals(p.Name, string.Format("{0}Id", itemType.Name), StringComparison.OrdinalIgnoreCase));
            if (keyProperty != null)
                return new SortingCollection(new[] { new SortingDescriptor() { Field = keyProperty.Name } });

            return new SortingCollection();
        }
    }
}
