using System;

namespace CqsDataFoundation.Query
{
    [Serializable]
    public class QueryBase<TResult> : IQuery<TResult>
    {        
        #region IQuery
        public virtual string CacheKey
        {
            get { return null; }
        }

        public virtual TimeSpan CacheExpirationTime
        {
            get { return QueryBaseStatics.DefaultExpirationTime; }
        }
        #endregion IQuery
    }

    internal static class QueryBaseStatics
    {
        internal static readonly TimeSpan DefaultExpirationTime = new TimeSpan(0, 30, 0);
    }
}
