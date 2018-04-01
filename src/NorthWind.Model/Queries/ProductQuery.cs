using System.Runtime.Serialization;
using NorthWind.Model.DTO;
using CqsDataFoundation.Query;

namespace NorthWind.Model.Queries
{
    [DataContract(Namespace = "http://www.ru/NorthWind")]
    public class ProductQuery : QueryBase<Product>
    {
        public ProductQuery(int id)
        {
            ProductId = id;
        }

        [DataMember(Order = 10)]
        public int ProductId { get; set; }
    }
}
