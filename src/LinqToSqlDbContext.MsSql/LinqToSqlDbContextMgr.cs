using System.Data;
using System.Data.Common;
using System.Data.Linq;
using System.Threading;
using CqsDataFoundation;

namespace Cqs.LinqToSqlDbContext.MsSql
{
    public abstract class LinqToSqlDbContextMgr<TContext> : DataContextMgrBase<TContext, DbTransaction>, IDataContext
        where TContext : DataContext
    {
        protected LinqToSqlDbContextMgr(IDbContextConfig cfg)
            : base(cfg)
        {
        }

        #region DbContextBase overrides
        protected override void InitConnection(TContext conn)
        {
            if (DefaultIsolationLevel != null)
                conn.ExecuteCommand("set transaction isolation level {0}; set nocount on;", DbHelper.ConvertToSqlString(DefaultIsolationLevel.Value));
            conn.CommandTimeout = _cfg.CommandTimeoutSec;
        }

        public override void CommitTran()
        {
            var t = Interlocked.Exchange(ref _tran, null);
            if (t != null)
                using (t) { t.Commit(); _context.Transaction = null; t.Dispose(); }
        }

        public override void RollbackTran()
        {
            var t = Interlocked.Exchange(ref _tran, null);
            if (t != null)
                using (t) { t.Rollback(); _context.Transaction = null; t.Dispose(); }
        }
        #endregion DbContextBase overrides

        public void BeginTran(System.Transactions.IsolationLevel isLvl = System.Transactions.IsolationLevel.ReadCommitted)
        {
            EnsureCreated();
            if ((_context.Connection.State & ConnectionState.Open) != ConnectionState.Open)
                _context.Connection.Open();
            _tran = _context.Connection.BeginTransaction(DbHelper.ConvertTodataIsoLevel(isLvl));
            _context.Transaction = _tran;
        }

        /// <summary>
        /// If set, then on connection opening executed an instruction, setting the isolation level.
        /// </summary>
        public System.Transactions.IsolationLevel? DefaultIsolationLevel
        {
            get;
            set;
        }

        public override void SubmitUnitOfWork()
        {
            DbContext.SubmitChanges();
            base.SubmitUnitOfWork();
        }
    }
}
