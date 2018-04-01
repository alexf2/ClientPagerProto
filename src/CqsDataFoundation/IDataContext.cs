using System;

namespace CqsDataFoundation
{
    public interface IDataContext : IDisposable
    {
        event Action<IDataContext> ChangesSubmitted;

        IDbContextConfig Configuration
        {
            get;
        }

        void EnsureCreated();

        void BeginTran(System.Transactions.IsolationLevel isLvl = System.Transactions.IsolationLevel.ReadCommitted);
        void CommitTran();
        void RollbackTran();
        void SubmitUnitOfWork();

        
        System.Transactions.IsolationLevel? DefaultIsolationLevel
        {
            get;
            set;
        }
    }
}