using AutoMapper;
using NorthWind.Model.DTO;
using NorthWind.Model.Queries;
using CqsDataFoundation.Query;
using Ent = NorthWind.Data.Entities;

namespace NorthWind.Dal.LinqToSql.MsSql.Handlers.Query
{
    public sealed class AllCategoriesQueryHandler : QueryHandlerPaggedLinqBase<NorthWindContextMgr, AllCategoriesQuery, DataPage<Category>, Category>
    {
        public AllCategoriesQueryHandler(NorthWindContextMgr context, bool sharedContext = false)
            : base(context, sharedContext)
        {
        }

        #region ICommandHandler
        public override DataPage<Category> Handle(AllCategoriesQuery q)
        {
            return GetPage(q, DbContextUser.DbContext.Categories, Mapper.Map<Ent.Category, Category>);
        }
        #endregion ICommandHandler
    }    
}
