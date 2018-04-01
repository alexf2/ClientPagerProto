using AutoMapper;
using NorthWind.Model.DTO;
using NorthWind.Model.Queries;
using CqsDataFoundation.Query;
using Ent = NorthWind.Data.Entities;

namespace NorthWind.Dal.LinqToSql.MsSql.Handlers.Query
{
    public sealed class AllSuppliersQueryHandler : QueryHandlerPaggedLinqBase<NorthWindContextMgr, AllSuppliersQuery, DataPage<Supplier>, Supplier>
    {
        public AllSuppliersQueryHandler(NorthWindContextMgr context, bool sharedContext = false)
            : base(context, sharedContext)
        {
        }

        #region ICommandHandler
        public override DataPage<Supplier> Handle(AllSuppliersQuery q)
        {
            return GetPage(q, DbContextUser.DbContext.Suppliers, Mapper.Map<Ent.Supplier, Supplier>);
        }
        #endregion ICommandHandler
    }
}
