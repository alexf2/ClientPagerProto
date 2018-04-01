using System;

namespace CqsDataFoundation.Query
{
    /// <summary>
    /// Represents a generic cachable query.
    /// </summary>
    /// <typeparam name="TResult">Query result type. Used as a distinguishing marker. Is needed for retrieving a query handler off IoC container.</typeparam>
    public interface IQuery<out TResult>
    {
        string CacheKey
        {
            get;
        }

        TimeSpan CacheExpirationTime
        {
            get;
        }
    }
}
