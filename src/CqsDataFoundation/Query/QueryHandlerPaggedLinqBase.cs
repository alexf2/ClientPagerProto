using System;
using System.Linq;
using CqsDataFoundation.Query.Providers;


namespace CqsDataFoundation.Query
{
    public abstract class QueryHandlerPaggedLinqBase<TDbContext, TQuery, TResult, TResultItem> : QueryHandlerBase<TDbContext, TQuery, TResult>
        where TQuery : IQueryPagged<TResult>
        where TDbContext : IDataContext
        where TResult : DataPage<TResultItem>, new()
    {        

        protected QueryHandlerPaggedLinqBase(TDbContext context, bool sharedContext = false)
            : base(context, sharedContext)
        {
        }

        protected virtual TResult CreateResult()
        {
            return null;
        }

        protected TResult GetPage(TQuery q, IQueryable<TResultItem> query)
        {
            var res = CreateResult() ?? new TResult();
  
            var prepared = BuildQuery(q, query, res, q.ReturnTotalRecods);            
            res.Data = prepared.ToList();

            return res;
        }

        protected DataPage<T> GetPage<T>(TQuery q, IQueryable<T> query)
        {
            var res = new DataPage<T>();

            var prepared = BuildQuery(q, query, res, q.ReturnTotalRecods);
            res.Data = prepared.ToList();

            return res;
        }

        protected DataPage<T2> GetPage<T, T2>(TQuery q, IQueryable<T> query, Func<T, T2> resultConversion)
        {
            var res = new DataPage<T2>();

            var prepared = BuildQuery(q, query, res, q.ReturnTotalRecods);
            res.Data = prepared.Select(resultConversion).ToList();

            return res;
        }

        #region Building query
        protected IQueryable<T> BuildQuery<T, TRes>(TQuery q, IQueryable<T> query, DataPage<TRes> res, bool returnTotalCount = true)
        {
            if (q.HasFiltering)
                query = ApplyFiltering(q, query);

            if (returnTotalCount)
            {
                res.TotalRecordsCount = query.Count();
                res.PageNumber = CalculateAcquiredPageNumber(res.TotalRecordsCount, q);
                res.TotalPages = CalculateTotalPages(res.TotalRecordsCount, q);
            }

            if (q.HasSorting)
                query = ApplySorting(q, query);
            if (q.HasPaging)
                query = ApplyPaging(q, query, returnTotalCount ? res:null);

            return query;
        }
        
        protected IQueryable<T> ApplyPaging<T, T2>(TQuery q, IQueryable<T> query, DataPage<T2> res)
        {
            return new LinqProvider<T>().ApplyPaging(query, q.PageSize, res == null ? q.PageNumber : res.PageNumber);
        }

        protected IQueryable<T> ApplySorting<T>(TQuery q, IQueryable<T> query)
        {
            return new LinqProvider<T>().ApplySorting(query, q.Sorting);
        }

        protected IQueryable<T> ApplyFiltering<T>(TQuery q, IQueryable<T> query)
        {
            return new LinqProvider<T>().ApplyFiltering(query, q.Filtering);
        }
        #endregion Building query

        #region Helpers
        protected static int CalculateAcquiredPageNumber(int toltalRecords, IQueryPagged<TResult> query)
        {
            if (toltalRecords == 0)
                return 0;

            var totalPages = CalculateTotalPages(toltalRecords, query);

            return Math.Min(totalPages, Math.Max(1, query.PageNumber));
        }

        public static int CalculateTotalPages(int toltalRecords, IQueryPagged<TResult> query)
        {
            if (query.PageSize == Constants.PageSizeNoPagging) 
                return toltalRecords == 0 ? 0 : 1;
            
            return toltalRecords / query.PageSize + (toltalRecords % query.PageSize > 0 ? 1 : 0);
        }
        #endregion Helpers
    }

    
}
