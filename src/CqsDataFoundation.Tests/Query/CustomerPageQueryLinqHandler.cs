using System.Linq;
using CqsDataFoundation.Query;

namespace CqsDataFoundation.Tests.Query
{
    public sealed class CustomerPageQueryLinqHandler : QueryHandlerPaggedLinqBase<CustomersDataContext, CustomerPageQuery, DataPage<Customer>, Customer>
    {
        public CustomerPageQueryLinqHandler(CustomersDataContext context, bool sharedContext = false)
            : base(context, sharedContext)
        {
        }

        #region ICommandHandler
        public override DataPage<Customer> Handle(CustomerPageQuery q)
        {
            return GetPage(q, DbContextUser.DataSource.AsQueryable());
        }
        #endregion ICommandHandler
    }
}
