using System.Data;
using System.Data.Entity;
using System.Threading;
using CqsDataFoundation;

namespace Cqs.EntityFrameworkDbContext.MsSql
{
    public abstract class EntityFrameworkDbContextMgr<TContext> : DataContextMgrBase<TContext, DbContextTransaction>, IDataContext
        where TContext : DbContext
    {
        protected EntityFrameworkDbContextMgr(IDbContextConfig cfg)
            : base(cfg)
        {
        }

        #region DbContextBase overrides
        protected override void InitConnection(TContext conn)
        {
            if (DefaultIsolationLevel != null)
                conn.Database.ExecuteSqlCommand("set transaction isolation level {0}; set nocount on;", DbHelper.ConvertToSqlString(DefaultIsolationLevel.Value));
            conn.Database.CommandTimeout = _cfg.CommandTimeoutSec;
        }

        public override void CommitTran()
        {
            var t = Interlocked.Exchange(ref _tran, null);
            if (t != null)
                using (t) { t.Commit(); t.Dispose(); }
        }

        public override void RollbackTran()
        {
            var t = Interlocked.Exchange(ref _tran, null);
            if (t != null)
                using (t) { t.Rollback(); t.Dispose(); }
        }
        #endregion DbContextBase overrides

        public void BeginTran(System.Transactions.IsolationLevel isLvl = System.Transactions.IsolationLevel.ReadCommitted)
        {
            EnsureCreated();

            if ((_context.Database.Connection.State & ConnectionState.Open) != ConnectionState.Open)
                _context.Database.Connection.Open();
            _tran = _context.Database.BeginTransaction(DbHelper.ConvertTodataIsoLevel(isLvl));                           
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
            DbContext.SaveChanges();
            base.SubmitUnitOfWork();
        }
    }
}
