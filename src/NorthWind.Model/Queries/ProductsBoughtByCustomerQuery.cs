using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using NorthWind.Model.DTO;
using CqsDataFoundation.Query;

namespace NorthWind.Model.Queries
{
    [DataContract(Namespace = "http://www.ru/NorthWind")]
    public class ProductsBoughtByCustomerQuery : QueryPaggedBase<ProductInfo, DataPage<ProductInfo>>
    {
        public ProductsBoughtByCustomerQuery(string contactName, int pageNumber, int size = Constants.PageSizeDefault)
            : base(size, pageNumber)
        {
            ContactName = contactName;
        }

        [MaxLength(30, ErrorMessage = "Customer name should not exceed 30 characters")]
        [DataMember(Order = 10)]
        public string ContactName { get; set; }
    }
}
