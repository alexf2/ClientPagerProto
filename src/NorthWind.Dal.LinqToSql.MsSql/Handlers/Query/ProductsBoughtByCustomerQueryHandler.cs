using System.Linq;
using NorthWind.Model.DTO;
using NorthWind.Model.Queries;
using CqsDataFoundation.Query;

namespace NorthWind.Dal.LinqToSql.MsSql.Handlers.Query
{
    public sealed class ProductsBoughtByCustomerQueryHandler : QueryHandlerPaggedLinqBase<NorthWindContextMgr, ProductsBoughtByCustomerQuery, DataPage<ProductInfo>, ProductInfo>
    {
        public ProductsBoughtByCustomerQueryHandler(NorthWindContextMgr context, bool sharedContext = false)
            : base(context, sharedContext)
        {
        }

        #region ICommandHandler
        public override DataPage<ProductInfo> Handle(ProductsBoughtByCustomerQuery q)
        {
            var iq = from p in DbContextUser.DbContext.Products
                    join od in DbContextUser.DbContext.OrderDetails on p.ProductID equals od.ProductID
                    join o in DbContextUser.DbContext.Orders on od.OrderID equals o.OrderID
                    join c in DbContextUser.DbContext.Customers on o.CustomerID equals c.CustomerID
                    where c.ContactName == q.ContactName
                    group p by p.ProductID
                        into g
                        from pp in DbContextUser.DbContext.Products
                        where pp.ProductID == g.Key
                        orderby pp.Category.CategoryName, pp.ProductName
                        select new ProductInfo() { ProductID = pp.ProductID, CategoryName = pp.Category.CategoryName, CompanyName = pp.Supplier.CompanyName, ProductName = pp.ProductName, BoughtNumber = g.Count() };

            q.ClearFilterig();
            q.ClearSorting();

            return GetPage(q, iq);
        }
        #endregion ICommandHandler
    }
}
