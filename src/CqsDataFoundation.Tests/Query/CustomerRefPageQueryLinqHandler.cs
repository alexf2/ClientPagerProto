using System.Linq;
using CqsDataFoundation.Query;

namespace CqsDataFoundation.Tests.Query
{
    public sealed class CustomerRefPageQueryLinqHandler : QueryHandlerPaggedLinqBase<CustomersDataContext, CustomerRefPageQuery, DataPage<CustomerRef>, CustomerRef>
    {
        public CustomerRefPageQueryLinqHandler(CustomersDataContext context, bool sharedContext = false)
            : base(context, sharedContext)
        {
        }

        #region ICommandHandler
        public override DataPage<CustomerRef> Handle(CustomerRefPageQuery q)
        {
            return GetPage(q, DbContextUser.DataSource.AsQueryable(), (c) => new CustomerRef() { Name  = c.Name, SurName = c.SurName});
        }
        #endregion ICommandHandler
    }
}
