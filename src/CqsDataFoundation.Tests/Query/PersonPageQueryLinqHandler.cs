using System.Linq;
using CqsDataFoundation.Query;

namespace CqsDataFoundation.Tests.Query
{
    public class PersonPageQueryLinqHandler : QueryHandlerPaggedLinqBase<PersonDataContext, PersonPageQuery, DataPage<Person>, Person>
    {
        public PersonPageQueryLinqHandler(PersonDataContext context, bool sharedContext = false)
            : base(context, sharedContext)
        {
        }

        public override DataPage<Person> Handle(PersonPageQuery q)
        {
            return GetPage(q, DbContextUser.DataSource.AsQueryable());
        }
    }
}
