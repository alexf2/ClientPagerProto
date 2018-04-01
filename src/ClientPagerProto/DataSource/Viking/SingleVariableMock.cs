using System.Collections.Generic;

namespace ClientPagerProto.DataSource.Viking
{
    public sealed class SingleVariableMock : IVariable
    {
        readonly bool _isCategorizable;

        public SingleVariableMock(IEnumerable<IVariableCategory> cats, bool isCategorizable)
        {
            Categories = cats;
            _isCategorizable = isCategorizable;
        }

        public IEnumerable<IVariableCategory> Categories { get; private set; }

        public bool IsCategorizableText { get { return _isCategorizable; } }
    }
}