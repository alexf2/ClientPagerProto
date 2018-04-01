using System;
using System.Threading;

namespace CqsDataFoundation
{
    public class ObjectDbContextMgr<T> : IDataContext where T:class
    {
        public class DbContextNullConfig : IDbContextConfig
        {
            static readonly Lazy<DbContextNullConfig> _instance = new Lazy<DbContextNullConfig>(() => new DbContextNullConfig(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            public static DbContextNullConfig Instance
            {
                get
                {
                    return _instance.Value;
                }
            }

            DbContextNullConfig()
            {
            }

            public string DefaultConnectionString
            {
                get { return "<connection string null>"; }
            }

            public string GetConnectionString(string name)
            {
                return DefaultConnectionString;
            }
            
            
            public int CommandTimeoutSec
            {
                get { return 30; }
            }
        }

        public event Action<IDataContext> ChangesSubmitted;
        
        bool _disposed;
        readonly bool _sharedSource;

        public ObjectDbContextMgr(T dataSource, bool sharedSource = false)
        {
            DataSource = dataSource;
            _sharedSource = sharedSource;
        }

        public T DataSource { get; private set; }

        #region IDbContext
        public IDbContextConfig Configuration
        {
            get { return DbContextNullConfig.Instance; }
        }

        public void EnsureCreated()
        {
            if (_disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }

        public virtual void BeginTran(System.Transactions.IsolationLevel isLvl = System.Transactions.IsolationLevel.ReadCommitted)
        { }
        public virtual void CommitTran() 
        { }
        public virtual void RollbackTran() 
        { }
                
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


        public System.Transactions.IsolationLevel? DefaultIsolationLevel
        {
            get;
            set;
        }
        #endregion IDbContext

        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;

            if (!_sharedSource)
            {
                var dsp = DataSource as IDisposable;
                if(dsp != null)
                    dsp.Dispose();
            }
        }
    }

 }
