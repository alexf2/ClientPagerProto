
namespace CqsDataFoundation.Query.Decorators
{
    public sealed class CacheQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        readonly IQueryHandler<TQuery, TResult> _decoratedHndl;
        readonly IDataCache _cache;

        public CacheQueryHandlerDecorator(IQueryHandler<TQuery, TResult> decorated, IDataCache cache)
        {
            _decoratedHndl = decorated;
            _cache = cache;
        }

        #region IQueryHandler
        public TResult Handle(TQuery query)
        {
            if (string.IsNullOrEmpty(query.CacheKey))
                return _decoratedHndl.Handle(query);
            else
            {
                TResult res = _cache.Get<TResult>(query.CacheKey);
                if (res == null)
                    lock (_cache)
                    {
                        res = _cache.Get<TResult>(query.CacheKey);
                        if (res == null)
                        {
                            res = _decoratedHndl.Handle(query);
                            _cache.Set(query.CacheKey, res, query.CacheExpirationTime);
                        }
                    }
                return res;
            }
        }

        public IDataContext DbContext
        {
            get { return _decoratedHndl.DbContext; }
        }
        #endregion IQueryHandler

        public void Dispose()
        {
            _decoratedHndl.Dispose();
        }
    }
}