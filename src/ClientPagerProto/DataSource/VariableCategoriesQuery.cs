using System.Runtime.Serialization;
using CqsDataFoundation.Query;
using ClientPagerProto.DataSource.Viking;

namespace ClientPagerProto.DataSource
{
    [DataContract(Namespace = "http://www.com/")]
    public sealed class VariableCategoriesPagedQuery : QueryPaggedBase<IVariableCategory, DataPage<IVariableCategory>>
    {
        public VariableCategoriesPagedQuery()
        {            
        }

        public VariableCategoriesPagedQuery(int size, int pageNumber)
            : base(size, pageNumber)
        {            
        }                
    }    
}
