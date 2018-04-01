using System.Runtime.Serialization;
using NorthWind.Model.DTO;
using CqsDataFoundation.Query;

namespace NorthWind.Model.Queries
{
    [DataContract(Namespace = "http://www.ru/NorthWind")]
    public class AllSuppliersQuery : QueryPaggedBase<Supplier, DataPage<Supplier>>
    {
        public AllSuppliersQuery(int pageNumber, int size = Constants.PageSizeDefault)
            : base(size, pageNumber)
        {            
        }
    }
}
