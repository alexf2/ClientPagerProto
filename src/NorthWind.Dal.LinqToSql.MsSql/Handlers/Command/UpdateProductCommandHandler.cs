using AutoMapper;
using NorthWind.Model.Commands;
using CqsDataFoundation.Command;
using Ent = NorthWind.Data.Entities;

namespace NorthWind.Dal.LinqToSql.MsSql.Handlers.Command
{
    public sealed class UpdateProductCommandHandler : CommandHandlerBase<NorthWindContextMgr, UpdateProductCommand>
    {
        public UpdateProductCommandHandler(NorthWindContextMgr ctx)
            : base(ctx)
        {
        }

        #region ICommandHandler
        public override void Handle(UpdateProductCommand command)
        {
            //http://www.codingodyssey.com/2009/04/07/linq-to-sql-updating-entities/
            var ent = Mapper.Map<UpdateProductCommand, Ent.Product>(command);
            var entOld = new Ent.Product() { ProductID = ent.ProductID };

            DbContextUser.DbContext.Products.Attach(ent, entOld);            
        }
        #endregion ICommandHandler
    }
}
