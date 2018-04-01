using System.Collections.Generic;
using CqsDataFoundation.Tests.Dto;
using CqsDataFoundation;

namespace CqsDataFoundation.Tests.Query.Decorators
{
    class ClientDataContext : ObjectDbContextMgr<IList<Client>>
    {
        public ClientDataContext(IList<Client> dataSource)
            : base(dataSource, false)
        { }

        public override void BeginTran(
            System.Transactions.IsolationLevel isLvl = System.Transactions.IsolationLevel.ReadCommitted)
        {
            BeginCount = BeginCount + 1;
        }

        public override void CommitTran()
        {
            CommitCount = CommitCount + 1;
        }

        public override void RollbackTran()
        {
            RollbackCount = RollbackCount + 1;
        }

        public int BeginCount { get; private set; }
        public int CommitCount { get; private set; }
        public int RollbackCount { get; private set; }
    }
}
