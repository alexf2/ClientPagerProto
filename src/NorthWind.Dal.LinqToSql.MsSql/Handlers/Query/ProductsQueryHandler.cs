using AutoMapper;
using NorthWind.Model.DTO;
using NorthWind.Model.Queries;
using CqsDataFoundation.Query;
using Ent = NorthWind.Data.Entities;

namespace NorthWind.Dal.LinqToSql.MsSql.Handlers.Query
{
    public sealed class ProductsQueryHandler : QueryHandlerPaggedLinqBase<NorthWindContextMgr, ProductsQuery, DataPage<Product>, Product>
    {
        public ProductsQueryHandler(NorthWindContextMgr context, bool sharedContext = false)
            : base(context, sharedContext)
        {
        }

        #region ICommandHandler
        public override DataPage<Product> Handle(ProductsQuery q)
        {
            return GetPage(q, DbContextUser.DbContext.Products, Mapper.Map<Ent.Product, Product>);
        }
        #endregion ICommandHandler
    }
}
