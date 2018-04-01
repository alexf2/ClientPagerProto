using AutoMapper;
using NorthWind.Model.Commands;
using NorthWind.Model.DTO;
using CqsDataFoundation.Command;
using Ent = NorthWind.Data.Entities;

namespace NorthWind.Dal.LinqToSql.MsSql.Handlers.Command
{
    public sealed class AddCategoryWithProductsCommandHandler : CommandHandlerBase<NorthWindContextMgr, AddCategoryWithProductsCommand>
    {
        public AddCategoryWithProductsCommandHandler(NorthWindContextMgr ctx)
            : base(ctx)
        {
        }

        #region ICommandHandler
        public override void Handle(AddCategoryWithProductsCommand command)
        {
            var ent = Mapper.Map<AddCategoryWithProductsCommand, Ent.Category>(command);

            if(command.Products != null)
                foreach (var p in command.Products)
                    ent.Products.Add(Mapper.Map<Product, Ent.Product>(p));

            DbContextUser.DbContext.Categories.InsertOnSubmit(ent);
            DbContextUser.ChangesSubmitted += (snd) =>
            {
                command.CategoryID = ent.CategoryID;

                if (command.Products != null)
                    for (int i = 0; i < ent.Products.Count; ++i)
                        command.Products[i].ProductID = ent.Products[i].ProductID;
            }; //gettinc auto increment field back
        }
        #endregion ICommandHandler
    }
}
