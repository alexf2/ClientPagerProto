using System.Runtime.Serialization;
using CqsDataFoundation.Query;

namespace CqsDataFoundation.Tests.Query
{
    [DataContract(Namespace = "http://www.com/")]
    public sealed class CustomerPageQuery : QueryPaggedBase<Customer, DataPage<Customer>>
    {
    }

    [DataContract(Namespace = "http://www.com/")]
    public sealed class CustomerRefPageQuery : QueryPaggedBase<CustomerRef, DataPage<CustomerRef>>
    {
    }
}
