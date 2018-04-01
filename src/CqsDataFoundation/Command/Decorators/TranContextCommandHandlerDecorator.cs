using System;
using System.Transactions;

namespace CqsDataFoundation.Command.Decorators
{
    public sealed class TranContextCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    {
        readonly ICommandHandler<TCommand> _decoratedHndl;
        readonly IsolationLevel _isLvl;

        public TranContextCommandHandlerDecorator(ICommandHandler<TCommand> decorated, IsolationLevel isLvl = IsolationLevel.ReadCommitted)
        {
            _decoratedHndl = decorated;
            _isLvl = isLvl;
        }

        #region ICommandHandler
        public void Handle(TCommand command)
        {
            _decoratedHndl.DbContext.BeginTran(_isLvl);
            try
            {
                _decoratedHndl.Handle(command);
                _decoratedHndl.DbContext.SubmitUnitOfWork();
                _decoratedHndl.DbContext.CommitTran();
            }
            catch (Exception)
            {
                _decoratedHndl.DbContext.RollbackTran();
                throw;
            }
        }

        public IDataContext DbContext
        {
            get { return _decoratedHndl.DbContext; }
        }
        #endregion ICommandHandler

        public void Dispose()
        {
            _decoratedHndl.Dispose();
        }
    }
}
