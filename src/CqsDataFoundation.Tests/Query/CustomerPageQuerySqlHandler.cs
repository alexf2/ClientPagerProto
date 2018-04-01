using System.Linq;
using System.Text;
using CqsDataFoundation.Query;

namespace CqsDataFoundation.Tests.Query
{
    public sealed class CustomerPageQuerySqlHandler : QueryHandlerPaggedSqlBase<CustomersDataContext, CustomerPageQuery, DataPage<Customer>, Customer>
    {
        public CustomerPageQuerySqlHandler(CustomersDataContext context)
            : base(context, false)
        {
        }

        #region ICommandHandler
        public override DataPage<Customer> Handle(CustomerPageQuery q)
        {
            var res = DbContextUser.DataSource.Skip((q.PageNumber - 1)*q.PageSize).Take(q.PageSize).Select(c => c);
            var cnt = DbContextUser.DataSource.Count();

            return new DataPage<Customer> { Data = res.ToList(), TotalRecordsCount = cnt, TotalPages = cnt / q.PageSize , PageNumber = q.PageNumber};
        }
        #endregion ICommandHandler

        public string GetSortSqlExpressionTest(CustomerPageQuery q)
        {
            var bld = new StringBuilder();
            GetSortSqlExpression(q, bld);
            return bld.ToString();
        }

        public string GetFilterExpressionTest(CustomerPageQuery q)
        {
            var bld = new StringBuilder();
            GetFilterExpression(q, bld);
            return bld.ToString();
        }
    }
}
