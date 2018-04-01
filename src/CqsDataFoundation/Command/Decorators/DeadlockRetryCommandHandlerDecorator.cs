using System;
using System.Data.Common;
using System.Threading;

namespace CqsDataFoundation.Command.Decorators
{
    public class DeadlockRetryCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    {
        readonly ICommandHandler<TCommand> _decoratedHndl;

        public DeadlockRetryCommandHandlerDecorator(ICommandHandler<TCommand> decorated, int retryCount = 5)
        {
            RetryCount = retryCount;
            _decoratedHndl = decorated;
        }

        #region ICommandHandler
        public void Handle(TCommand command)
        {
            HandleInternal(command, RetryCount);
        }

        public IDataContext DbContext
        {
            get { return _decoratedHndl.DbContext; }
        }
        #endregion ICommandHandler

        public int RetryCount { get; private set; }

        void HandleInternal(TCommand command, int count)
        {
            try
            {
                _decoratedHndl.Handle(command);
                _decoratedHndl.DbContext.SubmitUnitOfWork();
                return;
            }
            catch (Exception ex)
            {
                if (count <= 0 || !IsDeadlockException(ex))
                    throw;                
            }

            Thread.Sleep(300);
            HandleInternal(command, count - 1);
        }

        protected virtual bool IsDeadlockException(Exception ex)
        {
            while (ex != null)
            {
                if (ex is DbException && ex.Message.Contains("deadlock"))
                    return true;

                ex = ex.InnerException;
            }

            return false;
        }

        public void Dispose()
        {
            _decoratedHndl.Dispose();
        }
    }
}
