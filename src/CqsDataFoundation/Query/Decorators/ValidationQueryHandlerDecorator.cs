using System;
using System.ComponentModel.DataAnnotations;
using CqsDataFoundation.Validation;

namespace CqsDataFoundation.Query.Decorators
{
    public sealed class ValidationQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        readonly IServiceProvider _provider;
        readonly IQueryHandler<TQuery, TResult> _decoratedHndl;

        public ValidationQueryHandlerDecorator(IServiceProvider srvLocator, IQueryHandler<TQuery, TResult> decorated)
        {
            _provider = srvLocator;
            _decoratedHndl = decorated;
        }

        #region IQueryHandler
        public TResult Handle(TQuery query)
        {
            ValidationHelper.Validate(query, new ValidationContext(query, _provider, null));

            return _decoratedHndl.Handle(query);
        }

        public IDataContext DbContext
        {
            get { return _decoratedHndl.DbContext; }
        }
        #endregion IQueryHandler

        public void Dispose()
        {
            _decoratedHndl.Dispose();
        }
    }
}