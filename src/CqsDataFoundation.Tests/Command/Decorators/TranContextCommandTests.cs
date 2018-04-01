using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CqsDataFoundation.Tests.Dto;
using CqsDataFoundation.Tests.Query.Decorators;
using CqsDataFoundation.Command;
using CqsDataFoundation.Command.Decorators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CqsDataFoundation.Tests.Command.Decorators
{
    [TestClass]
    public sealed class TranContextCommandTests
    {
        private ICommandHandler<AddClientCommand> _handler;

        [TestInitialize]
        public void TestInitialize()
        {
            _handler = new TranContextCommandHandlerDecorator<AddClientCommand>(
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

            var ctx = (ClientDataContext) _handler.DbContext;

            Assert.AreEqual(1, ctx.BeginCount);
            Assert.AreEqual(1, ctx.CommitCount);
            Assert.AreEqual(0, ctx.RollbackCount);
        }

        [TestMethod]
        public void HandleInvalid()
        {
            var cmd = new AddClientCommand()
            {
                SurName = "Glenn"
            };

            try
            {
                _handler.Handle(cmd);
            }
            catch (AggregateException ex)
            {                
            }

            var ctx = (ClientDataContext)_handler.DbContext;

            Assert.AreEqual(1, ctx.BeginCount);
            Assert.AreEqual(0, ctx.CommitCount);
            Assert.AreEqual(1, ctx.RollbackCount);
        }
    }
}
