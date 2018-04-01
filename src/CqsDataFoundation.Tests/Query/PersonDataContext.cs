using CqsDataFoundation;

namespace CqsDataFoundation.Tests.Query
{
    public class PersonDataContext : ObjectDbContextMgr<Person[]>
    {
        public PersonDataContext(Person[] dataSource)
            : base(dataSource, false)
        { }
    }
}
