using System;

namespace CqsDataFoundation
{
    public static class DbHelper
    {
        public static System.Data.IsolationLevel ConvertTodataIsoLevel(System.Transactions.IsolationLevel iso)
        {
            switch (iso)
            {
                case System.Transactions.IsolationLevel.Chaos:
                    return System.Data.IsolationLevel.Chaos;

                case System.Transactions.IsolationLevel.ReadCommitted:
                    return System.Data.IsolationLevel.ReadCommitted;

                case System.Transactions.IsolationLevel.ReadUncommitted:
                    return System.Data.IsolationLevel.ReadUncommitted;

                case System.Transactions.IsolationLevel.RepeatableRead:
                    return System.Data.IsolationLevel.RepeatableRead;

                case System.Transactions.IsolationLevel.Serializable:
                    return System.Data.IsolationLevel.Serializable;

                case System.Transactions.IsolationLevel.Snapshot:
                    return System.Data.IsolationLevel.Snapshot;

                case System.Transactions.IsolationLevel.Unspecified:
                    return System.Data.IsolationLevel.Unspecified;
            }
            return System.Data.IsolationLevel.Unspecified;
        }

        public static string ConvertToSqlString(System.Transactions.IsolationLevel iso)
        {
            string isoLevel = "read committed";
            switch (iso)
            {
                case System.Transactions.IsolationLevel.Chaos:
                case System.Transactions.IsolationLevel.ReadUncommitted:
                    isoLevel = "READ UNCOMMITTED";
                    break;

                case System.Transactions.IsolationLevel.RepeatableRead:
                    isoLevel = "REPEATABLE READ";
                    break;

                case System.Transactions.IsolationLevel.Serializable:
                    isoLevel = "SERIALIZABLE";
                    break;

                case System.Transactions.IsolationLevel.Snapshot:
                    isoLevel = "SNAPSHOT";
                    break;
            }
            return isoLevel;
        }

        public static object GetStrParam(string val)
        {
            if (val != null)
                val = val.Trim();
            return string.IsNullOrEmpty(val) ? (object)DBNull.Value : (object)val;
        }
    }
}
