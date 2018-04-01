using System;

namespace CqsDataFoundation.Attrs
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class RetryAttribute : Attribute
    {
    }
}
