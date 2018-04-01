using System;
using System.Transactions;

namespace CqsDataFoundation.Command.Decorators
{
    public sealed class TranScopeCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    {
        readonly ICommandHandler<TCommand> _decoratedHndl;
        readonly TransactionScopeOption _opt;        

        public TranScopeCommandHandlerDecorator(ICommandHandler<TCommand> decorated, TransactionScopeOption opt = TransactionScopeOption.Required)
        {
            _decoratedHndl = decorated;
            _opt = opt;
            IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            Timeout = TransactionManager.MaximumTimeout;
        }

        #region ICommandHandler
        public void Handle(TCommand command)
        {
            using (var scope = new TransactionScope(_opt, new TransactionOptions() { IsolationLevel = this.IsolationLevel, Timeout = this.Timeout }))
            {
                _decoratedHndl.Handle(command);
                _decoratedHndl.DbContext.SubmitUnitOfWork();
                scope.Complete();
            }
        }

        public IDataContext DbContext
        {
            get { return _decoratedHndl.DbContext; }
        }
        #endregion ICommandHandler

        public IsolationLevel IsolationLevel
        {
            get;
            set;
        }

        public TimeSpan Timeout
        {
            get;
            set;
        }

        public void Dispose()
        {
            _decoratedHndl.Dispose();
        }
    }
}
