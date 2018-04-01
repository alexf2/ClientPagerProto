
namespace CqsDataFoundation.Command
{
    public abstract class CommandHandlerBase<TDbContext, TCommand> : ICommandHandler<TCommand>
        where TDbContext : IDataContext
    {
        readonly TDbContext _ctx;
        bool _disposed;
        readonly bool _sharedContext;

        protected CommandHandlerBase(TDbContext context, bool sharedContext = false)
        {
            _ctx = context;
            _sharedContext = sharedContext;
        }

        public abstract void Handle(TCommand q);

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
            if (!_sharedContext && _ctx != null)
                _ctx.Dispose();
        }

        public TDbContext DbContextUser
        {
            get { return _ctx; }
        }
    }
}
