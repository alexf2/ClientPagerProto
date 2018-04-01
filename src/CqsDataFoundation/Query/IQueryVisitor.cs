using CqsDataFoundation.Query.Filtering;
using CqsDataFoundation.Query.Sorting;

namespace CqsDataFoundation.Query
{
    internal interface IQueryVisitor
    {
        void Visit(FilteringCollection filtering);
        void Visit(SortingCollection sorting);
        void Visit(FilteringDescriptor fd);
        void Visit(SortingDescriptor fd);
    }
}
