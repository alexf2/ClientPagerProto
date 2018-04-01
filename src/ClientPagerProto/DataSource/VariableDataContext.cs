using CqsDataFoundation;
using ClientPagerProto.DataSource.Viking;

namespace ClientPagerProto.DataSource
{
    public class VariableDataContext : ObjectDbContextMgr<IVariable>
    {
        public VariableDataContext(IVariable dataSource)
            : base(dataSource, false)
        {
        }
    }
}