
namespace CqsDataFoundation.Query
{
    public abstract class QueryHandlerBase<TDbContext, TQuery, TResult> : IQueryHandler<TQuery, TResult> 
        where TQuery : IQuery<TResult> 
        where TDbContext: IDataContext
    {
        readonly TDbContext _ctx;
        bool _disposed;
        readonly bool _sharedContext;

        protected QueryHandlerBase(TDbContext context, bool sharedContext = false)
        {
            _ctx = context;
            _sharedContext = sharedContext;
        }

        public abstract TResult Handle(TQuery q);

        public IDataContext DbContext
        {
            get { return _ctx; }
        }
        
        public void Dispose()
        {
            Disposing(true);
        }

        protected virtual void Disposing(bool dispose)
        {
            if (_disposed) return;
            _disposed = true;
            if (!_sharedContext &&  _ctx != null)
                _ctx.Dispose();
        }

        public TDbContext DbContextUser
        {
            get { return _ctx; }
        }
    }
}