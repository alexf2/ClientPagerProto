using AutoMapper;
using NorthWind.Model.DTO;
using NorthWind.Model.Queries;
using CqsDataFoundation.Query;
using Ent = NorthWind.Data.Entities;

namespace NorthWind.Dal.LinqToSql.MsSql.Handlers.Query
{
    public sealed class CustomersPageSearchQueryHandler : QueryHandlerPaggedLinqBase<NorthWindContextMgr, CustomersPageSearchQuery, DataPage<Customer>, Customer>
    {
        public CustomersPageSearchQueryHandler(NorthWindContextMgr context, bool sharedContext = false)
            : base(context, sharedContext)
        {
        }

        #region ICommandHandler
        public override DataPage<Customer> Handle(CustomersPageSearchQuery q)
        {
            return GetPage(q, DbContextUser.DbContext.Customers, Mapper.Map<Ent.Customer, Customer>);
        }
        #endregion ICommandHandler
    }
}
