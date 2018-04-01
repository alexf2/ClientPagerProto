using System;
using System.Collections.Generic;
using CqsDataFoundation.Tests.Dto;
using CqsDataFoundation.Tests.Query.Decorators;
using CqsDataFoundation.Command;
using CqsDataFoundation.Command.Decorators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CqsDataFoundation.Tests.Command.Decorators
{
    [TestClass]
    public sealed class DeadlockRetryCommandTests
    {
        class DeadlockRetryCommandHandlerDecorator2<TCommand> : DeadlockRetryCommandHandlerDecorator<TCommand>
        {
            public DeadlockRetryCommandHandlerDecorator2(ICommandHandler<TCommand> decorated, int retryCount = 5)
                : base(decorated, retryCount)
            {
            }

            protected override bool IsDeadlockException(Exception ex)
            {                
                return true;
            }
        }

        private ICommandHandler<AddClientCommand> _handler;

        [TestInitialize]
        public void TestInitialize()
        {
            _handler = new DeadlockRetryCommandHandlerDecorator2<AddClientCommand>(
                new ValidationCommandHandlerDecorator<AddClientCommand>(null, new AddClientCommandHandler(new ClientDataContext(new List<Client>())))
            );
        }

        [TestMethod]
        public void HandleValid()
        {
            var cmd = new AddClientCommand()
            {
                GivenName = "Paul",
                SurName = "Glenn",
            };

            _handler.Handle(cmd);

            var ctx = (ClientDataContext)_handler.DbContext;

            Assert.AreEqual(1, ctx.DataSource.Count);
            Assert.AreEqual("Paul", ctx.DataSource[0].GivenName);
            Assert.AreEqual("Glenn", ctx.DataSource[0].SurName);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void HandleInvalid()
        {
            var cmd = new AddClientCommand()
            {
                SurName = "Glenn"
            };
            _handler.Handle(cmd);            
        }
    }
}
