using System;

namespace CqsDataFoundation.Command
{
    public interface ICommandHandler<in TCommand> : IDisposable
    {
        void Handle(TCommand command);

        IDataContext DbContext
        {
            get;
        }
    }    
}
