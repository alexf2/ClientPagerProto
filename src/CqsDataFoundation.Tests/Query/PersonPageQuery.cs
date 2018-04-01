using System.Runtime.Serialization;
using CqsDataFoundation.Query;

namespace CqsDataFoundation.Tests.Query
{
    [DataContract(Namespace = "http://www.com/")]
    public sealed class PersonPageQuery : QueryPaggedBase<Person, DataPage<Person>>
    {
    }
}
