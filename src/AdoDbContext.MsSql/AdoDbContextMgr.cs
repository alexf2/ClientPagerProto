using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using AutoMapper;
using CqsDataFoundation;
using CqsDataFoundation.Query;

namespace Cqs.AdoDbContext.MsSql
{
    public class AdoDbContextMgr : DataContextMgrBase<SqlConnection, IDbTransaction>, IDataContext
    {
        public struct RowsCount
        {
            public int TotalRows { get; set; }
        };

        const string PageQueryCommand = "with cte as " +
                "( " +
                    "  select count(*) over() TotalRows, {0}, row_number() over(order by {1}) RN from dbo.{2} " +
                    "  {3} " +
                ") " +

                "select * " +
                    "from cte where RN between (@PageNumber - 1) * @PageSize + 1 AND @PageNumber * @PageSize;";

        const string InsCommand = "insert into dbo.{0} ({1}) " +
                "values({2}); select cast(SCOPE_IDENTITY() as int);";

        const string UpdCommand = "update dbo.{0} set {1} {2}";


        public AdoDbContextMgr(IDbContextConfig cfg)
            : base(cfg)
        {
        }

        #region DbContextBase overrides
        protected override SqlConnection CreateDbContext(IDbContextConfig cfg)
        {
            return new SqlConnection(cfg.DefaultConnectionString);
        }
        protected override void InitConnection(SqlConnection conn)
        {
            conn.Open();
            if (DefaultIsolationLevel != null)
            {
                var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format("set transaction isolation level {0}; set nocount on;", DbHelper.ConvertToSqlString(DefaultIsolationLevel.Value));

                cmd.ExecuteNonQuery();
            }
        }
        public override void CommitTran()
        {
            IDbTransaction t = Interlocked.Exchange(ref _tran, null);
            if (t != null)
                using (t) { t.Commit(); t.Dispose(); }
        }

        public override void RollbackTran()
        {
            IDbTransaction t = Interlocked.Exchange(ref _tran, null);
            if (t != null)
                using (t) { t.Rollback(); t.Dispose(); }
        }
        #endregion DbContextBase overrides

        public SqlCommand CreateCommand(string cmdText, CommandType t = CommandType.Text)
        {
            EnsureCreated();

            var cmd = _context.CreateCommand();
            cmd.CommandType = t;
            cmd.CommandTimeout = _cfg.CommandTimeoutSec;
            cmd.CommandText = cmdText;

            return cmd;
        }

        public void BeginTran(System.Transactions.IsolationLevel isLvl = System.Transactions.IsolationLevel.ReadCommitted)
        {
            EnsureCreated();
            _tran = _context.BeginTransaction(DbHelper.ConvertTodataIsoLevel(isLvl));
        }

        /// <summary>
        /// If set, then on connection opening executed an instruction, setting the isolation level.
        /// </summary>
        public System.Transactions.IsolationLevel? DefaultIsolationLevel
        {
            get;
            set;
        }

        public static void AddPagingParams<TItem, TResult>(SqlCommand cmd, QueryPaggedBase<TItem, TResult> q)
        {
            cmd.Parameters.Add("PageSize", SqlDbType.Int).Value = q.PageSize;
            cmd.Parameters.Add("PageNumber", SqlDbType.Int).Value = q.PageNumber;
        }

        public static string GetPageCommand(string cols, string order, string table, string conditions)
        {
            return string.Format(PageQueryCommand, cols, order, table, string.IsNullOrEmpty(conditions) ? " " : "where " + conditions);
        }

        public static string GetAddCommand(string table, string fields, string vars)
        {
            return string.Format(InsCommand, table, fields, vars);
        }

        public static string GetUpdateCommand(string table, string fields, string vars, string where)
        {
            return string.Format(UpdCommand, table, fields, vars, string.IsNullOrEmpty(where) ? string.Empty : "where " + where);
        }

        public static IEnumerable<T> ExecuteReader<T>(SqlCommand cmd, Action<IDataReader> onFirst = null)
        {            
            var first = true;
            using (var rd = cmd.ExecuteReader())
                while (rd.Read())
                {
                    if (first)
                    {
                        first = false;
                        onFirst(rd);
                    }
                    yield return Mapper.Map<IDataRecord, T>((IDataRecord)rd);
                }
        }

        public static IList<T> ExecuteReaderBuffered<T>(SqlCommand cmd, Action<IDataReader> onFirst = null)
        {            
            var res = new List<T>();

            var first = true;
            using (var rd = cmd.ExecuteReader())
                while (rd.Read())
                {
                    if (first)
                    {
                        first = false;
                        onFirst(rd);
                    }

                    res.Add(Mapper.Map<IDataRecord, T>((IDataRecord)rd));
                }
            return res;            
        }

        public static T ExecuteReaderOnce<T>(SqlCommand cmd)
        {
            using (var rd = cmd.ExecuteReader(CommandBehavior.SingleResult))
                while (rd.Read())
                {
                    return Mapper.Map<IDataRecord, T>((IDataRecord)rd);
                }
            return default(T);
        }

        public static T ExecuteScalar<T>(SqlCommand cmd)
        {
            var res = cmd.ExecuteScalar();
            return res == null || res is DBNull ? default(T) : Mapper.Map<T>(res);
        }
    }
}
