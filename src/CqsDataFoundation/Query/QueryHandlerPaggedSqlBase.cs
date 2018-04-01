using System.Text;
using CqsDataFoundation.Query.Providers;


namespace CqsDataFoundation.Query
{
    public abstract class QueryHandlerPaggedSqlBase<TDbContext, TQuery, TResult, TResultItem> : QueryHandlerBase<TDbContext, TQuery, TResult>
        where TQuery : IQueryPagged<TResult>
        where TDbContext : IDataContext
        where TResult : DataPage<TResultItem>, new()
    {
        readonly bool _unicode;

        protected QueryHandlerPaggedSqlBase(TDbContext context, bool sharedContext, bool unicode = false)
            : base(context, sharedContext)
        {
            _unicode = unicode;
        }

        protected void GetSortSqlExpression(TQuery q, StringBuilder bld)
        {
            if(q.HasSorting)
                new SqlProvider(_unicode).BuildSortExpression(bld, q.Sorting);
        }

        protected void GetFilterExpression(TQuery q, StringBuilder bld)
        {
            if(q.HasFiltering)
                new SqlProvider(_unicode).BuildFilterExpression(bld, q.Filtering);            
        }        
    }
    
}
