using CqsDataFoundation.Query.Filtering;
using CqsDataFoundation.Query.Sorting;

namespace CqsDataFoundation.Query
{
    public interface IQueryPagged<out TResult> : IQuery<TResult>
    {
        /// <summary>
        /// Determines how many records is in a page.
        /// there is no any paging if Constants.PageSizeNoPagging is specified.
        /// </summary>
        int PageSize { get; }
        
        /// <summary>
        /// A page to return. 
        /// The numbering starts from 1.
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// Specifies whether the query handler should fill in DataPage<T>.TotalRecordsCount.
        /// </summary>
        bool ReturnTotalRecods { get; }

        bool HasPaging { get; }

        bool HasSorting { get; }

        bool HasFiltering { get; }

        SortingCollection Sorting { get; }
        FilteringCollection Filtering { get; }
    }
}
