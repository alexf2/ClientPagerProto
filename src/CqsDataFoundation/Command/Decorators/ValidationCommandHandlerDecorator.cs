using System;
using System.ComponentModel.DataAnnotations;
using CqsDataFoundation.Validation;

namespace CqsDataFoundation.Command.Decorators
{
    public sealed class ValidationCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    {
        readonly IServiceProvider _provider;
        readonly ICommandHandler<TCommand> _decoratedHndl;

        public ValidationCommandHandlerDecorator(IServiceProvider srvLocator, ICommandHandler<TCommand> decorated)
        {
            _provider = srvLocator;
            _decoratedHndl = decorated;
        }

        #region ICommandHandler
        public void Handle(TCommand query)
        {
            ValidationHelper.Validate(query, new ValidationContext(query, _provider, null));

            _decoratedHndl.Handle(query);
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
