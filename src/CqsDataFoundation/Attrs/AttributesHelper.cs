using System;
using System.Data;
using System.Reflection;

namespace CqsDataFoundation.Attrs
{
    public static class AttributesHelper
    {
        public static bool HasAttr<T>(Type t) where T : Attribute
        {
            return t.GetCustomAttribute<T>() != null;
        }

        public static bool NeedsCache(Type t)
        {
            return HasAttr<CacheAttribute>(t);
        }

        public static bool NeedsValidation(Type t)
        {
            return HasAttr<ValidationAttribute>(t);
        }

        public static bool NeedsRetry(Type t)
        {
            return HasAttr<RetryAttribute>(t);
        }

        public static bool NeedsTransaction(Type t)
        {
            return HasAttr<TransactionAttribute>(t);
        }

        public static IsolationLevel? GetTransactionlevel(Type t)
        {
            var attr = t.GetCustomAttribute<TransactionAttribute>();
            return attr == null ? (IsolationLevel?)null : attr.Level;
        }
    }
}
