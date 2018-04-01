using System.Threading;

using AutoMapper;
using NorthWind.Model.Commands;
using Ent = NorthWind.Data.Entities;
using NorthWind.Model.DTO;

namespace NorthWind.Dal.LinqToSql.MsSql
{
    static class MappingsRegistrar
    {
        static int _registered;

        public static void EnsureRegistered()
        {
            if (Interlocked.CompareExchange(ref _registered, 1, 0) == 0)
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<AddProductCommand, Ent.Product>()
                        .ForMember(dst => dst.ProductID, mc => mc.Ignore());
                    cfg.CreateMap<UpdateProductCommand, Ent.Product>();
                    cfg.CreateMap<AddCategoryWithProductsCommand, Ent.Category>().
                        ForSourceMember(src => src.Products, mc => mc.Ignore());

                    cfg.CreateMap<Ent.Category, Category>();
                    cfg.CreateMap<Ent.Supplier, Supplier>();
                    cfg.CreateMap<Ent.Customer, Customer>();
                    cfg.CreateMap<Ent.Product, Product>();
                });
            }
        }
    }
}
