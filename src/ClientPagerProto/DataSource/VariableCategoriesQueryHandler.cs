using System.Linq;
using CqsDataFoundation.Query;
using ClientPagerProto.DataSource.Viking;

namespace ClientPagerProto.DataSource
{
    public class VariableCategoriesQueryHandler :
        QueryHandlerPaggedLinqBase<VariableDataContext, VariableCategoriesPagedQuery, DataPage<IVariableCategory>, IVariableCategory>
    {
        public VariableCategoriesQueryHandler(VariableDataContext context, bool sharedContext = false)
            : base(context, sharedContext)
        {
        }

        #region ICommandHandler
        public override DataPage<IVariableCategory> Handle(VariableCategoriesPagedQuery q)
        {
            var variable = DbContextUser.DataSource;

            return GetPage(q, variable.Categories.AsQueryable());
        }
        #endregion ICommandHandler
    }    
}
