using System.Runtime.Serialization;
using NorthWind.Model.DTO;
using CqsDataFoundation.Query;

namespace NorthWind.Model.Queries
{
    [DataContract(Namespace = "http://www.ru/NorthWind")]
    public class AllCategoriesQuery : QueryPaggedBase<Category, DataPage<Category>>
    {
        public AllCategoriesQuery(int pageNumber, int size = Constants.PageSizeDefault)
            : base(size, pageNumber)
        {            
        }

    }
}
