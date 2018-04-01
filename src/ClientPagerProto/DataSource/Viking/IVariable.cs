using System.Collections.Generic;

namespace ClientPagerProto.DataSource.Viking
{
    public interface IVariable
    {
        IEnumerable<IVariableCategory> Categories { get; }
        bool IsCategorizableText { get; }
    }
}
