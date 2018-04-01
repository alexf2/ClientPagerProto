using System;
using System.Threading;

namespace CqsDataFoundation
{
    public abstract class DataContextMgrBase<TContext, TTran>
        where TContext : class, IDisposable
        where TTran : class
    {
        public event Action<IDataContext> ChangesSubmitted;

        protected readonly IDbContextConfig _cfg;

        protected TContext _context;
        protected TTran _tran;

        protected bool _disposed;
        protected readonly object _lockObj = new object();

        protected DataContextMgrBase(IDbContextConfig cfg)
        {
            _cfg = cfg;
        }

        public TContext DbContext
        {
            get { EnsureCreated(); return _context; }
        }
        public TTran Tran
        {
            get { return _tran; }
        }

        public IDbContextConfig Configuration
        {
            get { return _cfg; }
        }

        protected abstract TContext CreateDbContext(IDbContextConfig cfg);
        protected abstract void InitConnection(TContext conn);
        public abstract void CommitTran();
        public abstract void RollbackTran();

        public void EnsureCreated()
        {
            if (_disposed)
                throw new ObjectDisposedException(this.GetType().Name);

            if (_context == null)
                lock (_lockObj)
                    if (_context == null)
                    {
                        TContext tmp = null;
                        try
                        {
                            tmp = CreateDbContext(_cfg);
                            InitConnection(tmp);
                        }
                        catch (Exception)
                        {
                            if (tmp != null)
                                tmp.Dispose();
                            throw;
                        }
                        Interlocked.Exchange(ref _context, tmp);
                    }
        }

        public virtual void SubmitUnitOfWork()
        {
            OnSubmitted();
        }

        void OnSubmitted()
        {            
            var handler = Volatile.Read(ref ChangesSubmitted);
            if (handler != null)
                handler((IDataContext)this);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            var conn = Interlocked.Exchange(ref _context, null);
            if (conn != null)
            {
                RollbackTran();
                conn.Dispose();
            }
        }
    }

}